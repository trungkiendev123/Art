using System;
using System.Collections.Generic;

namespace ArtisShopping.Models
{
    public partial class Product
    {
        public Product()
        {
            Carts = new HashSet<Cart>();
            Favourites = new HashSet<Favourite>();
            OrderDetails = new HashSet<OrderDetail>();
            Reviews = new HashSet<Review>();
        }

        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Image { get; set; }
        public long? Price { get; set; }
        public string? ShortDescription { get; set; }
        public string? Description { get; set; }
        public int? NumberView { get; set; }
        public int? CategoryId { get; set; }
        public int? SellerId { get; set; }

        public virtual Category? Category { get; set; }
        public virtual Account? Seller { get; set; }
        public virtual ICollection<Cart> Carts { get; set; }
        public virtual ICollection<Favourite> Favourites { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
    }
}
