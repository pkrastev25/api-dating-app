using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace api_dating_app.models
{
    /// <summary>
    /// Author: Petar Krastev
    /// </summary>
    public class UserModel
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public byte[] PasswordHash { get; set; }

        public byte[] PasswordSalt { get; set; }

        public string Gender { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string KnownAs { get; set; }

        public DateTime Created { get; set; }

        public DateTime LastActive { get; set; }

        public string Introduction { get; set; }

        public string LookingFor { get; set; }

        public string Interests { get; set; }

        public string City { get; set; }

        public string Country { get; set; }

        public ICollection<PhotoModel> Photos { get; set; }

        public ICollection<LikeModel> Likers { get; set; }

        public ICollection<LikeModel> Likees { get; set; }

        public ICollection<MessageModel> MessagesSend { get; set; }

        public ICollection<MessageModel> MessagesRecieved { get; set; }

        public UserModel()
        {
            Photos = new Collection<PhotoModel>();
        }
    }
}