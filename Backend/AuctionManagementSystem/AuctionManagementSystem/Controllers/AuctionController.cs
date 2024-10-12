using AuctionManagementSystem.DTOs;
using AuctionManagementSystem.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuctionManagementSystem.Controllers
{
    //Auction Controller is where the both Auction and Product models are handled

    [Route("api/[controller]")]
    [ApiController]
    public class AuctionController : ControllerBase
    {
        private readonly SampleDBContext _dbContext;
        public AuctionController(SampleDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        // POST: api/Auction
        [HttpPost]
        public ActionResult CreateAuction(AuctionAndProductDetailsCreateModel auctionAndProductDetailsCreateModel)
        {
            if (auctionAndProductDetailsCreateModel == null)
            {
                return BadRequest();
            }

            int? UserId = HttpContext.Session.GetInt32("UserId");

            if (UserId == null)
            {
                return BadRequest("User is not signed in");
            }

            Seller? seller = _dbContext.Sellers.FirstOrDefault(s => s.UserId == UserId);

            if (seller == null)
            {
                return NotFound("Seller Not Found");
            }

            Category? category = _dbContext.Categories.FirstOrDefault(c => c.CategoryId == auctionAndProductDetailsCreateModel.CategoryId);

            if (category == null)
            {
                return NotFound("Category Not Found");
            }

            Auction auction = new Auction()
            {
                StartingPrice = auctionAndProductDetailsCreateModel.StartingPrice,
                StartingDate = auctionAndProductDetailsCreateModel.StartingDate,
                EndDate = auctionAndProductDetailsCreateModel.EndDate,
                SellerId = seller.SellerId,
                Seller = seller
            };

            _dbContext.Auctions.Add(auction);
            _dbContext.SaveChanges();

            Product product = new Product()
            {
                Name = auctionAndProductDetailsCreateModel.ProductName,
                Description = auctionAndProductDetailsCreateModel.ProductDescription,
                AuctionId = auction.AuctionId,
                Auction = auction,
                CategoryId = auctionAndProductDetailsCreateModel.CategoryId,
                Category = category
            };

            _dbContext.Products.Add(product);
            _dbContext.SaveChanges();

            return Ok("Auction Created Successfully");
        }

        // GET: api/Auction/Seller
        [HttpGet("Seller")]
        public ActionResult<List<AuctionAndProductDetailsViewUpdateModel>> GetSellerAuctions()
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
                return NotFound("Seller Not Found");
            }

            List<AuctionAndProductDetailsViewUpdateModel> auctionAndProductDetailsViewUpdateModels = new List<AuctionAndProductDetailsViewUpdateModel>();

            if (seller.Auctions == null || seller.Auctions.Count == 0)
            {
                return auctionAndProductDetailsViewUpdateModels;
            }

            foreach (var auction in seller.Auctions)
            {
                Product? product = _dbContext.Products.FirstOrDefault(p => p.AuctionId == auction.AuctionId);

                if (product == null)
                {
                    return NotFound("Product Not Found");
                }

                AuctionAndProductDetailsViewUpdateModel auctionAndProductDetailsViewUpdateModel = new AuctionAndProductDetailsViewUpdateModel()
                {
                    ProductId = product.ProductId,
                    AuctionId = auction.AuctionId,
                    ProductName = product.Name,
                    CategoryId = product.CategoryId,
                    ProductDescription = product.Description,
                    StartingPrice = auction.StartingPrice,
                    StartingDate = auction.StartingDate,
                    EndDate = auction.EndDate
                };

                auctionAndProductDetailsViewUpdateModels.Add(auctionAndProductDetailsViewUpdateModel);
            }

            return auctionAndProductDetailsViewUpdateModels;
        }

        // GET: api/Auction/All
        [HttpGet("All")]
        public ActionResult<List<AuctionAndProductDetailsViewUpdateModel>> GetAllAuctions()
        {
            List<AuctionAndProductDetailsViewUpdateModel> auctionAndProductDetailsViewUpdateModels = new List<AuctionAndProductDetailsViewUpdateModel>();

            List<Auction> auctions = _dbContext.Auctions.ToList();

            if (auctions.Count == 0)
            {
                return auctionAndProductDetailsViewUpdateModels;
            }

            foreach(Auction auction in auctions)
            {
                Product? product = _dbContext.Products.FirstOrDefault(p => p.AuctionId == auction.AuctionId);

                if (product == null)
                {
                    return NotFound("Product Not Found");
                }

                AuctionAndProductDetailsViewUpdateModel auctionAndProductDetailsViewUpdateModel = new AuctionAndProductDetailsViewUpdateModel()
                {
                    ProductId = product.ProductId,
                    AuctionId = auction.AuctionId,
                    ProductName = product.Name,
                    CategoryId = product.CategoryId,
                    ProductDescription = product.Description,
                    StartingPrice = auction.StartingPrice,
                    StartingDate = auction.StartingDate,
                    EndDate = auction.EndDate
                };

                auctionAndProductDetailsViewUpdateModels.Add(auctionAndProductDetailsViewUpdateModel);
            }

            return auctionAndProductDetailsViewUpdateModels;
        }

        // GET: api/Auction/1
        [HttpGet("{AuctionId}")]
        public ActionResult<AuctionAndProductDetailsViewUpdateModel> GetSingleAuction(int AuctionId)
        {
            Auction? auction = _dbContext.Auctions
                .Include(a => a.Product)
                .FirstOrDefault(a =>  a.AuctionId == AuctionId);

            if(auction == null)
            {
                return NotFound("Auction Not Found");
            }

            if (auction.Product == null)
            {
                return NotFound("Product Not Found");
            }

            AuctionAndProductDetailsViewUpdateModel auctionAndProductDetailsViewUpdateModel = new AuctionAndProductDetailsViewUpdateModel()
            {
                ProductId = auction.Product.ProductId,
                AuctionId = auction.AuctionId,
                ProductName = auction.Product.Name,
                CategoryId = auction.Product.CategoryId,
                ProductDescription = auction.Product.Description,
                StartingPrice = auction.StartingPrice,
                StartingDate = auction.StartingDate,
                EndDate = auction.EndDate
            };

            return auctionAndProductDetailsViewUpdateModel;
        }

        // PUT: api/Auction
        [HttpPut]
        public ActionResult<AuctionAndProductDetailsViewUpdateModel> UpdateAuction(AuctionAndProductDetailsViewUpdateModel auctionAndProductDetailsViewUpdateModel)
        {
            if (auctionAndProductDetailsViewUpdateModel == null)
            {
                return BadRequest();
            }

            Auction? auction = _dbContext.Auctions
                .Include(a => a.Product)
                .FirstOrDefault(a => a.AuctionId == auctionAndProductDetailsViewUpdateModel.AuctionId);

            if (auction == null)
            {
                return NotFound("Auction Record Not Found");
            }

            if (auction.Product == null)
            {
                return NotFound("Product Not Found");
            }

            auction.StartingPrice = auctionAndProductDetailsViewUpdateModel.StartingPrice;
            auction.StartingDate = auctionAndProductDetailsViewUpdateModel.StartingDate;
            auction.EndDate = auctionAndProductDetailsViewUpdateModel.EndDate;

            _dbContext.SaveChanges();

            auction.Product.Name = auctionAndProductDetailsViewUpdateModel.ProductName;
            auction.Product.CategoryId = auctionAndProductDetailsViewUpdateModel.CategoryId;
            auction.Product.Description = auctionAndProductDetailsViewUpdateModel.ProductDescription;

            _dbContext.SaveChanges();

            return Ok("Auction Updated Successfully");
        }

        // DELETE: api/Auction/1
        [HttpDelete("{AuctionId}")]
        public ActionResult DeleteAuction(int AuctionId)
        {
            Auction? auction = _dbContext.Auctions
                .Include(a => a.Bids)
                .FirstOrDefault(a => a.AuctionId == AuctionId);

            if (auction == null)
            {
                return NotFound("Auction Not Found");
            }

            if(auction.Bids != null && auction.Bids.Count > 0)
            {
                return BadRequest("Cannot Delete the auction. Because there are Bids associated with it");
            }

            _dbContext.Auctions.Remove(auction);
            _dbContext.SaveChanges();

            return Ok("Auction Deleted Successfully");
        }
    }
}
