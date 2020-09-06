using System;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication4.Models;

namespace WebApplication4.Pages
{
    public class EditBanModel : PageModel
    {

        [BindProperty]
        public Account Account { get; set; }

        [BindProperty]
        public String Message { get; set; }

        [BindProperty]
        public Account AdminAccount { get; set; }


        [BindProperty]
        public DateHelper StartDateHelper { get; set; }

        [BindProperty]
        public DateHelper FinishDateHelper { get; set; }

        public static string Email;

        private readonly UserManager<Account> UM;

        private readonly DatabaseContext db;


        public EditBanModel(DatabaseContext _db, UserManager<Account> _UserManager)
        {
            db = _db;
            UM = _UserManager;
        }

        public void OnGet(string email, int adminID)
        {
            Email = email;
            AdminAccount = UM.FindByIdAsync(adminID.ToString()).Result;

            Account =  UM.FindByEmailAsync(Email).Result;

            Account.IsBan = Account.CheckIsBan();
        }


        public IActionResult OnPostUnBan(int adminId)
        {
            Account = UM.FindByEmailAsync(Email).Result;

            Account.IsBan = false;
            Account.BanDateFinish = DateTime.Now;

            AdminAccount = UM.FindByIdAsync(adminId.ToString()).Result;

            try
            {
                db.SaveChanges();
            }
            catch (Exception e)
            {
                Message = e.Message;
                return Page();
            }
            return RedirectToPage("AdminHome"
                , AdminAccount);
        }


        public async Task<IActionResult> OnPost(int adminId)
        {
            var account = UM.FindByEmailAsync(Email).Result;
            
            AdminAccount = UM.FindByIdAsync(adminId.ToString()).Result;

            int[] StartDate = Array.ConvertAll(StartDateHelper.DateString.Split("/"), s => int.Parse(s));
            int[] FinishDate = Array.ConvertAll(FinishDateHelper.DateString.Split("/"), s => int.Parse(s));


            account.BanDateStart = new DateTime(StartDate[0], StartDate[1], StartDate[2], StartDateHelper.Hour
                , StartDateHelper.Minute, StartDateHelper.Second, new PersianCalendar());

            account.BanDateFinish = new DateTime(FinishDate[0], FinishDate[1], FinishDate[2], FinishDateHelper.Hour
                , FinishDateHelper.Minute, FinishDateHelper.Second, new PersianCalendar());





            try
            {
                await db.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Message = e.Message;
                return Page();
            }


            return RedirectToPage("AdminHome"
                , AdminAccount);
        }

    }
}