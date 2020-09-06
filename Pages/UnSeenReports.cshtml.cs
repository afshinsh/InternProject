using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DNTPersianUtils.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OfficeOpenXml;
using WebApplication4.Models;

namespace WebApplication4.Pages
{
    public class UnSeenReportsModel : PageModel
    {
        [BindProperty]
        public Account AdminAccount { get;  set; }

        [BindProperty]
        public String Message { get; set; }

        [BindProperty]
        public List<Report> UnSeenReports { get; set; }


        public DatabaseContext db;

        private readonly UserManager<Account> UM;

        public UnSeenReportsModel(DatabaseContext _db, UserManager<Account> _UM)
        {
            db = _db;
            UM = _UM;
        }
        public void OnGet(Account account)
        {
            AdminAccount = account;
            GenerateList();
            
        }
        public void GenerateList()
        {
            List<Report> Reports = db.Reports.ToList();

            Reports.RemoveAll(Match);

            
            UnSeenReports = Reports;
            
        }

        private bool Match(Report obj)
        {
            return obj.IsSeen || obj.UserID == AdminAccount.Id;
        }

        public async Task<IActionResult> OnPost(int AdminId)
        {
            await Task.Yield();
            AdminAccount = UM.FindByIdAsync(AdminId.ToString()).Result;
            GenerateList();

            var stream = new MemoryStream();

            using (var package = new ExcelPackage(stream))
            {
                var WS = package.Workbook.Worksheets.Add("Sheet1");
                WS.Cells["A1"].Value = "گزارشات دیده نشده";

                WS.Cells["A2"].Value = "زمان گرفتن گزارشات";
                WS.Cells["B2"].Value = DateTime.Now.ToLongPersianDateTimeString(); 

                WS.Cells["A5"].Value = "شسناسه";
                WS.Cells["B5"].Value = "سمت";

                WS.Cells["C5"].Value = "نام کاربری";
                WS.Cells["D5"].Value = "تیتر";
                WS.Cells["E5"].Value = "تاریخ";
                WS.Cells["F5"].Value = "محتوا";
                WS.Cells["G5"].Value = "شناسه مدیر ناظر";
                WS.Cells["H5"].Value = "مدت گزارش";



                int rowStart = 6;
                foreach (var item in UnSeenReports)
                {




                    WS.Row(rowStart).Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    WS.Row(rowStart).Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(string.Format("pink")));

                    var account = UM.Users.ToList().Find(x => x.Id == item.UserID); 
                    if (account == null)
                        continue;



                    WS.Cells[string.Format("A{0}", rowStart)].Value = item.ID;

                    WS.Cells[string.Format("B{0}", rowStart)].Value = (account.IsAdmin ? "مدیر" : "کاربر عادی");
                    WS.Cells[string.Format("C{0}", rowStart)].Value = account.UserName;
                    WS.Cells[string.Format("D{0}", rowStart)].Value = item.Title;
                    WS.Cells[string.Format("E{0}", rowStart)].Value = item.SubmitDate.ToShortPersianDateString();
                    WS.Cells[string.Format("F{0}", rowStart)].Value = item.Content;
                    WS.Cells[string.Format("G{0}", rowStart)].Value = item.AdminSeenID;
                    WS.Cells[string.Format("H{0}", rowStart)].Value = (item.EndTime - item.BeginTime).TotalHours;


                    item.AdminSeenID = AdminAccount.Id;
                    item.IsSeen = true;
                    
                    


                    rowStart++;
                }

                WS.Cells["A:AZ"].AutoFitColumns();
                package.Save();
            }
            stream.Position = 0;
            string excelName = $"UnSeenDailyReports-{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.xlsx";
            try
            {
                db.SaveChanges();
            }
            catch (Exception e)
            {
                Message = e.Message;
                return Page();
            }

            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
        }


    }
}