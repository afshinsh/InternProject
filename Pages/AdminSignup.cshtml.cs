
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using WebApplication4.Models;

namespace WebApplication4.Pages
{
    public class AdminSignupModel : PageModel
    {

        [BindProperty]
        public Account Account { get; set; }

        public IEnumerable<Account> Accounts { get; set; }

        [BindProperty]
        public string Message { get; set; }


        [BindProperty]
        public DateHelper DateHelper { get; set; }


        private readonly DatabaseContext db;


        private readonly UserManager<Account> UserManager;



        public AdminSignupModel(DatabaseContext _db, UserManager<Account> _UserManager)
        {
            db = _db;
            UserManager = _UserManager;
        }

        public void OnGet()
        {
            Account = new Account();
        }

        public IActionResult OnPostCancel()
        {
            return RedirectToPage("index");
        }

        public async Task<IActionResult> OnPost()
        {

            Message = Tools.CheckValidation(Account.Email, Account.Password, Account.BornDate, UserManager, false);
            if (Message != null)
                return Page();

            if (DateHelper.DateString != null)
            {
                int[] Date = Array.ConvertAll(DateHelper.DateString.Split("/"), s => int.Parse(s));
                Account.BornDate = new DateTime(Date[0], Date[1], Date[2], new PersianCalendar());

            }

            Account.IsBan = false;
            Account.IsAdmin = true;
            Account.IsActive = false;

            var Result = await UserManager.CreateAsync(Account, Account.Password);

          

            if (Result.Succeeded)
                await UserManager.AddToRoleAsync(Account, "Admin");
            else
            {
                Message = Result.Errors.ToString();
                return Page();
            }

            try
            {
                await db.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Message = e.Message;
                return Page();
            }

            return RedirectToPage("index");
        }



    }
}