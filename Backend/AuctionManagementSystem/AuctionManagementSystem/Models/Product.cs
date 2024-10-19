using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuctionManagementSystem.Models
{
    public class Product
    {
        [Key] public int ProductId { get; set; }

        [Required][MaxLength(100)] public required string Name { get; set; }

        [Required][MaxLength(500)] public required string Description { get; set; }

        public int IsDispatched { get; set; } = 0;

        //Foreign key to the Auction
        [ForeignKey("Auction")] public int AuctionId { get; set; }

        //Navigation Property for the related Auction
        public virtual required Auction Auction { get; set; }

        //Foreign Key to the Category
        [ForeignKey("Category")] public int CategoryId { get; set; }

        //Navigation property for the related category
        public virtual required Category Category { get; set; }
    }
}
