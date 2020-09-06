using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication4.Models;

namespace WebApplication4.Pages
{


    public class IndexModel : PageModel
    {
        [BindProperty]
        public Account Account { get; set; }

        [BindProperty]
        public String Message { get; set; }

        [BindProperty]
        public bool RememberMe { get; set; }


        private readonly UserManager<Account> UserManager;

        private readonly SignInManager<Account> SignInManager;

        private readonly RoleManager<AppRole> RoleManager;



        public IndexModel(UserManager<Account> _UserManager,

         SignInManager<Account> _SignInManager,

         RoleManager<AppRole> _RoleManager)
        {
            UserManager = _UserManager;
            SignInManager = _SignInManager;
            RoleManager = _RoleManager;
        }

        public async Task OnGetAsync()
        {
            if(!await RoleManager.RoleExistsAsync("Admin"))
                 await RoleManager.CreateAsync(new AppRole("Admin"));

            if (!await RoleManager.RoleExistsAsync("UsualUser"))
                await RoleManager.CreateAsync(new AppRole("UsualUser"));


                Account = new Account();
            
        }

        public async Task<IActionResult> OnGetLogout()
        {
            await SignInManager.SignOutAsync();
            return RedirectToPage("index");
        }

        
        public async Task<IActionResult> OnPost()
        {
            
            var account = await Login(Account.UserName, Account.Password);

            if (account == null)
            {
                Message = "نام کاربری یا رمزعبور اشتباه است";
                return Page();
            }



            if (!account.IsActive)
            {
                Message = "کاربر توسط ادمین فعال نشده است";
                return Page();
            }   

            else if (account.IsAdmin)
                return RedirectToPage("AdminHome", account);

            else
                return RedirectToPage("Home", account);


        }



        private void CheckRememberMe(string username, string password)
        {
            if (RememberMe)
            {
                Response.Cookies.Append("username", username);
                Response.Cookies.Append("password", password);
            }
        }
        
        private async Task<Account> Login(string username, string password)
        {


            var result = await SignInManager.PasswordSignInAsync(username, password, RememberMe, false);

            CheckRememberMe(username, password);


            if (result.Succeeded)
                return UserManager.FindByNameAsync(username).Result;


            return null;
        }

    }
}
