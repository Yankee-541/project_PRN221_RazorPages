using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebRazor.Pages.Account
{
    public class LogoutModel : PageModel
    {
        public IActionResult OnGet()
        {
            HttpContext.Session.Remove("CustSession");
            HttpContext.Session.Remove("Admin");

            return RedirectToPage("/index");
        }
    }
}
