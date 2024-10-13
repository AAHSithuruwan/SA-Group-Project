using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuctionManagementSystem.Models
{
    public class Auction
    {
        [Key] public int AuctionId { get; set; }

        [Required] public required float StartingPrice { get; set; }

        [Required] public required float BidIncrement { get; set; }

        [Required] public required DateTime StartingDate { get; set; }

        [Required] public required DateTime EndDate { get; set; }

        //Foreign key to the Seller 
        [ForeignKey("Seller")] public int SellerId { get; set; }
    
        //Navigation property to the related Seller
        public virtual required Seller Seller { get; set; }
    
        //Navigation property to the Bid (1:M)
        public virtual ICollection<Bid>? Bids { get; set; }

        //Navigation property to the Product (1:1)
        public virtual Product? Product { get; set; }
    }
}
