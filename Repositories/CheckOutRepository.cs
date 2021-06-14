using System.Linq;
using KezyMart.Models;

namespace KezyMart.Repositories
{
    public class CheckOutRepository : ICheckOutRepository
    {
        private readonly AppDbContext _db;

        public CheckOutRepository(AppDbContext context)
        {
            _db = context;
        }

        public bool CheckOrderValidity(int id)
        {
            // Validate if the order exists
            return _db.Orders.Any(
                o => o.OrderId == id);
        }

        public Order GetOrder(int id)
        {
            return _db.Orders.Find(id);
        }
    }
}