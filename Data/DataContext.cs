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
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        /// <summary>
        /// Creates a table in the DB with values.
        /// </summary>
        public DbSet<Value> Values { get; set; }
    }
}