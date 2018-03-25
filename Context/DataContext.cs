using api_dating_app.models;
using Microsoft.EntityFrameworkCore;

namespace api_dating_app.Data
{
    /// <summary>
    /// Author: Petar Krastev
    /// </summary>
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<UserModel> Users { get; set; }

        public DbSet<PhotoModel> Photos { get; set; }

        public DbSet<LikeModel> Likes { get; set; }

        public DbSet<MessageModel> Messages { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<LikeModel>()
                .HasKey(k => new {k.LikerId, k.LikeeId});

            builder.Entity<LikeModel>()
                .HasOne(u => u.Likee)
                .WithMany(u => u.Likers)
                .HasForeignKey(u => u.LikerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<LikeModel>()
                .HasOne(u => u.Liker)
                .WithMany(u => u.Likees)
                .HasForeignKey(u => u.LikeeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<MessageModel>()
                .HasOne(u => u.Sender)
                .WithMany(m => m.MessagesSend)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<MessageModel>()
                .HasOne(u => u.Recipient)
                .WithMany(m => m.MessagesRecieved)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}