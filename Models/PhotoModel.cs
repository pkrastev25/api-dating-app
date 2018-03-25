using System;

namespace api_dating_app.models
{
    /// <summary>
    /// Author: Petar Krastev
    /// </summary>
    public class PhotoModel
    {
        public int Id { get; set; }

        public string Url { get; set; }

        public string Description { get; set; }

        public DateTime DateAdded { get; set; }

        public bool IsMain { get; set; }

        public string PublicId { get; set; }

        public UserModel User { get; set; }

        public int UserId { get; set; }
    }
}