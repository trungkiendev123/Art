using System;
using System.Collections.Generic;

namespace ArtisShopping.Models
{
    public partial class Cart
    {
        public int Id { get; set; }
        public int? AccountId { get; set; }
        public int? ProductId { get; set; }

        public virtual Account? Account { get; set; }
        public virtual Product? Product { get; set; }
    }
}
