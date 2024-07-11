using ArtisShopping.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace ArtisShopping.Pages
{
    public class RegisterModel : PageModel
    {
        private readonly artisContext _context;

        public RegisterModel(artisContext context)
        {
            _context = context;
        }

        [BindProperty]
        public string FullName { get; set; }

        [BindProperty]
        public string Email { get; set; }

        [BindProperty]
        public string Password { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (await _context.Accounts.AnyAsync(u => u.Email == Email))
            {
                TempData["ErrorMessage"] = "Email đã tồn tại";
                return Page();
            }

            var user = new Account
            {
                FullName = FullName,
                Email = Email,
                Password = Password,
                Role = 0 // Default role
            };

            _context.Accounts.Add(user);
            await _context.SaveChangesAsync();

            HttpContext.Session.SetInt32("UserId", user.Id);
            HttpContext.Session.SetString("UserEmail", user.Email);
            HttpContext.Session.SetInt32("UserRole", user.Role.GetValueOrDefault());

            TempData["SuccessMessage"] = "Đăng kí thành công";
            return RedirectToPage("/Login");
        }
    }
}
