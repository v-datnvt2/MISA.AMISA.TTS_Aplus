using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MISA.AMIS.Core.Services.CommonFunctions
{
    public static class ValidateFn
    {
        //Kiểm tra định dạng email
        public static bool IsValidEmail(string emailString)
        {
            if (string.IsNullOrEmpty(emailString))
            {
                return false;
            }

            Regex emailReg = new Regex(@"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?");
            if (emailReg.IsMatch(emailString))
                return true;
            else return false;
        }

        //Kiểm tra định dạng tên người 
        public static bool IsPersonName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }
            //Regex regex = new Regex("^(\\b[A-Za-z]*\\b\\s+\\b[A-Za-z]*\\b+\\.[A-Za-z])$",
            //                        RegexOptions.IgnoreCase
            //                        | RegexOptions.CultureInvariant
            //                        | RegexOptions.IgnorePatternWhitespace
            //                        | RegexOptions.Compiled
            //                        );

            Regex regex = new Regex(@"\A[\p{L}\s]+\Z");
            if (regex.IsMatch(name)) return true;
            else return false;
        }
    }
}
