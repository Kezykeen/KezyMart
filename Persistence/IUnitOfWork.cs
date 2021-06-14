using KezyMart.Repositories;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace KezyMart.Persistence
{
    public interface IUnitOfWork
    {
        void Complete();
        EntityEntry Entry<T>(T entity);
        void Dispose();
    }
}