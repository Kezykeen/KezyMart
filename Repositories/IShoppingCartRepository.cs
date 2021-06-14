using System.Collections.Generic;
using KezyMart.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KezyMart.Repositories
{
    public interface IShoppingCartRepository
    {
        int AddToCart(Product product);
        void CreateOrder(Order order);
        void Dispose();
        void EmptyCart();
        ShoppingCartRepository GetCart(HttpContext context);
        ShoppingCartRepository GetCart(Controller controller);
        CartItem GetCartById(int Id);
        string GetCartId(HttpContext context);
        int GetCartItemCount();
        List<CartItem> GetCartItems();
        int GetCount();
        decimal GetTotal();
        void MigrateCart(string userName);
        int RemoveFromCart(int Id);
    }
}