﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KezyMart.Models
{
    public class ShoppingCart
    {
        public int Id { get; set; }
        public IEnumerable<CartItem> CartItems { get; set; }
    }
}
