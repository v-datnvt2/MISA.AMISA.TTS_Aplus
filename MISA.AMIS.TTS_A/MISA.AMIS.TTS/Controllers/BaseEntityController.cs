using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.AMIS.Core.Entities;
using MISA.AMIS.Core.Interfaces.Repository;
using MISA.AMIS.Core.Interfaces.Service;
using MISA.AMIS.Core.Resources.ResourceVI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MISA.AMIS.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseEntityController<CustomEntity> : ControllerBase
    {
        #region Declaration

        readonly IBaseService<CustomEntity> _baseService;
        ServiceResult _serviceResult;
        public BaseEntityController(IBaseService<CustomEntity> baseService)
        {
            _baseService = baseService;
            _serviceResult = new ServiceResult();
        }

        #endregion


        #region Methods

        [HttpGet]
        public virtual IActionResult Get()
        {
            try
            {
                _serviceResult = _baseService.Get();
                return StatusCode((int)_serviceResult.ServiceResultCode, _serviceResult.Data);
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
                return StatusCode(500, errorMsg);
            }
        }

        [HttpGet("{entityID}")]
        public virtual IActionResult GetEntityByID(Guid entityID)
        {
            try
            {
                _serviceResult = _baseService.GetByID(entityID);
                // Kiểm tra dữ liệu trả về
                return StatusCode((int)_serviceResult.ServiceResultCode, _serviceResult.Data);

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

        [HttpGet("NewCode")]
        public virtual IActionResult GetNewEntityCode()
        {
            try
            {

                _serviceResult = _baseService.GetNewCode();
                // Kiểm tra dữ liệu trả về
                return StatusCode((int)_serviceResult.ServiceResultCode, _serviceResult.Data);
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
        public virtual IActionResult Add(CustomEntity entitty)
        {
            try
            {
                _serviceResult = new ServiceResult();
                _serviceResult = _baseService.Add(entitty);

                // Kiểm tra dữ liệu trả về
                return StatusCode((int)_serviceResult.ServiceResultCode, _serviceResult.Data);
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


        [HttpPut("{entityID}")]
        public virtual IActionResult Update(Guid entityID, CustomEntity entity)
        {


            //Thực hiện cập nhật thông tin đối tượng
            try
            {
                _serviceResult = _baseService.Update(entityID, entity);
                return StatusCode((int)_serviceResult.ServiceResultCode, _serviceResult.Data);
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

        [HttpDelete("{entityID}")]
        public virtual IActionResult Delete(Guid entityID)
        {
            try
            {
                _serviceResult = _baseService.Delete(entityID);
                // Kiểm tra dữ liệu trả về
                return StatusCode((int)_serviceResult.ServiceResultCode, _serviceResult.Data);

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

        [HttpPost("Multiple/Delete")]
        public virtual IActionResult DeleteMultiple([FromBody] List<Guid> entityGuids)
        {
            try
            {
                _serviceResult = _baseService.DeleteMultiple(entityGuids);
                // Kiểm tra dữ liệu trả về
                return StatusCode((int)_serviceResult.ServiceResultCode, _serviceResult.Data);

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

        #endregion
    }
}
