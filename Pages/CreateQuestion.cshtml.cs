using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication4.Models;

namespace WebApplication4.Pages
{
    public class CreateQuestionModel : PageModel
    {
        [BindProperty]
        public string Message { get; set; }

        [BindProperty]
        public Question Question { get; set; }

        [BindProperty]
        public IFormFile AttachFile { get; set; }

        public UserManager<Account> UM { get;  set; }


        private readonly IWebHostEnvironment WebHostEnvironment;
        private readonly DatabaseContext db;

        public CreateQuestionModel(DatabaseContext _db, UserManager<Account> _UM,
            IWebHostEnvironment webHostEnvironment)
        {
            db = _db;
            UM = _UM;
            WebHostEnvironment = webHostEnvironment;
        }

        public void OnGet(Account account)
        {
            Question = new Question
            {
                OwnerId = account.Id
            };

        }

        private string ProcessUploadFiles()
        {
            
            string UploadsFolder =
                Path.Combine(WebHostEnvironment.WebRootPath, "AttachFiles");
            string UniqeFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(AttachFile.FileName);
            string FilePath = Path.Combine(UploadsFolder, UniqeFileName);
            using (var FileStream = new FileStream(FilePath, FileMode.Create))
            {
                AttachFile.CopyTo(FileStream);
            }
            
            return FilePath;
        }

        public IActionResult OnPost(int UserId)
        {
            Question.OwnerId = UserId;
            Question.Date = DateTime.Now;
            Question.Status = Question.Statuses.InQueue.ToString();


            if (AttachFile != null)
                Question.AttachFilePath = ProcessUploadFiles();

            try
            {
                db.Questions.Add(Question);
                db.SaveChanges();
            }
            catch(Exception e)
            {
                Message = e.Message;
                return RedirectToPage(Question.OwnerId);
            }
            return RedirectToPage("Home", UM.FindByIdAsync(Question.OwnerId.ToString()).Result);
        }
    }
}