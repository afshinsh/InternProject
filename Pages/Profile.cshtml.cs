using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication4.Models;


namespace WebApplication4.Pages
{
    public class ProfileModel : PageModel
    {
        [BindProperty]
        public Account Account { get; set; }



        private readonly UserManager<Account> UM;



        public ProfileModel(UserManager<Account> _UserManager)
        {
            UM = _UserManager;
        }
        public void OnGet(Account account)
        {
            Account = account;
            
        }

        public IActionResult OnPostEdit(int id)
        {
            return RedirectToPage("EditProfile", UM.FindByIdAsync(id.ToString()).Result);
        }


    }
}