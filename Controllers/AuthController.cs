using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using api_dating_app.Data;
using api_dating_app.DTOs;
using api_dating_app.models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace api_dating_app.Controllers
{
    /// <summary>
    /// Author: Petar Krastev
    /// </summary>
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly IAuthRepository _authRepository;
        private readonly IConfiguration _configurationService;
        private readonly IMapper _mapperService;

        public AuthController(IAuthRepository authRepository, IConfiguration configurationService,
            IMapper mapperService)
        {
            _authRepository = authRepository;
            _configurationService = configurationService;
            _mapperService = mapperService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserForRegisterDto userForRegisterDto)
        {
            if (!string.IsNullOrEmpty(userForRegisterDto.UserName))
            {
                userForRegisterDto.UserName = userForRegisterDto.UserName.ToLower();
            }

            if (await _authRepository.UserExists(userForRegisterDto.UserName))
            {
                ModelState.AddModelError("UserName", "User name already exists!");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userToCreate = _mapperService.Map<UserModel>(userForRegisterDto);

            var createdUser = await _authRepository.Register(userToCreate, userForRegisterDto.Password);

            if (createdUser == null)
            {
                throw new Exception("The registration process failed!");
            }

            var userToReturn = _mapperService.Map<UserForDetailDto>(createdUser);

            return CreatedAtRoute(
                "GetUser",
                new {controller = "Users", userId = createdUser.Id},
                userToReturn
            );
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserForLoginDto userForLoginDto)
        {
            var userFromRepo =
                await _authRepository.Login(userForLoginDto.UserName.ToLower(), userForLoginDto.Password);

            if (userFromRepo == null)
            {
                return Unauthorized();
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configurationService.GetSection("AppSettings:Token").Value);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, userFromRepo.Id.ToString()),
                    new Claim(ClaimTypes.Name, userFromRepo.UserName)
                }),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha512Signature
                )
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
            var user = _mapperService.Map<UserForListDto>(userFromRepo);

            return Ok(new {tokenString, user});
        }
    }
}