using System.ComponentModel.DataAnnotations;

namespace AuctionManagementSystem.Models
{
    public class User
    {
        [Key] public int UserId { get; set; }

        [Required][MaxLength(50)] public required string Email { get; set; }

        [Required][MaxLength(100)] public required string Password { get; set; }

        [MaxLength(50)] public string? FirstName { get;  set;}

        [MaxLength(50)] public string? LastName { get; set; }

        [MaxLength(300)] public string? Address { get; set; }

        [MaxLength(12)] public string? PhoneNumber { get; set; }

    }
}
