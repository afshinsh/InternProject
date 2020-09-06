using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication4.Models;

namespace WebApplication4.Pages
{
    public class QuestionDetailsModel : PageModel
    {

        [BindProperty]
        public Question Question { get; set; }

        [BindProperty]
        public Answer Answer { get; set; }


        readonly DatabaseContext db;

        public QuestionDetailsModel(DatabaseContext _db)
        {
            db = _db;
        }
        public void OnGet(int QuestionId)
        {
            Question = db.Questions.Find(QuestionId);
            Answer = db.Answers.Find(Question.AnswerId);
        }
    }
}