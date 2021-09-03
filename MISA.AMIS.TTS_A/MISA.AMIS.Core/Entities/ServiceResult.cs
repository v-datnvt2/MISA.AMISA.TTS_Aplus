using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MISA.AMIS.Core.Enums.EnumClass;

namespace MISA.AMIS.Core.Entities
{
    public class ServiceResult
    {

        #region Property
        /// <summary>
        /// Kết quả thực hiện nghiệp vụ
        /// </summary>
        public bool IsValid { get; set; } = true;

        /// <summary>
        /// Thông tin gửi kèm kết quả thực hiện
        /// </summary>
        public object Data { get; set; }

        /// <summary>
        /// Thông điệp gửi kèm kết quả thực hiện
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// HTTP StatusCode tương ứng với kết quả thực hiện
        /// </summary>
        public ServiceCode ServiceResultCode { get; set; } = ServiceCode.OK;

        #endregion

    }
}
