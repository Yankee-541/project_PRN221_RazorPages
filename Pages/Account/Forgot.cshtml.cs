using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Net.Mail;
using WebRazor.Helpers;
using WebRazor.Models;

namespace WebRazor.Pages.Account
{
    public class ForgotModel : PageModel
    {
        private readonly PRN221DBContext dbContext;

        public ForgotModel(PRN221DBContext dbContext)
        {
            this.dbContext = dbContext;
        }
        [BindProperty]
        public string email { get; set; }
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (String.IsNullOrEmpty(email))
            {
                ViewData["msg"] = "Please enter your email to get password!";
                return Page();
            }

            var account = await dbContext.Accounts.FirstOrDefaultAsync(a => a.Email.Equals(email));
            if (account == null)
            {
                ViewData["msg"] = "Not found email in for system, please check again!";
                return Page();
            }

            String passwordGenerate = Password_encryption.GeneratePassword(8);
            account.Password = Password_encryption.HashPassWord(passwordGenerate);

            dbContext.Update(account);
            if (await dbContext.SaveChangesAsync() <= 0)
            {
                ViewData["msg"] = "System error, please try again!";
                return Page();
            }

            string bodyMail = "Hello! Your new password is: " + passwordGenerate + "";

            var body = $@"<p>Click below link to change password: </p><br/><a href='https://localhost:7090/Account/ResetPassword?email={email}'>Click here to change password</a> "+ bodyMail;
            SendMailHelper.SendMail(email, body);
            ViewData["msg"] = "Please check you email!";
            return Page();

        }
    }
}
