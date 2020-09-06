using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebApplication4.Models;

namespace WebApplication4.Pages
{
    public class EditProfileModel : PageModel
    {
        [BindProperty]
        public  Account Account { get; set; }

        [BindProperty]
        public Account EditedAccount { get; set; }

        [BindProperty]
        public DateHelper DateHelper { get; set; }

        [BindProperty]
        public string Message { get; set; }

        private readonly DatabaseContext db;

        private readonly UserManager<Account> UM;


        public EditProfileModel(DatabaseContext _db, UserManager<Account> _UserManager)
        {
            db = _db;
            UM = _UserManager;
        }
        public void OnGet(Account account)
        {
            Account = account;
            EditedAccount = new Account();
            DateHelper = new DateHelper();

        }

        public IActionResult OnPostCancel(int id)
        {
            return RedirectToPage("Profile", UM.FindByIdAsync(id.ToString()).Result);
        }

        public async Task<IActionResult> OnPost(string email)
        {

            var account = UM.FindByEmailAsync(email).Result;
            AssignDatas(account, EditedAccount);

            Message = Tools.CheckValidation(email, account.Password, account.BornDate, UM, true);
            if (Message != null)
                return Page();

            try
            {
                await db.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Message = e.Message;
                return RedirectToPage(UM.FindByEmailAsync(email).Result);
            }
           

            return RedirectToPage("Profile", account);
        }

        private void AssignDatas(Account account, Account editedAccount)
        {
            account.UserName = editedAccount.UserName;
            account.NormalizedUserName = editedAccount.UserName.Normalize();

            account.Email = editedAccount.Email;
            account.NormalizedEmail = editedAccount.Email.Normalize();


            if (editedAccount.Password != null)
                if(Tools.CheckPassValidation(editedAccount.Password))
                    account.Password = editedAccount.Password;

            if (DateHelper.DateString != null)
            {
                int[] Date = Array.ConvertAll(DateHelper.DateString.Split("/"), s => int.Parse(s));
                account.BornDate = new DateTime(Date[0], Date[1], Date[2], new PersianCalendar());
            }

        }
    }
}