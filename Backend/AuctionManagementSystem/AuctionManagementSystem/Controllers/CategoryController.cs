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
    public class CategoryController : ControllerBase
    {
        private readonly CategoryService _categoryService;
        public CategoryController(CategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        // POST: api/Category
        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromForm] CategoryDetailsCreateModel categoryDetailsCreateModel)
        {
            if (categoryDetailsCreateModel == null)
            {
                return BadRequest();
            }

            var isCategoryCreated = await _categoryService.CreateCategory(categoryDetailsCreateModel);

            if(isCategoryCreated == false)
            {
                return BadRequest("Category already exists");
            }

            return Ok("Category Created Successfully");
        }

        // GET: api/Category/All
        [HttpGet("All")]
        public async Task<IActionResult> GetAllCategories()
        {
            var allCategories = await _categoryService.GetAllCategories();

            return Ok(allCategories);
        }

        // GET: api/Category/1
        [HttpGet("{categoryId:int}")]
        public async Task<IActionResult> GetCategoryById([FromRoute] int categoryId)
        {
            var category = await _categoryService.GetCategoryById(categoryId);

            if (category == null)
            {
                return NotFound("Category Not Found");
            }

            return Ok(category);
        }

        // PUT: api/Category
        [HttpPut]
        public async Task<IActionResult> UpdateCategory([FromForm] CategoryDetailsUpdateModel categoryDetailsUpdateModel)
        {
            if (categoryDetailsUpdateModel == null)
            {
                return BadRequest();
            }

            var updatedCategory = await _categoryService.UpdateCategory(categoryDetailsUpdateModel);

            if(updatedCategory == null)
            {
                return NotFound("Category Not Found");
            }

            return Ok(updatedCategory);
        }

        // DELETE: api/Category/1
        [HttpDelete("{categoryId:int}")]
        public async Task<IActionResult> DeleteCategory([FromRoute] int categoryId)
        {
            var (isCategoryFound, isCategoryDeleted) = await _categoryService.DeleteCategory(categoryId);

            if(isCategoryFound == false)
            {
                return NotFound("Category Not Found");
            }

            if (isCategoryDeleted == false)
            {
                return BadRequest("Cannot Delete Category. There are Products associated with this category.");
            }

            return Ok("Category Deleted Successfully");
        }
    }
}
