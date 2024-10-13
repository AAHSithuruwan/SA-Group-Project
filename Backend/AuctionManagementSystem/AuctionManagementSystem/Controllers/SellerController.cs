using AuctionManagementSystem.DTOs;
using AuctionManagementSystem.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuctionManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SellerController : ControllerBase
    {
        private readonly SampleDBContext _dbContext;
        public SellerController(SampleDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        // POST: api/Seller
        [HttpPost]
        public ActionResult CreateSeller(SellerDetailsModel sellerDetailsModel)
        {
            if (sellerDetailsModel == null)
            {
                return BadRequest();
            }

            int? UserId = HttpContext.Session.GetInt32("UserId");

            if (UserId == null)
            {
                return BadRequest("User is not signed in");
            }

            User? user = _dbContext.Users.FirstOrDefault(u => u.UserId == UserId);

            if (user == null) 
            {
                return BadRequest("User Not Found"); 
            }

            if (_dbContext.Sellers.FirstOrDefault(s => s.UserId == UserId) != null)
            {
                return BadRequest("Seller Account Already Exists");
            }

            Seller seller = new Seller()
            {
                FirstName = sellerDetailsModel.FirstName,
                LastName = sellerDetailsModel.LastName,
                Email = sellerDetailsModel.Email,
                PhoneNumber = sellerDetailsModel.PhoneNumber,
                Address = sellerDetailsModel.Address,
                UserId = (int)UserId,
                User = user
            };

            _dbContext.Sellers.Add(seller);
            _dbContext.SaveChanges();

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "Images", "SellerImages", seller.SellerId.ToString() + ".png");

            if (sellerDetailsModel.SellerImage != null && sellerDetailsModel.SellerImage.Length > 0)
            {
                using(var stream = new FileStream(filePath, FileMode.Create))
                {
                    sellerDetailsModel.SellerImage.CopyTo(stream);
                }
            }
            else
            {
                var defaultImagePath = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "images", "SellerImages", "default.png");

                System.IO.File.Copy(defaultImagePath, filePath);
            }

            return Ok("Seller Created Successfully");
        }

        // GET: api/Seller
        [HttpGet]
        public ActionResult<Seller> GetSeller()
        {
            int? UserId = HttpContext.Session.GetInt32("UserId");

            if (UserId == null)
            {
                return BadRequest("User is not signed in");
            }

            Seller? seller = _dbContext.Sellers.FirstOrDefault(s => s.UserId == UserId);

            if(seller == null)
            {
                return BadRequest("Seller Not Found");
            }

            return Ok(seller);
        }

        // PUT: api/Seller
        [HttpPut]
        public ActionResult<Seller> UpdateSeller(SellerDetailsModel sellerDetailsModel)
        {
            int? UserId = HttpContext.Session.GetInt32("UserId");

            if (UserId == null)
            {
                return BadRequest("User is not signed in");
            }

            Seller? seller =_dbContext.Sellers.FirstOrDefault(s => s.UserId == UserId);

            if(seller == null)
            {
                return BadRequest("Seller Not Found");
            }

            seller.FirstName = sellerDetailsModel.FirstName;
            seller.LastName = sellerDetailsModel.LastName;
            seller.Email = sellerDetailsModel.Email;
            seller.PhoneNumber = sellerDetailsModel.PhoneNumber;
            seller.Address = sellerDetailsModel.Address;

            _dbContext.SaveChanges();

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "images", "SellerImages", seller.SellerId.ToString() + ".png");

            if (sellerDetailsModel.SellerImage != null && sellerDetailsModel.SellerImage.Length > 0)
            {
                System.IO.File.Delete(filePath);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    sellerDetailsModel.SellerImage.CopyTo(stream);
                }
            }

            return Ok(seller);
        }

        // DELETE: api/Seller
        [HttpDelete]
        public ActionResult DeleteSeller()
        {
            int? UserId = HttpContext.Session.GetInt32("UserId");

            if (UserId == null)
            {
                return BadRequest("User is not signed in");
            }

            Seller? seller = _dbContext.Sellers
                .Include(s => s.Auctions)
                .FirstOrDefault(s => s.UserId == UserId);


            if (seller == null)
            {
                return BadRequest("Seller Not Found");
            }

            if (seller.Auctions != null && seller.Auctions.Count != 0)
            {
                return BadRequest("Cannot delete the seller, Because there are auctions related to this seller");
            }

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "images", "SellerImages", seller.SellerId.ToString() + ".png");

            _dbContext.Sellers.Remove(seller);
            _dbContext.SaveChanges();

            System.IO.File.Delete(filePath);

            return Ok("Seller Deleted Successfully");
        }
    }
}
