using MISA.AMIS.Core.Entities;
using MISA.AMIS.Core.Enums;
using MISA.AMIS.Core.Interfaces.Repository;
using MISA.AMIS.Core.Interfaces.Service;
using MISA.AMIS.Core.Resources.ResourceVI;
using MISA.AMIS.Core.Services.CommonFunctions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MISA.AMIS.Core.CustomAttribute.CustomAttribute;
using static MISA.AMIS.Core.Enums.EnumClass;

namespace MISA.AMIS.Core.Services
{
    public class BaseService<CustomEntity> : IBaseService<CustomEntity>
    {
        ServiceResult _serviceResult;
        readonly IBaseRepository<CustomEntity> _baseRepository;
        string _entityType;
        public BaseService(IBaseRepository<CustomEntity> baseRepository)
        {
            _baseRepository = baseRepository;
            _serviceResult = new ServiceResult();
            _entityType = typeof(CustomEntity).Name;
        }

        public virtual ServiceResult Add(CustomEntity customEntity)
        {
            try
            {
                //Xử lí nghiệp vụ
                var validateResult = BaseValidate(Guid.NewGuid(), customEntity, 0);

                if (validateResult[0] == "0")
                {
                    //Gọi kết nối csdl thực hiện lưu dữ liệu
                    _serviceResult.Data = _baseRepository.Add(customEntity);
                }
                else
                {
                    _serviceResult = CreateErrorValidateResult(validateResult);
                }
            }
            catch (Exception ex)
            {
                _serviceResult = CreateExceptionResult(ex);
            }
            return _serviceResult;
        }

        public List<string> BaseValidate(Guid entityID, CustomEntity customEntity, int validateMode)
        {

            //Validate các thuộc tính của object theo attribute
            var properties = typeof(CustomEntity).GetProperties();
            foreach (var property in properties)
            {
                var propName = property.Name;
                var propValue = property.GetValue(customEntity) == null ? "" : property.GetValue(customEntity).ToString();

                //Validate trường bỏ trống
                var notNullFields = property.GetCustomAttributes(typeof(NotNull), true);
                if (notNullFields.Length > 0)
                {
                    if (propValue.Length == 0)
                        return new List<string>() { "1", propName, propValue };
                }

                //Validate định dạng email
                var emailFields = property.GetCustomAttributes(typeof(EmailField), true);
                if (emailFields.Length > 0 && !ValidateFn.IsValidEmail(propValue))
                    return new List<string>() { "2", propName, propValue };

                //Validate định dạng tên
                var personNameFields = property.GetCustomAttributes(typeof(PersonName), true);
                if (personNameFields.Length > 0 && !ValidateFn.IsPersonName(propValue))
                {
                    return new List<string>() { "2", propName, propValue };
                }

                //Validate dữ liệu trùng lặp
                var uniqueFields = property.GetCustomAttributes(typeof(Unique), true);
                if (uniqueFields.Length > 0)
                {
                    //Tìm trong CSDL các đối tượng có cùng( thuộc tính: giá trị )với đối tượng cần validate
                    List<CustomEntity> existingEntites = _baseRepository.CheckDuplicatedField(propName, propValue, CommonFn.GetPropTypeByName<CustomEntity>(propName));
                    if (validateMode == 0 && existingEntites.Any())
                    {
                        if (propName == $"{_entityType}ID")
                            continue;
                        return new List<string>() { "3", propName, propValue };
                    }
                    else if (validateMode == 1 && existingEntites.Any())
                    {
                        foreach (var ee in existingEntites)
                        {
                            if (CommonFn.GetCustomObjectValue(ee, $"{_entityType}ID").ToString() != entityID.ToString())
                                return new List<string>() { "3", propName, propValue };
                        }
                    }
                }
            }
            return new List<string>() { "0", "", "" };
        }

        public virtual ServiceResult CheckDuplicatedField(string fieldName, object fieldValue, Type fieldType)
        {
            throw new NotImplementedException();
        }

        public virtual List<string> CustomValidate(CustomEntity entity)
        {
            return new List<string>() { "0", "", "" };
        }

        /// <summary>
        /// Xóa đối tượng theo ID
        /// </summary>
        /// <param name="entityID">ID đối tượng cần xóa</param>
        /// <returns>Kết quả thực hiện 1:Thành công 0:Thất bại</returns>
        public ServiceResult Delete(Guid entityID)
        {
            try
            {
                _serviceResult.Data = _baseRepository.Delete(entityID);
                return _serviceResult;
            }
            catch (Exception ex)
            {
                _serviceResult = CreateExceptionResult(ex);
            }
            return _serviceResult;
        }

