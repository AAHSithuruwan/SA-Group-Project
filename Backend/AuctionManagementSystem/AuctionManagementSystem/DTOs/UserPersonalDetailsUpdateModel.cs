﻿namespace AuctionManagementSystem.DTOs
{
    public class UserPersonalDetailsUpdateModel
    {
        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? Address { get; set; }

        public string? PhoneNumber { get; set; }

        public IFormFile? UserImage { get; set; }
    }
}
