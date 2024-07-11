using ArtisShopping.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace ArtisShopping.Pages
{
    public class FilterProductModel : PageModel
    {
        private readonly artisContext _context;

        public FilterProductModel(artisContext context)
        {
            _context = context;
        }

        public IList<Product> Products { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        [BindProperty]
        public int CurrentCategoryId { get; set; }

        public async Task OnGetAsync(int? categoryId, int pageIndex = 1, int pageSize = 8)
        {
            IQueryable<Product> productIQ = from p in _context.Products
                                            select p;

            if (categoryId.HasValue)
            {
                productIQ = productIQ.Where(p => p.CategoryId == categoryId);
                CurrentCategoryId = categoryId.Value;
            }

            int totalProducts = await productIQ.CountAsync();
            TotalPages = (int)Math.Ceiling(totalProducts / (double)pageSize);

            Products = await productIQ
                            .Include(p => p.Category)
                            .Include(p => p.Seller)
                            .Skip((pageIndex - 1) * pageSize)
                            .Take(pageSize)
                            .ToListAsync();
            CurrentPage = pageIndex;
        }
        public async Task<IActionResult> OnGetAddToCartAsync(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                // Lưu lại trang hiện tại để redirect sau khi đăng nhập thành công
                HttpContext.Session.SetString("ReturnUrl", "/FilterProduct?categoryId="+ product.CategoryId);

                // Redirect đến trang login
                return RedirectToPage("/Login");
            }
            
            if (product == null)
            {
                TempData["ErrorMessage"] = "Sản phẩm không tồn tại.";
                return RedirectToPage("/FilterProduct", new { categoryId = product.CategoryId });
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
                return RedirectToPage("/FilterProduct", new { categoryId = product.CategoryId });
            }

            // Kiểm tra sản phẩm có trong order và không phải trạng thái hoàn tiền hoặc đã hủy
            var existingOrderItem = await _context.OrderDetails.Include(x => x.Order).Where(x => x.ProductId == productId && x.Order.Status != 4).FirstOrDefaultAsync();



            if (existingOrderItem != null)
            {
                TempData["ErrorMessage"] = "Sản phẩm đã có trong đơn hàng đang triển khai";
                return RedirectToPage("/FilterProduct", new { categoryId = product.CategoryId });
            }

            //// Thêm sản phẩm vào giỏ hàng
            var cartItem = new Cart { AccountId = accID, ProductId = productId };
            _context.Carts.Add(cartItem);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Thêm sản phẩm vào giỏ hàng thành công";
            return RedirectToPage("/FilterProduct", new { categoryId = product.CategoryId });
        }
    }
}
