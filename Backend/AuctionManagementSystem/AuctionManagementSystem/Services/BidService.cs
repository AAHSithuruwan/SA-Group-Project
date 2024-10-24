using AuctionManagementSystem.Data;
using AuctionManagementSystem.DTOs;
using AuctionManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace AuctionManagementSystem.Services
{
    public class BidService
    {
        private readonly ApplicationDBContext _dbContext;
        public BidService(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<(bool, bool, string?, string?)> CreateBid(BidDetailsCreateModel bidDetailsCreateModel, int userId)
        {
            User? user = await _dbContext.Users.FirstOrDefaultAsync(u => u.UserId == userId);

            if (user == null)
            {
                return (false, false, null, null);
            }

            Auction? auction = await _dbContext.Auctions
                .Include(a => a.Bids)
                .FirstOrDefaultAsync(a => a.AuctionId == bidDetailsCreateModel.AuctionId);

            if (auction == null)
            {
                return (true, false, null, null);
            }

            if (auction.EndDate < DateTime.Now)
            {
                return (true, true, "The Auction is already closed", null);
            }

            float nextBidPrice = auction.StartingPrice;

            if (auction.Bids != null && auction.Bids.Count > 0)
            {
                //Getting the highest bid value
                Bid? lastBid = auction.Bids
                    .OrderByDescending(b => b.BidDate)
                    .FirstOrDefault();

                if (lastBid != null)
                {
                    nextBidPrice = lastBid.Price + auction.BidIncrement;
                }
            }

            //Check if the new bidding price is valid
            if (bidDetailsCreateModel.Price < nextBidPrice)
            {
                return (true, true, null, "Next Minimum Bid Price = " + nextBidPrice.ToString());
            }

            Bid bid = new Bid()
            {
                Price = bidDetailsCreateModel.Price,
                BidDate = DateTime.Now,
                ShippingName = bidDetailsCreateModel.ShippingName,
                ShippingAddress = bidDetailsCreateModel.ShippingAddress,
                ShippingPhoneNumber = bidDetailsCreateModel.ShippingPhoneNumber,
                UserId = (int)userId,
                User = user,
                AuctionId = bidDetailsCreateModel.AuctionId,
                Auction = auction
            };

            await _dbContext.Bids.AddAsync(bid);
            await _dbContext.SaveChangesAsync();

            return (true, true, null, null);
        }

        public async Task<List<BidDetailsViewModel>?> GetAllBids(int auctionId)
        {
            Auction? auction = await _dbContext.Auctions
                .Include(a => a.Bids)
                .FirstOrDefaultAsync(a => a.AuctionId == auctionId);

            if (auction == null)
            {
                return null; 
            }

            List<BidDetailsViewModel> bidDetailsViewModels = new List<BidDetailsViewModel>();

            if (auction.Bids == null || auction.Bids.Count == 0)
            {
                return bidDetailsViewModels;
            }

            foreach (var bid in auction.Bids.OrderByDescending(b => b.BidId))
            {
                BidDetailsViewModel bidDetailsViewModel = new BidDetailsViewModel()
                {
                    AuctionId = auctionId,
                    BidId = bid.BidId,
                    BidDate = bid.BidDate,
                    Price = bid.Price,
                    ShippingName = bid.ShippingName,
                    ShippingAddress = bid.ShippingAddress,
                    ShippingPhoneNumber = bid.ShippingPhoneNumber,
                };

                bidDetailsViewModels.Add(bidDetailsViewModel);
            }

            return bidDetailsViewModels;
        }

        public async Task<List<BidDetailsViewModel>?> GetUserBids(int auctionId, int userId)
        {
            Auction? auction = await _dbContext.Auctions.FirstOrDefaultAsync(a => a.AuctionId == auctionId);

            if (auction == null)
            {
                return null;
            }

            List<Bid> bids = await _dbContext.Bids.Where(b => b.AuctionId == auctionId && b.UserId == userId).ToListAsync();

            List<BidDetailsViewModel> bidDetailsViewModels = new List<BidDetailsViewModel>();

            if (bids == null || bids.Count == 0)
            {
                return bidDetailsViewModels;
            }

            foreach (var bid in bids)
            {
                BidDetailsViewModel bidDetailsViewModel = new BidDetailsViewModel()
                {
                    AuctionId = auctionId,
                    BidId = bid.BidId,
                    BidDate = bid.BidDate,
                    Price = bid.Price,
                    ShippingName = bid.ShippingName,
                    ShippingAddress = bid.ShippingAddress,
                    ShippingPhoneNumber = bid.ShippingPhoneNumber,
                };

                bidDetailsViewModels.Add(bidDetailsViewModel);
            }

            return bidDetailsViewModels;
        }

        public async Task<BidDetailsViewModel?> GetBidById(int bidId)
        {
            Bid? bid = await _dbContext.Bids.FirstOrDefaultAsync(b => b.BidId == bidId);

            if (bid == null)
            {
                return null;
            }

            BidDetailsViewModel bidDetailsViewModel = new BidDetailsViewModel()
            {
                AuctionId = bid.AuctionId,
                BidId = bid.BidId,
                BidDate = bid.BidDate,
                Price = bid.Price,
                ShippingName = bid.ShippingName,
                ShippingAddress = bid.ShippingAddress,
                ShippingPhoneNumber = bid.ShippingPhoneNumber,
            };

            return bidDetailsViewModel;
        }
    }
}
