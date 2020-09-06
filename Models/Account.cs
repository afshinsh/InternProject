using DNTPersianUtils.Core;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication4.Models
{
    public class Account : IdentityUser<int>
    {





        public string Password { get; set; }


        public DateTime BornDate { get; set; }

        
        public bool IsBan { get; set; }

        public DateTime BanDateStart { get; set; }

        public DateTime BanDateFinish { get; set; }

	    public bool IsAdmin { get; set; }

        public bool IsActive { get; set; }

        public bool CheckIsBan()
        {
            double totalDays = (BanDateFinish - BanDateStart).TotalDays;
            double diffrent = (DateTime.Now - BanDateStart).TotalDays;
            if ((BanDateStart - DateTime.Now).TotalDays > 0 || diffrent >= totalDays)
                return false;
            return true;
        }

        public string ShowPeriod()
        {

            return BanDateFinish.ToShortPersianDateTimeString(); 

        }

    }
}
