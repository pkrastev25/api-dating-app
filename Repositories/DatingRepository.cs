using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api_dating_app.Helpers;
using api_dating_app.models;
using Microsoft.EntityFrameworkCore;

namespace api_dating_app.Data
{
    /// <summary>
    /// Author: Petar Krastev
    /// </summary>
    public class DatingRepository : IDatingRepository
    {
        private readonly DataContext _context;

        public DatingRepository(DataContext context)
        {
            _context = context;
        }

        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }

        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<PagedListHelper<UserModel>> GetUsers(UserParamsHelper userParams)
        {
            var users = _context.Users.Include(p => p.Photos).OrderByDescending(u => u.LastActive).AsQueryable();
            users = users.Where(u => u.Id != userParams.UserId);
            users = users.Where(u => u.Gender == userParams.Gender);

            if (userParams.Likers)
            {
                var userLikers = await GetUserLikes(userParams.UserId, userParams.Likers);
                users = users.Where(u => userLikers.Any(liker => liker.LikerId == u.Id));
            }

            if (userParams.Likees)
            {
                var userLikees = await GetUserLikes(userParams.UserId, userParams.Likers);
                users = users.Where(u => userLikees.Any(likee => likee.LikeeId == u.Id));
            }

            if (userParams.MinAge != 18 || userParams.MaxAge != 99)
            {
                var min = DateTime.Today.AddYears(-userParams.MaxAge - 1);
                var max = DateTime.Today.AddYears(-userParams.MinAge);

                users = users.Where(u =>
                    u.DateOfBirth >= min && u.DateOfBirth <= max
                );
            }

            if (string.IsNullOrEmpty(userParams.OrderBy))
                return await PagedListHelper<UserModel>.CreateAsync(users, userParams.PageNumber, userParams.PageSize);
            {
                switch (userParams.OrderBy)
                {
                    case "created":
                        users = users.OrderByDescending(u => u.Created);
                        break;
                    default:
                        users = users.OrderByDescending(u => u.LastActive);
                        break;
                }
            }

            return await PagedListHelper<UserModel>.CreateAsync(users, userParams.PageNumber, userParams.PageSize);
        }

        public async Task<UserModel> GetUser(int userId)
        {
            var user = await _context.Users.Include(p => p.Photos).FirstOrDefaultAsync(u => u.Id == userId);

            return user;
        }

        public async Task<PhotoModel> GetPhoto(int photoId)
        {
            var photo = await _context.Photos.FirstOrDefaultAsync(p => p.Id == photoId);

            return photo;
        }

        public async Task<PhotoModel> GetMainPhotoForUser(int userId)
        {
            return await _context.Photos
                .Where(u => u.UserId == userId)
                .FirstOrDefaultAsync(p => p.IsMain);
        }

        public async Task<LikeModel> GetLike(int userId, int recipientId)
        {
            return await _context.Likes.FirstOrDefaultAsync(u => u.LikerId == userId && u.LikeeId == recipientId);
        }

        public async Task<MessageModel> GetMessage(int messageId)
        {
            return await _context.Messages
                .FirstOrDefaultAsync(m => m.Id == messageId);
        }

        public async Task<PagedListHelper<MessageModel>> GetMessagesForUser(MessageParamsHelper messageParamsHelper)
        {
            var messages = _context.Messages
                .Include(u => u.Sender)
                .ThenInclude(p => p.Photos)
                .Include(u => u.Recipient)
                .ThenInclude(p => p.Photos)
                .AsQueryable();

            switch (messageParamsHelper.MessageContainer)
            {
                case "Inbox":
                    messages = messages.Where(u =>
                        u.RecipientId == messageParamsHelper.UserId && u.IsRecipientDeleted == false);
                    break;
                case "Outbox":
                    messages = messages.Where(u =>
                        u.SenderId == messageParamsHelper.UserId && u.IsSenderDeleted == false);
                    break;
                default:
                    messages = messages.Where(u =>
                        u.RecipientId == messageParamsHelper.UserId && u.IsRead == false &&
                        u.IsRecipientDeleted == false);
                    break;
            }

            messages = messages.OrderByDescending(d => d.MessageSentTime);

            return await PagedListHelper<MessageModel>.CreateAsync(messages, messageParamsHelper.PageNumber,
                messageParamsHelper.PageSize);
        }

        public async Task<IEnumerable<MessageModel>> GetMessageThread(int userId, int recipientId)
        {
            var messages = await _context.Messages
                .Include(u => u.Sender)
                .ThenInclude(p => p.Photos)
                .Include(u => u.Recipient)
                .ThenInclude(p => p.Photos)
                .Where(m => m.RecipientId == userId && m.IsRecipientDeleted == false && m.SenderId == recipientId ||
                            m.RecipientId == recipientId && m.IsSenderDeleted == false && m.SenderId == userId)
                .OrderByDescending(m => m.MessageSentTime)
                .ToListAsync();

            return messages;
        }

        private async Task<IEnumerable<LikeModel>> GetUserLikes(int userId, bool likers)
        {
            var user = await _context.Users
                .Include(x => x.Likees)
                .Include(x => x.Likers)
                .FirstOrDefaultAsync(u => u.Id == userId);

            return likers
                ? user.Likees.Where(u => u.LikeeId == userId)
                : user.Likers.Where(u => u.LikerId == userId);
        }
    }
}