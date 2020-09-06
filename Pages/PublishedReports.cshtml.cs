using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication4.Models;

namespace WebApplication4.Pages
{
    public class PublishedReportsModel : PageModel
    {

        [BindProperty]
        public Account Account { get; set; }

        private readonly DatabaseContext db;

        public List<Report> Reports { get; set; }

        public PublishedReportsModel(DatabaseContext _db)
        {
            db = _db;
        }
        public void OnGet(Account account)
        {
            Account = account;
            Reports = db.Reports.ToList();
            Reports.RemoveAll(NotMach);
        }

        private bool NotMach(Report obj)
        {
            return obj.UserID != Account.Id;
        }
    }
}