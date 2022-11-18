using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebRazor.Helpers;
using WebRazor.Models;

namespace WebRazor.Pages.Account
{
    public class ResetPasswordUserModel : PageModel
    {
        private readonly PRN221DBContext dbContext;
        public ResetPasswordUserModel(PRN221DBContext dbContext)
        {
            this.dbContext = dbContext;
        }
        [BindProperty]
        public string NewPassword { get; set; }
        [BindProperty]
        public string RePassword { get; set; }
        [BindProperty]
        public string Email { get; set; }
        public void OnGetAsync(string email)
        {
            Email = email;
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (RePassword != NewPassword)
            {
                ViewData["msg-repassword"] = "Re-password not match";
                return Page();
            }
            var acc = await dbContext.Accounts.SingleOrDefaultAsync(a => a.Email.Equals(Email));
            try
            {
                acc.Password = Password_encryption.HashPassWord(NewPassword);
                dbContext.Accounts.Update(acc);
                await dbContext.SaveChangesAsync();
                return RedirectToPage("/Account/Login");
            }
            catch (Exception e)
            {
                ViewData["errMsg"] = "Can not change password";
                Console.WriteLine(e.StackTrace);
                return Page();
            }
        }
    }
}
