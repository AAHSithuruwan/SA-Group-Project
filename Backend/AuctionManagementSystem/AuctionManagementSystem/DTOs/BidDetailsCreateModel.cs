using System.ComponentModel.DataAnnotations;

namespace AuctionManagementSystem.DTOs
{
    public class BidDetailsCreateModel
    {
        public required int AuctionId { get; set; }

        public required float Price { get; set; }

        public required string ShippingName { get; set; }

        public required string ShippingPhoneNumber { get; set; }

        public required string ShippingAddress { get; set; }
    }
}
