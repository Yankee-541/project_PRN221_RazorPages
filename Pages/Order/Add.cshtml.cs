using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using WebRazor.Models;

namespace WebRazor.Pages.Order
{
    public class AddModel : PageModel
    {
        private readonly PRN221DBContext dbContext;

        public AddModel(PRN221DBContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [BindProperty]
        public Models.Account Auth { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (HttpContext.Session.GetString("CustSession") == null)
            {
                return Redirect("/Account/Login");
            }

            Auth = JsonSerializer.Deserialize<Models.Account>(HttpContext.Session.GetString("CustSession"));

            if (Auth == null)
            {
                return Redirect("/Account/Login");
            }

            Models.Product product = (await dbContext.Products.FirstOrDefaultAsync(p => p.ProductId == id));

            if (product == null || product.UnitsInStock == 0)
            {
                TempData["fail"] = "Quantity = 0";
            }
            else

                try
                {
                    Models.Order order = new Models.Order();
                    order.CustomerId = Auth.CustomerId;
                    order.OrderDate = DateTime.Now;

                    await dbContext.Orders.AddAsync(order);
                    await dbContext.SaveChangesAsync();
                    order = await dbContext.Orders.OrderBy(o => o.OrderDate).LastOrDefaultAsync();
                    

                    OrderDetail od = new OrderDetail();
                    od.OrderId = order.OrderId;
                    od.ProductId = (int)id;
                    od.Quantity = 1;
                    od.UnitPrice = (decimal)product.UnitPrice;
                    od.Discount = 0;

                    await dbContext.OrderDetails.AddAsync(od);
                    await dbContext.SaveChangesAsync();

                    TempData["success"] = "Order successfull";
                }
                catch (Exception e)
                {
                    TempData["fail"] = e.Message;
                }

            return Redirect("/Product/Detail/" + id);
        }
    }
}
