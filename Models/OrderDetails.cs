using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using KezyMart.Models;

namespace KezyMart.Models
{
    public class OrderDetails
    {
        [Key]
        public int Id { get; set; }
        public int Quantity { get; set; }
        public int UnitPrice { get; set; }
        [ForeignKey("Product")]
        public int ProductId { get; set; }
        [ForeignKey("Order")]
        public int OrderId { get; set; }
        public virtual Product Product { get; set; }
        public virtual Order Order { get; set; }
    }
}