using MISA.AMIS.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.Core.Interfaces.Service
{
    public interface IBaseService<CustomEntity>
    {
        #region Get
        ServiceResult Get();

        ServiceResult GetByID(Guid entiyID);


        #endregion


        ServiceResult Add(CustomEntity customEntity);

        ServiceResult Update(Guid entityID, CustomEntity customEntity);

        ServiceResult Delete(Guid entityID);


        #region Utils

        ServiceResult CheckDuplicatedField(string fieldName, object fieldValue, Type fieldType);

        List<string> BaseValidate(Guid entityID, CustomEntity customEntity, int validateMode);

        List<string> CustomValidate(CustomEntity customEntity);
        ServiceResult GetNewCode();
        ServiceResult DeleteMultiple(List<Guid> entityGuids);

        #endregion
    }
}
