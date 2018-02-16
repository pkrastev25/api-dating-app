using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace api_dating_app.models
{
    /// <summary>
    /// Represents a model of a user which will be stored in the DB.
    /// </summary>
    public class UserModel
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
        /// Represents the hashed version of the password.
        /// </summary>
        public byte[] PasswordHash { get; set; }

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
        /// Represents the date in which the user was born.
        /// </summary>
        public DateTime DateOfBirth { get; set; }

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
        /// Represents all photos of the user.
        /// </summary>
        public ICollection<PhotoModel> Photos { get; set; }

        public ICollection<LikeModel> Likers { get; set; }

        public ICollection<LikeModel> Likees { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        public UserModel()
        {
            Photos = new Collection<PhotoModel>();
        }
    }
}