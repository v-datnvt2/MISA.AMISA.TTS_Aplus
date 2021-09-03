using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.Core.CustomAttribute
{
    public class CustomAttribute
    {
        [AttributeUsage(AttributeTargets.Property)]
        public class EmailField : Attribute
        {
        }

        [AttributeUsage(AttributeTargets.Property)]
        public class Unique : Attribute
        {
        }

        [AttributeUsage(AttributeTargets.Property)]
        public class NotNull : Attribute
        {
        }

        [AttributeUsage(AttributeTargets.Property)]
        public class PersonName : Attribute
        {
        }

        [AttributeUsage(AttributeTargets.Property)]
        public class NotMap : Attribute
        {
        }
        [AttributeUsage(AttributeTargets.Property)]
        public class PhoneNumber : Attribute
        {
        }
    }
}
