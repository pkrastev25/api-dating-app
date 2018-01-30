namespace api_dating_app.DTOs
{
    /// <summary>
    /// Data Transfer Object (DTO) that carries all information for
    /// updating an user. This class is used to provide only a subset
    /// of all user information which he could update.
    /// </summary>
    public class UserForUpdateDto
    {
        /// <summary>
        /// Represents the introduction of the user.
        /// </summary>
        public string Introduction { get; set; }

        /// <summary>
        /// Represents what is the user looking for.
        /// </summary>
        public string LookingFor { get; set; }

        /// <summary>
        /// Represents the interests of the user.
        /// </summary>
        public string Interests { get; set; }

        /// <summary>
        /// Represents the city of the user.
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// Represents the country of the user.
        /// </summary>
        public string Country { get; set; }
    }
}