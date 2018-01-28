using System.Collections.Generic;
using System.Threading.Tasks;
using api_dating_app.Data;
using api_dating_app.DTOs;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api_dating_app.Controllers
{
    /// <summary>
    /// Provides API endpoints for the users.
    /// </summary>
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
        /// <returns>200 together with all users</returns>
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
        /// <param name="id">The id of the specific user</param>
        /// <returns>200 together with the user</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _datingRepository.GetUser(id);
            var userToReturn = _mapperService.Map<UserForDetailDto>(user);

            return Ok(userToReturn);
        }
    }
}