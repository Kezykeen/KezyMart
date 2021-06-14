using System.Linq;
using KezyMart.Models;
using Microsoft.EntityFrameworkCore;

namespace KezyMart.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _db;

        public ProductRepository(AppDbContext context)
        {
            _db = context;
        }
        public Product FindProduct(int? id)
        {
            return _db.Products.Find(id);
        }

        public IQueryable<Product> GetProductWithCategory()
        {
            return _db.Products.Include(c=> c.Category);
        }

        public void Add(Product product)
        {
            _db.Products.Add(product);
        }

        public void Remove(Product product)
        {
            _db.Products.Remove(product);
        }
    }
}