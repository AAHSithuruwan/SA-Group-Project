using AuctionManagementSystem.Data;
using AuctionManagementSystem.DTOs;
using AuctionManagementSystem.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuctionManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ApplicationDBContext _dbContext;
        public CategoryController(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        // POST: api/Category
        [HttpPost]
        public ActionResult CreateCategory(CategoryDetailsCreateModel categoryDetailsCreateModel)
        {
            if (categoryDetailsCreateModel == null)
            {
                return BadRequest();
            }

            Category? existingCategory = _dbContext.Categories.FirstOrDefault(c => c.Name == categoryDetailsCreateModel.Name);

            if (existingCategory != null)
            {
                return BadRequest("Category Already Exists");
            }

            Category category = new Category 
            { 
                Name = categoryDetailsCreateModel.Name 
            };

            _dbContext.Categories.Add(category);

            _dbContext.SaveChanges();

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "images", "CategoryImages", category.CategoryId.ToString() + ".png");

            if (categoryDetailsCreateModel.CategoryImage != null && categoryDetailsCreateModel.CategoryImage.Length > 0)
            {
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    categoryDetailsCreateModel.CategoryImage.CopyTo(stream);
                }
            }

            return Ok("Category Created Successfully");
        }

        // GET: api/Category/All
        [HttpGet("All")]
        public ActionResult<List<Category>> GetAllCategories()
        {
            List<Category> categories = _dbContext.Categories.ToList();

            return Ok(categories);
        }

        // GET: api/Category/1
        [HttpGet("{CategoryId}")]
        public ActionResult<Category> GetCategory(int CategoryId)
        {
            Category? category = _dbContext.Categories.FirstOrDefault(c => c.CategoryId == CategoryId);

            if (category == null)
            {
                return NotFound("Category Not Found");
            }

            return Ok(category);
        }

        // PUT: api/Category
        [HttpPut]
        public ActionResult UpdateCategory(CategoryDetailsUpdateModel categoryDetailsUpdateModel)
        {
            if (categoryDetailsUpdateModel == null)
            {
                return BadRequest();
            }

            Category? category = _dbContext.Categories.FirstOrDefault(c => c.CategoryId == categoryDetailsUpdateModel.CategoryId);

            if (category == null)
            {
                return NotFound("Category Not Found");
            }

            category.Name = categoryDetailsUpdateModel.Name;

            _dbContext.SaveChanges();

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "images", "CategoryImages", category.CategoryId.ToString() + ".png");

            if (categoryDetailsUpdateModel.CategoryImage != null && categoryDetailsUpdateModel.CategoryImage.Length > 0)
            {
                System.IO.File.Delete(filePath);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    categoryDetailsUpdateModel.CategoryImage.CopyTo(stream);
                }
            }

            return Ok("Category Updated Successfully");
        }

        // DELETE: api/Category/1
        [HttpDelete("{CategoryId}")]
        public ActionResult DeleteCategory(int CategoryId)
        {
            Category? category = _dbContext.Categories
                .Include(c => c.Products)
                .FirstOrDefault(c => c.CategoryId == CategoryId);

            if (category == null)
            {
                return NotFound("Categoory Not Found");
            }

            if (category.Products != null && category.Products.Count > 0)
            {
                return BadRequest("Cannot Delete Category. There are Products associated with this category.");
            }

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "images", "CategoryImages", category.CategoryId.ToString() + ".png");

            _dbContext.Categories.Remove(category);
            _dbContext.SaveChanges();

            System.IO.File.Delete(filePath);

            return Ok("Category Deleted Successfully");
        }
    }
}
