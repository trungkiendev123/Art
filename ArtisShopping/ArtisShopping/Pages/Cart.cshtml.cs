using ArtisShopping.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace ArtisShopping.Pages
{
    public class CartModel : PageModel
    {
        private readonly artisContext _context;

        public CartModel(artisContext context)
        {
            _context = context;
        }

        public List<Cart> CartItems { get; set; }
        public long? TotalOrderValue { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                // Lưu lại trang hiện tại để redirect sau khi đăng nhập thành công
                HttpContext.Session.SetString("ReturnUrl", "/Cart");

                // Redirect đến trang login
                return RedirectToPage("/Login");
            }

            CartItems = await _context.Carts
                .Include(c => c.Product)
                .Where(c => c.AccountId == HttpContext.Session.GetInt32("UserId").Value)
                .ToListAsync();
            TotalOrderValue = CartItems.Sum(c => c.Product.Price);
            return Page();
        }

       

        public async Task<IActionResult> OnPostRemoveFromCartAsync(int cartId)
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                // Lưu lại trang hiện tại để redirect sau khi đăng nhập thành công
                HttpContext.Session.SetString("ReturnUrl", "/Cart");

                // Redirect đến trang login
                return RedirectToPage("/Login");
            }
            var cart = await _context.Carts.FindAsync(cartId);
            if (cart != null)
            {
                _context.Carts.Remove(cart);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Sản phẩm đã được xóa khỏi giỏ hàng";
            }

            return RedirectToPage();
        }
    }

}
