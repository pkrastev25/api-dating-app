namespace api_dating_app.models
{
    /// <summary>
    /// Represents a model of a user which will be stored in the DB.
    /// </summary>
    public class User
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
    }
}