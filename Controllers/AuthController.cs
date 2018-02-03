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
    /// Provides API endpoints for the authentication process.
    /// </summary>
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly IAuthRepository _authRepository;
        private readonly IConfiguration _configurationService;
        private readonly IMapper _mapperService;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// 
        /// <param name="authRepository">A reference to the authentication repository</param>
        /// <param name="configurationService">A reference to the configurationService of the project</param>
        /// <param name="mapperService">A reference to the service</param>
        public AuthController(IAuthRepository authRepository, IConfiguration configurationService,
            IMapper mapperService)
        {
            _authRepository = authRepository;
            _configurationService = configurationService;
            _mapperService = mapperService;
        }

        /// <summary>
        /// API endpoint for the registration procees. Validates the user data and carries
        /// the registration.
        /// </summary>
        /// 
        /// <param name="userForRegisterDto">Contains all the user data needed to register</param>
        /// <returns>201 - if the process failed, 400 - if the process is successful</returns>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserForRegisterDto userForRegisterDto)
        {
            if (!string.IsNullOrEmpty(userForRegisterDto.UserName))
            {
                userForRegisterDto.UserName = userForRegisterDto.UserName.ToLower();
            }

            // Check if the user already exists
            if (await _authRepository.UserExists(userForRegisterDto.UserName))
            {
                ModelState.AddModelError("UserName", "User name already exists!");
            }

            // Validate the data
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userToCreate = new UserModel
            {
                UserName = userForRegisterDto.UserName
            };

            var createdUser = await _authRepository.Register(userToCreate, userForRegisterDto.Password);

            if (createdUser != null)
            {
                return StatusCode(201);
            }

            throw new Exception("The registration process failed!");
        }

        /// <summary>
        /// API endpoint for the login process. Validates if the user already exists and creates
        /// a JWT token used to authorize all further API requests.
        /// </summary>
        /// 
        /// <param name="userForLoginDto">Contains all the user data needed to login</param>
        /// <returns>401 - if the process failed, 200 - if the process is successful</returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserForLoginDto userForLoginDto)
        {
            var userFromRepo =
                await _authRepository.Login(userForLoginDto.UserName.ToLower(), userForLoginDto.Password);

            if (userFromRepo == null)
            {
                return Unauthorized();
            }

            // Generate token
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