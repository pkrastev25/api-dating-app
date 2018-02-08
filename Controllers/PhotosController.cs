using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using api_dating_app.Data;
using api_dating_app.DTOs;
using api_dating_app.Helpers;
using api_dating_app.models;
using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace api_dating_app.Controllers
{
    /// <summary>
    /// Provides API endpoints for managing photos.
    /// </summary>
    [Authorize]
    [Route("api/users/{userId}/photos")]
    public class PhotosController : Controller
    {
        private readonly IDatingRepository _datingRepository;
        private readonly IMapper _mapperService;
        private readonly IOptions<CloudinarySettingsHelper> _cloudinaryConfigService;
        private readonly Cloudinary _cloudinaryService;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// 
        /// <param name="datingRepository">A reference to the repository</param>
        /// <param name="mapperService">A reference to the service</param>
        /// <param name="cloudinaryConfigService">A reference to the service</param>
        public PhotosController(IDatingRepository datingRepository, IMapper mapperService,
            IOptions<CloudinarySettingsHelper> cloudinaryConfigService)
        {
            _datingRepository = datingRepository;
            _mapperService = mapperService;
            _cloudinaryConfigService = cloudinaryConfigService;

            // Create the Cloudinary account
            Account account = new Account(
                cloudinaryConfigService.Value.CloudName,
                cloudinaryConfigService.Value.ApiKey,
                cloudinaryConfigService.Value.ApiSecret
            );

            _cloudinaryService = new Cloudinary(account);
        }

        /// <summary>
        /// API endpoint used to retreive a photo. A conversion of
        /// the photo data is done with the help of 
        /// <see cref="PhotoForReturnDto"/>.
        /// </summary>
        /// 
        /// <param name="photoId">The id of the photo</param>
        /// <returns>200 - if process is successful</returns>
        [HttpGet("{photoId}", Name = "GetPhoto")]
        public async Task<IActionResult> GetPhoto(int photoId)
        {
            var photoFromRepo = await _datingRepository.GetPhoto(photoId);
            var photo = _mapperService.Map<PhotoForReturnDto>(photoFromRepo);

            return Ok(photo);
        }

        /// <summary>
        /// API endpoint used to add a photo to an user. A conversion
        /// of the photo data is done with the help of
        /// <see cref="PhotoForCreationDto"/> and <see cref="PhotoForReturnDto"/>.
        /// </summary>
        /// 
        /// <param name="userId">The id of the user</param>
        /// <param name="photoForCreation">The photo information</param>
        /// <returns>400, 401 - if the process failed, 201 - if the process is successful</returns>
        [HttpPost]
        public async Task<IActionResult> AddPhotoForUser(int userId, PhotoForCreationDto photoForCreation)
        {
            var user = await _datingRepository.GetUser(userId);

            // Check if the user exists
            if (user == null)
            {
                return BadRequest("Could not find user!");
            }

            // Extract the user id from the JWT token
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            // Check if the requesting user can update the user
            if (currentUserId != user.Id)
            {
                return Unauthorized();
            }

            var file = photoForCreation.File;
            var uploadResult = new ImageUploadResult();

            if (file.Length > 0)
            {
                using (var stream = file.OpenReadStream())
                {
                    var uploadParams = new ImageUploadParams
                    {
                        File = new FileDescription(file.Name, stream),
                        // Crop the photos before they are uploaded
                        Transformation = new Transformation()
                            .Width(500)
                            .Height(500)
                            .Crop("fill")
                            .Gravity("face")
                    };

                    // Upload the photo to Cloudinary
                    uploadResult = _cloudinaryService.Upload(uploadParams);
                }
            }

            // Map the url and the id from Cloudinary
            photoForCreation.Url = uploadResult.Uri.ToString();
            photoForCreation.PublicId = uploadResult.PublicId;

            // Convert the DTO to a model
            var photo = _mapperService.Map<PhotoModel>(photoForCreation);
            photo.User = user;

            // Make main photo if the user does not have one
            if (!user.Photos.Any(m => m.IsMain))
            {
                photo.IsMain = true;
            }

            // Add the photo to the user's photo collection
            user.Photos.Add(photo);

            // Save the changes
            if (!await _datingRepository.SaveAll())
            {
                return BadRequest("Could not add the photo");
            }

            // Converts the model to a DTO
            var photoToReturn = _mapperService.Map<PhotoForReturnDto>(photo);

            // Return the photo together with the id
            return CreatedAtRoute("GetPhoto", new {photoId = photo.Id}, photoToReturn);
        }

        /// <summary>
        /// API endpoint used to update a specific photo. A conversion of
        /// the photo data is done with the help of 
        /// <see cref="PhotoForUpdateDto"/>.
        /// </summary>
        /// 
        /// <param name="userId">The id of the user</param>
        /// <param name="photoId">The id of the photo</param>
        /// <param name="photoForUpdate">The update photo information</param>
        /// <returns>400, 401, 404 - if the process failed, 204 - if the process is successful</returns>
        [HttpPost("{photoId}")]
        public async Task<IActionResult> UpdatePhoto(int userId, int photoId,
            [FromBody] PhotoForUpdateDto photoForUpdate)
        {
            // Validate the data
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Extract the user id from the JWT token
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }

            var photoFromRepo = await _datingRepository.GetPhoto(photoId);

            // Check if the photo exists
            if (photoFromRepo == null)
            {
                return NotFound();
            }

            // Set the requested photo as the main photo
            if (!photoForUpdate.IsMain)
            {
                var result = await SetMainPhoto(userId, photoFromRepo);

                return result;
            }

            return BadRequest("Could not update photo!");
        }

        /// <summary>
        /// API endpoint used to delete a specific photo.
        /// </summary>
        /// 
        /// <param name="userId">The id of the user</param>
        /// <param name="photoId">The id of the photo</param>
        /// <returns>400, 401, 404 - if the process failed, 200 - if the process successful</returns>
        [HttpDelete("{photoId}")]
        public async Task<IActionResult> DeletePhoto(int userId, int photoId)
        {
            // Check if the requesting user has access
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }

            var photoFromRepo = await _datingRepository.GetPhoto(photoId);

            // Check if the photo exists
            if (photoFromRepo == null)
            {
                return NotFound();
            }

            // Check if the requested photo is the main photo
            if (photoFromRepo.IsMain)
            {
                return BadRequest("You cannot delete the main photo!");
            }

            // Delete a photo stored into Cloudinary and the DB
            if (photoFromRepo.PublicId != null)
            {
                var deleteParams = new DeletionParams(photoFromRepo.PublicId);
                var result = _cloudinaryService.Destroy(deleteParams);

                if (result.Result == "ok")
                {
                    _datingRepository.Delete(photoFromRepo);
                }
            }

            // Delete a photo stored only in the DB, used only for the seeded data
            if (photoFromRepo.PublicId == null)
            {
                _datingRepository.Delete(photoFromRepo);
            }

            // Save the changes
            if (await _datingRepository.SaveAll())
            {
                return Ok();
            }

            return BadRequest("Failed to delete the photo!");
        }

        /// <summary>
        /// Helper method used to set a photo to a main photo.
        /// </summary>
        /// 
        /// <param name="userId">The id of the user</param>
        /// <param name="photoFromRepo">The photo information stored in DB</param>
        /// <returns>400 - if the process failed, 204 - of the process successful</returns>
        private async Task<IActionResult> SetMainPhoto(int userId, PhotoModel photoFromRepo)
        {
            // Check if the requested photo is already the main photo
            if (photoFromRepo.IsMain)
            {
                return BadRequest("This is already the main photo!");
            }

            var currentMainPhoto = await _datingRepository.GetMainPhotoForUser(userId);

            // Change the current main photo
            if (currentMainPhoto != null)
            {
                currentMainPhoto.IsMain = false;
            }

            photoFromRepo.IsMain = true;

            // Save the changes
            if (await _datingRepository.SaveAll())
            {
                return NoContent();
            }

            return BadRequest("Could not set photo to main!");
        }
    }
}