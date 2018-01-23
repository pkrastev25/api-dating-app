using api_dating_app.models;
using Microsoft.EntityFrameworkCore;

namespace api_dating_app.Data
{
    /// <summary>
    /// A DbContext instance represents a session with the database and can be used to query and save
    /// instances of your entities. DbContext is a combination of the Unit Of Work and Repository patterns.
    /// </summary>
    public class DataContext : DbContext
    {
        /// <summary>
        /// Constructor. All incoming params are injected via dependency
        /// injection.
        /// </summary>
        /// <param name="options">Additional information needed by EF</param>
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        /// <summary>
        /// Creates and provides a reference to the table which is
        /// populated by <see cref="Value"/> entries in the DB.
        /// </summary>
        public DbSet<Value> Values { get; set; }

        /// <summary>
        /// Creates and provides a reference to the table which is
        /// populated by <see cref="User"/> entries in the DB.
        /// </summary>
        public DbSet<User> Users { get; set; }
    }
}