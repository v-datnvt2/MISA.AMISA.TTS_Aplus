using MISA.AMIS.Core.Resources.ResourceVI;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.Core.Services.CommonFunctions
{
    public static class CommonFn
    {
        public static DateTime ToCsharpDate(string dateString)
        {
            return DateTime.ParseExact(dateString, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
        }
        public static bool IsNumber(string numberString)
        {
            if (numberString == "")
            {
                return false;
            }
            bool isNumeric = int.TryParse("123", out _);
            return isNumeric;
        }

        public static object GetCustomObjectValue(object customObject, string property)
        {
            object propValue = customObject.GetType().GetProperty(property).GetValue(customObject, null);
            return propValue;
        }

        public static Dictionary<string, List<string>> GetPropertyAttributes<CustomType>()
        {
            var dictionary = new Dictionary<string, List<string>>();

            var properties = typeof(CustomType).GetProperties();
            foreach (var property in properties)
            {
                var propName = property.Name;
                var customAttributes = property.GetCustomAttributes(true);
                dictionary.Add(propName, new List<string>((string[])customAttributes));
            }
            return dictionary;
        }

        public static Type GetPropTypeByName<CustomType>(string propName)
        {
            foreach (var prop in typeof(CustomType).GetProperties())
            {
                if (prop.Name == propName)
                    return prop.PropertyType;
            }
            return null;
        }

        /// <summary>
        /// Lấy giá trị hiển thị của các thuộc tính dựa theo custom attribute
        /// </summary>
        /// <param name="propName">Tên thuộc tính</param>
        /// <param name="propValue">Giá trị</param>
        /// <param name="type">Kiểu dữ liệu của thuộc tính</param>
        /// <returns></returns>
        public static string GetDisplayValue<CustomType>(string propName, object propValue)
        {
            var propertyInfo = typeof(CustomType).GetProperty(propName);
            if (propValue == null)
            {
                return "";
            }
            else
            {
                var displayValue = propValue;
                if (propertyInfo.PropertyType.FullName.Contains("Enum"))
                    displayValue = ResourceEnumVI.ResourceManager.GetString($"{propName}_{propValue}");
                else if (propertyInfo.PropertyType.FullName.Contains("DateTime"))
                    displayValue = ConvertDMYDate(propValue);

                return displayValue == null ? "" : displayValue.ToString();
            }

        }

        public static string ConvertDMYDate(object dateTime)
        {
            //DateTime dt = DateTime.ParseExact(dateTime.ToString(), "dd/MM/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture);

            //string dmyDate = dt.ToString("dd/M/yyyy", CultureInfo.InvariantCulture);
            string dmyDate = string.Format("{0:dd/MM/yyyy}", dateTime);
            return dmyDate;
        }
    }
}
