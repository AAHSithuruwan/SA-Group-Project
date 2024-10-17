using AuctionManagementSystem.Data;
using AuctionManagementSystem.DTOs;
using AuctionManagementSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AuctionManagementSystem.Services
{
    public class UserService
    {
        private readonly ApplicationDBContext _dbContext;
        public UserService(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<User?> SignUp(User user)
        {
            //Check if the email address is available
            var registered_user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
            if (registered_user != null)
            {
                return null;
            }

            // Hash the password
            var passwordHasher = new PasswordHasher<User>();
            user.Password = passwordHasher.HashPassword(user, user.Password);

            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "images", "UserImages", user.UserId.ToString() + ".png");

            var defaultImagePath = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "images", "UserImages", "default.png");

            System.IO.File.Copy(defaultImagePath, filePath);

            return user;
        }

        public async Task<User?> SignIn(User signInDetails)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == signInDetails.Email);

            if (user == null)
            {
                return null;
            }

            var passwordHasher = new PasswordHasher<User>();
            var verificationResult = passwordHasher.VerifyHashedPassword(user, user.Password, signInDetails.Password);

            if (verificationResult == PasswordVerificationResult.Success)
            {
                return user;
            }
            else
            {
                return null;
            }
        }

        public async Task<User?> GetUser(int userId)
        {
            var user = await _dbContext.Users.FindAsync(userId);

            if (user == null)
            {
                return null;
            }
            return user;
        }

        public async Task<User?> UpdateUserPersonalDetails(UserPersonalDetailsUpdateModel userPersonalDetailsUpdateModel, int userId)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.UserId == userId);

            if (user == null)
            {
                return null;
            }

            if (userPersonalDetailsUpdateModel.FirstName != null)
            {
                user.FirstName = userPersonalDetailsUpdateModel.FirstName;
            }

            if (userPersonalDetailsUpdateModel.LastName != null)
            {
                user.LastName = userPersonalDetailsUpdateModel.LastName;
            }

            if (userPersonalDetailsUpdateModel.Address != null)
            {
                user.Address = userPersonalDetailsUpdateModel.Address;
            }

            if (userPersonalDetailsUpdateModel.PhoneNumber != null)
            {
                user.PhoneNumber = userPersonalDetailsUpdateModel.PhoneNumber;
            }

            await _dbContext.SaveChangesAsync();

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "images", "UserImages", user.UserId.ToString() + ".png");

            if (userPersonalDetailsUpdateModel.UserImage != null && userPersonalDetailsUpdateModel.UserImage.Length > 0)
            {
                System.IO.File.Delete(filePath);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    userPersonalDetailsUpdateModel.UserImage.CopyTo(stream);
                }
            }

            return user;
        }

        public async Task<(User?, bool, string?)> UpdateUserEmail(User userDetails, int userId)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.UserId == userId);

            if (user == null)
            {
                return (null, false, null);
            }

            if(user.Email == userDetails.Email)
            {
                return (user, false, "Please provide a new email address");
            }

            var registeredUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == userDetails.Email);

            if (registeredUser != null)
            {
                return (user, false, "Email Address is already registered with another account");
            }

            var passwordHasher = new PasswordHasher<User>();
            var verificationResult = passwordHasher.VerifyHashedPassword(user, user.Password, userDetails.Password);

            if (verificationResult == PasswordVerificationResult.Failed)
            {
                return (user, false, null);
            }

            user.Email = userDetails.Email;

            await _dbContext.SaveChangesAsync();

            return (user, true, null);
        }

        public async Task<(User?, bool)> UpdateUserPassword(UserPasswordUpdateModel userPasswordUpdateModel, int userId)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.UserId == userId);

            if (user == null)
            {
                return (null, false);
            }

            var passwordHasher = new PasswordHasher<User>();
            var verificationResult = passwordHasher.VerifyHashedPassword(user, user.Password, userPasswordUpdateModel.OldPassword);

            if (verificationResult == PasswordVerificationResult.Failed)
            {
                return (user, false);
            }

            user.Password = passwordHasher.HashPassword(user, userPasswordUpdateModel.NewPassword);

            await _dbContext.SaveChangesAsync();

            return (user, true);
        }

        public async Task<(bool,bool)> DeleteUser(int userId)
        {
            var user = await _dbContext.Users
                .Include(u => u.Seller)
                .FirstOrDefaultAsync(u => u.UserId == userId);

            if (user == null)
            {
                return (false, false);
            }

            if (user.Seller != null)
            {
                var auctions = await _dbContext.Auctions
                    .Where(a => a.SellerId == user.Seller.SellerId)
                    .ToListAsync();

                if (auctions.Count > 0)
                {
                    return (true, false);
                }
            }

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "images", "UserImages", user.UserId.ToString() + ".png");

            _dbContext.Users.Remove(user);
            await _dbContext.SaveChangesAsync();

            System.IO.File.Delete(filePath);

            return (true, true);
        }
    }
}
