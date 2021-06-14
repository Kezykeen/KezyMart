using System.Collections.Generic;
using System.Linq;

namespace KezyMart.Models
{
    public static class ExtensionMethods
    {
        public static int Total(this ShoppingCart cartParam)
        {
            int total;
            if (cartParam == null)
            {
                total = 0;
            }
            else
            {
                total = cartParam.CartItems.Aggregate(0,
                    (current, cartItem) => current + cartItem.Product.Price * cartItem.Count);
            }

            return total;
        }

        public static IEnumerable<CartItem> FilterByPrice(this ShoppingCart cart, decimal minimumPrice)
        {
            return cart.CartItems.Where(cartItem => (cartItem?.Product.Price ?? 0) >= minimumPrice);
        }
    }
}
