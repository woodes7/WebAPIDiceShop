using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel
{
    public class OrderRequest
    {
        public List<OrderItem> Items { get; set; } = new();
        public decimal TotalAmount { get; set; }
        public BillingaddressDto BillingAddress { get; set; } = new(); // Usa tu DTO real
        public string? CouponCode { get; set; }
    }

}
