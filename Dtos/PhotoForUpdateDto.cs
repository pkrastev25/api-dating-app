namespace api_dating_app.DTOs
{
    /// <summary>
    /// Data Transfer Object (DTO) that carries all information for
    /// updating a photo.
    /// </summary>
    public class PhotoForUpdateDto
    {
        /// <summary>
        /// Represents whether the photo is used as a main photo.
        /// </summary>
        public bool IsMain { get; set; }
    }
}