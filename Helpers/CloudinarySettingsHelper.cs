namespace api_dating_app.Helpers
{
    /// <summary>
    /// Helper class used to carry the authentication credentials for 
    /// Cloudinary.
    /// </summary>
    public class CloudinarySettingsHelper
    {
        /// <summary>
        /// Specifies the account name for Cloudinary.
        /// </summary>
        public string CloudName { get; set; }

        /// <summary>
        /// Specifies the API key used for Cloudinary.
        /// </summary>
        public string ApiKey { get; set; }

        /// <summary>
        /// Specifies the API secred used for Cloudinary.
        /// </summary>
        public string ApiSecret { get; set; }
    }
}