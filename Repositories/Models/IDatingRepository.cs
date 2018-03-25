using System.Collections.Generic;
using System.Threading.Tasks;
using api_dating_app.Helpers;
using api_dating_app.models;

namespace api_dating_app.Data
{
    /// <summary>
    /// Author: Petar Krastev
    /// </summary>
    public interface IDatingRepository
    {
        void Add<T>(T entity) where T : class;

        void Delete<T>(T entity) where T : class;

        Task<bool> SaveAll();

        Task<PagedListHelper<UserModel>> GetUsers(UserParamsHelper userParamsHelper);

        Task<UserModel> GetUser(int userId);

        Task<PhotoModel> GetPhoto(int photoId);

        Task<PhotoModel> GetMainPhotoForUser(int userId);

        Task<LikeModel> GetLike(int userId, int recipientId);

        Task<MessageModel> GetMessage(int messageId);

        Task<PagedListHelper<MessageModel>> GetMessagesForUser(MessageParamsHelper messageParamsHelper);

        Task<IEnumerable<MessageModel>> GetMessageThread(int userId, int recipientId);
    }
}