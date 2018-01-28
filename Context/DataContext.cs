using api_dating_app.models;
using Microsoft.EntityFrameworkCore;

namespace api_dating_app.Data
{
    /// <summary>
    /// Context used to provide access to the database.
    /// </summary>
    public class DataContext : DbContext
    {
        /// <summary>
        /// Constructor. All incoming params are injected via dependency
        /// injection.
        /// </summary>
        /// 
        /// <param name="options">Additional information needed by EF</param>
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        /// <summary>
        /// Creates and provides a reference to the table which is
        /// populated by <see cref="ValueModel"/> entries in the DB.
        /// </summary>
        public DbSet<ValueModel> Values { get; set; }

        
        /// <summary>
        /// Creates and provides a reference to the table which is
        /// populated by <see cref="UserModel"/> entries in the DB.
        /// </summary>
        public DbSet<UserModel> Users { get; set; }

        /// <summary>
        /// Creates and provides a reference to the table which is
        /// populated by <see cref="PhotoModel"/> entries in the DB.
        /// </summary>
        public DbSet<PhotoModel> Photos { get; set; }
       
    }
}