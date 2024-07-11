using ArtisShopping.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace ArtisShopping.Pages
{
    public class LoginModel : PageModel
    {
        private readonly artisContext _context;

        public LoginModel(artisContext context)
        {
            _context = context;
        }

        [BindProperty]
        public string Email { get; set; }

        [BindProperty]
        public string Password { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            var userEmailCheck = await _context.Accounts.SingleOrDefaultAsync(u => u.Email.Equals(Email));
            if(userEmailCheck == null)
            {
                TempData["ErrorMessage"] = "Email không tồn tại";
                return Page();
            }
            var userPasswordCheck = await _context.Accounts.SingleOrDefaultAsync(u => u.Email.Equals(Email) && u.Password.Equals(Password));
            if (userPasswordCheck != null)
            {
                HttpContext.Session.SetInt32("UserId", userPasswordCheck.Id);
                HttpContext.Session.SetString("UserEmail", userPasswordCheck.Email);
                HttpContext.Session.SetInt32("UserRole", (int)userPasswordCheck.Role);
                TempData["SuccessMessage"] = "Đăng Nhập Thành Công!";
                var returnUrl = HttpContext.Session.GetString("ReturnUrl");
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    HttpContext.Session.Remove("ReturnUrl");
                    return Redirect(returnUrl);
                }
                else
                {
                    return RedirectToPage("/Index");
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Sai mật khẩu cho email này";
                return Page();
            }
        }
    }
}
