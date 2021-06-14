using KezyMart.Models;
using Microsoft.EntityFrameworkCore;

namespace KezyMart.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext _db;

        public CategoryRepository(AppDbContext context)
        {
            _db = context;
        }

        public DbSet<Category> GetCategory()
        {
            return _db.Categories;
        }
    }
}