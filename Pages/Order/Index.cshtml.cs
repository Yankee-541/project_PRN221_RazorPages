using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using WebRazor.Models;

namespace WebRazor.Pages.Order
{
    public class IndexModel : PageModel
    {
        private readonly PRN221DBContext dbContext;

        public IndexModel(PRN221DBContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [BindProperty]
        public Models.Account Auth { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Models.Account acc = JsonSerializer.Deserialize<Models.Account>(HttpContext.Session.GetString("CustSession"));

            Auth = await dbContext.Accounts.FirstOrDefaultAsync(c => c.AccountId == acc.AccountId);
            var cus = await dbContext.Customers.ToListAsync();
            var ord = await dbContext.Orders.ToListAsync();
            var ordDe = await dbContext.OrderDetails.ToListAsync();
            var pro = await dbContext.Products.ToListAsync();

            return Page();
        }


    }
}
