using Dapper;
using Microsoft.Extensions.Configuration;
using MISA.AMIS.Core.Interfaces.Repository;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MISA.AMIS.Core.CustomAttribute.CustomAttribute;

namespace MISA.AMIS.Infrastructure.Repository
{
    public class BaseRepository<CustomEntity> : IBaseRepository<CustomEntity>
    {
        protected static string _connectionString;
        protected string _className;
        public BaseRepository(IConfiguration configuration)
        {
            _className = typeof(CustomEntity).Name;
            _connectionString = configuration.GetConnectionString("localConnection");
        }

        public virtual int Add(CustomEntity entity)
        {
            // Tạo câu truy vấn thêm mới dữ liệu
            var dynamicParams = new DynamicParameters();
            var properties = entity.GetType().GetProperties();

            foreach (var prop in properties)
            {
                //Lấy tên prop
                var propName = prop.Name;

                //Lấy giá trị của prop
                var propValue = prop.GetValue(entity);
                var customAttrs = prop.GetCustomAttributes(typeof(NotMap), true);
                if (customAttrs.Length == 0)
                    dynamicParams.Add($"@{propName}", propValue);
            }

            // Khởi tạo đối tượng kết nối với csdl, Thực thi SQL để thêm mới dữ liệu
            using (IDbConnection dbConnection = new MySqlConnection(_connectionString))
            {
                string sqlCommand = $"Proc_Insert{_className}";
                var rowAffected = dbConnection.Execute(sqlCommand, param: dynamicParams, commandType:CommandType.StoredProcedure);
                return rowAffected;
            }
        }

        public virtual List<CustomEntity> CheckDuplicatedField(string fieldName, object fieldValue, Type fieldType)
        {
            DynamicParameters dynamicParams = new DynamicParameters();
            dynamicParams.Add("@fieldValue", fieldValue.ToString());
            string sqlCommand = $"SELECT *  FROM {_className} WHERE {fieldName} = @fieldValue";
            
            // Khởi tạo đối tượng kết nối với csdl, Thực thi SQL để thêm mới dữ liệu
            using (IDbConnection dbConnection = new MySqlConnection(_connectionString))
            {
                var duplicateEntities = dbConnection.Query<CustomEntity>(sqlCommand, param: dynamicParams).ToList();
                return duplicateEntities;
            }
        }

        /// <summary>
        /// Xóa thông tin đối tượng theo ID
        /// 
        /// </summary>
        /// <param name="entityID"></param>
        /// <returns></returns>
        /// CreatedBy datnvt (18/08)
        public int Delete(Guid entityID)
        {
            //Tạo query xóa bản ghi
            string sqlCommand = $"DELETE FROM  {_className} WHERE {_className}ID= @entityID";
            DynamicParameters dynamicParams = new();
            dynamicParams.Add("@entityID", entityID.ToString());

            //Thực hiện query  xóa bản ghi
            using (IDbConnection dbConnection = new MySqlConnection(_connectionString))
            {
                var rowAffected = dbConnection.Execute(sqlCommand, param: dynamicParams);
                return rowAffected;
            }
        }

        ///// <summary>
        ///// Lấy tất cả bản ghi.
        ///// </summary>
        ///// <returns>Danh sách đối tượng</returns>
        ///// CreatedBy datnvt (18/08)
        public List<CustomEntity> Get()
        {
            // Khởi tạo đối tượng kết nối với csdl, Thực thi SQL để thêm mới dữ liệu
            using (IDbConnection dbConnection = new MySqlConnection(_connectionString))
            {
                //Lấy dữ liệu 
                //string sqlCommand = $"SELECT * FROM {_className}";
                string procName = $"Proc_Get{_className}s";
                List<CustomEntity> entities = ((List<CustomEntity>)dbConnection.Query<CustomEntity>(procName, commandType: CommandType.StoredProcedure));
                return entities;
            }
        }

