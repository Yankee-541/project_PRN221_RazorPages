using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using WebRazor.Models;

namespace WebRazor.Pages.Cart
{
    public class RemoveModel : PageModel
    {
        private readonly PRN221DBContext dbContext;

        public RemoveModel(PRN221DBContext dbContext)
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


            try
            {
                Dictionary<int, int> list;
                var session = HttpContext.Session.GetString("cart");

                if (session == null)
                {
                    list = new Dictionary<int, int>();
                }
                else
                {
                    list = JsonSerializer.Deserialize<Dictionary<int, int>>(session);
                }

                if ((list.Where(p => p.Key == id)).Count() != 0)
                {
                    list.Remove((int) id);
                }


                HttpContext.Session.SetString("cart", JsonSerializer.Serialize(list));
                TempData["success"] = "Add to cart successfull";
            }
            catch (Exception e)
            {
                TempData["fail"] = e.Message;
            }

            return Redirect("/Cart");
        }
    }
}
