using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using WebApplication4.Models;

namespace WebApplication4.Pages
{
    public class HomeModel : PageModel
    {
        [BindProperty]
        public Account Account { get; set; }

        [BindProperty]
        public List<Question> AnsweredQuestions { get; set; }

        [BindProperty]
        public List<Answer> Answers { get; set; }


        readonly DatabaseContext db;

        readonly SignInManager<Account> SignInManager;

        private readonly UserManager<Account> UserManager;

           

        public HomeModel(DatabaseContext _db, SignInManager<Account> _SignInManager,
        UserManager<Account> _UserManager)
        {
            db = _db;
            SignInManager = _SignInManager;
            UserManager = _UserManager;
        }

        private bool Match(Question obj)
        {
            if (obj.Status == Question.Statuses.Answered.ToString() 
                || obj.Status == Question.Statuses.Seen.ToString())
                return false;
            return true;
        }

        public void OnGet(Account account)
        {
            Account = account;
            Account.IsBan = Account.CheckIsBan();

            AnsweredQuestions = db.Questions.ToList();
            AnsweredQuestions.RemoveAll(Match);


            Answers = db.Answers.ToList();

        }

        

        public void OnGetBack(int id)
        {
            Account = UserManager.FindByIdAsync(id.ToString()).Result;
            Account.IsBan = Account.CheckIsBan();

            AnsweredQuestions = db.Questions.ToList();
            AnsweredQuestions.RemoveAll(Match);

            Answers = db.Answers.ToList();
        }

        public IActionResult OnPostProfile(int id)
        {
            return RedirectToPage("Profile", UserManager.FindByIdAsync(id.ToString()).Result);
        }

        public IActionResult OnPostCreateQuestion(int UserId)
        {
            return RedirectToPage("CreateQuestion", new Account
            {
                Id = UserId
            });
        }

        public IActionResult OnPostCreate(int id)
        {
            return RedirectToPage("CreateReport", UserManager.FindByIdAsync(id.ToString()).Result);
        }

        public IActionResult OnPostReports(int id)
        {
            return RedirectToPage("PublishedReports", UserManager.FindByIdAsync(id.ToString()).Result);
        }


        public async Task<IActionResult> OnPostLogout()
        {
            await SignInManager.SignOutAsync();
            return RedirectToPage("index");
        }

    }
}