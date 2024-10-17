using System.ComponentModel.DataAnnotations;

namespace AuctionManagementSystem.DTOs
{
    public class CategoryDetailsUpdateModel
    {
        public required int CategoryId { get; set; }

        public required string Name { get; set; }

        public IFormFile? CategoryImage { get; set; }
    }
}
