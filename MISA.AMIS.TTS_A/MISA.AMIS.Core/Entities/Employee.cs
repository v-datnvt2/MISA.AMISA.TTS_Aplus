using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MISA.AMIS.Core.CustomAttribute.CustomAttribute;
using static MISA.AMIS.Core.Enums.EnumClass;

namespace MISA.AMIS.Core.Entities
{
    public class Employee: Person
    {
        #region Property

        [NotNull]
        [Unique]
        public Guid EmployeeID { get; set; }

        [NotNull]
        [Unique]
        public string EmployeeCode { get; set; }

        [NotNull]
        public Guid? DepartmentID { get; set; }

        [NotMap]
        public string DepartmentName { get; set; }

        public string PositionName { get; set; }

        public string IdentityPlace { get; set; }

        public DateTime? IdentityDate { get; set; }

        public string MobilePhoneNumber { get; set; }

        public string LandlinePhoneNumber { get; set; }

        public string BankAccountNumber { get; set; }

        public string BankName { get; set; }

        public string BankBranch { get; set; }


        #endregion
    }
}
