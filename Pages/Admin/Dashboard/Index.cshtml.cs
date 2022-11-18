using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using WebRazor.Models;
using Microsoft.EntityFrameworkCore;

namespace WebRazor.Pages.Admin.Dashboard
{
    public class IndexModel : PageModel
    {
        public PRN221DBContext db;
        public double wsale { get; set; }
        public long totalOrder { get; set; }
        public long totalCus { get; set; }
        public long totalGuest { get; set; }

        public IndexModel(PRN221DBContext dBContext)
        {
            db = dBContext;
        }
        public void OnGet()
        {
            decimal wsales = 0;
            DateTime from = DateTime.Now.AddDays(-7);
            List<Models.Order> lo = db.Orders.Include(o => o.OrderDetails)
                .Where(o => o.ShippedDate >= from && o.ShippedDate <= DateTime.Now).ToList()
                ;

            lo.ForEach(o =>
            {
                List<Models.OrderDetail> lod = o.OrderDetails.ToList();
                wsales += lod.Sum(o => o.Quantity * o.UnitPrice *(1 - (decimal)o.Discount));
            });
            ViewData["totalO"] = db.Orders.Count();
            ViewData["totalC"] = db.Customers.Count();
            ViewData["totalG"] = db.Accounts.Count();
            ViewData["newC"] = db.Customers.Where(o => o.CreateDate >= DateTime.Now.AddMonths(-1)).Count();
            ViewData["wsales"] = wsales;

            Dictionary<int, decimal> longs = db.Orders.Include(o => o.OrderDetails).ToList()
                .GroupBy(o => o.OrderDate.Value.Month)
                .ToDictionary(e => e.Key, e => e.Sum(e => e.OrderDetails.Sum(e => e.UnitPrice* e.Quantity *(1 - (decimal)e.Discount))));
            ViewData["data"] = SerializeObject(longs);
        }

        public HtmlString SerializeObject(object value)
        {
            using (var stringWriter = new StringWriter())
            using (var jsonWriter = new JsonTextWriter(stringWriter))
            {
                var serializer = new JsonSerializer
                {
                    // Let's use camelCasing as is common practice in JavaScript
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                };

                // We don't want quotes around object names
                jsonWriter.QuoteName = false;
                serializer.Serialize(jsonWriter, value);

                return new HtmlString(stringWriter.ToString());
            }
        }
    }
}
