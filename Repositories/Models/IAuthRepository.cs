using System.Threading.Tasks;
using api_dating_app.models;

namespace api_dating_app.Data
{
    /// <summary>
    /// Represents an abstraction of the authentication logic, follows the
    /// repository pattern. Adds another level of abstraction between the
    /// database and the controller.
    /// </summary>
    public interface IAuthRepository
    {
        /// <summary>
        /// An abstraction of the registration process.
        /// </summary>
        Task<User> Register(User user, string password);

        /// <summary>
        /// An abstraction of the login process.
        /// </summary>
        Task<User> Login(string userName, string password);

        /// <summary>
        /// An abstraction of the verification of duplicate user names.
        /// </summary>
        Task<bool> UserExists(string username);
    }
}