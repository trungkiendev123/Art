using System;
using System.Collections.Generic;

namespace ArtisShopping.Models
{
    public partial class Account
    {
        public Account()
        {
            Carts = new HashSet<Cart>();
            Favourites = new HashSet<Favourite>();
            Products = new HashSet<Product>();
            Reviews = new HashSet<Review>();
        }

        public int Id { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? FullName { get; set; }
        public int? Role { get; set; }

        public virtual ICollection<Cart> Carts { get; set; }
        public virtual ICollection<Favourite> Favourites { get; set; }
        public virtual ICollection<Product> Products { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
    }
}
