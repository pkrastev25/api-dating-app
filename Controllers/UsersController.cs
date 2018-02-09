using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using api_dating_app.Data;
using api_dating_app.DTOs;
using api_dating_app.Helpers;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api_dating_app.Controllers
{
    /// <summary>
    /// Provides API endpoints for managing users.
    /// </summary>
    [ServiceFilter(typeof(LogUserActivityHelper))]
    [Authorize]
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private readonly IDatingRepository _datingRepository;
        private readonly IMapper _mapperService;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// 
        /// <param name="datingRepository">A reference to the dating repository</param>
        /// <param name="mapperService">A reference to the mapper service</param>
        public UsersController(IDatingRepository datingRepository, IMapper mapperService)
        {
            _datingRepository = datingRepository;
            _mapperService = mapperService;
        }

        /// <summary>
        /// API endpoint used to retreive all users. A conversion of
        /// the user data is done with the help of
        /// <see cref="UserForListDto"/>.
        /// </summary>
        /// 
        /// <returns>200 - if the process is successful</returns>
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _datingRepository.GetUsers();
            var usersToReturn = _mapperService.Map<IEnumerable<UserForListDto>>(users);

            return Ok(usersToReturn);
        }

        /// <summary>
        /// API endpoint used to retreive a specific user. A conversion
        /// of the user data is done with the help of
        /// <see cref="UserForDetailDto"/>.
        /// </summary>
        /// 
        /// <param name="userId">The id of the specific user</param>
        /// <returns>200 - if the process is successful</returns>
        [HttpGet("{userId}", Name = "GetUser")]
        public async Task<IActionResult> GetUser(int userId)
        {
            var user = await _datingRepository.GetUser(userId);
            var userToReturn = _mapperService.Map<UserForDetailDto>(user);

            return Ok(userToReturn);
        }

        /// <summary>
        /// API endpoint used to update a specific user. A conversion
        /// of the user data is done with the help of
        /// <see cref="UserForUpdateDto"/>.
        /// </summary>
        /// 
        /// <param name="userId">The id of the user</param>
        /// <param name="userForUpdate">The update user information</param>
        /// <returns>400, 401, 404 - if the process is failed, 204 - if the process is successful</returns>
        /// <exception cref="Exception">The user failed to update</exception>
        [HttpPut("{userId}")]
        public async Task<IActionResult> UpdateUser(int userId, [FromBody] UserForUpdateDto userForUpdate)
        {
            // Validate the data
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Extract the user id from the JWT token
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var userFromRepo = await _datingRepository.GetUser(userId);

            // Check if the user exists
            if (userFromRepo == null)
            {
                return NotFound($"Could not find user with an id of {userId}");
            }

            // Check if the requesting user can update the user
            if (currentUserId != userFromRepo.Id)
            {
                return Unauthorized();
            }

            // Coverts the DTO to a model
            _mapperService.Map(userForUpdate, userFromRepo);

            // Save the changes
            if (await _datingRepository.SaveAll())
            {
                return NoContent();
            }

            throw new Exception($"Updating user {userId} failed on save!");
        }
    }
}