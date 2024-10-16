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

        public async Task<User> SignUp(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "The user cannot be null.");
            }

            //Check if the email address is available
            var registered_user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
            if (registered_user != null)
            {
                throw new InvalidOperationException("Email Address is already registered with another account");
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

        public async Task<User> SignIn(User signInDetails)
        {
            if(signInDetails == null)
            {
                throw new ArgumentNullException(nameof(signInDetails), "Sign In Details cannot be null");
            }

            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == signInDetails.Email);

            if (user == null)
            {
                throw new UnauthorizedAccessException("Invalid Email Address");
            }

            var passwordHasher = new PasswordHasher<User>();
            var verificationResult = passwordHasher.VerifyHashedPassword(user, user.Password, signInDetails.Password);

            if (verificationResult == PasswordVerificationResult.Success)
            {
                return user;
            }
            else
            {
                throw new UnauthorizedAccessException("Invalid Password");
            }
        }

        public async Task<User> GetUser(int userId)
        {
            var user = await _dbContext.Users.FindAsync(userId);

            if (user == null)
            {
                throw new KeyNotFoundException("User not found");
            }
            return user;
        }

        public async Task<User> UpdateUserPersonalDetails(UserPersonalDetailsUpdateModel userPersonalDetailsUpdateModel, int userId)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.UserId == userId);

            if (user == null)
            {
                throw new KeyNotFoundException("User Not Found");
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

        public async Task<User> UpdateUserEmail(User userDetails, int userId)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.UserId == userId);

            if (user == null)
            {
                throw new KeyNotFoundException("User Not Found");
            }

            if(user.Email == userDetails.Email)
            {
                throw new InvalidOperationException("Please provide a new email");
            }

            var registeredUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == userDetails.Email);

            if (registeredUser != null)
            {
                throw new InvalidOperationException("Email Address is already registered with another account");
            }

            var passwordHasher = new PasswordHasher<User>();
            var verificationResult = passwordHasher.VerifyHashedPassword(user, user.Password, userDetails.Password);

            if (verificationResult == PasswordVerificationResult.Failed)
            {
                throw new UnauthorizedAccessException("Incorrect Password");
            }

            user.Email = userDetails.Email;

            await _dbContext.SaveChangesAsync();

            return user;
        }

        public async Task<User> UpdateUserPassword(UserPasswordUpdateModel userPasswordUpdateModel, int userId)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.UserId == userId);

            if (user == null)
            {
                throw new KeyNotFoundException("User Not Found");
            }

            var passwordHasher = new PasswordHasher<User>();
            var verificationResult = passwordHasher.VerifyHashedPassword(user, user.Password, userPasswordUpdateModel.OldPassword);

            if (verificationResult == PasswordVerificationResult.Failed)
            {
                throw new UnauthorizedAccessException("Incorrect Current Password");
            }

            user.Password = passwordHasher.HashPassword(user, userPasswordUpdateModel.NewPassword);

            await _dbContext.SaveChangesAsync();

            return user;
        }

        public async Task DeleteUser(int userId)
        {
            var user = await _dbContext.Users
                .Include(u => u.Seller)
                .FirstOrDefaultAsync(u => u.UserId == userId);

            if (user == null)
            {
                throw new KeyNotFoundException("User Not Found");
            }

            if (user.Seller != null)
            {
                var auctions = await _dbContext.Auctions
                    .Where(a => a.SellerId == user.Seller.SellerId)
                    .ToListAsync();

                if (auctions.Count > 0)
                {
                    throw new InvalidOperationException("User cannot be deleted. Because there are auctions associated with this seller account");
                }
            }

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "images", "UserImages", user.UserId.ToString() + ".png");

            _dbContext.Users.Remove(user);
            await _dbContext.SaveChangesAsync();

            System.IO.File.Delete(filePath);
        }
    }
}
