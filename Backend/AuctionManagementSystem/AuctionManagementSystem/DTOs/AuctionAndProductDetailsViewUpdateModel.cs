namespace AuctionManagementSystem.DTOs
{
    public class AuctionAndProductDetailsViewUpdateModel
    {
        public required int AuctionId { get; set; }

        public int ProductId { get; set; }

        public required string ProductName { get; set; }

        public required string ProductDescription { get; set; }

        public required int CategoryId { get; set; }

        public required float StartingPrice { get; set; }

        public required DateTime StartingDate { get; set; }

        public required DateTime EndDate { get; set; }
    }
}
