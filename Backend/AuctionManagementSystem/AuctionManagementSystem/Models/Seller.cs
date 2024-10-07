using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuctionManagementSystem.Models
{
    public class Seller
    {
        [Key] public int SellerId { get; set; }

        [Required][MaxLength(50)] public required string FirstName { get; set; }

        [Required][MaxLength(50)] public required string LastName { get; set; }

        [Required][MaxLength(50)] public required string Email { get; set; }

        [Required][MaxLength(15)] public required string PhoneNumber { get; set; }

        [Required][MaxLength(100)] public required string Address { get; set; }

        //Foreign key to User
        [ForeignKey("User")] public int UserId {  get; set; }
        
        //Navigation property to User
        public virtual required User User { get; set; }
    
        //Navigation property to Auction{!:M)
        public virtual ICollection<Auction>? Auctions { get; set; }
    }
}
