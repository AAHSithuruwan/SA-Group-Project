using AuctionManagementSystem.DTOs;
using AuctionManagementSystem.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuctionManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BidController : ControllerBase
    {
        private readonly SampleDBContext _dbContext;
        public BidController(SampleDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        // POST: api/Bid
        [HttpPost]
        public ActionResult CreateBid(BidDetailsCreateModel bidDetailsCreateModel)
        {
            int? UserId = HttpContext.Session.GetInt32("UserId");

            if (UserId == null)
            {
                return BadRequest("User is not signed in");
            }

            User? user = _dbContext.Users.FirstOrDefault(u => u.UserId == UserId);

            if (user == null)
            {
                return NotFound("User Not Found");
            }

            Auction? auction = _dbContext.Auctions
                .Include(a => a.Bids)
                .FirstOrDefault(a => a.AuctionId == bidDetailsCreateModel.AuctionId);

            if (auction == null)
            {
                return NotFound("Auction Not Found");
            }

            if (auction.EndDate < DateTime.Now)
            {
                return BadRequest("The Auction is already closed");
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
                return BadRequest("Next Minimum Bid Price = " + nextBidPrice.ToString());
            }

            Bid bid = new Bid()
            {
                Price = bidDetailsCreateModel.Price,
                BidDate = DateTime.Now,
                ShippingName = bidDetailsCreateModel.ShippingName,
                ShippingAddress = bidDetailsCreateModel.ShippingAddress,
                ShippingPhoneNumber = bidDetailsCreateModel.ShippingPhoneNumber,
                UserId = (int)UserId,
                User = user,
                AuctionId = bidDetailsCreateModel.AuctionId,
                Auction = auction
            };

            _dbContext.Bids.Add(bid);
            _dbContext.SaveChanges();

            return Ok("Bid Placed Successfully");
        }

        // GET: api/Bid/All/1
        [HttpGet("All/{AuctionId}")]
        public ActionResult<List<BidDetailsViewModel>> GetAllBids(int AuctionId)
        {
            Auction? auction = _dbContext.Auctions
                .Include(a => a.Bids)
                .FirstOrDefault(a => a.AuctionId == AuctionId);

            if (auction == null)
            {
                return NotFound("Auction Not Found");
            }

            List<BidDetailsViewModel> bidDetailsViewModels = new List<BidDetailsViewModel>();

            if (auction.Bids == null || auction.Bids.Count == 0)
            {
                return bidDetailsViewModels;
            }

            foreach (var bid in auction.Bids)
            {
                BidDetailsViewModel bidDetailsViewModel = new BidDetailsViewModel()
                {
                    AuctionId = AuctionId,
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

        // GET: api/Bid/User/1
        [HttpGet("User/{AuctionId}")]
        public ActionResult<List<BidDetailsViewModel>> GetUserBids(int AuctionId)
        {
            int? UserId = HttpContext.Session.GetInt32("UserId");

            if (UserId == null)
            {
                return BadRequest("User is not signed in");
            }

            Auction? auction = _dbContext.Auctions.FirstOrDefault(a => a.AuctionId == AuctionId);

            if (auction == null)
            {
                return NotFound("Auction Not Found");
            }

            List<Bid> bids = _dbContext.Bids.Where(b => b.AuctionId == AuctionId && b.UserId == UserId).ToList();

            List<BidDetailsViewModel> bidDetailsViewModels = new List<BidDetailsViewModel>();

            if (bids == null || bids.Count == 0)
            {
                return bidDetailsViewModels;
            }

            foreach (var bid in bids)
            {
                BidDetailsViewModel bidDetailsViewModel = new BidDetailsViewModel()
                {
                    AuctionId = AuctionId,
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

        // GET: api/Bid/1
        [HttpGet("{BidId}")]
        public ActionResult<BidDetailsViewModel> GetBid(int BidId)
        {
            Bid? bid = _dbContext.Bids.FirstOrDefault(b => b.BidId == BidId);

            if (bid == null)
            {
                return NotFound("Bid Not Found");
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
