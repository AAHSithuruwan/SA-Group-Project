using Microsoft.EntityFrameworkCore;

namespace AuctionManagementSystem.Models
{
    public class SampleDBContext : DbContext
    {
        public SampleDBContext(DbContextOptions<SampleDBContext> options) : base(options) { }

        public virtual DbSet<User> User { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(k => k.UserId);
            });
        }
    }
}
