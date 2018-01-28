using System;

namespace api_dating_app.DTOs
{
    /// <summary>
    /// Data Transfer Object (DTO) that carries all information for
    /// the user when appearing in a list. This class is used to
    /// hide all  specific information from the database and provide
    /// a clean format for the frontend.
    /// </summary>
    public class UserForListDto
    {
        /// <summary>
        /// Represents the unique identifier for this user inside
        /// the DB.
        /// </summary>
        public int Id { get; set; }
        
        /// <summary>
        /// Represents the name of the user.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Represents the gender of the user.
        /// </summary>
        public string Gender { get; set; }

        /// <summary>
        /// Represents the age of the user.
        /// </summary>
        public int Age { get; set; }

        /// <summary>
        /// Represents the nickname of the user.
        /// </summary>
        public string KnownAs { get; set; }

        /// <summary>
        /// Represents the date in which the user was created.
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        /// Represents the date in which the user was last active.
        /// </summary>
        public DateTime LastActive { get; set; }

        /// <summary>
        /// Represents the city of the user.
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// Represents the country of the user.
        /// </summary>
        public string Country { get; set; }
        
        /// <summary>
        /// Represents ths url of the main photo of the user.
        /// </summary>
        public string PhotoUrl { get; set; }
    }
}