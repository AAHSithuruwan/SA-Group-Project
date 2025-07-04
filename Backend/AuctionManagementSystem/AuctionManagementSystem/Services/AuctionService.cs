﻿using AuctionManagementSystem.Data;
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

        public async Task<List<AuctionAndProductDetailsViewModel>?> GetAuctionsBySeller(int userId)
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
                Product? product = await _dbContext.Products
                    .Include(p => p.Category)
                    .FirstOrDefaultAsync(p => p.AuctionId == auction.AuctionId);

                if (product != null)
                {
                    Bid? lastBid = await _dbContext.Bids
                    .Include(b => b.User)
                    .Where(b => b.AuctionId == auction.AuctionId)
                    .OrderByDescending(b => b.BidDate)
                    .FirstOrDefaultAsync();

                    float nextBidPrice = auction.StartingPrice;
                    float? highestBidPrice = null;
                    string? highestBidShippingName = null;
                    string? highestBidShippingAddress = null;
                    string? highestBidShippingPhoneNumber = null;
                    string? highestBidderEmail = null;

                    if (lastBid != null)
                    {
                        nextBidPrice = lastBid.Price + auction.BidIncrement;
                        highestBidPrice = lastBid.Price;
                        highestBidShippingName = lastBid.ShippingName;
                        highestBidShippingAddress = lastBid.ShippingAddress;
                        highestBidShippingPhoneNumber = lastBid.ShippingPhoneNumber;
                        highestBidderEmail = lastBid.User.Email;
                    }

                    AuctionAndProductDetailsViewModel auctionAndProductDetailsViewModel = new AuctionAndProductDetailsViewModel()
                    {
                        ProductId = product.ProductId,
                        AuctionId = auction.AuctionId,
                        SellerId = seller.SellerId,
                        SellerFirstName = seller.FirstName,
                        SellerLastName = seller.LastName,
                        SellerAddress = seller.Address,
                        SellerEmail = seller.Email,
                        SellerPhoneNumber = seller.PhoneNumber,
                        ProductName = product.Name,
                        IsDispatched = product.IsDispatched,
                        CategoryId = product.CategoryId,
                        CategoryName = product.Category.Name,
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
                        HighestBidderEmail = highestBidderEmail,
                    };

                    auctionAndProductDetailsViewModels.Add(auctionAndProductDetailsViewModel);
                } 
            }

            return SortAuctions(auctionAndProductDetailsViewModels);
        }

        public async Task<List<AuctionAndProductDetailsViewModel>> GetAuctionsByUser(int userId)
        {
            List<Auction> auctions = await _dbContext.Auctions
                .Include(a => a.Bids)
                .Include(a => a.Seller)
                .ToListAsync();

            List<AuctionAndProductDetailsViewModel> auctionAndProductDetailsViewModels = new List<AuctionAndProductDetailsViewModel>();

            if(auctions == null || auctions.Count == 0)
            {
                return auctionAndProductDetailsViewModels;
            }

            foreach (var auction in auctions)
            {
                if (auction.Bids != null && auction.Bids.Count > 0)
                {
                    foreach (var bid in auction.Bids)
                    {
                        if (bid.UserId == userId)
                        {
                            Product? product = await _dbContext.Products
                                .Include(p => p.Category)
                                .FirstOrDefaultAsync(p => p.AuctionId == auction.AuctionId);

                            if (product != null)
                            {

                                Bid? lastBid = await _dbContext.Bids
                                    .Include(b => b.User)
                                    .Where(b => b.AuctionId == auction.AuctionId)
                                    .OrderByDescending(b => b.BidDate)
                                    .FirstOrDefaultAsync();

                                float nextBidPrice = auction.StartingPrice;
                                float? highestBidPrice = null;
                                string? highestBidShippingName = null;
                                string? highestBidShippingAddress = null;
                                string? highestBidShippingPhoneNumber = null;
                                string? highestBidderEmail = null;

                                if (lastBid != null)
                                {
                                    nextBidPrice = lastBid.Price + auction.BidIncrement;
                                    highestBidPrice = lastBid.Price;
                                    highestBidShippingName = lastBid.ShippingName;
                                    highestBidShippingAddress = lastBid.ShippingAddress;
                                    highestBidShippingPhoneNumber = lastBid.ShippingPhoneNumber;
                                    highestBidderEmail = lastBid.User.Email;
                                }

                                AuctionAndProductDetailsViewModel auctionAndProductDetailsViewModel = new AuctionAndProductDetailsViewModel()
                                {
                                    ProductId = product.ProductId,
                                    AuctionId = auction.AuctionId,
                                    SellerId = auction.Seller.SellerId,
                                    SellerFirstName = auction.Seller.FirstName,
                                    SellerLastName = auction.Seller.LastName,
                                    SellerAddress = auction.Seller.Address,
                                    SellerEmail = auction.Seller.Email,
                                    SellerPhoneNumber = auction.Seller.PhoneNumber,
                                    ProductName = product.Name,
                                    IsDispatched = product.IsDispatched,
                                    CategoryId = product.CategoryId,
                                    CategoryName = product.Category.Name,
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
                                    HighestBidderEmail = highestBidderEmail,
                                };

                                auctionAndProductDetailsViewModels.Add(auctionAndProductDetailsViewModel);

                                break;
                            }
                        }
                    }
                }

            }

            return SortAuctions(auctionAndProductDetailsViewModels);
        }

        public async Task<List<AuctionAndProductDetailsViewModel>> GetAuctionsByCategory(int categoryId)
        {
            List<Auction> auctions = await _dbContext.Auctions
                .Include(a => a.Seller)
                .ToListAsync();

            List<AuctionAndProductDetailsViewModel> auctionAndProductDetailsViewModels = new List<AuctionAndProductDetailsViewModel>();

            if(auctions == null || auctions.Count == 0)
            {
                return auctionAndProductDetailsViewModels;
            }
            
            foreach (var auction in auctions)
            {
                Product? product = await _dbContext.Products
                    .Include(p => p.Category)
                    .FirstOrDefaultAsync(p => p.AuctionId == auction.AuctionId);

                if (product != null && product.CategoryId == categoryId)
                {
                    Bid? lastBid = await _dbContext.Bids
                        .Include(b => b.User)
                        .Where(b => b.AuctionId == auction.AuctionId)
                        .OrderByDescending(b => b.BidDate)
                        .FirstOrDefaultAsync();

                    float nextBidPrice = auction.StartingPrice;
                    float? highestBidPrice = null;
                    string? highestBidShippingName = null;
                    string? highestBidShippingAddress = null;
                    string? highestBidShippingPhoneNumber = null;
                    string? highestBidderEmail = null;

                    if (lastBid != null)
                    {
                        nextBidPrice = lastBid.Price + auction.BidIncrement;
                        highestBidPrice = lastBid.Price;
                        highestBidShippingName = lastBid.ShippingName;
                        highestBidShippingAddress = lastBid.ShippingAddress;
                        highestBidShippingPhoneNumber = lastBid.ShippingPhoneNumber;
                        highestBidderEmail = lastBid.User.Email;
                    }

                    AuctionAndProductDetailsViewModel auctionAndProductDetailsViewModel = new AuctionAndProductDetailsViewModel()
                    {
                        ProductId = product.ProductId,
                        AuctionId = auction.AuctionId,
                        SellerId = auction.Seller.SellerId,
                        SellerFirstName = auction.Seller.FirstName,
                        SellerLastName = auction.Seller.LastName,
                        SellerAddress = auction.Seller.Address,
                        SellerEmail = auction.Seller.Email,
                        SellerPhoneNumber = auction.Seller.PhoneNumber,
                        ProductName = product.Name,
                        IsDispatched = product.IsDispatched,
                        CategoryId = product.CategoryId,
                        CategoryName = product.Category.Name,
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
                        HighestBidderEmail = highestBidderEmail,
                    };

                    auctionAndProductDetailsViewModels.Add(auctionAndProductDetailsViewModel);
                }
            }
            
            return SortAuctions(auctionAndProductDetailsViewModels);
        }

        public async Task<List<AuctionAndProductDetailsViewModel>> GetAllAuctions()
        {
            List<AuctionAndProductDetailsViewModel> auctionAndProductDetailsViewModels = new List<AuctionAndProductDetailsViewModel>();

            List<Auction> auctions = await _dbContext.Auctions
                .Include(a => a.Seller)
                .ToListAsync();

            if (auctions == null || auctions.Count == 0)
            {
                return auctionAndProductDetailsViewModels;
            }

            foreach (Auction auction in auctions)
            {
                Product? product = await _dbContext.Products
                    .Include(p => p.Category)
                    .FirstOrDefaultAsync(p => p.AuctionId == auction.AuctionId);

                if (product != null)
                {
                    Bid? lastBid = await _dbContext.Bids
                        .Include(b => b.User)
                        .Where(b => b.AuctionId == auction.AuctionId)
                        .OrderByDescending(b => b.BidDate)
                        .FirstOrDefaultAsync();

                    float nextBidPrice = auction.StartingPrice;
                    float? highestBidPrice = null;
                    string? highestBidShippingName = null;
                    string? highestBidShippingAddress = null;
                    string? highestBidShippingPhoneNumber = null;
                    string? highestBidderEmail = null;

                    if (lastBid != null)
                    {
                        nextBidPrice = lastBid.Price + auction.BidIncrement;
                        highestBidPrice = lastBid.Price;
                        highestBidShippingName = lastBid.ShippingName;
                        highestBidShippingAddress = lastBid.ShippingAddress;
                        highestBidShippingPhoneNumber = lastBid.ShippingPhoneNumber;
                        highestBidderEmail = lastBid.User.Email;
                    }

                    AuctionAndProductDetailsViewModel auctionAndProductDetailsViewModel = new AuctionAndProductDetailsViewModel()
                    {
                        ProductId = product.ProductId,
                        AuctionId = auction.AuctionId,
                        SellerId = auction.Seller.SellerId,
                        SellerFirstName = auction.Seller.FirstName,
                        SellerLastName = auction.Seller.LastName,
                        SellerAddress = auction.Seller.Address,
                        SellerEmail = auction.Seller.Email,
                        SellerPhoneNumber = auction.Seller.PhoneNumber,
                        ProductName = product.Name,
                        IsDispatched = product.IsDispatched,
                        CategoryId = product.CategoryId,
                        CategoryName = product.Category.Name,
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
                        HighestBidderEmail = highestBidderEmail,
                    };

                    auctionAndProductDetailsViewModels.Add(auctionAndProductDetailsViewModel);
                }
            }

            return SortAuctions(auctionAndProductDetailsViewModels);
        }

        public async Task<(bool, bool, bool, AuctionAndProductDetailsViewModel?)> GetAuctionById(int auctionId)
        {
            Auction? auction = await _dbContext.Auctions
                .Include(a => a.Seller)
                .FirstOrDefaultAsync(a => a.AuctionId == auctionId);

            if (auction == null)
            {
                return (false, false, false, null);
            }

            Product? product = await _dbContext.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.AuctionId == auctionId);

            if (product == null)
            {
                return (true, false, false, null);
            }

            if (product.Category == null)
            {
                return (true, true, false, null);
            }

            Bid? lastBid = await _dbContext.Bids
                    .Include(b => b.User)
                    .Where(b => b.AuctionId == auction.AuctionId)
                    .OrderByDescending(b => b.BidDate)
                    .FirstOrDefaultAsync();

            float nextBidPrice = auction.StartingPrice;
            float? highestBidPrice = null;
            string? highestBidShippingName = null;
            string? highestBidShippingAddress = null;
            string? highestBidShippingPhoneNumber = null;
            string? highestBidderEmail = null;

            if (lastBid != null)
            {
                nextBidPrice = lastBid.Price + auction.BidIncrement;
                highestBidPrice = lastBid.Price;
                highestBidShippingName = lastBid.ShippingName;
                highestBidShippingAddress = lastBid.ShippingAddress;
                highestBidShippingPhoneNumber = lastBid.ShippingPhoneNumber;
                highestBidderEmail = lastBid.User.Email;
            }

            AuctionAndProductDetailsViewModel auctionAndProductDetailsViewModel = new AuctionAndProductDetailsViewModel()
            {
                ProductId = product.ProductId,
                AuctionId = auction.AuctionId,
                SellerId = auction.Seller.SellerId,
                SellerFirstName = auction.Seller.FirstName,
                SellerLastName = auction.Seller.LastName,
                SellerAddress = auction.Seller.Address,
                SellerEmail = auction.Seller.Email,
                SellerPhoneNumber = auction.Seller.PhoneNumber,
                ProductName = product.Name,
                IsDispatched = product.IsDispatched,
                CategoryId = product.CategoryId,
                CategoryName = product.Category.Name,
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
                HighestBidderEmail = highestBidderEmail,
            };

            return (true, true, true, auctionAndProductDetailsViewModel);
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

        public List<AuctionAndProductDetailsViewModel> SortAuctions(List<AuctionAndProductDetailsViewModel> auctions)
        {
            var currentTime = DateTime.UtcNow;

            var statusOrder = new Dictionary<string, int>
            {
                { "Ongoing", 1 },
                { "Not Started", 2 },
                { "Ended", 3 },
                { "Dispatched", 4 }
            };

            var sortedAuctions = auctions.Select(auction => new
            {
                Auction = auction,
                Status = GetAuctionStatus(auction, currentTime)
            })
            .OrderBy(a => statusOrder[a.Status]) // Sort by custom status order
            .ThenBy(a => a.Auction.StartingDate) // Optionally sort by starting date within the same status
            .Select(a => a.Auction) // Select back the auction details
            .ToList();

            return sortedAuctions;
        }

        private string GetAuctionStatus(AuctionAndProductDetailsViewModel auction, DateTime currentTime)
        {
            if (auction.IsDispatched == 1) // Check if the auction is dispatched
            {
                return "Dispatched"; // Return dispatched status
            }
            else if (currentTime < auction.StartingDate)
            {
                return "Not Started";
            }
            else if (currentTime > auction.EndDate)
            {
                return "Ended";
            }
            else
            {
                return "Ongoing";
            }
        }

    }
}
