using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuctionManagementSystem.Models
{
    public class Bid
    {
        [Key] public int BidId { get; set; }

        [Required] public required float Price { get; set; }

        [Required][MaxLength(100)] public required string ShippingName  { get; set;}

        [Required][MaxLength(100)] public required string ShippingPhoneNumber { get; set; }

        [Required][MaxLength(100)] public required string ShippingAddress { get; set; }

        //Foreign key to User
        [ForeignKey("User")] public int UserId { get; set; }    

        //Navigation property for related User
        public virtual required User User { get; set; }

        //Foreign key to the Auction
        [ForeignKey("Auction")] public int AuctionId { get; set; }

        //Navigation Property for related Auction
        public virtual required Auction Auction { get; set; }
    }
}
