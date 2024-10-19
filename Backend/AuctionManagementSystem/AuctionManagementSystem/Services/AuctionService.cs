using AuctionManagementSystem.Data;
using AuctionManagementSystem.DTOs;
using AuctionManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace AuctionManagementSystem.Services
{
    public class AuctionService
    {
        //Auction Service is where the both Auction and Product models are handled

        private readonly ApplicationDBContext _dbContext;
        public AuctionService(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<(bool, bool, bool)> CreateAuction(AuctionAndProductDetailsCreateModel auctionAndProductDetailsCreateModel, int userId)
        {
            Seller? seller = await _dbContext.Sellers.FirstOrDefaultAsync(s => s.UserId == userId);

            if (seller == null)
            {
                return (false, false, false);
            }

            Category? category = await _dbContext.Categories.FirstOrDefaultAsync(c => c.CategoryId == auctionAndProductDetailsCreateModel.CategoryId);

            if (category == null)
            {
                return (true, false, false);
            }

            Auction auction = new Auction()
            {
                StartingPrice = auctionAndProductDetailsCreateModel.StartingPrice,
                BidIncrement = auctionAndProductDetailsCreateModel.BidIncrement,
                StartingDate = auctionAndProductDetailsCreateModel.StartingDate,
                EndDate = auctionAndProductDetailsCreateModel.EndDate,
                SellerId = seller.SellerId,
                Seller = seller
            };

            await _dbContext.Auctions.AddAsync(auction);
            await _dbContext.SaveChangesAsync();

            Product product = new Product()
            {
                Name = auctionAndProductDetailsCreateModel.ProductName,
                Description = auctionAndProductDetailsCreateModel.ProductDescription,
                AuctionId = auction.AuctionId,
                Auction = auction,
                CategoryId = auctionAndProductDetailsCreateModel.CategoryId,
                Category = category
            };

            await _dbContext.Products.AddAsync(product);
            await _dbContext.SaveChangesAsync();

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "images", "ProductImages", product.ProductId.ToString() + ".png");

            if (auctionAndProductDetailsCreateModel.ProductImage != null && auctionAndProductDetailsCreateModel.ProductImage.Length > 0)
            {
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    auctionAndProductDetailsCreateModel.ProductImage.CopyTo(stream);
                }
            }

            return (true, true, true);
        }

        public async Task<List<AuctionAndProductDetailsViewModel>?> GetSellerAuctions(int userId)
        {
            Seller? seller = await _dbContext.Sellers
                .Include(s => s.Auctions)
                .FirstOrDefaultAsync(s => s.UserId == userId);

            if (seller == null)
            {
                return null;
            }

            List<AuctionAndProductDetailsViewModel> auctionAndProductDetailsViewModels = new List<AuctionAndProductDetailsViewModel>();

            if (seller.Auctions == null || seller.Auctions.Count == 0)
            {
                return auctionAndProductDetailsViewModels;
            }

            foreach (var auction in seller.Auctions)
            {
                Product? product = await _dbContext.Products.FirstOrDefaultAsync(p => p.AuctionId == auction.AuctionId);

                if (product != null)
                {
                    Bid? lastBid = await _dbContext.Bids
                    .Where(b => b.AuctionId == auction.AuctionId)
                    .OrderByDescending(b => b.BidDate)
                    .FirstOrDefaultAsync();

                    float nextBidPrice = auction.StartingPrice;
                    float? highestBidPrice = null;
                    string? highestBidShippingName = null;
                    string? highestBidShippingAddress = null;
                    string? highestBidShippingPhoneNumber = null;

                    if (lastBid != null)
                    {
                        nextBidPrice = lastBid.Price + auction.BidIncrement;
                        highestBidPrice = lastBid.Price;
                        highestBidShippingName = lastBid.ShippingName;
                        highestBidShippingAddress = lastBid.ShippingAddress;
                        highestBidShippingPhoneNumber = lastBid.ShippingPhoneNumber;
                    }

                    AuctionAndProductDetailsViewModel auctionAndProductDetailsViewModel = new AuctionAndProductDetailsViewModel()
                    {
                        ProductId = product.ProductId,
                        AuctionId = auction.AuctionId,
                        ProductName = product.Name,
                        IsDispatched = product.IsDispatched,
                        CategoryId = product.CategoryId,
                        ProductDescription = product.Description,
                        StartingPrice = auction.StartingPrice,
                        NextBidPrice = nextBidPrice,
                        BidIncrement = auction.BidIncrement,
                        StartingDate = auction.StartingDate,
                        EndDate = auction.EndDate,
                        HighestBidPrice = highestBidPrice,
                        HighestBidShippingName = highestBidShippingName,
                        HighestBidShippingAddress = highestBidShippingAddress,
                        HighestBidShippingPhoneNumber = highestBidShippingPhoneNumber,
                    };

                    auctionAndProductDetailsViewModels.Add(auctionAndProductDetailsViewModel);
                } 
            }

            return auctionAndProductDetailsViewModels;
        }

        public async Task<List<AuctionAndProductDetailsViewModel>> GetAllAuctions()
        {
            List<AuctionAndProductDetailsViewModel> auctionAndProductDetailsViewModels = new List<AuctionAndProductDetailsViewModel>();

            List<Auction> auctions = await _dbContext.Auctions.ToListAsync();

            if (auctions.Count == 0)
            {
                return auctionAndProductDetailsViewModels;
            }

            foreach (Auction auction in auctions)
            {
                Product? product = await _dbContext.Products.FirstOrDefaultAsync(p => p.AuctionId == auction.AuctionId);

                if (product != null)
                {
                    Bid? lastBid = await _dbContext.Bids
                    .Where(b => b.AuctionId == auction.AuctionId)
                    .OrderByDescending(b => b.BidDate)
                    .FirstOrDefaultAsync();

                    float nextBidPrice = auction.StartingPrice;
                    float? highestBidPrice = null;
                    string? highestBidShippingName = null;
                    string? highestBidShippingAddress = null;
                    string? highestBidShippingPhoneNumber = null;

                    if (lastBid != null)
                    {
                        nextBidPrice = lastBid.Price + auction.BidIncrement;
                        highestBidPrice = lastBid.Price;
                        highestBidShippingName = lastBid.ShippingName;
                        highestBidShippingAddress = lastBid.ShippingAddress;
                        highestBidShippingPhoneNumber = lastBid.ShippingPhoneNumber;
                    }

                    AuctionAndProductDetailsViewModel auctionAndProductDetailsViewModel = new AuctionAndProductDetailsViewModel()
                    {
                        ProductId = product.ProductId,
                        AuctionId = auction.AuctionId,
                        ProductName = product.Name,
                        IsDispatched = product.IsDispatched,
                        CategoryId = product.CategoryId,
                        ProductDescription = product.Description,
                        StartingPrice = auction.StartingPrice,
                        NextBidPrice = nextBidPrice,
                        BidIncrement = auction.BidIncrement,
                        StartingDate = auction.StartingDate,
                        EndDate = auction.EndDate,
                        HighestBidPrice = highestBidPrice,
                        HighestBidShippingName = highestBidShippingName,
                        HighestBidShippingAddress = highestBidShippingAddress,
                        HighestBidShippingPhoneNumber = highestBidShippingPhoneNumber
                    };

                    auctionAndProductDetailsViewModels.Add(auctionAndProductDetailsViewModel);
                }
            }

            return auctionAndProductDetailsViewModels;
        }

        public async Task<(bool, bool, AuctionAndProductDetailsViewModel?)> GetAuctionById(int auctionId)
        {
            Auction? auction = await _dbContext.Auctions
                .Include(a => a.Product)
                .FirstOrDefaultAsync(a => a.AuctionId == auctionId);

            if (auction == null)
            {
                return (false, false, null);
            }

            if (auction.Product == null)
            {
                return (true, false, null);
            }

            Bid? lastBid = await _dbContext.Bids
                    .Where(b => b.AuctionId == auction.AuctionId)
                    .OrderByDescending(b => b.BidDate)
                    .FirstOrDefaultAsync();

            float nextBidPrice = auction.StartingPrice;
            float? highestBidPrice = null;
            string? highestBidShippingName = null;
            string? highestBidShippingAddress = null;
            string? highestBidShippingPhoneNumber = null;

            if (lastBid != null)
            {
                nextBidPrice = lastBid.Price + auction.BidIncrement;
                highestBidPrice = lastBid.Price;
                highestBidShippingName = lastBid.ShippingName;
                highestBidShippingAddress = lastBid.ShippingAddress;
                highestBidShippingPhoneNumber = lastBid.ShippingPhoneNumber;
            }

            AuctionAndProductDetailsViewModel auctionAndProductDetailsViewModel = new AuctionAndProductDetailsViewModel()
            {
                ProductId = auction.Product.ProductId,
                AuctionId = auction.AuctionId,
                ProductName = auction.Product.Name,
                IsDispatched = auction.Product.IsDispatched,
                CategoryId = auction.Product.CategoryId,
                ProductDescription = auction.Product.Description,
                StartingPrice = auction.StartingPrice,
                NextBidPrice = nextBidPrice,
                BidIncrement = auction.BidIncrement,
                StartingDate = auction.StartingDate,
                EndDate = auction.EndDate,
                HighestBidPrice = highestBidPrice,
                HighestBidShippingName = highestBidShippingName,
                HighestBidShippingAddress = highestBidShippingAddress,
                HighestBidShippingPhoneNumber = highestBidShippingPhoneNumber
            };

            return (true, true, auctionAndProductDetailsViewModel);
        }

        public async Task<(bool, bool, bool)> DispatchProduct(int auctionId)
        {
            Auction? auction = await _dbContext.Auctions
                .Include(a => a.Product)
                .FirstOrDefaultAsync(a => a.AuctionId == auctionId);

            if (auction == null)
            {
                return (false, false, false);
            }

            if (auction.Product == null)
            {
                return (true, false, false);
            }

            if (auction.EndDate > DateTime.Now)
            {
                return (true, true, false);
            }

            auction.Product.IsDispatched = 1;
            await _dbContext.SaveChangesAsync();

            return (true, true, true);
        }

        public async Task<(bool, bool, bool)> UpdateAuction(AuctionAndProductDetailsUpdateModel auctionAndProductDetailsUpdateModel)
        {
            Auction? auction = await _dbContext.Auctions
                .Include(a => a.Product)
                .FirstOrDefaultAsync(a => a.AuctionId == auctionAndProductDetailsUpdateModel.AuctionId);

            if (auction == null)
            {
                return (false, false,false);
            }

            if (auction.Product == null)
            {
                return (true, false, false);
            }

            if(auction.Product.IsDispatched == 1)
            {
                return (true, true, false);
            }

            auction.StartingPrice = auctionAndProductDetailsUpdateModel.StartingPrice;
            auction.BidIncrement = auctionAndProductDetailsUpdateModel.BidIncrement;
            auction.StartingDate = auctionAndProductDetailsUpdateModel.StartingDate;
            auction.EndDate = auctionAndProductDetailsUpdateModel.EndDate;

            await _dbContext.SaveChangesAsync();

            auction.Product.Name = auctionAndProductDetailsUpdateModel.ProductName;
            auction.Product.CategoryId = auctionAndProductDetailsUpdateModel.CategoryId;
            auction.Product.Description = auctionAndProductDetailsUpdateModel.ProductDescription;

            await _dbContext.SaveChangesAsync();

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "images", "ProductImages", auction.Product.ProductId.ToString() + ".png");

            if (auctionAndProductDetailsUpdateModel.ProductImage != null && auctionAndProductDetailsUpdateModel.ProductImage.Length > 0)
            {
                System.IO.File.Delete(filePath);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    auctionAndProductDetailsUpdateModel.ProductImage.CopyTo(stream);
                }
            }

            return (true, true, true);
        }

        public async Task<(bool, bool, bool)> DeleteAuction(int auctionId)
        {
            Auction? auction = await _dbContext.Auctions
                .Include(a => a.Bids)
                .FirstOrDefaultAsync(a => a.AuctionId == auctionId);

            if (auction == null)
            {
                return (false, false, false);
            }

            Product? product = await _dbContext.Products.FirstOrDefaultAsync(p => p.AuctionId == auctionId);

            if (product == null)
            {
                return (true, false, false);
            }

            if (auction.Bids != null && auction.Bids.Count > 0)
            {
                return (true, true, false);
            }

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "images", "ProductImages", product.ProductId + ".png");

            _dbContext.Auctions.Remove(auction);
            await _dbContext.SaveChangesAsync();

            System.IO.File.Delete(filePath);

            return (true, true, true);
        }
    }
}
