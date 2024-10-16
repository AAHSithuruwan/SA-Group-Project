using AuctionManagementSystem.Data;
using AuctionManagementSystem.DTOs;
using AuctionManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace AuctionManagementSystem.Services
{
    public class SellerService
    {
        private readonly ApplicationDBContext _dbContext;
        public SellerService(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<(bool, bool)> CreateSeller(SellerDetailsModel sellerDetailsModel, int userId)
        {
            User? user = await _dbContext.Users.FirstOrDefaultAsync(u => u.UserId == userId);

            if (user == null)
            {
                return (false, false);
            }

            if (await _dbContext.Sellers.FirstOrDefaultAsync(s => s.UserId == userId) != null)
            {
                return (true, false);
            }

            Seller seller = new Seller()
            {
                FirstName = sellerDetailsModel.FirstName,
                LastName = sellerDetailsModel.LastName,
                Email = sellerDetailsModel.Email,
                PhoneNumber = sellerDetailsModel.PhoneNumber,
                Address = sellerDetailsModel.Address,
                UserId = userId,
                User = user
            };

            await _dbContext.Sellers.AddAsync(seller);
            await _dbContext.SaveChangesAsync();

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "Images", "SellerImages", seller.SellerId.ToString() + ".png");

            if (sellerDetailsModel.SellerImage != null && sellerDetailsModel.SellerImage.Length > 0)
            {
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    sellerDetailsModel.SellerImage.CopyTo(stream);
                }
            }
            else
            {
                var defaultImagePath = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "images", "SellerImages", "default.png");

                System.IO.File.Copy(defaultImagePath, filePath);
            }

            return (true, true);
        }

        public async Task<Seller?> GetSeller(int userId)
        {
            Seller? seller = await _dbContext.Sellers.FirstOrDefaultAsync(s => s.UserId == userId);

            if (seller == null)
            {
                return null;
            }

            return seller;
        }

        public async Task<Seller?> UpdateSeller(SellerDetailsModel sellerDetailsModel, int userId)
        {
            Seller? seller = await _dbContext.Sellers.FirstOrDefaultAsync(s => s.UserId == userId);

            if (seller == null)
            {
                return null;
            }

            seller.FirstName = sellerDetailsModel.FirstName;
            seller.LastName = sellerDetailsModel.LastName;
            seller.Email = sellerDetailsModel.Email;
            seller.PhoneNumber = sellerDetailsModel.PhoneNumber;
            seller.Address = sellerDetailsModel.Address;

            await _dbContext.SaveChangesAsync();

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "images", "SellerImages", seller.SellerId.ToString() + ".png");

            if (sellerDetailsModel.SellerImage != null && sellerDetailsModel.SellerImage.Length > 0)
            {
                System.IO.File.Delete(filePath);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    sellerDetailsModel.SellerImage.CopyTo(stream);
                }
            }

            return seller;
        }

        public async Task<(bool, bool)> DeleteSeller(int userId)
        {
            Seller? seller = await _dbContext.Sellers
                .Include(s => s.Auctions)
                .FirstOrDefaultAsync(s => s.UserId == userId);

            if (seller == null)
            {
                return (false, false);
            }

            if (seller.Auctions != null && seller.Auctions.Count != 0)
            {
                return (true, false);
            }

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "images", "SellerImages", seller.SellerId.ToString() + ".png");

            _dbContext.Sellers.Remove(seller);
            await _dbContext.SaveChangesAsync();

            System.IO.File.Delete(filePath);

            return (true, true);
        }
    }
}
