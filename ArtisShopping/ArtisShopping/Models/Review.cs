using System;
using System.Collections.Generic;

namespace ArtisShopping.Models
{
    public partial class Review
    {
        public int Id { get; set; }
        public int? AccountId { get; set; }
        public int? ProductId { get; set; }
        public DateTime? PostDate { get; set; }
        public string? Comment { get; set; }

        public virtual Account? Account { get; set; }
        public virtual Product? Product { get; set; }
    }
}
