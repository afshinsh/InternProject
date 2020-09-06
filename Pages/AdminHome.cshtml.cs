using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebApplication4.Models;

using Microsoft.AspNetCore.Identity;
using System;

namespace WebApplication4.Pages
{
    public class AdminHomeModel : PageModel
    {
       

        [BindProperty]
        public Account Account { get; set; }

        [BindProperty]
        public String Message { get; set; }

        [BindProperty]
        public List<Question> InQueueQuestions { get; set; }

        private readonly UserManager<Account> UM;

        private readonly DatabaseContext db;

        public AdminHomeModel(DatabaseContext _db, UserManager<Account> _UserManager)
        {
            db = _db;
            UM = _UserManager;
        }

        public List<Account> Accounts { get; set; }
        public async Task OnGet(Account account)
        {
            if (account == null || !account.IsAdmin)
                Accounts = null;
            else
            {
                Account = account;
                Accounts = await UM.Users.ToListAsync();
                Accounts.RemoveAll(MatchAccount);

                InQueueQuestions = await db.Questions.ToListAsync();
                InQueueQuestions.RemoveAll(MatchQuestion);
            }
        }

        private bool MatchAccount(Account obj)
        {
            if (!obj.IsAdmin || (obj.IsAdmin && !obj.IsActive))
                return false;
            return true;
        }

        private bool MatchQuestion(Question obj)
        {
            if (obj.Status == Question.Statuses.InQueue.ToString())
                return false;
            return true;

        }

        public async Task OnGetBack(int id)
        {
            Accounts = await UM.Users.ToListAsync();

            Account = UM.FindByIdAsync(id.ToString()).Result;

        }

        public IActionResult OnPostPublishedReports(int id)
        {
            return RedirectToPage("PublishedReports", UM.FindByIdAsync(id.ToString()).Result);
        }


        public IActionResult OnPostCreateReport(int id)
        {
            return RedirectToPage("CreateReport", UM.FindByIdAsync(id.ToString()).Result);
        }

        public IActionResult OnPostExcelUnSeen(int id)
        {
            return RedirectToPage("UnSeenReports", UM.FindByIdAsync(id.ToString()).Result);
        }

        public IActionResult OnPostExcelSeen(int id)
        {
            return RedirectToPage("SeenReports", UM.FindByIdAsync(id.ToString()).Result);
        }

        public IActionResult OnPostUnAnsweredQuestion(int id)
        {

            return RedirectToPage("UnAnsweredQuestion", UM.FindByIdAsync(id.ToString()).Result);
        }


        public IActionResult OnPostProfile(int id)
        {
            return RedirectToPage("Profile", UM.FindByIdAsync(id.ToString()).Result);
        }

        public IActionResult OnPostAnswerQuestion(int adminId)
        {
            return RedirectToPage("UnAnsweredQuestion", UM.FindByIdAsync(adminId.ToString()).Result);
        }



        public async Task<IActionResult> OnPostDelete(int id, int adminID)
        {
            Account = UM.FindByIdAsync(id.ToString()).Result;
            
            if (Account == null)
            {
                return NotFound();
            }

            await UM.DeleteAsync(Account);

            return RedirectToPage("AdminHome", UM.FindByIdAsync(adminID.ToString()).Result);
        }

        public IActionResult OnPostActive(int id, int adminID)
        {
            Account = UM.FindByIdAsync(id.ToString()).Result;
            Account.IsActive = true;
            try
            {
                db.SaveChanges();
            }
            catch (Exception e)
            {
                Message = e.Message;
                return Page();
            }
            return RedirectToPage("AdminHome", UM.FindByIdAsync(adminID.ToString()).Result);
        }

        public IActionResult OnPostDeActive(int id, int adminID)
        {
            Account = UM.FindByIdAsync(id.ToString()).Result;
            Account.IsActive = false;

            try
            {
                db.SaveChanges();
            }
            catch (Exception e)
            {
                Message = e.Message;
                return Page();
            }

            return RedirectToPage("AdminHome", UM.FindByIdAsync(adminID.ToString()).Result);
        }
    }
}