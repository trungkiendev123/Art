namespace ArtisShopping.Models
{
    public class CategoryWithProductCount
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int ProductCount { get; set; }

        public string image { get; set; }
    }
}
