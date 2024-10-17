using AuctionManagementSystem.Data;
using AuctionManagementSystem.DTOs;
using AuctionManagementSystem.Models;
using AuctionManagementSystem.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        [HttpPost]
        public async Task<IActionResult> CreateBid([FromBody] BidDetailsCreateModel bidDetailsCreateModel)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return BadRequest("User is not signed in");
            }

            var (isUserFound, isAuctionFound, auctionClosedMessage, invalidBidPriceMessage) = await _bidService.CreateBid(bidDetailsCreateModel, (int)userId);

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
        [HttpGet("User/{auctionId:int}")]
        public async Task<IActionResult> GetUserBids([FromRoute] int auctionId)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return BadRequest("User is not signed in");
            }

            var userBids = await _bidService.GetUserBids(auctionId, (int)userId);

            if (userBids == null)
            {
                return NotFound("Auction Not Found");
            }

            return Ok(userBids);
        }

        // GET: api/Bid/1
        [HttpGet("{bidId:int}")]
        public async Task<IActionResult> GetBid([FromRoute] int bidId)
        {
            var bid = await _bidService.GetBid(bidId);

            if (bid == null)
            {
                return NotFound("Bid Not Found");
            }

            return Ok(bid);
        }
    }
}
