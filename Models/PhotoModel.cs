using System;

namespace api_dating_app.models
{
    /// <summary>
    /// Represents a model of a photo which will be stored inside
    /// the DB.
    /// </summary>
    public class PhotoModel
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
        /// Specifies the user to which the photo belongs to.
        /// Note, this create a relation in the DB which deletes the
        /// photo if the user is deleted!
        /// </summary>
        public UserModel User { get; set; }

        /// <summary>
        /// Specifies the unique identifier of an user to which the
        /// photo belongs to.
        /// Note, this create a relation in the DB which deletes the
        /// photo if the user is deleted!
        /// </summary>
        public int UserId { get; set; }
    }
}