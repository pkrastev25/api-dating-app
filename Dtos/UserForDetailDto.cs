using System;
using System.Collections.Generic;
using api_dating_app.models;

namespace api_dating_app.DTOs
{
    /// <summary>
    /// Data Transfer Object (DTO) that carries all information for
    /// the details of an user. This class is used to hide all 
    /// specific information from the database and provide a clean
    /// format for the frontend.
    /// </summary>
    public class UserForDetailDto
    {
        /// <summary>
        /// Represents the unique identifier for this user inside
        /// the DB.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Represents the salting added to the password, enhances the
        /// password's security.
        /// </summary>
        public byte[] PasswordSalt { get; set; }

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

        /// <summary>
        /// Represents ths url of the main photo of the user.
        /// </summary>
        public string PhotoUrl { get; set; }

        /// <summary>
        /// Represents all photos of the user.
        /// </summary>
        public ICollection<PhotoForDetailDto> Photos { get; set; }
    }
}