using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.Core.Interfaces.Repository
{
    public interface IBaseRepository<CustomEntity>
    {
        #region Get
        List<CustomEntity> Get();

        CustomEntity GetByID(Guid entityID);


        #endregion


        int Add(CustomEntity customEntity);

        int Update(Guid entityID, CustomEntity customEntity);

        int Delete(Guid entityID);


        #region Utils

        List<CustomEntity> CheckDuplicatedField(string fieldName, object fieldValue, Type fieldType);
        string GetHighestCode();
        int DeleteMultiple(List<Guid> guids);

        #endregion

    }
}
