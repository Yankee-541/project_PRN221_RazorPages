using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using WebRazor.Models;

namespace WebRazor.Pages.Account
{
    public class ProfileModel : PageModel
    {
        private readonly PRN221DBContext dbContext;

        [BindProperty]
        public Models.Account Auth { get; set; }

        public ProfileModel(PRN221DBContext dbContext)
        {
            this.dbContext = dbContext;

        }

        public async Task<IActionResult> OnGetAsync()
        {
            if (HttpContext.Session.GetString("CustSession") == null)
            {
                return Redirect("/Account/Login");
            }

            Auth = JsonSerializer.Deserialize<Models.Account>(HttpContext.Session.GetString("CustSession"));

            if (Auth == null)
            {
                return NotFound();
            } else
            {
                Auth.Customer = await dbContext.Customers.FirstOrDefaultAsync(c => c.CustomerId == Auth.CustomerId);
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                Models.Account auth = JsonSerializer.Deserialize<Models.Account>(HttpContext.Session.GetString("CustSession"));

                var acc = await dbContext.Accounts.FirstOrDefaultAsync(a => a.AccountId == auth.AccountId);

                if (acc.CustomerId != null)
                {
                    acc.Customer = await dbContext.Customers.FirstOrDefaultAsync(c => c.CustomerId == acc.CustomerId);
                }

                acc.Email = Auth.Email;
                acc.Customer.CompanyName = Auth.Customer.CompanyName;
                acc.Customer.ContactName = Auth.Customer.ContactName;
                acc.Customer.ContactTitle = Auth.Customer.ContactTitle;
                acc.Customer.Address = Auth.Customer.Address;

                auth.Email = Auth.Email;

                await dbContext.SaveChangesAsync();

                HttpContext.Session.Remove("CustSession");
                HttpContext.Session.SetString("CustSession", JsonSerializer.Serialize(auth));

                ViewData["success"] = "Update Successfull";
                return Page();

            } catch (Exception e)
            {
                ViewData["fail"] = e.Message;
                return Page();
            }

        }

    }
}
