using System;
using Microsoft.AspNetCore.Http;

namespace api_dating_app.DTOs
{
    /// <summary>
    /// Data Transfer Object (DTO) that carries all information for
    /// creating a photo.
    /// </summary>
    public class PhotoForCreationDto
    {
        /// <summary>
        /// Represents the url of the photo.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Represents the file of the photo.
        /// </summary>
        public IFormFile File { get; set; }

        /// <summary>
        /// Represents the description of the photo.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Represents the date the photo was added.
        /// </summary>
        public DateTime DateAdded { get; set; }

        /// <summary>
        /// Represents the id used by Cloudinary.
        /// </summary>
        public string PublicId { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PhotoForCreationDto()
        {
            DateAdded = DateTime.Now;
        }
    }
}