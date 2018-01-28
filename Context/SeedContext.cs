using System.Collections.Generic;
using api_dating_app.models;
using Newtonsoft.Json;

namespace api_dating_app.Data
{
    /// <summary>
    /// Context used to populate the database with predefined data.
    /// Note, this class should be only used in development!
    /// </summary>
    public class SeedContext
    {
        /// <summary>
        /// Specifies the password for all users stored inside
        /// <see cref="UserSeedDataContext.json"/>.
        /// </summary>
        private const string PASSWORD = "password";

        // SERVICES
        private readonly DataContext _context;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// 
        /// <param name="context">Reference to the database service</param>
        public SeedContext(DataContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Populates the database by creating users from a predefined json file
        /// <see cref="UserSeedDataContext.json"/>. Recreates the password salt and
        /// hash.
        /// </summary>
        public void SeedUserData()
        {
            /*
            // Removes old data in the DB
            _context.Users.RemoveRange(_context.Users);
            _context.SaveChanges();
            */

            // Seed users
            var userData = System.IO.File.ReadAllText("Context/UserSeedDataContext.json");
            var users = JsonConvert.DeserializeObject<List<UserModel>>(userData);

            foreach (var user in users)
            {
                // Create the password hash
                CreatePasswordHash(PASSWORD, out var passwordHash, out var passwordSalt);

                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
                user.UserName = user.UserName.ToLower();

                _context.Users.Add(user);
            }

            _context.SaveChanges();
        }

        /// <summary>
        /// Creates a password hash and a password salt based on the current
        /// password. For security reasons, we do not store the password itself
        /// inside the DB.
        /// </summary>
        /// 
        /// <param name="password">The password of the user</param>
        /// <param name="passwordHash">Used to store the password hash</param>
        /// <param name="passwordSalt">Used to store store the password salt</param>
        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            /*
             * 'using' keyword makes sure that the dispose method is called on an
             * class implementing IDisposable interface. This is very handy, because
             * the created instance is freed from memory right after usage. It does
             * not wait to be garbale collected
             */
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
    }
}