namespace AuctionManagementSystem.DTOs
{
    public class BidDetailsViewModel
    {
        public required int AuctionId { get; set; }

        public required int BidId { get; set; }

        public required DateTime BidDate { get; set; }

        public required float Price { get; set; }

        public required string ShippingName { get; set; }

        public required string ShippingPhoneNumber { get; set; }

        public required string ShippingAddress { get; set; }
    }
}
