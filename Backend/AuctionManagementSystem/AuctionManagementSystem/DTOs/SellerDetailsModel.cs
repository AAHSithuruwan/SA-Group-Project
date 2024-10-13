
namespace AuctionManagementSystem.DTOs
{
    public class SellerDetailsModel
    {
        public required string FirstName { get; set; }

        public required string LastName { get; set; }

        public required string Email { get; set; }

        public required string PhoneNumber { get; set; }

        public required string Address { get; set; }

        public IFormFile? SellerImage { get; set; }
    }
}
