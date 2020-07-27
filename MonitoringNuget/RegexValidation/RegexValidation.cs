using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MonitoringNuget.RegexValidation
{
    public static class RegexValidation
    {
        public static bool CustomerNrValidation(string customernr)
        {
            return Regex.IsMatch(customernr, @"^CU\d{5,5}$");
        }

        public static bool HomepageValidation(string homepage)
        {
            return Regex.IsMatch(homepage, @"^((http:|https:)\/\/)?([A-Za-z]+\.)?[A-Za-z]+\.([a-z]){2,4}(\/.+)*$");
        }

        public static bool PasswordValidation(string password)
        {
            return Regex.IsMatch(password, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z\d]{8,}$");
        }

        public static bool EMailValidation(string email)
        {

            return Regex.IsMatch(email
                               , @"^[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?$");
        }
    }
}
