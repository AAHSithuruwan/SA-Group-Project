﻿namespace AuctionManagementSystem.DTOs
{
    public class UserPasswordUpdateModel
    {
        public required string NewPassword { get; set; }

        public required string OldPassword { get; set; }
    }
}
