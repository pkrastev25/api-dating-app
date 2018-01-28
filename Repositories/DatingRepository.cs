using System.Collections.Generic;
using System.Threading.Tasks;
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
        public async Task<IEnumerable<UserModel>> GetUsers()
        {
            var users = await _context.Users.Include(p => p.Photos).ToListAsync();

            return users;
        }

        /// <summary>
        /// Retrieves a specific user from the database.
        /// </summary>
        /// 
        /// <param name="id">The id of the user</param>
        /// <returns>The specific user///</returns>
        public async Task<UserModel> GetUser(int id)
        {
            var user = await _context.Users.Include(p => p.Photos).FirstOrDefaultAsync(u => u.Id == id);

            return user;
        }
    }
}