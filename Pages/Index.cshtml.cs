using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Linq;
using WebRazor.Models;

namespace WebRazor.Pages
{
    public class IndexModel : PageModel
    {
        private readonly PRN221DBContext dbContext;

        public IndexModel(PRN221DBContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [BindProperty]
        public List<Category> Categories { get; set; }
        public Dictionary<Models.Product, int> Cart { get; set; } = new Dictionary<Models.Product, int>();
        [BindProperty]
        public List<Models.Product> Products { get; set; } = new List<Models.Product>();

        [BindProperty]
        public List<Models.Product> BestSaleProducts { get; set; }

        [BindProperty]
        public List<Models.Product> NewProducts { get; set; }

        [FromQuery(Name = "id")]
        public string CatId { get; set; }

        public void OnGet()
        {
            Categories = dbContext.Categories.ToList();

            var products = dbContext.Products.ToList();

            if (CatId != null)
            {
                Products = dbContext.Products
                    .Where(p => p.CategoryId == Int32.Parse(CatId))
                    .ToList();
            }
            else
            {
                var idsBestSale = dbContext.OrderDetails
                    .GroupBy(d => d.ProductId)
                    .Select(g => new { ProductId = g.Key, Sum = g.Sum(d => d.Quantity) })
                    .OrderByDescending(o => o.Sum)
                    .Take(4);

                BestSaleProducts = new List<Models.Product>();
                foreach (var id in idsBestSale)
                {
                    BestSaleProducts.Add(products.First(p => p.ProductId == id.ProductId));
                }

                NewProducts = dbContext.Products
                    .OrderByDescending(p => p.ProductId).Take(4).ToList();

            }

            var cart = HttpContext.Session.GetString("cart");

            Dictionary<int, int> list;
            if (cart != null)
            {
                list = JsonSerializer.Deserialize<Dictionary<int, int>>(cart);
                foreach (var item in list)
                {
                    Models.Product product = dbContext.Products.FirstOrDefault(p => p.ProductId == item.Key);

                    Cart.Add(product, item.Value);
                }
            }
        }

    }
}
