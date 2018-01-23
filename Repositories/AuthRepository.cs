using System.Linq;
using System.Threading.Tasks;
using api_dating_app.models;
using Microsoft.EntityFrameworkCore;

namespace api_dating_app.Data
{
    /// <summary>
    /// Represents a concrete implementation of the <see cref="IAuthRepository"/>.
    /// Adds another level of abstraction between the database and the controller.
    /// </summary>
    public class AuthRepository : IAuthRepository
    {
        // SERVICES
        private readonly DataContext _context;

        /// <summary>
        /// Constructor. All incoming params are injected via dependency
        /// injection.
        /// </summary>
        /// <param name="context">Context to the database</param>
        public AuthRepository(DataContext context)
        {
            _context = context;
        }
        
        /// <summary>
        /// Handles the registration of the user inside the DB.
        /// </summary>
        /// <param name="user">The <see cref="User"/> that will be saved into the DB</param>
        /// <param name="password">The password of the user</param>
        /// <returns>Registered user</returns>
        public async Task<User> Register(User user, string password)
        {
            // 'out' keyword passes variables by reference
            CreatePasswordHash(password, out var passwordHash, out var passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return user;
        }

        /// <summary>
        /// Handles the login process by comparing the current user name
        /// and current password with the one saved into the DB.
        /// </summary>
        /// <param name="userName">The name of the user</param>
        /// <param name="password">The password of the user</param>
        /// <returns>An user if verification is successful, null otherwise</returns>
        public async Task<User> Login(string userName, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName == userName);

            if (user == null)
            {
                return null;
            }

            return !VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt) ? null : user;
        }

        /// <summary>
        /// Verifies whether the current user name already exists inside the DB.
        /// </summary>
        /// <param name="username">The name of the user</param>
        /// <returns>True if the user name is already taken, false otherwise</returns>
        public async Task<bool> UserExists(string username)
        {
            return await _context.Users.AnyAsync(x => x.UserName == username);
        }
        
        /// <summary>
        /// Creates a password hash and a password salt based on the current
        /// password. For security reasons, we do not store the password itself
        /// inside the DB.
        /// </summary>
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
        
        /// <summary>
        /// Converts the password to a password hash and a password salt. Verifies whether
        /// they match.
        /// </summary>
        /// <param name="password">The password of the user</param>
        /// <param name="userPasswordHash">The password hash of the user</param>
        /// <param name="userPasswordSalt">The password salt of the user</param>
        /// <returns>True, if the password matches, false, otherwise</returns>
        private bool VerifyPasswordHash(string password, byte[] userPasswordHash, byte[] userPasswordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(userPasswordSalt))
            {
                var computeHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

                if (computeHash.Where((t, i) => t != userPasswordHash[i]).Any())
                {
                    return false;
                }
            }

            return true;
        }
    }
}