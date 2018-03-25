using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using api_dating_app.Data;
using api_dating_app.DTOs;
using api_dating_app.Helpers;
using api_dating_app.models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api_dating_app.Controllers
{
    /// <summary>
    /// Author: Petar Krastev
    /// </summary>
    [ServiceFilter(typeof(LogUserActivityHelper))]
    [Authorize]
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private readonly IDatingRepository _datingRepository;
        private readonly IMapper _mapperService;

        public UsersController(IDatingRepository datingRepository, IMapper mapperService)
        {
            _datingRepository = datingRepository;
            _mapperService = mapperService;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers(UserParamsHelper userParamsHelper)
        {
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var userFromRepo = await _datingRepository.GetUser(currentUserId);
            userParamsHelper.UserId = currentUserId;

            if (string.IsNullOrEmpty(userParamsHelper.Gender))
            {
                userParamsHelper.Gender = userFromRepo.Gender == "male" ? "female" : "male";
            }

            var users = await _datingRepository.GetUsers(userParamsHelper);
            var usersToReturn = _mapperService.Map<IEnumerable<UserForListDto>>(users);
            Response.AddPagination(users.CurrentPage, users.PageSize, users.TotalCount, users.TotalPages);

            return Ok(usersToReturn);
        }

        [HttpGet("{userId}", Name = "GetUser")]
        public async Task<IActionResult> GetUser(int userId)
        {
            var user = await _datingRepository.GetUser(userId);
            var userToReturn = _mapperService.Map<UserForDetailDto>(user);

            return Ok(userToReturn);
        }

        [HttpPut("{userId}")]
        public async Task<IActionResult> UpdateUser(int userId, [FromBody] UserForUpdateDto userForUpdate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var userFromRepo = await _datingRepository.GetUser(userId);

            if (userFromRepo == null)
            {
                return NotFound($"Could not find user with an id of {userId}");
            }

            if (currentUserId != userFromRepo.Id)
            {
                return Unauthorized();
            }

            _mapperService.Map(userForUpdate, userFromRepo);

            if (await _datingRepository.SaveAll())
            {
                return NoContent();
            }

            throw new Exception($"Updating user {userId} failed on save!");
        }

        [HttpPost("{userId}/like/{recipientId}")]
        public async Task<IActionResult> LikeUser(int userId, int recipientId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }

            var like = await _datingRepository.GetLike(userId, recipientId);

            if (like != null)
            {
                return BadRequest("You already liked this user!");
            }

            if (await _datingRepository.GetUser(recipientId) == null)
            {
                return NotFound();
            }

            like = new LikeModel
            {
                LikerId = userId,
                LikeeId = recipientId
            };

            _datingRepository.Add(like);

            if (await _datingRepository.SaveAll())
            {
                return Ok(new { });
            }

            return BadRequest("Failed to like user!");
        }
    }
}