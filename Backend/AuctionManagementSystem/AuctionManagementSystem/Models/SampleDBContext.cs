using Microsoft.EntityFrameworkCore;

namespace AuctionManagementSystem.Models
{
    public class SampleDBContext : DbContext
    {
        public SampleDBContext(DbContextOptions<SampleDBContext> options) : base(options) { }

        public virtual DbSet<User> Users { get; set; }

        public virtual DbSet<Seller> Sellers { get; set; }

        public virtual DbSet<Bid> Bids { get; set; }

        public virtual DbSet<Auction> Auctions { get; set; }

        public virtual DbSet<Product> Products { get; set; }

        public virtual DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(k => k.UserId);
            });

            modelBuilder.Entity<Seller>(entity =>
            {
                entity.HasKey(k => k.SellerId);
            });

            modelBuilder.Entity<Bid>(entity =>
            {
                entity.HasKey(k => k.BidId);
            });

            modelBuilder.Entity<Bid>()
                .HasOne(b => b.Auction)
                .WithMany(a => a.Bids)
                .HasForeignKey(b => b.AuctionId)
                .OnDelete(DeleteBehavior.Restrict); //You cannot delete action if there is bids associated with it

            modelBuilder.Entity<Auction>(entity =>
            {
                entity.HasKey(k => k.AuctionId);
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(k => k.ProductId);
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(k => k.CategoryId);
            });
        }
    }
}
