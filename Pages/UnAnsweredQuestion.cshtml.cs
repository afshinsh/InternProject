using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebApplication4.Models;

namespace WebApplication4.Pages
{


    public class UnAnsweredQuestionModel : PageModel
    {

        [BindProperty]
        public Account Account { get; set; }

        [BindProperty]
        public List<Question> InQueueQuestions { get; set; }

        private readonly DatabaseContext db;


        public UnAnsweredQuestionModel(DatabaseContext _db)
        {
            db = _db;
        }

        public async Task OnGet(Account account)
        {
            Account = account;

            InQueueQuestions = await db.Questions.ToListAsync();
            InQueueQuestions.RemoveAll(MatchQuestion);

            await db.SaveChangesAsync();
        }


        private bool MatchQuestion(Question obj)
        {
            if (obj.Status == Question.Statuses.InQueue.ToString() 
                || obj.Status == Question.Statuses.Seen.ToString())
            {
                obj.Status = "Seen";
                return false;
            }
            return true;

        }
          
    }
}