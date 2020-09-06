using System;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication4.Models;

namespace WebApplication4.Pages
{
    public class CreateReportModel : PageModel
    {
        [BindProperty]
        public Report Report { get; set; }

        [BindProperty]
        public String DateSubmit { get; set; }

        [BindProperty]
        public String Message { get; set; }


        private readonly DatabaseContext db;

        private readonly UserManager<Account> UM;

        public CreateReportModel(DatabaseContext _db, UserManager<Account> _UserManager)
        {
            db = _db;
            UM = _UserManager;
        }

        public void OnGet(Account account, string message)
        {
            Report = new Report
            {
                UserID = account.Id,
            };
            Message = message;
           
        }

        public  IActionResult OnGetBack(int id)
        {

            var account = UM.FindByIdAsync(id.ToString()).Result;


            if (account.IsAdmin)
                return RedirectToPage("AdminHome", UM.FindByIdAsync(id.ToString()).Result);
            else
                return RedirectToPage("Home", UM.FindByIdAsync(id.ToString()).Result);

        }
        public async Task<IActionResult> OnPost(int id)
        {
            int[] Date = Array.ConvertAll(DateSubmit.Split("/"), s => int.Parse(s));

            PersianCalendar pc = new PersianCalendar();

            Report.UserID = id;


            Report.Username = UM.FindByIdAsync(id.ToString()).Result.UserName;

            Report.SubmitDate = new DateTime(Date[0], Date[1], Date[2], pc);
            
            if(!Tools.CheckBeginEndTime(Report.BeginTime, Report.EndTime))
            {
                Message = "زمان شروع و پایان هماهنگ نیست";
                return Page();//RedirectToPage("CreateReport", UM.FindByIdAsync(id.ToString()).Result);
            }


            await db.Reports.AddAsync(Report);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Message = e.Message;
                return RedirectToPage("CreateReport", UM.FindByIdAsync(id.ToString()).Result);
            }

            if (UM.FindByIdAsync(id.ToString()).Result.IsAdmin)
                return RedirectToPage("AdminHome", UM.FindByIdAsync(id.ToString()).Result);
            else
                return RedirectToPage("Home", UM.FindByIdAsync(id.ToString()).Result);
        }
    }
}