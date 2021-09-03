using Dapper;
using Microsoft.Extensions.Configuration;
using MISA.AMIS.Core.Entities;
using MISA.AMIS.Core.Interfaces.Repository;
using MISA.AMIS.Core.Services.CommonFunctions;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.Infrastructure.Repository
{
    public class EmployeeRepository : BaseRepository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(IConfiguration configuration):base(configuration)
        {

        }
        public PagingResult Filter_Employee(int pageNumber, int pageSize, string searchKey)
        {
            if (!string.IsNullOrEmpty(searchKey))
            {
                searchKey = FormatFn.RemoveAccent(searchKey);
            }

            //Tạo query lấy các bản ghi phù hợp
            DynamicParameters dynamicParams = new();
            string sqlCommand = "Proc_FilterEmployee";
            //dynamicParams.Add("@DepartmentID", departmentID);
            dynamicParams.Add("@SearchKey", searchKey);
            dynamicParams.Add("@PageSize", pageSize);
            dynamicParams.Add("@PageNumber", pageNumber);

            dynamicParams.Add("TotalPage", dbType: DbType.Int32, direction: ParameterDirection.Output);
            dynamicParams.Add("TotalRecord", dbType: DbType.Int32, direction: ParameterDirection.Output);

            using (IDbConnection dbConnection = new MySqlConnection(_connectionString))
            {
                List<Employee> filteredEmployees = (List<Employee>)dbConnection.Query<Employee>(sqlCommand, param: dynamicParams, commandType: CommandType.StoredProcedure);
                int totalPage = dynamicParams.Get<int>("TotalPage");
                int totalRecord = dynamicParams.Get<int>("TotalRecord");
                return new PagingResult(totalRecord, totalPage, filteredEmployees);
            }
        }
    }
}
