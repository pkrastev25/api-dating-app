using System.Collections.Generic;
using api_dating_app.models;
using Newtonsoft.Json;

namespace api_dating_app.Data
{
    /// <summary>
    /// Author: Petar Krastev
    /// </summary>
    public class SeedContext
    {
        private const string Password = "password";

        private readonly DataContext _context;

        public SeedContext(DataContext context)
        {
            _context = context;
        }

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
                CreatePasswordHash(Password, out var passwordHash, out var passwordSalt);

                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
                user.UserName = user.UserName.ToLower();

                _context.Users.Add(user);
            }

            _context.SaveChanges();
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
    }
}