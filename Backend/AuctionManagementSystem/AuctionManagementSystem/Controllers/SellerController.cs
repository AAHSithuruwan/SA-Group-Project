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
    public class SellerController : ControllerBase
    {
        private readonly SellerService _sellerService;
        public SellerController(SellerService sellerService)
        {
            _sellerService = sellerService;
        }

        // POST: api/Seller
        [HttpPost]
        public async Task<IActionResult> CreateSeller([FromBody] SellerDetailsModel sellerDetailsModel)
        {
            if (sellerDetailsModel == null)
            {
                return BadRequest();
            }

            int? userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return BadRequest("User is not signed in");
            }

            var (userFound, isSuccess) = await _sellerService.CreateSeller(sellerDetailsModel, (int)userId);

            if(userFound == false)
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
        [HttpGet]
        public async Task<IActionResult> GetSeller()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return BadRequest("User is not signed in");
            }

            var seller = await _sellerService.GetSeller((int)userId);

            if (seller == null)
            {
                return NotFound("Seller Not Found");
            }

            return Ok(seller);
        }

        // PUT: api/Seller
        [HttpPut]
        public async Task<IActionResult> UpdateSeller(SellerDetailsModel sellerDetailsModel)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return BadRequest("User is not signed in");
            }

            var seller = await _sellerService.UpdateSeller(sellerDetailsModel, (int)userId);

            if (seller == null)
            {
                return NotFound("Seller Not Found");
            }

            return Ok(seller);
        }

        // DELETE: api/Seller
        [HttpDelete]
        public async Task<IActionResult> DeleteSeller()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return BadRequest("User is not signed in");
            }

            var (sellerFound, isDeleted) = await _sellerService.DeleteSeller((int)userId);

            if(sellerFound == false)
            {
                return NotFound("Seller Not Found");
            }

            if(isDeleted == false)
            {
                return BadRequest("Cannot delete the seller, Because there are auctions related to this seller");
            }

            return Ok("Seller Deleted Successfully");
        }
    }
}
