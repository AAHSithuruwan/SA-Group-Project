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
        private readonly SampleDBContext _dbContext;
        public CategoryController(SampleDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        // POST: api/Category
        [HttpPost]
        public ActionResult CreateCategory(Category category)
        {
            if (category == null)
            {
                return BadRequest();
            }

            Category? existingCategory = _dbContext.Categories.FirstOrDefault(c => c.Name == category.Name);

            if (existingCategory != null)
            {
                return BadRequest("Category Already Exists");
            }

            _dbContext.Categories.Add(category);

            _dbContext.SaveChanges();

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
        public ActionResult UpdateCategory(CategoryUpdateModel categoryUpdateModel)
        {
            if (categoryUpdateModel == null)
            {
                return BadRequest();
            }

            Category? category = _dbContext.Categories.FirstOrDefault(c => c.CategoryId == categoryUpdateModel.CategoryId);

            if (category == null)
            {
                return NotFound("Category Not Found");
            }

            category.Name = categoryUpdateModel.Name;

            _dbContext.SaveChanges();

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

            _dbContext.Categories.Remove(category);
            _dbContext.SaveChanges();

            return Ok("Category Deleted Successfully");
        }
    }
}
