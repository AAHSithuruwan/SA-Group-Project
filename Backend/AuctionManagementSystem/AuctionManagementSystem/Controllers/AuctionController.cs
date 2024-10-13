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
                BidIncrement = auctionAndProductDetailsCreateModel.BidIncrement,
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

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "images", "ProductImages", product.ProductId.ToString() + ".png");

            if (auctionAndProductDetailsCreateModel.ProductImage != null && auctionAndProductDetailsCreateModel.ProductImage.Length > 0)
            {
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    auctionAndProductDetailsCreateModel.ProductImage.CopyTo(stream);
                }
            }

            return Ok("Auction Created Successfully");
        }

        // GET: api/Auction/Seller
        [HttpGet("Seller")]
        public ActionResult<List<AuctionAndProductDetailsViewModel>> GetSellerAuctions()
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

            List<AuctionAndProductDetailsViewModel> auctionAndProductDetailsViewModels = new List<AuctionAndProductDetailsViewModel>();

            if (seller.Auctions == null || seller.Auctions.Count == 0)
            {
                return auctionAndProductDetailsViewModels;
            }

            foreach (var auction in seller.Auctions)
            {
                Product? product = _dbContext.Products.FirstOrDefault(p => p.AuctionId == auction.AuctionId);

                if (product == null)
                {
                    return NotFound("Product Not Found");
                }

                Bid? lastBid = _dbContext.Bids
                    .Where(b => b.AuctionId == auction.AuctionId)
                    .OrderByDescending(b => b.BidDate)
                    .FirstOrDefault();

                float nextBidPrice = auction.StartingPrice;
                float? highestBidPrice = null;
                string? highestBidShippingName = null;
                string? highestBidShippingAddress = null;
                string? highestBidShippingPhoneNumber = null;

                if (lastBid != null) 
                {
                    nextBidPrice = lastBid.Price + auction.BidIncrement;
                    highestBidPrice = lastBid.Price;
                    highestBidShippingName = lastBid.ShippingName;
                    highestBidShippingAddress = lastBid.ShippingAddress;
                    highestBidShippingPhoneNumber = lastBid.ShippingPhoneNumber;
                }

                AuctionAndProductDetailsViewModel auctionAndProductDetailsViewModel = new AuctionAndProductDetailsViewModel()
                {
                    ProductId = product.ProductId,
                    AuctionId = auction.AuctionId,
                    ProductName = product.Name,
                    CategoryId = product.CategoryId,
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
                };

                auctionAndProductDetailsViewModels.Add(auctionAndProductDetailsViewModel);
            }

            return auctionAndProductDetailsViewModels;
        }

        // GET: api/Auction/All
        [HttpGet("All")]
        public ActionResult<List<AuctionAndProductDetailsViewModel>> GetAllAuctions()
        {
            List<AuctionAndProductDetailsViewModel> auctionAndProductDetailsViewModels = new List<AuctionAndProductDetailsViewModel>();

            List<Auction> auctions = _dbContext.Auctions.ToList();

            if (auctions.Count == 0)
            {
                return auctionAndProductDetailsViewModels;
            }

            foreach(Auction auction in auctions)
            {
                Product? product = _dbContext.Products.FirstOrDefault(p => p.AuctionId == auction.AuctionId);

                if (product == null)
                {
                    return NotFound("Product Not Found");
                }

                Bid? lastBid = _dbContext.Bids
                    .Where(b => b.AuctionId == auction.AuctionId)
                    .OrderByDescending(b => b.BidDate)
                    .FirstOrDefault();

                float nextBidPrice = auction.StartingPrice;
                float? highestBidPrice = null;
                string? highestBidShippingName = null;
                string? highestBidShippingAddress = null;
                string? highestBidShippingPhoneNumber = null;

                if (lastBid != null)
                {
                    nextBidPrice = lastBid.Price + auction.BidIncrement;
                    highestBidPrice = lastBid.Price;
                    highestBidShippingName = lastBid.ShippingName;
                    highestBidShippingAddress = lastBid.ShippingAddress;
                    highestBidShippingPhoneNumber = lastBid.ShippingPhoneNumber;
                }

                AuctionAndProductDetailsViewModel auctionAndProductDetailsViewModel = new AuctionAndProductDetailsViewModel()
                {
                    ProductId = product.ProductId,
                    AuctionId = auction.AuctionId,
                    ProductName = product.Name,
                    CategoryId = product.CategoryId,
                    ProductDescription = product.Description,
                    StartingPrice = auction.StartingPrice,
                    NextBidPrice = nextBidPrice,
                    BidIncrement = auction.BidIncrement,
                    StartingDate = auction.StartingDate,
                    EndDate = auction.EndDate,
                    HighestBidPrice = highestBidPrice,
                    HighestBidShippingName = highestBidShippingName,
                    HighestBidShippingAddress = highestBidShippingAddress,
                    HighestBidShippingPhoneNumber = highestBidShippingPhoneNumber
                };

                auctionAndProductDetailsViewModels.Add(auctionAndProductDetailsViewModel);
            }

            return auctionAndProductDetailsViewModels;
        }

        // GET: api/Auction/1
        [HttpGet("{AuctionId}")]
        public ActionResult<AuctionAndProductDetailsViewModel> GetSingleAuction(int AuctionId)
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

            Bid? lastBid = _dbContext.Bids
                    .Where(b => b.AuctionId == auction.AuctionId)
                    .OrderByDescending(b => b.BidDate)
                    .FirstOrDefault();

            float nextBidPrice = auction.StartingPrice;
            float? highestBidPrice = null;
            string? highestBidShippingName = null;
            string? highestBidShippingAddress = null;
            string? highestBidShippingPhoneNumber = null;

            if (lastBid != null)
            {
                nextBidPrice = lastBid.Price + auction.BidIncrement;
                highestBidPrice = lastBid.Price;
                highestBidShippingName = lastBid.ShippingName;
                highestBidShippingAddress = lastBid.ShippingAddress;
                highestBidShippingPhoneNumber = lastBid.ShippingPhoneNumber;
            }

            AuctionAndProductDetailsViewModel auctionAndProductDetailsViewModel = new AuctionAndProductDetailsViewModel()
            {
                ProductId = auction.Product.ProductId,
                AuctionId = auction.AuctionId,
                ProductName = auction.Product.Name,
                CategoryId = auction.Product.CategoryId,
                ProductDescription = auction.Product.Description,
                StartingPrice = auction.StartingPrice,
                NextBidPrice = nextBidPrice,
                BidIncrement = auction.BidIncrement,
                StartingDate = auction.StartingDate,
                EndDate = auction.EndDate,
                HighestBidPrice = highestBidPrice,
                HighestBidShippingName = highestBidShippingName,
                HighestBidShippingAddress = highestBidShippingAddress,
                HighestBidShippingPhoneNumber = highestBidShippingPhoneNumber
            };

            return auctionAndProductDetailsViewModel;
        }

        // PUT: api/Auction
        [HttpPut]
        public ActionResult<AuctionAndProductDetailsUpdateModel> UpdateAuction(AuctionAndProductDetailsUpdateModel auctionAndProductDetailsUpdateModel)
        {
            if (auctionAndProductDetailsUpdateModel == null)
            {
                return BadRequest();
            }

            Auction? auction = _dbContext.Auctions
                .Include(a => a.Product)
                .FirstOrDefault(a => a.AuctionId == auctionAndProductDetailsUpdateModel.AuctionId);

            if (auction == null)
            {
                return NotFound("Auction Record Not Found");
            }

            if (auction.Product == null)
            {
                return NotFound("Product Not Found");
            }

            auction.StartingPrice = auctionAndProductDetailsUpdateModel.StartingPrice;
            auction.BidIncrement = auctionAndProductDetailsUpdateModel.BidIncrement;
            auction.StartingDate = auctionAndProductDetailsUpdateModel.StartingDate;
            auction.EndDate = auctionAndProductDetailsUpdateModel.EndDate;

            _dbContext.SaveChanges();

            auction.Product.Name = auctionAndProductDetailsUpdateModel.ProductName;
            auction.Product.CategoryId = auctionAndProductDetailsUpdateModel.CategoryId;
            auction.Product.Description = auctionAndProductDetailsUpdateModel.ProductDescription;

            _dbContext.SaveChanges();

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "images", "ProductImages", auction.Product.ProductId.ToString() + ".png");

            if (auctionAndProductDetailsUpdateModel.ProductImage != null && auctionAndProductDetailsUpdateModel.ProductImage.Length > 0)
            {
                System.IO.File.Delete(filePath);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    auctionAndProductDetailsUpdateModel.ProductImage.CopyTo(stream);
                }
            }

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

            Product? product = _dbContext.Products.FirstOrDefault(p => p.AuctionId == AuctionId);

            if (product == null)
            {
                return NotFound("Product Not Found");
            }

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "images", "ProductImages", product.ProductId + ".png");

            _dbContext.Auctions.Remove(auction);
            _dbContext.SaveChanges();

            System.IO.File.Delete(filePath);

            return Ok("Auction Deleted Successfully");
        }
    }
}
