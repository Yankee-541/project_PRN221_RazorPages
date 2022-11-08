using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebRazor.Models;

namespace WebRazor.Pages.Admin.Product
{
    public class IndexModel : PageModel
    {
        private readonly WebRazor.Models.PRN221DBContext _context;

        public IndexModel(WebRazor.Models.PRN221DBContext context)
        {
            _context = context;
        }

        public List<Category> Categories { get; set; }
        public IList<Models.Product> Product { get;set; } = default!;

        public async Task OnGetAsync()
        {

            Categories = _context.Categories.ToList();
            int totalProduct = getTotalProducts();
            countPages = (int)Math.Ceiling((double)totalProduct / pageSize);

            if (currentPage < 1)
            {
                currentPage = 1;
            }
            if (currentPage > countPages)
            {
                currentPage = countPages;
            }
            if (!string.IsNullOrEmpty(search))
            {
                var qr = (from a in _context.Products orderby a.ProductId ascending select a).Where(a => a.ProductName.Contains(search)).Skip((currentPage - 1) * pageSize).Take(pageSize);
                Product = await qr.ToListAsync();
            }
            else if (categoryChoose > 0)
            {
                var qr = (from p in _context.Products select p).Where(p => p.CategoryId == categoryChoose)
                    .Skip((currentPage - 1) * pageSize).Take(pageSize);                  
                Product = await qr.ToListAsync();
            }
            else if (categoryChoose > 0 && !string.IsNullOrEmpty(search))
            {
                var qr = (from p in _context.Products select p).Where(p => p.CategoryId == categoryChoose && p.ProductName.Contains(search))
                    .Skip((currentPage - 1) * pageSize).Take(pageSize);
                Product = await qr.ToListAsync();
            }
            else
            {
                var qr = (from a in _context.Products orderby a.ProductId ascending select a).Skip((currentPage - 1) * pageSize).Take(pageSize);
                Product = await qr.ToListAsync();
            }
        }

        private int getTotalProducts()
        {
            var list = (from p in _context.Products select p).ToList();

            if (!String.IsNullOrEmpty(search))
            {
                return list.Where(p => p.ProductName.ToLower().Contains(search.ToLower())).ToList().Count;
            }
            else if (categoryChoose > 0)
            {
                return list.Where(p => p.CategoryId == categoryChoose).ToList().Count;
            }
            else
            {
                return list.Count;
            }
        }

        public const int pageSize = 10;
        [BindProperty(SupportsGet = true, Name = "currentPage")]
        public int currentPage { get; set; }
        [BindProperty(SupportsGet = true)]
        public string search { get; set; }
        [BindProperty(SupportsGet = true)]
        public int categoryChoose { get; set; }
        public int countPages { get; set; }
    }
}
