using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.Core.Enums
{
    public static class EnumClass
    {
        /// <summary>
        /// Enum giới tính
        /// </summary>
        /// crby
        /// crdate
        public enum Gender
        {
            Female = 0,
            Male = 1,
            Other = 2
        }

        /// <summary>
        /// Enum trạng thái của ServiceResult
        /// </summary>
        /// crby
        /// crdate
        public enum ServiceCode
        {
            OK = 200,
            Created = 201,
            NoContent = 204,
            BadRequest = 400,
            NotFound = 404,
            InternalServerError = 500,
        }
    }
}
