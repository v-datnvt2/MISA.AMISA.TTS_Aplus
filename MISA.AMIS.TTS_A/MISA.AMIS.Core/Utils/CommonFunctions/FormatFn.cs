using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.Core.Services.CommonFunctions
{
    public static class FormatFn
    {
        public static string RemoveAccent(string text)
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }

        public static DateTime? GetYMDDate(string dateString)
        {
            dateString = dateString.Replace("-", "/");
            dateString = dateString.Replace("\\", "/");
            var str = dateString.Split("/");
            return str.Length switch
            {
                1 => DateTime.Parse($"{str[0]}/01/01"),
                2 => DateTime.Parse($"{str[1]}/{str[0]}/01"),
                3 => DateTime.Parse($"{str[2]}/{str[1]}/{str[0]}"),
                _ => null,
            };
        }

        public static string FormatPhoneNumber(string phoneString)
        {
            string numericPhone = new String(phoneString.Where(Char.IsDigit).ToArray());
            return phoneString;
        }
    }
}
