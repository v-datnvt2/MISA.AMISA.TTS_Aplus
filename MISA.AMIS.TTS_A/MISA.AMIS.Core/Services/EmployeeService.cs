using MISA.AMIS.Core.Entities;
using MISA.AMIS.Core.Interfaces.Repository;
using MISA.AMIS.Core.Interfaces.Service;
using MISA.AMIS.Core.Resources.ResourceVI;
using MISA.AMIS.Core.Services.CommonFunctions;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static MISA.AMIS.Core.Enums.EnumClass;

namespace MISA.AMIS.Core.Services
{
    public class EmployeeService : BaseService<Employee>, IEmployeeService
    {
        readonly IEmployeeRepository _employeeRepository;
        ServiceResult _serviceResult;
        public EmployeeService(IEmployeeRepository employeeRepository, IBaseRepository<Employee> baseRepository) : base(baseRepository)
        {
            _employeeRepository = employeeRepository;
            _serviceResult = new ServiceResult();
        }

        public override ServiceResult Add(Employee employee)
        {
            try
            {
                //Custom validate
                CustomValidate(employee);
                //Gọi kết nối csdl thực hiện lưu dữ liệu
                _serviceResult = base.Add(employee);
                return _serviceResult;
            }
            catch (Exception ex)
            {
                var devMsg = ex.Message;
                object errorMsg;

                //Dữ liệu gửi lên bị trùng
                if (devMsg.Contains("Duplicate"))
                {

                    errorMsg = new
                    {
                        devMsg,
                        userMsg = ResourceGeneralVI.Validate_Msg_ExistedImportField,
                        errorCode = "RQ_02"
                    };
                }
                else
                //Lỗi kết nối đến csdl
                {
                    errorMsg = new
                    {
                        devMsg,
                        userMsg = ResourceGeneralVI.Exception_Msg_DBConnection,
                        errorCode = "DB_01"
                    };
                }
                _serviceResult.IsValid = false;
                _serviceResult.Data = errorMsg;
                return _serviceResult;
            }
        }

        public ServiceResult Filter_Employee(int pageNumber, int pageSize, string searchKey)
        {
            try
            {
                //Gọi kết nối csdl thực hiện lưu dữ liệu
                PagingResult pagingResult = _employeeRepository.Filter_Employee(pageNumber, pageSize, searchKey);
                List<Employee> filtedEmployees = (List<Employee>)pagingResult.Entities;
                if (filtedEmployees.Any())
                {
                    _serviceResult.Data = pagingResult;
                    _serviceResult.ServiceResultCode = ServiceCode.OK;
                }
                else
                {
                    _serviceResult.ServiceResultCode = ServiceCode.NoContent;
                }
                return _serviceResult;
            }
            catch (Exception ex)
            {
                //Lỗi kết nối đến csdl
                var errorMsg = new
                {
                    devMsg = ex.Message,
                    userMsg = ResourceGeneralVI.Exception_Msg_DBConnection,
                    errorCode = "DB_01"
                };
                _serviceResult.IsValid = false;
                _serviceResult.Data = errorMsg;
                return _serviceResult;
            }
        }

        //public ServiceResult Import(IFormFile)
        //{
        //    return _serviceResult;
        //}

        public override List<string> CustomValidate(Employee employee)
        {
            var validateResult = new List<string>();
            Regex empployeeCodeReg = new Regex(@"^NV-[0-9]*$");
            if (empployeeCodeReg.IsMatch(employee.EmployeeCode))
            {
                validateResult.Add("0");
                return validateResult;
            }
            else
            {
                validateResult.Add("3");
                validateResult.Add("EmployeeCode");
                return validateResult;
            };
        }

        public override ServiceResult Update(Guid employeeID, Employee employee)
        {
            try
            {
                //Custom validate
                CustomValidate(employee);
                //Gọi kết nối csdl thực hiện lưu dữ liệu
                _serviceResult = base.Update(employeeID, employee);
                return _serviceResult;
            }
            catch (Exception ex)
            {
                var devMsg = ex.Message;
                object errorMsg;
                if (devMsg.Contains("Duplicate"))
                {
                    //Dữ liệu gửi lên bị trùng
                    errorMsg = new
                    {
                        devMsg,
                        userMsg = ResourceGeneralVI.Validate_Msg_ExistedImportField,
                        errorCode = "RQ_02"
                    };
                }
                else
                {
                    //Lỗi kết nối đến csdl
                    errorMsg = new
                    {
                        devMsg,
                        userMsg = ResourceGeneralVI.Exception_Msg_DBConnection,
                        errorCode = "DB_01"
                    };
                }
                _serviceResult.IsValid = false;
                _serviceResult.Data = errorMsg;
                return _serviceResult;
            }
        }
        public ServiceResult ExportData(int pageSize, int pageNumber, string searchKey, List<string> propNames, bool isAllPage)
        {
            if (isAllPage == true)
            {
                pageNumber = 1;
                pageSize = 100000;
            }

            PagingResult pagingEmployee = _employeeRepository.Filter_Employee(pageNumber, pageSize, searchKey);
            if (pagingEmployee.TotalRecord > 0)
            {
                List<Employee> employees = (List<Employee>)pagingEmployee.Entities;
                var stream = new MemoryStream();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");

                    // tạo fontsize và fontfamily cho sheet
                    workSheet.Cells.Style.Font.Size = 11;
                    workSheet.Cells.Style.Font.Name = "Calibri";

                    // chỉnh style cho bảng
                    workSheet.Row(1).Style.Font.Bold = true;
                    workSheet.Row(1).Style.Fill.PatternType = ExcelFillStyle.Solid;
                    workSheet.Row(1).Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.AliceBlue);
                    workSheet.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                    //Gán tên các cột và độ rộng
                    List<string> columnTitles = new List<string>();
                    List<int> columnWidths = new List<int>();
                    for (var i = 0; i < propNames.Count; i++)
                    {
                        var columnTitle = ResourcePropertyVI.ResourceManager.GetString("Prop_" + propNames[i]);
                        workSheet.Cells[1, i + 1].Value = columnTitle;
                        columnWidths.Add(columnTitle.Length + 5);
                    }

                    // Gán data list vào sheet
                    var rowIndex = 2;
                    foreach (var employee in employees)
                    {
                        for (int i = 0; i < propNames.Count; i++)
                        {
                            var propValue = CommonFn.GetCustomObjectValue(employee, propNames[i]);
                            var displayValue = CommonFn.GetDisplayValue<Employee>(propNames[i], propValue);

                            workSheet.Cells[rowIndex, i + 1].Value = displayValue.ToString();
                            columnWidths[i] = Math.Max(columnWidths[i], displayValue.ToString().Length + 5);
                        }
                        rowIndex++;
                    }

                    //Điều chỉnh độ rộng các cột
                    for (var i = 0; i < columnWidths.Count; i++)
                    {
                        workSheet.Column(i + 1).Width = columnWidths[i];
                    }

                    package.Save();
                }

                stream.Position = 0;
                _serviceResult.Data = stream;
            }
            else
            {
                _serviceResult.ServiceResultCode = ServiceCode.NoContent;
            }
            return _serviceResult;
        }
    }
}
