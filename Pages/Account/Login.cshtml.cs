using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using WebRazor.Helpers;
using WebRazor.Models;

namespace WebRazor.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly PRN221DBContext dbContext;

        public LoginModel(PRN221DBContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [BindProperty]
        public Models.Account Account { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            string password = Password_encryption.HashPassWord(Account.Password);
            var acc = await dbContext.Accounts
                .SingleOrDefaultAsync(a => a.Email.Equals(Account.Email) && a.Password.Equals(password));

            if (acc == null)
            {
                ViewData["msg"] = "Email/ Password is wrong";
                return Page();
            }
            //if (acc.CustomerId != null)
            //{
            //    var customer = await dbContext.Customers.SingleOrDefaultAsync(c => c.CustomerId == acc.CustomerId && c.Active == true);
            //    if (customer == null)
            //    {
            //        ViewData["msg"] = "Account is not active. Please contact admin for help!";
            //        return Page();
            //    }
            //}
            if (acc.Role == 2)
            {
                acc.Customer = null;
                HttpContext.Session.SetString("CustSession", JsonSerializer.Serialize(acc));
                return RedirectToPage("/index");
            }
            else if (acc.Role == 1)
            {
                HttpContext.Session.SetString("Admin", JsonSerializer.Serialize(acc));
            }
            return Redirect("/Admin/Dashboard");
        }

        public IActionResult OnGetLogout()
        {
            HttpContext.Session.Remove("CustSession");
            HttpContext.Session.Remove("Admin");

            return RedirectToPage("~/index");
        }
    }
}
