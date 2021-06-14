using KezyMart.Models;

namespace KezyMart.Repositories
{
    public interface ICheckOutRepository
    {
        bool CheckOrderValidity(int id);
        Order GetOrder(int id);
    }
}