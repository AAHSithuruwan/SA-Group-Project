namespace AuctionManagementSystem.DTOs
{
    public class AuctionAndProductDetailsViewModel
    {
        public required int AuctionId { get; set; }

        public required int ProductId { get; set; }

        public required int SellerId { get; set; }

        public required string SellerFirstName { get; set; }

        public required string SellerLastName { get; set; }

        public required string SellerAddress { get; set; }

        public required string SellerPhoneNumber { get; set; }

        public required string SellerEmail { get; set; }

        public required string ProductName { get; set; }

        public required string ProductDescription { get; set; }

        public required int IsDispatched { get; set; }

        public required int CategoryId { get; set; }

        public required string CategoryName { get; set; }

        public required float StartingPrice { get; set; }

        public required float NextBidPrice { get; set; }

        public required float BidIncrement { get; set; }

        public required DateTime StartingDate { get; set; }

        public required DateTime EndDate { get; set; }

        public float? HighestBidPrice { get; set; }

        public string? HighestBidShippingName { get; set; }

        public string? HighestBidShippingAddress { get; set; }

        public string? HighestBidShippingPhoneNumber { get; set; }
    }
}
