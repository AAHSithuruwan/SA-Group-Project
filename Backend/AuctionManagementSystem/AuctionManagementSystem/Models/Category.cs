using System.ComponentModel.DataAnnotations;

namespace AuctionManagementSystem.Models
{
    public class Category
    {
        [Key] public int CategoryId { get; set; }

        [Required][MaxLength(50)] public required string Name { get; set; }
    
        //Navigation Property for the Product (1:M)
        public virtual ICollection<Product>? Products { get; set; }
    }
}