        /// <summary>
        /// Lấy tất cả đối tượng
        /// </summary>
        /// <returns>Danh sách đối tượng</returns>
        public ServiceResult Get()
        {
            try
            {
                List<CustomEntity> res = _baseRepository.Get();
                _serviceResult.Data = res;

                if (res.Any())
                {
                    _serviceResult.IsValid = true;
                    _serviceResult.ServiceResultCode = ServiceCode.OK;
                    return _serviceResult;
                }
                else
                {
                    _serviceResult.IsValid = false;
                    _serviceResult.ServiceResultCode = ServiceCode.NoContent;
                    return _serviceResult;
                }
            }
            catch (Exception ex)
            {
                _serviceResult = CreateExceptionResult(ex);
            }
            return _serviceResult;
        }

        /// <summary>
        /// Lấy thông tin đối tượng theo ID
        /// </summary>
        /// <param name="entityID">ID đối tượng cần tìm</param>
        /// <returns>Phòng ban tìm được</returns>
        /// datnvt (15/08)
        public ServiceResult GetByID(Guid entityID)
        {
            try
            {
                var searchResult = _baseRepository.GetByID(entityID);
                _serviceResult.Data = searchResult;
                if (searchResult == null)
                {
                    _serviceResult.IsValid = false;
                    _serviceResult.ServiceResultCode = ServiceCode.NoContent;
                }
                return _serviceResult; ;
            }
            catch (Exception ex)
            {
                _serviceResult = CreateExceptionResult(ex);
            }
            return _serviceResult;
        }


        /// <summary>
        /// Cập nhật thông tin đối tượng
        /// </summary>
        /// <param name="entityID">ID đối tượng cần chỉnh sửa</param>
        /// <param name="entity">object chứa thông tin đối tượng</param>
        /// <returns>Kết quả thực hiện </returns>
        /// datnvt (15/08)
        public virtual ServiceResult Update(Guid entityID, CustomEntity entity)
        {
            //Kiểm tra dữ liệu đầu vào và định dạng
            //employeeID: Guid
            try
            {
                //Xử lí nghiệp vụ
                var validateResult = BaseValidate(entityID, entity, 1);

                if (validateResult[0] == "0")
                {
                    //Gọi kết nối csdl thực hiện lưu dữ liệu
                    _serviceResult.Data = _baseRepository.Update(entityID, entity);
                }
                else
                {
                    _serviceResult = CreateErrorValidateResult(validateResult);
                }
            }
            catch (Exception ex)
            {
                _serviceResult = CreateExceptionResult(ex);
            }
            return _serviceResult;
        }

        /// <summary>
        /// Tạo mã mới cho đối tượng
        /// </summary>
        /// <typeparam name="CustomEntity"></typeparam>
        /// <returns>Mã mới cho đối tượng</returns>
        public ServiceResult GetNewCode()
        {
            try
            {
                string highestCode = _baseRepository.GetHighestCode();
                int highestCodeNumber = 0;
                //Kiểm tra dữ liệu trả về
                if (highestCode != null && highestCode != "")
                {
                    // Xử lí sinh mã  mới NV-xxx
                    try
                    {
                        //Lấy giá trị số lớn nhất trong mã nhân viên hiện tại
                        highestCodeNumber = int.Parse(highestCode.Split("NV")[1]);
                    }
                    catch (Exception)
                    {
                        //Lỗi dữ liệu trả về không hợp lệ không ở dang NV-xxx
                        var errorMsg = new
                        {
                            devMsg = ResourceGeneralVI.Exception_Msg_InvalidDBData,
                            userMsg = ResourceGeneralVI.Exception_Msg_Common,
                            errorCode = "DB_02"
                        };
                        _serviceResult.IsValid = false;
                        _serviceResult.Data = errorMsg;
                        _serviceResult.ServiceResultCode = ServiceCode.InternalServerError;
                        return _serviceResult; ;
                    }
                }

                //Tạo xâu mã mới và trả về
                _serviceResult.Data = $"NV{highestCodeNumber + 1}";
            }
            catch (Exception ex)
            {
                _serviceResult = CreateExceptionResult(ex);
            }
            return _serviceResult;
        }

