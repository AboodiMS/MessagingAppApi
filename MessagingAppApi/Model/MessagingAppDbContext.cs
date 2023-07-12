using MessagingAppApi.Model.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace MessagingAppApi.Model
{
    public class MessagingAppDbContext: DbContext
    {
       public DbSet<Message> Messages { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<RoomMembership> RoomMemberships { get; set; }
        public DbSet<User> Users { get; set; }

        public MessagingAppDbContext(DbContextOptions<MessagingAppDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RoomMembership>()
                        .HasOne(b => b.Room)
                        .WithMany(a => a.RoomMemberships)
                        .HasForeignKey(b => b.RoomId);

            modelBuilder.Entity<RoomMembership>()
                        .HasOne(b => b.User)
                        .WithMany(a => a.RoomMemberships)
                        .HasForeignKey(b => b.UserId);

            modelBuilder.Entity<Message>()
                        .HasOne(b => b.Room)
                        .WithMany(a => a.Messages)
                        .HasForeignKey(b => b.RoomId);
        }
    }
}
