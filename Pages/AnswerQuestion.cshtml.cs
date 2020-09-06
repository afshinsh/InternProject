using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication4.Models;
using System.IO;

namespace WebApplication4.Pages
{
    public class AnswerQuestionModel : PageModel
    {

        [BindProperty]
        public Answer Answer { get; set; }

        [BindProperty]
        public Question Question { get; set; }


        [BindProperty]
        public String Message { get; set; }

        private readonly DatabaseContext db;

        private readonly UserManager<Account> UM;

        public AnswerQuestionModel(DatabaseContext _db, UserManager<Account> _UM)
        {
            db = _db;
            UM = _UM;
        }

        public void OnGet(int QuestionId, int AdminId)
        {
            Answer = new Answer
            {
                OwnerId = AdminId ,
                QuestionId = QuestionId
            };
            Question = db.Questions.Find(QuestionId);
        }

        public IActionResult OnPostDownload(int QuestionId)
        {
            Question = db.Questions.Find(QuestionId);

            var memory = new MemoryStream();

            using (var FileStream = new FileStream(Question.AttachFilePath, FileMode.Open))
            {
                FileStream.CopyTo(memory);
            }
            memory.Position = 0;

            return File(memory, GetContentType(Question.AttachFilePath), Path.GetFileName(Question.AttachFilePath));

        }
        private string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types[ext];
        }

        private Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {".txt", "text/plain"},
                {".pdf", "application/pdf"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.ms-word"},
                {".xls", "application/vnd.ms-excel"},
                {".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"},
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"},
                {".csv", "text/csv"}
            };
        }

        public IActionResult OnPost(int AdminId, int QuestionId)
        {
            Answer.QuestionId = QuestionId;
            Answer.OwnerId = AdminId;
            Answer.Date = DateTime.Now;

            Question question = db.Questions.Find(QuestionId);
            
            try
            {
                db.Answers.Add(Answer);
                db.SaveChanges();
                Answer = db.Answers.ToList()[db.Answers.Count() - 1];
                question.AnswerId = Answer.Id;
                question.ResponserId = AdminId;
                question.Status = Question.Statuses.Answered.ToString();
                Account QuestionOwner = UM.FindByIdAsync(question.OwnerId.ToString()).Result;
                db.SaveChanges();
                return RedirectToPage("AdminHome", UM.FindByIdAsync(AdminId.ToString()).Result);
            }
            catch(Exception e)
            {
                Message = e.Message;
                return RedirectToPage("AnswerQuestion",new Question
                {
                    Id = QuestionId,
                    ResponserId = AdminId
                });
            }
        }

    }

}        
