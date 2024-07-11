using ArtisShopping.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace ArtisShopping.Pages
{
    public class ProductDetailModel : PageModel
    {
        private readonly artisContext _context;

        public ProductDetailModel(artisContext context)
        {
            _context = context;
        }

        public Product Product { get; set; }
        public List<Review> Reviews { get; set; }

        public async Task OnGetAsync(int id)
        {
            Product = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Reviews)
                .ThenInclude(r => r.Account)
                .FirstOrDefaultAsync(m => m.Id == id);

            Reviews = await _context.Reviews
                .Include(r => r.Account)
                .Where(r => r.ProductId == id)
                .ToListAsync();
        }
        public async Task<IActionResult> OnGetAddToCartAsync(int productId)
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                // Lưu lại trang hiện tại để redirect sau khi đăng nhập thành công
                HttpContext.Session.SetString("ReturnUrl", "/ProductDetail?id="+ productId);

                // Redirect đến trang login
                return RedirectToPage("/Login");
            }
            var product = await _context.Products.FindAsync(productId);
            if (product == null)
            {
                TempData["ErrorMessage"] = "Sản phẩm không tồn tại.";
                return RedirectToPage("/ProductDetail", new { id = productId });
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
                return RedirectToPage("/ProductDetail", new { id = productId });
            }

            // Kiểm tra sản phẩm có trong order và không phải trạng thái hoàn tiền hoặc đã hủy
            var existingOrderItem = await _context.OrderDetails.Include(x => x.Order).Where(x => x.ProductId == productId && x.Order.Status != 4).FirstOrDefaultAsync();



            if (existingOrderItem != null)
            {
                TempData["ErrorMessage"] = "Sản phẩm đã có trong đơn hàng đang triển khai";
                return RedirectToPage("/ProductDetail", new { id = productId });
            }

            //// Thêm sản phẩm vào giỏ hàng
            var cartItem = new Cart { AccountId = accID, ProductId = productId };
            _context.Carts.Add(cartItem);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Thêm sản phẩm vào giỏ hàng thành công";
            return RedirectToPage("/ProductDetail", new { id = productId });
        }
    }
}
