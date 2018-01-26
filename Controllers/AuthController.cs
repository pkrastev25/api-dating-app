using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using api_dating_app.Data;
using api_dating_app.DTOs;
using api_dating_app.models;
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
        // SERVICES
        private readonly IAuthRepository _authRepository;

        private readonly IConfiguration _configuration;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="authRepository">A reference to the authentication repository\\</param>
        /// <param name="configuration">A reference to the configuration of the project</param>
        public AuthController(IAuthRepository authRepository, IConfiguration configuration)
        {
            _authRepository = authRepository;
            _configuration = configuration;
        }

        /// <summary>
        /// API endpoint for the registration procees. Validates the user data and carries
        /// the registration.
        /// </summary>
        /// <param name="userForRegisterDto">Contains all the user data needed to register</param>
        /// <returns>201 if the registration was successful, 400 otherwise</returns>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserForRegisterDto userForRegisterDto)
        {
            if (!string.IsNullOrEmpty(userForRegisterDto.UserName))
            {
                userForRegisterDto.UserName = userForRegisterDto.UserName.ToLower();
            }

            // Validate request
            if (await _authRepository.UserExists(userForRegisterDto.UserName))
            {
                ModelState.AddModelError("UserName", "User name already exists!");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userToCreate = new User
            {
                UserName = userForRegisterDto.UserName
            };

            var createdUser = await _authRepository.Register(userToCreate, userForRegisterDto.Password);

            return StatusCode(201);
        }

        /// <summary>
        /// API endpoint for the login process. Validates if the user already exists and creates
        /// a JWT token used to authorize all further API requests.
        /// </summary>
        /// <param name="userForLoginDto">Contains all the user data needed to login</param>
        /// <returns>401 if the user does not exist, 200 with the JWT token as a header</returns>
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
            var key = Encoding.ASCII.GetBytes(_configuration.GetSection("AppSettings:Token").Value);
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

            return Ok(new {tokenString});
        }
    }
}