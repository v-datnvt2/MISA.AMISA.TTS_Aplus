using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MISA.AMIS.Core.CustomAttribute.CustomAttribute;
using static MISA.AMIS.Core.Enums.EnumClass;

namespace MISA.AMIS.Core.Entities
{
    public class Person:BaseEntity
    {
        [NotNull]
        public string FullName { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public Gender? Gender { get; set; }

        public string IdentityNumber { get; set; }

        public string Address { get; set; }

        public string Email { get; set; }
    }
}
