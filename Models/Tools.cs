using Microsoft.AspNetCore.Identity;
using System;
using System.Text.RegularExpressions;

namespace WebApplication4.Models
{
    public class Tools
    {
        public static bool IsThereUser(string email , UserManager<Account> userManager)
        {
            if (userManager.FindByEmailAsync(email).Result != null)
                return true;
            return false;
        }



        public static bool CheckPassValidation(string input)
        {

            var hasNumber = new Regex(@"[0-9]+");
            var hasUpperChar = new Regex(@"[A-Z]+");
            var hasMinimum8Chars = new Regex(@".{8,}");

            var isValidated = hasNumber.IsMatch(input) && hasUpperChar.IsMatch(input)
                && hasMinimum8Chars.IsMatch(input);

            return isValidated;
        }

        public static bool CheckBeginEndTime(DateTime beginTime, DateTime endTime)
        {
            if ((endTime - beginTime).TotalMinutes < 0)
                return false;
            return true;
        }

        public static bool CheckDateValidation(DateTime date)
        {
            DateTime StandardAge = new DateTime(DateTime.Now.Year - 10, DateTime.Now.Month
                , DateTime.Now.Day);

            

            if ((date - StandardAge).TotalDays  < 3650)
                return false;
            return true;
        }

        public static string CheckValidation(string email, string password
            , DateTime date, UserManager<Account> userManager, bool editProfile)
        {
            string Message;
            if (Tools.IsThereUser(email, userManager) && !editProfile)
                Message = "کاربر با این نام هم اکنون وجود دارد";
            else if (!Tools.CheckPassValidation(password))
                Message = "گذرواژه باید حداقل 8 رقم شامل حروف بزرگ و کوچک و اعداد باشد";
            else if (Tools.CheckDateValidation(date))
                Message = "(تاریخ تولد نامعتبر است(حداقل سن 10 سال است";
            else
                return null;
            return Message;

        }
    }
}
