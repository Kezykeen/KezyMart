using System;
using System.Collections.Generic;
using System.Linq;
using KezyMart.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KezyMart.Repositories
{
    public class ShoppingCartRepository : IShoppingCartRepository
    {
        private AppDbContext _db;
        public string ShoppingCartId { get; set; }

        public const string CartSessionKey = "CartId";

        public ShoppingCartRepository(AppDbContext context)
        {
            _db = context;
        }

        public ShoppingCartRepository() {}

        public string GetCartId(HttpContext context)
        {
            if (context.Session.GetString(CartSessionKey) != null)
                return context.Session.GetString(CartSessionKey);

            if (!string.IsNullOrWhiteSpace(context.User.Identity.Name))
            {
                context.Session.SetString(CartSessionKey, context.User.Identity.Name);
            }
            else
            {
                // Generate a new random GUID using System.Guid class.     
                Guid tempCartId = Guid.NewGuid();
                context.Session.SetString(CartSessionKey, tempCartId.ToString());
            }

            return context.Session.GetString(CartSessionKey);
        }

        public ShoppingCartRepository GetCart(HttpContext context)
        {
            var cart = new ShoppingCartRepository();
            cart.ShoppingCartId = cart.GetCartId(context);
            return cart;
        }

        public ShoppingCartRepository GetCart(Controller controller)
        {
            return GetCart(controller.HttpContext);
        }

        public int GetCount()
        {
            int? count = (from cartItems in _db.CartItems
                          where cartItems.CartId == ShoppingCartId
                          select (int?)cartItems.Count).Sum();

            return count ?? 0;
        }

        public int AddToCart(Product product)
        {
            CartItem cartItem = GetCartItem(product);

            if (cartItem == null)
            {
                //Create a new cart item if no cart item exists.
                cartItem = new CartItem
                {
                    ProductId = product.Id,
                    CartId = ShoppingCartId,
                    Count = 1,
                    DateCreated = DateTime.Now
                };

                _db.CartItems.Add(cartItem);
            }
            else
            {
                cartItem.Count++;
            }
            var itemCount = cartItem.Count;

            _db.SaveChanges();
            return itemCount;
        }

        private CartItem GetCartItem(Product product)
        {
            return _db.CartItems.SingleOrDefault(
                c => c.CartId == ShoppingCartId
                && c.ProductId == product.Id);
        }

        public List<CartItem> GetCartItems()
        {
            return _db.CartItems.Where(
                c => c.CartId == ShoppingCartId).ToList();
        }

        public int GetCartItemCount()
        {
            var count = _db.CartItems.Where(c => c.CartId == ShoppingCartId).ToArray().Length;

            return count;
        }

        public void CreateOrder(Order order)
        {
            int orderTotal = 0;

            var cartItems = GetCartItems();

            foreach (var item in cartItems)
            {
                var orderDetail = new OrderDetails
                {
                    ProductId = item.ProductId,
                    OrderId = order.OrderId,
                    UnitPrice = item.Product.Price,
                    Quantity = item.Count
                };

                orderTotal += (item.Count * item.Product.Price);

                _db.OrderDetails.Add(orderDetail);
            }

            order.Total = orderTotal;

            //Save Order
            _db.Orders.Add(order);
            _db.SaveChanges();

            EmptyCart();
        }

        public void MigrateCart(string userName)
        {
            var shoppingCart = GetCartItems();

            foreach (CartItem item in shoppingCart)
            {
                item.CartId = userName;
            }

            _db.SaveChanges();
        }

        public decimal GetTotal()
        {
            decimal? total = (from items in _db.CartItems
                              where items.CartId == ShoppingCartId
                              select (int?)items.Count *
                              items.Product.Price).Sum();

            return total ?? decimal.Zero;
        }

        public int RemoveFromCart(int id)
        {
            CartItem cartItem = GetCartById(id);

            int itemCount = 0;

            if (cartItem == null)
                return itemCount;

            if (cartItem.Count > 1)
            {
                cartItem.Count--;
                itemCount = cartItem.Count;
            }
            else
            {
                _db.CartItems.Remove(cartItem);
            }

            _db.SaveChanges();

            return itemCount;
        }

        public CartItem GetCartById(int id)
        {
            return _db.CartItems.Single(
                c => c.CartId == ShoppingCartId
                && c.RecordId == id);
        }

        public void EmptyCart()
        {
            var cartItems = GetCartItems();

            foreach (var cartItem in cartItems)
            {
                _db.CartItems.Remove(cartItem);
            }

            _db.SaveChanges();
        }

        public void Dispose()
        {
            if (_db == null)
                return;

            _db.Dispose();
            _db = null;
        }
    }
}