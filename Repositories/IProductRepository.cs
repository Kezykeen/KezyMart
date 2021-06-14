using System.Linq;
using KezyMart.Models;

namespace KezyMart.Repositories
{
    public interface IProductRepository
    {
        void Add(Product product);
        Product FindProduct(int? id);
        IQueryable<Product> GetProductWithCategory();
        void Remove(Product product);
    }
}