        protected virtual ServiceResult CreateErrorValidateResult(List<string> validateResult)
        {
            string resutlCode = validateResult[0];
            string resultField = validateResult[1];
            string resultFieldValue = validateResult[2];
            string devMsg = string.Empty;
            string userMsg = string.Empty;
            string errorCode = "RQ_01";
            switch (resutlCode)
            {
                case ("1"):
                    devMsg = string.Format(ResourceGeneralVI.ResourceManager.GetString("Exception_Msg_EmptyField"), ResourcePropertyVI.ResourceManager.GetString($"Prop_{resultField}"), resultFieldValue);
                    userMsg = string.Format(ResourceGeneralVI.ResourceManager.GetString("Exception_Msg_EmptyField"), ResourcePropertyVI.ResourceManager.GetString($"Prop_{resultField}"), resultFieldValue);
                    break;
                case ("2"):
                    devMsg = string.Format(ResourceGeneralVI.ResourceManager.GetString("Exception_Msg_InvalidInputData"), ResourcePropertyVI.ResourceManager.GetString($"Prop_{resultField}"), resultFieldValue);
                    userMsg = string.Format(ResourceGeneralVI.ResourceManager.GetString("Exception_Msg_InvalidInputData"), ResourcePropertyVI.ResourceManager.GetString($"Prop_{resultField}"), resultFieldValue);
                    break;
                case ("3"):
                    devMsg = string.Format(ResourceGeneralVI.ResourceManager.GetString("Validate_Msg_ExistedImportField"), ResourcePropertyVI.ResourceManager.GetString($"Prop_{resultField}"), resultFieldValue);
                    userMsg = string.Format(ResourceGeneralVI.ResourceManager.GetString("Validate_Msg_ExistedImportField"), ResourcePropertyVI.ResourceManager.GetString($"Prop_{resultField}"), resultFieldValue);
                    break;
                case ("4"):
                    devMsg = string.Format(ResourceGeneralVI.ResourceManager.GetString("Validate_DevMsg_UnmatchedID"), ResourcePropertyVI.ResourceManager.GetString($"Prop_{resultField}"), resultFieldValue);
                    userMsg = string.Format(ResourceGeneralVI.ResourceManager.GetString("Validate_Msg_UnmatchedID"), ResourcePropertyVI.ResourceManager.GetString($"Prop_{resultField}"), resultFieldValue);
                    break;
                default:
                    break;
            }

            _serviceResult.IsValid = false;
            _serviceResult.Data = new { devMsg, userMsg, errorCode };
            _serviceResult.ServiceResultCode = ServiceCode.BadRequest;
            return _serviceResult;
        }
        protected virtual ServiceResult CreateExceptionResult(Exception ex)
        {
            var devMsg = ex.Message;
            object errorMsg;
            if (devMsg.Contains("Duplicate"))
            {
                //Dữ liệu gửi lên bị trùng
                errorMsg = new
                {
                    devMsg,
                    userMsg = string.Format(ResourceGeneralVI.ResourceManager.GetString("Validate_Msg_ExistedImportField"), "Dữ liệu", "<>"),
                    errorCode = "RQ_02"
                };
                _serviceResult.ServiceResultCode = ServiceCode.BadRequest;
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
                _serviceResult.ServiceResultCode = ServiceCode.InternalServerError;
            }
            _serviceResult.IsValid = false;
            _serviceResult.Data = errorMsg;
            return _serviceResult;
        }

        public ServiceResult DeleteMultiple(List<Guid> guids)
        {
            try
            {
                int rowAffected = _baseRepository.DeleteMultiple(guids);
                if (rowAffected == 0)
                {
                    _serviceResult.IsValid = false;
                    var errorMsg = new
                    {
                        devMsg = ResourceGeneralVI.Failed_Delete_Msg,
                        userMsg = ResourceGeneralVI.Failed_Delete_Msg,
                        errorCode = "DB_01"
                    };
                    _serviceResult.ServiceResultCode = ServiceCode.InternalServerError;
                }
            }
            catch (Exception ex)
            {
                {
                    _serviceResult.IsValid = false;
                    var errorMsg = new
                    {
                        devMsg = ex.Message,
                        userMsg = ResourceGeneralVI.Failed_Delete_Msg,
                        errorCode = "DB_05"
                    };
                    _serviceResult.Data = errorMsg;
                    _serviceResult.ServiceResultCode = ServiceCode.InternalServerError;
                }
            }
            return _serviceResult;
        }
    }
}
