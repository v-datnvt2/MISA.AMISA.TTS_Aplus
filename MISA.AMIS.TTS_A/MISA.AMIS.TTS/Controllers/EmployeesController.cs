using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.AMIS.Core.Entities;
using MISA.AMIS.Core.Enums;
using MISA.AMIS.Core.Interfaces.Repository;
using MISA.AMIS.Core.Interfaces.Service;
using MISA.AMIS.Core.Resources.ResourceVI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static MISA.AMIS.Core.Enums.EnumClass;

namespace MISA.AMIS.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [EnableCors]
    public class EmployeesController : BaseEntityController<Employee>
    {
        readonly IEmployeeService _employeeService;
        public EmployeesController(IEmployeeService employeeService, IBaseService<Employee> baseService) : base(baseService)
        {
            _employeeService = employeeService;
        }


        [HttpGet("filter")]
        public IActionResult Filter_Employee(int pageNumber, int pageSize, string searchKey)
        {
            try
            {
                ServiceResult serviceResult = _employeeService.Filter_Employee(pageNumber, pageSize, searchKey);
                //Kiểm tra dữ liệu trả về
                return StatusCode((int)serviceResult.ServiceResultCode, serviceResult.Data);
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
                var response = StatusCode(500, errorMsg);
                return response;
            }
        }

        [HttpPost]
        public override IActionResult Add(Employee employee)
        {
            try
            {
                ServiceResult serviceResult = new ServiceResult();
                serviceResult = _employeeService.Add(employee);

                // Kiểm tra dữ liệu trả về
                return StatusCode((int)serviceResult.ServiceResultCode, serviceResult.Data);
            }
            catch (Exception ex)
            {
                //Lỗi kết nối đến csdl
                var errorMsg = new
                {
                    devMsg = ex.Message,
                    userMsg = ResourceGeneralVI.Exception_Msg_Common,
                    errorCode = "DB_01"
                };
                var response = StatusCode(500, errorMsg);
                return response;
            }
        }

        [HttpPut("{employeeID}")]
        public override IActionResult Update(Guid employeeID, Employee employee)
        {
            //Thực hiện cập nhật thông tin nhân viên
            try
            {
                ServiceResult serviceResult = _employeeService.Update(employeeID, employee);
                return StatusCode((int)serviceResult.ServiceResultCode, serviceResult.Data);
            }
            catch (Exception ex)
            {
                var devMsg = ex.Message;
                if (devMsg.Contains("Duplicate"))
                {
                    //Dữ liệu gửi lên bị trùng
                    var errorMsg = new
                    {
                        devMsg,
                        userMsg = ResourceGeneralVI.Validate_Msg_ExistedImportField,
                        errorCode = "RQ_02"
                    };
                    var response = StatusCode(400, errorMsg);
                    return response;
                }
                else
                {
                    //Lỗi kết nối đến csdl
                    var errorMsg = new
                    {
                        devMsg,
                        userMsg = ResourceGeneralVI.Exception_Msg_DBConnection,
                        errorCode = "DB_01"
                    };
                    var response = StatusCode(500, errorMsg);
                    return response;
                }
            }
        }

        [HttpPost("export")]
        public IActionResult ExportData(int pageSize, int pageNumber, string searchKey, bool isAllPage, List<string> propNames)
        {
            //List<string> propNames= new List<string>();
            ServiceResult serviceResult = _employeeService.ExportData(pageSize, pageNumber, searchKey, propNames, isAllPage);
            if (serviceResult.ServiceResultCode == ServiceCode.OK)
            {
                var stream = (MemoryStream)serviceResult.Data;
                string excelName = $"Export_Employee_{DateTime.Now:yyyyMMddHHmmssfff}.xlsx";

                //return File(stream, "application/octet-stream", excelName);  
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
            }
            else
            {
                return StatusCode((int)serviceResult.ServiceResultCode, serviceResult.Data);
            }
        }
    }
}

