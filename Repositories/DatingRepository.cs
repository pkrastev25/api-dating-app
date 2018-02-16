using System.Linq;
using System.Threading.Tasks;
using api_dating_app.Helpers;
using api_dating_app.models;
using Microsoft.EntityFrameworkCore;

namespace api_dating_app.Data
{
    /// <summary>
    /// Represents a concrete implementation of the <see cref="IDatingRepository"/>.
    /// Adds another level of abstraction between the database and the controller.
    /// </summary>
    public class DatingRepository : IDatingRepository
    {
        private readonly DataContext _context;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// 
        /// <param name="context">Context to the database</param>
        public DatingRepository(DataContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Adds an element to the database. For simplicity, it uses
        /// an abstract class implementation.
        /// </summary>
        /// 
        /// <param name="entity">The element to the added</param>
        /// <typeparam name="T">The type of the element</typeparam>
        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }

        /// <summary>
        /// Removes an element from the database. For simplicity, it uses
        /// an abstract class implementation.
        /// </summary>
        /// 
        /// <param name="entity">The element to be removed</param>
        /// <typeparam name="T">The type of the element</typeparam>
        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }

        /// <summary>
        /// Saves the elements to the database.
        /// </summary>
        /// 
        /// <returns>True if new elements were saved, false otherwise</returns>
        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        /// <summary>
        /// Retrives all users from the database.
        /// </summary>
        /// 
        /// <returns>All users from the database</returns>
        public async Task<PagedListHelper<UserModel>> GetUsers(UserParamsHelper userParams)
        {
            var users = _context.Users.Include(p => p.Photos).OrderByDescending(u => u.LastActive).AsQueryable();
            users = users.Where(u => u.Id != userParams.UserId);
            users = users.Where(u => u.Gender == userParams.Gender);

            if (userParams.MinAge != 18 || userParams.MaxAge != 99)
            {
                users = users.Where(u =>
                    u.DateOfBirth.CalculateAge() >= userParams.MinAge &&
                    u.DateOfBirth.CalculateAge() <= userParams.MaxAge);
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

        /// <summary>
        /// Retrieves a specific user from the database.
        /// </summary>
        /// 
        /// <param name="userId">The photoId of the user</param>
        /// <returns>The specific user</returns>
        public async Task<UserModel> GetUser(int userId)
        {
            var user = await _context.Users.Include(p => p.Photos).FirstOrDefaultAsync(u => u.Id == userId);

            return user;
        }

        /// <summary>
        /// Retrieves a specific photo from the database.
        /// </summary>
        /// 
        /// <param name="photoId">The id of the photo</param>
        /// <returns>The specific photo</returns>
        public Task<PhotoModel> GetPhoto(int photoId)
        {
            var photo = _context.Photos.FirstOrDefaultAsync(p => p.Id == photoId);

            return photo;
        }

        /// <summary>
        /// Retrieves the main photo for a specific user from the 
        /// database.
        /// </summary>
        /// 
        /// <param name="userId">The id of the user</param>
        /// <returns>The main photo for the specific user</returns>
        public Task<PhotoModel> GetMainPhotoForUser(int userId)
        {
            return _context.Photos
                .Where(u => u.UserId == userId)
                .FirstOrDefaultAsync(p => p.IsMain);
        }
    }
}