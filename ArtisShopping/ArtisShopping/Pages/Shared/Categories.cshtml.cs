using ArtisShopping.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace ArtisShopping.Pages.Shared
{
    public class CategoriesModel : ViewComponent
    {
        private readonly artisContext _context;

        public CategoriesModel(artisContext context)
        {
            _context = context;
        }
        public IEnumerable<Category> categories { get; set; }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            categories = await _context.Categories.ToListAsync();
            return View(categories);
        }
    }
}
