using KezyMart.Models;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace KezyMart.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _db;
       
        public UnitOfWork(AppDbContext context)
        {
            _db = context;
        }

        public void Complete()
        {
            _db.SaveChanges();
        }

        public EntityEntry Entry<T>(T entity)
        {
            return _db.Entry(entity);
        }

        public void Dispose()
        {
            _db.Dispose();
        }


    }
}