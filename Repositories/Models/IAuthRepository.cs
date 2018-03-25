using System.Threading.Tasks;
using api_dating_app.models;

namespace api_dating_app.Data
{
    /// <summary>
    /// Author: Petar Krastev
    /// </summary>
    public interface IAuthRepository
    {
        Task<UserModel> Register(UserModel user, string password);

        Task<UserModel> Login(string userName, string password);

        Task<bool> UserExists(string username);
    }
}