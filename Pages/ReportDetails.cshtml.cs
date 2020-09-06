using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication4.Models;

namespace WebApplication4.Pages
{
    public class ReportDetailsModel : PageModel
    {
        [BindProperty]
        public Report Report { get; set; }


        [BindProperty]
        public string Username { get; set; }

        [BindProperty]
        public int PageCode { get; set; }

        [BindProperty]
        public int UserID { get; set; }

        private readonly DatabaseContext db;


        private readonly UserManager<Account> UM;

        public ReportDetailsModel(DatabaseContext _db, UserManager<Account> _UM)
        {
            db = _db;
            UM = _UM;
        }
        public void OnGet(int ID, int userID, int pageCode)
        {
            Report = db.Reports.Find(ID);
            PageCode = pageCode;
            UserID = userID;

            Username = UM.FindByIdAsync(Report.UserID.ToString()).Result.UserName;
        }

        public IActionResult OnGetBack(int UserID, int pageCode)
        {
            Account User = UM.FindByIdAsync(UserID.ToString()).Result;
            switch (pageCode)
            {
                case 0:
                    return RedirectToPage("SeenReports", User);
                    
                case 1:
                    return RedirectToPage("UnSeenReports", User);
                 
                default:
                    return RedirectToPage("PublishedReports", User); 


            }
        }
    }
}