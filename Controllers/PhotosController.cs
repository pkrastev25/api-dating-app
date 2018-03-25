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
    /// Author: Petar Krastev
    /// </summary>
    [Authorize]
    [Route("api/users/{userId}/photos")]
    public class PhotosController : Controller
    {
        private readonly IDatingRepository _datingRepository;
        private readonly IMapper _mapperService;
        private readonly Cloudinary _cloudinaryService;

        public PhotosController(IDatingRepository datingRepository, IMapper mapperService,
            IOptions<CloudinarySettingsHelper> cloudinaryConfigService)
        {
            _datingRepository = datingRepository;
            _mapperService = mapperService;

            var account = new Account(
                cloudinaryConfigService.Value.CloudName,
                cloudinaryConfigService.Value.ApiKey,
                cloudinaryConfigService.Value.ApiSecret
            );

            _cloudinaryService = new Cloudinary(account);
        }

        [HttpGet("{photoId}", Name = "GetPhoto")]
        public async Task<IActionResult> GetPhoto(int photoId)
        {
            var photoFromRepo = await _datingRepository.GetPhoto(photoId);
            var photo = _mapperService.Map<PhotoForReturnDto>(photoFromRepo);

            return Ok(photo);
        }

        [HttpPost]
        public async Task<IActionResult> AddPhotoForUser(int userId, PhotoForCreationDto photoForCreation)
        {
            var user = await _datingRepository.GetUser(userId);

            if (user == null)
            {
                return BadRequest("Could not find user!");
            }

            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

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

                    uploadResult = _cloudinaryService.Upload(uploadParams);
                }
            }

            photoForCreation.Url = uploadResult.Uri.ToString();
            photoForCreation.PublicId = uploadResult.PublicId;

            var photo = _mapperService.Map<PhotoModel>(photoForCreation);
            photo.User = user;

            if (!user.Photos.Any(m => m.IsMain))
            {
                photo.IsMain = true;
            }

            user.Photos.Add(photo);

            if (!await _datingRepository.SaveAll())
            {
                return BadRequest("Could not add the photo");
            }

            var photoToReturn = _mapperService.Map<PhotoForReturnDto>(photo);

            return CreatedAtRoute("GetPhoto", new {photoId = photo.Id}, photoToReturn);
        }

        [HttpPost("{photoId}")]
        public async Task<IActionResult> UpdatePhoto(int userId, int photoId,
            [FromBody] PhotoForUpdateDto photoForUpdate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }

            var photoFromRepo = await _datingRepository.GetPhoto(photoId);

            if (photoFromRepo == null)
            {
                return NotFound();
            }

            if (photoForUpdate.IsMain)
            {
                return BadRequest("Could not update photo!");
            }

            return await SetMainPhoto(userId, photoFromRepo);
        }

        [HttpDelete("{photoId}")]
        public async Task<IActionResult> DeletePhoto(int userId, int photoId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }

            var photoFromRepo = await _datingRepository.GetPhoto(photoId);

            if (photoFromRepo == null)
            {
                return NotFound();
            }

            if (photoFromRepo.IsMain)
            {
                return BadRequest("You cannot delete the main photo!");
            }

            if (photoFromRepo.PublicId != null)
            {
                var deleteParams = new DeletionParams(photoFromRepo.PublicId);
                var result = _cloudinaryService.Destroy(deleteParams);

                if (result.Result == "ok")
                {
                    _datingRepository.Delete(photoFromRepo);
                }
            }

            if (photoFromRepo.PublicId == null)
            {
                _datingRepository.Delete(photoFromRepo);
            }

            if (await _datingRepository.SaveAll())
            {
                return Ok();
            }

            return BadRequest("Failed to delete the photo!");
        }

        private async Task<IActionResult> SetMainPhoto(int userId, PhotoModel photoFromRepo)
        {
            if (photoFromRepo.IsMain)
            {
                return BadRequest("This is already the main photo!");
            }

            var currentMainPhoto = await _datingRepository.GetMainPhotoForUser(userId);

            if (currentMainPhoto != null)
            {
                currentMainPhoto.IsMain = false;
            }

            photoFromRepo.IsMain = true;

            if (await _datingRepository.SaveAll())
            {
                return NoContent();
            }

            return BadRequest("Could not set photo to main!");
        }
    }
}