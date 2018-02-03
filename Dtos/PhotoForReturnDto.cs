using System;

namespace api_dating_app.DTOs
{
    /// <summary>
    /// Data Transfer Object (DTO) that carries all information for
    /// a photo uploaded to Cloudinary.
    /// </summary>
    public class PhotoForReturnDto
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
        /// Represents the date in which the photo was added.
        /// </summary>
        public DateTime DateAdded { get; set; }

        /// <summary>
        /// Specifies whether this is the main photo of the user.
        /// </summary>
        public bool IsMain { get; set; }

        /// <summary>
        /// Represents the id used by Cloudinary.
        /// </summary>
        public string PublicId { get; set; }
    }
}