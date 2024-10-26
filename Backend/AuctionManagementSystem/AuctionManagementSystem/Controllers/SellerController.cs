using AuctionManagementSystem.DTOs;
using AuctionManagementSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AuctionManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SellerController : ControllerBase
    {
        private readonly SellerService _sellerService;
        public SellerController(SellerService sellerService)
        {
            _sellerService = sellerService;
        }

        // POST: api/Seller
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateSeller([FromForm] SellerDetailsModel sellerDetailsModel)
        {
            if (sellerDetailsModel == null)
            {
                return BadRequest();
            }

            String? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return Unauthorized("User is not signed in");
            }

            var (isUserFound, isSuccess) = await _sellerService.CreateSeller(sellerDetailsModel, int.Parse(userId));

            if(isUserFound == false)
            {
                return NotFound("User Not Found");
            }

            if (isSuccess == false)
            {
                return BadRequest("Seller Account Already Exists");
            }

            return Ok("Seller Created Successfully"); 
        }

        // GET: api/Seller
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetSeller()
        {
            String? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return Unauthorized("User is not signed in");
            }

            var seller = await _sellerService.GetSeller(int.Parse(userId));

            if (seller == null)
            {
                return NotFound("Seller Not Found");
            }

            return Ok(seller);
        }

        // PUT: api/Seller
        [Authorize]
        [HttpPut]
        public async Task<IActionResult> UpdateSeller([FromForm] SellerDetailsModel sellerDetailsModel)
        {
            String? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return Unauthorized("User is not signed in");
            }

            var seller = await _sellerService.UpdateSeller(sellerDetailsModel, int.Parse(userId));

            if (seller == null)
            {
                return NotFound("Seller Not Found");
            }

            return Ok(seller);
        }

        // DELETE: api/Seller
        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> DeleteSeller()
        {
            String? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return Unauthorized("User is not signed in");
            }

            var (isSellerFound, isSellerDeleted) = await _sellerService.DeleteSeller(int.Parse(userId));

            if(isSellerFound == false)
            {
                return NotFound("Seller Not Found");
            }

            if(isSellerDeleted == false)
            {
                return BadRequest("Cannot delete the seller, Because there are auctions related to this seller");
            }

            return Ok("Seller Deleted Successfully");
        }
    }
}
