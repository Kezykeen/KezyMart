using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using KezyMart.Models;

namespace KezyMart.ViewModels
{
    public class ShoppingCartViewModel
    {
        [Key]
        public int Id { get; set; }
        public List<CartItem> CartItems { get; set; }
        public decimal CartTotal { get; set; }
    }
}