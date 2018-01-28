using System.ComponentModel.DataAnnotations;

namespace api_dating_app.DTOs
{
    /// <summary>
    /// Data Transfer Object (DTO) that carries all information for
    /// a user that would like to register.
    /// </summary>
    public class UserForRegisterDto
    {
        /// <summary>
        /// Represents the name of the user. Contains validation logic.
        /// </summary>
        [Required]
        public string UserName { get; set; }

        /// <summary>
        /// Represents the password of the user. Contains validation logic.
        /// </summary>
        [Required]
        [StringLength(8, MinimumLength = 4, ErrorMessage = "You must specify a password between 4 and 8 characters!")]
        public string Password { get; set; }
    }
}