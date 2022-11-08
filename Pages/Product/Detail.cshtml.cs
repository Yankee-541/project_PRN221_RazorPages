using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebRazor.Models;

namespace WebRazor.Pages.Product
{
    public class DetailModel : PageModel
    {
        private readonly PRN221DBContext dbContext;

        public DetailModel(PRN221DBContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [BindProperty]
        public Models.Product Product { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Product = await dbContext.Products.FirstOrDefaultAsync(m => m.ProductId == id);
            var cat = await dbContext.Categories.ToListAsync();

            if (Product == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Product = await dbContext.Products.FirstOrDefaultAsync(m => m.ProductId == id);
            var cat = await dbContext.Categories.ToListAsync();

            if (Product == null)
            {
                return NotFound();
            }
            return Page();
        }

        
    }
}
