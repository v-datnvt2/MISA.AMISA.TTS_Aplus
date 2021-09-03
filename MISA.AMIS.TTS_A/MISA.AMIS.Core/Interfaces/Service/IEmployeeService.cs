using MISA.AMIS.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.Core.Interfaces.Service
{
    public interface IEmployeeService:IBaseService<Employee>
    {
        ServiceResult Filter_Employee(int pageNumber, int pageSize, string searchKey );
        ServiceResult ExportData(int pageSize, int pageNumber, string searchKey, List<string> propNames, bool isAllPage);
    }
}
