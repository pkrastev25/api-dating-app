using System.Threading.Tasks;
using api_dating_app.Helpers;
using api_dating_app.models;

namespace api_dating_app.Data
{
    /// <summary>
    /// Represents an abstraction of the dating logic, follows the
    /// repository pattern. Adds another level of abstraction between the
    /// database and the controller.
    /// </summary>
    public interface IDatingRepository
    {
        /// <summary>
        /// An abstraction of adding an element to the database
        /// process.
        /// </summary>
        void Add<T>(T entity) where T : class;

        /// <summary>
        /// An abstraction of removing an element from the database
        /// process.
        /// </summary>
        void Delete<T>(T entity) where T : class;

        /// <summary>
        /// An abstraction of saving elements to the database
        /// process.
        /// </summary>
        Task<bool> SaveAll();

        /// <summary>
        /// An abstraction of retrieving all users from the database
        /// process.
        /// </summary>
        Task<PagedListHelper<UserModel>> GetUsers(UserParamsHelper userParamsHelper);

        /// <summary>
        /// An abstraction of retrieving a single user from the
        /// database process.
        /// </summary>
        Task<UserModel> GetUser(int userId);

        /// <summary>
        /// An abstraction of retrieving a photo from the database
        /// process.
        /// </summary>
        Task<PhotoModel> GetPhoto(int photoId);

        /// <summary>
        /// An abstraction of retrieving the main photo of an user
        /// from the database process.
        /// </summary>
        Task<PhotoModel> GetMainPhotoForUser(int userId);
    }
}