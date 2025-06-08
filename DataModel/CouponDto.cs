#nullable disable
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace DataModel
{
    public class CouponDto
    {
        public string Code { get; set; }           
        public decimal DiscountAmount { get; set; } 
        public DateTime ExpirationDate { get; set; } 
        public int Quantity { get; set; } = 1;     
        public int? UserId { get; set; }         
    }
}
