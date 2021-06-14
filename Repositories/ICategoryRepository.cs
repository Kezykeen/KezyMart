using KezyMart.Models;
using Microsoft.EntityFrameworkCore;

namespace KezyMart.Repositories
{
    public interface ICategoryRepository
    {
        DbSet<Category> GetCategory();
    }
}