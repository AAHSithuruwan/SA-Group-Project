﻿using AuctionManagementSystem.DTOs;
using AuctionManagementSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AuctionManagementSystem.Controllers
{
    //Auction Controller is where the both Auction and Product models are handled

    [Route("api/[controller]")]
    [ApiController]
    public class AuctionController : ControllerBase
    {
        private readonly AuctionService _auctionService;
        public AuctionController(AuctionService auctionService)
        {
            _auctionService = auctionService;
        }

        // POST: api/Auction
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateAuction([FromForm] AuctionAndProductDetailsCreateModel auctionAndProductDetailsCreateModel)
        {
            if (auctionAndProductDetailsCreateModel == null)
            {
                return BadRequest();
            }

            String? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return Unauthorized("User is not signed in");
            }

            var (isSellerFound, isCategoryFound, isAuctionCreated) = await _auctionService.CreateAuction(auctionAndProductDetailsCreateModel, int.Parse(userId));

            if (isSellerFound == false)
            {
                return NotFound("Seller Not Found");
            }

            if (isCategoryFound == false)
            {
                return NotFound("Category Not Found");
            }

            if (isAuctionCreated == false)
            {
                return BadRequest();
            }

            return Ok("Auction Created Successfully");
        }

        // GET: api/Auction/Seller
        [Authorize]
        [HttpGet("Seller")]
        public async Task<IActionResult> GetAuctionsBySeller()
        {
            String? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return Unauthorized("User is not signed in");
            }

            var sellerAuctions = await _auctionService.GetAuctionsBySeller(int.Parse(userId));

            if(sellerAuctions == null)
            {
                return NotFound("Seller Not Found");
            }

            return Ok(sellerAuctions);
        }

        // GET: api/Auction/User
        [Authorize]
        [HttpGet("User")]
        public async Task<IActionResult> GetAuctionsByUser()
        {
            String? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return Unauthorized("User is not signed in");
            }

            var userAuctions = await _auctionService.GetAuctionsByUser(int.Parse(userId));

            return Ok(userAuctions);
        }

        // GET: api/Auction/Category/1
        [HttpGet("Category/{categoryId:int}")]
        public async Task<IActionResult> GetAuctionsByCategory([FromRoute] int categoryId)
        {
            var categoryAuctions = await _auctionService.GetAuctionsByCategory(categoryId);

            return Ok(categoryAuctions);
        }

        // GET: api/Auction/All
        [HttpGet("All")]
        public async Task<IActionResult> GetAllAuctions()
        {
            var allAuctions = await _auctionService.GetAllAuctions();

            return Ok(allAuctions);
        }

        // GET: api/Auction/1
        [HttpGet("{auctionId:int}")]
        public async Task<IActionResult> GetAuctionById([FromRoute] int auctionId)
        {
            var (isAuctionFound, isProductFound, isCategoryFound, auction) = await _auctionService.GetAuctionById(auctionId);

            if (isAuctionFound == false)
            {
                return NotFound("Auction Not Found");
            }

            if (isProductFound == false)
            {
                return NotFound("Product Not Found");
            }

            if (isCategoryFound == false)
            {
                return NotFound("Category Not Found");
            }

            return Ok(auction);
        }

        // GET: api/Auction/DispatchProduct/1
        [Authorize]
        [HttpGet("DispatchProduct/{auctionId:int}")]
        public async Task<IActionResult> DispatchProduct([FromRoute] int auctionId)
        {
            var (isAuctionFound, isProductFound, isProductDispatched) = await _auctionService.DispatchProduct(auctionId);

            if (isAuctionFound == false)
            {
                return NotFound("Auction Not Found");
            }

            if (isProductFound == false)
            {
                return NotFound("Product Not Found");
            }

            if (isProductDispatched == false)
            {
                return BadRequest("Cannot Dispatch the Product, The Auction is not yet Closed");
            }

            return Ok("Product Dispatched Successfully");
        }

        // PUT: api/Auction
        [Authorize]
        [HttpPut]
        public async Task<IActionResult> UpdateAuction([FromForm] AuctionAndProductDetailsUpdateModel auctionAndProductDetailsUpdateModel)
        {
            if (auctionAndProductDetailsUpdateModel == null)
            {
                return BadRequest();
            }

            var (isAuctionFound, isProductFound, isAuctionUpdated) = await _auctionService.UpdateAuction(auctionAndProductDetailsUpdateModel);

            if (isAuctionFound == false)
            {
                return NotFound("Auction Not Found");
            }

            if(isProductFound == false)
            {
                return NotFound("Product Not Found");
            }

            if(isAuctionUpdated == false)
            {
                return BadRequest("Cannot Update The Auction, Product is already Dispatched");
            }

            return Ok("Auction Updated Successfully");
        }

        // DELETE: api/Auction/1
        [Authorize]
        [HttpDelete("{auctionId:int}")]
        public async Task<IActionResult> DeleteAuction([FromRoute] int auctionId)
        {
            var (isAuctionFound, isProductFound, isAuctionDeleted) = await _auctionService.DeleteAuction(auctionId);

            if(isAuctionFound == false)
            {
                return NotFound("Auction Not Found");
            }

            if(isProductFound == false)
            {
                return NotFound("Product Not Found");
            }

            if (isAuctionDeleted == false)
            {
                return BadRequest("Cannot Delete the auction. Because there are Bids associated with it");
            }

            return Ok("Auction Deleted Successfully");
        }
    }
}
