using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebRazor.Models;

namespace WebRazor.Pages.Admin.Customers
{
    public class IndexModel : PageModel
    {
        private readonly PRN221DBContext _context;
        [BindProperty(SupportsGet = true)]
        public string search { get; set; }
        public IndexModel(PRN221DBContext context)
        {
            _context = context;
        }
        [BindProperty(SupportsGet = true, Name = "currentPage")]
        public int currentPage { get; set; }

        [BindProperty(SupportsGet = true)]
        public int month { get; set; }
        public int totalPages { get; set; }

        public const int pageSize = 5;

        public int totalCustomer { get; set; }

        public List<Models.Customer> Customers { get; set; }

        public IActionResult OnGet()
        {
            if (HttpContext.Session.GetString("Admin") == null)
            {
                return Forbid();
            }
            if (currentPage < 1)
            {
                currentPage = 1;
            }
            totalCustomer = getTotalCustomer();

            totalPages = (int)Math.Ceiling((double)totalCustomer / pageSize);

            Customers = getAllCustomer();

            return Page();
        }

        private int getTotalCustomer()
        {
            var list = _context.Customers.ToList();
            if (list.Count == 0)
            {
                return 0;
            }

            if (month < 1)
            {
                if (string.IsNullOrEmpty(search))
                {
                    return list.Count;
                }
                return list.Where(c => c.ContactName.ToLower().Contains(search.ToLower())).ToList().Count;
            }
            else
            {

                if (string.IsNullOrEmpty(search))
                {
                    return list.Where(c => c.CreateDate.Value.Month == month)
                   .ToList().Count;
                }
                return list.Where(c => c.CreateDate.Value.Month == month
                        && c.ContactName.ToLower().Contains(search.ToLower()))
                           .ToList().Count;
            }
        }

        private List<Models.Customer> getAllCustomer()
        {
            var list = _context.Customers.ToList();
            if (list.Count == 0)
            {
                return new List<Models.Customer>();
            }
            if (month < 1)
            {
                if (string.IsNullOrEmpty(search))
                {
                    return list.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
                }
                return list.Where(c => c.ContactName.ToLower().Contains(search.ToLower())).Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
            }
            else
            {

                if (string.IsNullOrEmpty(search))
                {
                    return list.Where(c => c.CreateDate.Value.Month == month)
                    .Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
                }
                return list.Where(c => c.CreateDate.Value.Month == month
                        && c.ContactName.ToLower().Contains(search.ToLower()))
                            .Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
            }
        }
    }
}