        /// <summary>
        /// Lấy thông tin của 1 đối tượng theo ID
        /// </summary>
        /// <param name="entityID"></param>
        /// <returns>Thông tin đối tượng</returns>
        /// CreatedBy datnvt (18/08)
        public CustomEntity GetByID(Guid entityID)
        {
            //Tạo query lấy dữ liẹu
            string sqlCommand = $"SELECT * FROM View_{_className} WHERE {_className}ID= @entityID";
            DynamicParameters dynamicParams = new();
            dynamicParams.Add("@entityID", entityID.ToString());

            // Khởi tạo đối tượng kết nối với csdl, Thực thi SQL để thêm mới dữ liệu
            using (IDbConnection dbConnection = new MySqlConnection(_connectionString))
            {
                CustomEntity entity = dbConnection.QueryFirstOrDefault<CustomEntity>(sqlCommand, param: dynamicParams);
                return entity;
            }
        }

        /// <summary>
        /// Cập nhật thông tin đối tượng
        /// </summary>
        /// <param name="entityID">ID đối tượng</param>
        /// <param name="entity">Object chứa thông tin đối tượng</param>
        /// <returns></returns>
        /// CreatedBy datnvt (18/08)
        public int Update(Guid entityID, CustomEntity entity)
        {
            // Tạo câu truy vấn thêm mới dữ liệu
            string updateColumnString = string.Empty;
            var dynamicParams = new DynamicParameters();
            var properties = entity.GetType().GetProperties();

            foreach (var prop in properties)
            {
                //Lấy tên prop
                var propName = prop.Name;

                //Lấy giá trị của prop
                var propValue = prop.GetValue(entity);
                var customAttrs = prop.GetCustomAttributes(typeof(NotMap), true);
                if (customAttrs.Length == 0)
                {
                    dynamicParams.Add($"@{propName}", propValue);
                    updateColumnString += $"{propName} = @{propName}, ";
                }

            }
            updateColumnString = updateColumnString.Remove(updateColumnString.Length - 2, 2);
            dynamicParams.Add($"@entityID", entityID.ToString());

            // Khởi tạo đối tượng kết nối với csdl, Thực thi SQL để thêm mới dữ liệu
            using (IDbConnection dbConnection = new MySqlConnection(_connectionString))
            {
                string sqlCommand = $"UPDATE  {_className} SET {updateColumnString} WHERE {_className}ID= @entityID";
                var rowAffected = dbConnection.Execute(sqlCommand, param: dynamicParams);
                return rowAffected;
            }
        }

        public string GetHighestCode()
        {
            // Khởi tạo đối tượng kết nối với csdl, Thực hiện query lấy mảng mã đối tượng  từ csdl
            using (IDbConnection dbConnection = new MySqlConnection(_connectionString))
            {
                string sqlCommand = $"Select {_className}Code from {_className} order by length({_className}Code) desc ,{_className}Code desc limit 1";
                var entityCode = dbConnection.QueryFirstOrDefault<string>(sqlCommand);
                return entityCode;
            }
        }

        public List<CustomEntity> GetByProperty(string fieldName, string fieldValue)
        {
            var dynamicParams = new DynamicParameters();
            dynamicParams.Add("@fieldValue", fieldValue);
            // Khởi tạo đối tượng kết nối với csdl, Thực hiện query đếm tổng số bản ghi của đối tượng  từ csdl
            using (IDbConnection dbConnection = new MySqlConnection(_connectionString))
            {
                string sqlCommand = $"SELECT * FROM  {_className} WHERE {fieldName} = @fieldValue";
                List<CustomEntity> entities = dbConnection.Query<CustomEntity>(sqlCommand, param: dynamicParams).ToList();
                return entities;
            }
        }

        public int DeleteMultiple(List<Guid> entityIDs)
        {
            int rowAffacted = 0;
            //Tạo kết nối đến CSDL
            IDbConnection dbConnection = new MySqlConnection(_connectionString);
            dbConnection.Open();

            //Bắt đầu transaction
            var transaction = dbConnection.BeginTransaction();
            string sqlCommand = $"DELETE FROM  {_className} WHERE {_className}ID= @entityID";
            for (int i = 0; i < entityIDs.Count; i++)
            {
                DynamicParameters dynamicParams = new();
                dynamicParams.Add("@entityID", entityIDs[i].ToString());
                rowAffacted += dbConnection.Execute(sqlCommand, transaction: transaction, param: dynamicParams);
            }
            if (rowAffacted == entityIDs.Count())
            {
                transaction.Commit();
            }
            else
            {
                transaction.Rollback();
            }

            return rowAffacted;
        }

    }
}
