namespace api_dating_app.DTOs
{
    /// <summary>
    /// Data Transfer Object (DTO) that carries all information for
    /// a user that would like to login.
    /// </summary>
    public class UserForLoginDto
    {
        /// <summary>
        /// Represents the name of the user.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Represents the password of the user.
        /// </summary>
        public string Password { get; set; }
    }
}