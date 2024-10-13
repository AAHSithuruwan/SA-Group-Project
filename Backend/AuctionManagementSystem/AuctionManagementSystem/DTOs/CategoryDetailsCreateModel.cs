namespace AuctionManagementSystem.DTOs
{
    public class CategoryDetailsCreateModel
    {
        public required string Name { get; set; }

        public required IFormFile CategoryImage { get; set; }
    }
}
