using System;

namespace api_dating_app.DTOs
{
    /// <summary>
    /// Data Transfer Object (DTO) that carries all information for
    /// the details of a photo. This class is used to hide all 
    /// specific information from the database and provide a clean
    /// format for the frontend.
    /// </summary>
    public class PhotoForDetailDto
    {
        /// <summary>
        /// Represents the unique identifier for this photo inside
        /// the DB.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Represents the url of the photo.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Represents the description of the photo.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Represnets the data in which the photo was added.
        /// </summary>
        public DateTime DateAdded { get; set; }

        /// <summary>
        /// Specifies whether this photo is the main one for the user.
        /// </summary>
        public bool IsMain { get; set; }
    }
}