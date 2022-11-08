using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebRazor.Models;

namespace WebRazor.Pages.Admin.Order
{

    public class OrderDetailsModel : PageModel
    {
        private readonly PRN221DBContext dbContext;

        public OrderDetailsModel(PRN221DBContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public Models.Order order { get; set; }
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (HttpContext.Session.GetString("Admin") == null)
            {
                return Forbid();
            }

            if (id == null)
            {
                return NotFound();
            }

            order = await dbContext.Orders.FirstOrDefaultAsync(o => o.OrderId == id);

            if (order == null)
            {
                return NotFound();
            }

            order.OrderDetails = (from od in dbContext.OrderDetails select od)
                .Where(o => o.OrderId == order.OrderId)
                .ToList();
            foreach (var o in order.OrderDetails)
            {
                o.Product = await dbContext.Products.FirstOrDefaultAsync(p => p.ProductId == o.ProductId);
            }
            return Page();
        }
    }
}
