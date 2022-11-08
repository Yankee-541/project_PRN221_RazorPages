using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebRazor.Models;

namespace WebRazor.Pages.Admin.Customers
{
    public class UpdateModel : PageModel
    {
        private readonly PRN221DBContext dbContext;

        public UpdateModel(PRN221DBContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IActionResult> OnGetAsync(string? id)
        {
            if (HttpContext.Session.GetString("Admin") == null)
            {
                return Forbid();
            }

            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var customer = await dbContext.Customers.SingleOrDefaultAsync(c => c.CustomerId.Equals(id));

            if (customer == null)
            {
                return BadRequest();
            }

            if (customer.Active == true)
            {
                customer.Active = false;
            }
            else
            {
                customer.Active = true;
            }

            dbContext.SaveChanges();

            return Redirect("index");
        }
    }
}
