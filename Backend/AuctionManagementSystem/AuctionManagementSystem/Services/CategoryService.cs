using AuctionManagementSystem.Data;
using AuctionManagementSystem.DTOs;
using AuctionManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace AuctionManagementSystem.Services
{
    public class CategoryService
    {
        private readonly ApplicationDBContext _dbContext;
        public CategoryService(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> CreateCategory(CategoryDetailsCreateModel categoryDetailsCreateModel)
        {
            Category? existingCategory = await _dbContext.Categories.FirstOrDefaultAsync(c => c.Name == categoryDetailsCreateModel.Name);

            if (existingCategory != null)
            {
                return false;
            }

            Category category = new Category
            {
                Name = categoryDetailsCreateModel.Name
            };

            await _dbContext.Categories.AddAsync(category);

            await _dbContext.SaveChangesAsync();

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "images", "CategoryImages", category.CategoryId.ToString() + ".png");

            if (categoryDetailsCreateModel.CategoryImage != null && categoryDetailsCreateModel.CategoryImage.Length > 0)
            {
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    categoryDetailsCreateModel.CategoryImage.CopyTo(stream);
                }
            }

            return true;
        }

        public async Task<List<Category>> GetAllCategories()
        {
            List<Category> categories = await _dbContext.Categories.ToListAsync();

            return categories;
        }

        public async Task<Category?> GetCategoryById(int categoryId)
        {
            Category? category = await _dbContext.Categories.FirstOrDefaultAsync(c => c.CategoryId == categoryId);

            if (category == null)
            {
                return null;
            }

            return category;
        }

        public async Task<Category?> UpdateCategory(CategoryDetailsUpdateModel categoryDetailsUpdateModel)
        {
            Category? category = await _dbContext.Categories.FirstOrDefaultAsync(c => c.CategoryId == categoryDetailsUpdateModel.CategoryId);

            if (category == null)
            {
                return null;
            }

            category.Name = categoryDetailsUpdateModel.Name;

            await _dbContext.SaveChangesAsync();

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "images", "CategoryImages", category.CategoryId.ToString() + ".png");

            if (categoryDetailsUpdateModel.CategoryImage != null && categoryDetailsUpdateModel.CategoryImage.Length > 0)
            {
                System.IO.File.Delete(filePath);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    categoryDetailsUpdateModel.CategoryImage.CopyTo(stream);
                }
            }

            return category;
        }

        public async Task<(bool, bool)> DeleteCategory(int categoryId)
        {
            Category? category = await _dbContext.Categories
                .Include(c => c.Products)
                .FirstOrDefaultAsync(c => c.CategoryId == categoryId);

            if (category == null)
            {
                return (false, false);
            }

            if (category.Products != null && category.Products.Count > 0)
            {
                return (true, false);
            }

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "images", "CategoryImages", category.CategoryId.ToString() + ".png");

            _dbContext.Categories.Remove(category);
            await _dbContext.SaveChangesAsync();

            System.IO.File.Delete(filePath);

            return (true, true);
        }
    }
}
