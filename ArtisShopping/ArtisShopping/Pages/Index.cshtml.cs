using ArtisShopping.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Security.Principal;
using static System.Net.Mime.MediaTypeNames;

namespace ArtisShopping.Pages
{
    public class IndexModel : PageModel
    {
        private readonly artisContext _context;

        public IndexModel(artisContext context)
        {
            _context = context;
        }

        public List<CategoryWithProductCount> CategoriesWithProductCount { get; set; }
        public List<Product> PopularProducts { get; set; }
        public List<Product> FavoriteProducts { get; set; }

        public async Task OnGetAsync()
        {
            // Danh sách Category và số lượng sản phẩm trong mỗi Category
            CategoriesWithProductCount = await _context.Categories
                .Select(c => new CategoryWithProductCount
                {
                    CategoryId = c.Id,
                    CategoryName = c.Name,
                    ProductCount = c.Products.Count(),
                    image = c.Image
                })
                .ToListAsync();

            // Sản phẩm có lượt view cao nhất (lấy 8 sản phẩm)
            PopularProducts = await _context.Products
                .OrderByDescending(p => p.NumberView)
                .Take(8)
                .ToListAsync();

            // Sản phẩm được nhiều người yêu thích nhất (lấy 8 sản phẩm)
            FavoriteProducts = await _context.Products
                .OrderByDescending(p => p.Favourites.Count)
                .Take(8)
                .ToListAsync();
        }
        public async Task<IActionResult> OnGetAddToCartAsync(int productId)
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                // Lưu lại trang hiện tại để redirect sau khi đăng nhập thành công
                HttpContext.Session.SetString("ReturnUrl", "/Index");

                // Redirect đến trang login
                return RedirectToPage("/Login");
            }
            var product = await _context.Products.FindAsync(productId);
            if (product == null)
            {
                TempData["ErrorMessage"] = "Sản phẩm không tồn tại.";
                return RedirectToPage("/Index");
            }

            // Lấy thông tin người dùng từ session hoặc context phù hợp
            var userEmail = HttpContext.Session.GetString("UserEmail");
            int accID = (int)HttpContext.Session.GetInt32("UserId");

            // Kiểm tra sản phẩm đã có trong giỏ hàng chưa
            var existingCartItem = await _context.Carts
                .FirstOrDefaultAsync(c => c.AccountId == accID && c.ProductId == productId);
            if (existingCartItem != null)
            {
                TempData["ErrorMessage"] = "Sản phẩm đã có trong giỏ hàng.";
                return RedirectToPage("/Index");
            }

            // Kiểm tra sản phẩm có trong order và không phải trạng thái hoàn tiền hoặc đã hủy
            var existingOrderItem = await _context.OrderDetails.Include(x => x.Order).Where(x => x.ProductId== productId && x.Order.Status != 4).FirstOrDefaultAsync();
        


            if (existingOrderItem != null)
            {
                TempData["ErrorMessage"] = "Sản phẩm đã có trong đơn hàng đang triển khai";
                return RedirectToPage("/Index");
            }

            //// Thêm sản phẩm vào giỏ hàng
            var cartItem = new Cart { AccountId = accID, ProductId = productId };
            _context.Carts.Add(cartItem);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Thêm sản phẩm vào giỏ hàng thành công";
            return RedirectToPage("/Index");
        }
    }
}