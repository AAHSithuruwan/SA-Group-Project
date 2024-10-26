using AuctionManagementSystem.DTOs;
using AuctionManagementSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AuctionManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BidController : ControllerBase
    {
        private readonly BidService _bidService;
        public BidController(BidService bidService)
        {
            _bidService = bidService;
        }

        // POST: api/Bid
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateBid([FromBody] BidDetailsCreateModel bidDetailsCreateModel)
        {
            String? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return Unauthorized("User is not signed in");
            }

            var (isUserFound, isAuctionFound, auctionClosedMessage, invalidBidPriceMessage) = await _bidService.CreateBid(bidDetailsCreateModel, int.Parse(userId));

            if (isUserFound == false)
            {
                return NotFound("User Not Found");
            }

            if (isAuctionFound == false)
            {
                return NotFound("Auction Not Found");
            }

            if (auctionClosedMessage != null)
            {
                return BadRequest(auctionClosedMessage);
            }

            if (invalidBidPriceMessage != null)
            {
                return BadRequest(invalidBidPriceMessage);
            }

            return Ok("Bid Placed Successfully");
        }

        // GET: api/Bid/All/1
        [Authorize]
        [HttpGet("All/{auctionId:int}")]
        public async Task<IActionResult> GetAllBids([FromRoute] int auctionId)
        {
            var allBids = await _bidService.GetAllBids(auctionId);

            if(allBids == null)
            {
                return NotFound("Auction Not Found");
            }

            return Ok(allBids);
        }

        // GET: api/Bid/User/1
        [Authorize]
        [HttpGet("User/{auctionId:int}")]
        public async Task<IActionResult> GetUserBids([FromRoute] int auctionId)
        {
            String? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return Unauthorized("User is not signed in");
            }

            var userBids = await _bidService.GetUserBids(auctionId, int.Parse(userId));

            if (userBids == null)
            {
                return NotFound("Auction or User Not Found");
            }

            return Ok(userBids);
        }

        // GET: api/Bid/1
        [Authorize]
        [HttpGet("{bidId:int}")]
        public async Task<IActionResult> GetBidById([FromRoute] int bidId)
        {
            var bid = await _bidService.GetBidById(bidId);

            if (bid == null)
            {
                return NotFound("Bid Not Found");
            }

            return Ok(bid);
        }
    }
}
