using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.Core.Entities
{
    public class PagingResult
    {
        #region Constructor
        public PagingResult(int totalRecord, int totalPage, object entities)
        {
            TotalRecord = totalRecord;
            TotalPage = totalPage;
            Entities = entities;
        }

        #endregion

        #region Property

        /// <summary>
        /// Tổng số bản ghi theo điều kiện lọc
        /// </summary>
        /// crby
        /// crdate
        public int TotalRecord { get; set; }

        /// <summary>
        /// Số trang kết quả tìm được
        /// </summary>
        /// crby
        /// crdate
        public int TotalPage { get; set; }

        /// <summary>
        /// Danh sách bản ghi của trang được yêu cầu
        /// </summary>
        /// crby
        /// crdate
        public object Entities { get; set; }

        #endregion
    }
}
