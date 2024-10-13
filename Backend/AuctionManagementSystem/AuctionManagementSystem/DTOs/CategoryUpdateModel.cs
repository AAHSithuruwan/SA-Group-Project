using System.ComponentModel.DataAnnotations;

namespace AuctionManagementSystem.DTOs
{
    public class CategoryUpdateModel
    {
        public required int CategoryId { get; set; }

        public required string Name { get; set; }
    }
}
