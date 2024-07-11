using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ArtisShopping.Pages
{
    public class LogoutModel : PageModel
    {
        public IActionResult OnGet()
        {
            HttpContext.Session.Clear();
            TempData["SuccessMessage"] = "You have been logged out.";
            return RedirectToPage("/Index");
        }
    }
}
