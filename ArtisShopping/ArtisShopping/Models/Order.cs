using System;
using System.Collections.Generic;

namespace ArtisShopping.Models
{
    public partial class Order
    {
        public Order()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        public int Id { get; set; }
        public int? AccountId { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public string? Note { get; set; }
        public string? ReceiveName { get; set; }
        public int? Status { get; set; }

        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
