using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Srinivas.UseFulExtenstions
{
    public interface IGenericRepository<T> where T : class
    {
        #region Query Based
        IEnumerable<T> GetAll();
        T GetById(int id);
        void Add(T entity);
        void Update(T entity);
        void Delete(int id);
        #endregion
        #region Procedure Based
        Task<IEnumerable<T>> GetAllAsync(string procedurename, IList<System.Data.SqlClient.SqlParameter> sqlParameterCollections = null);
        Task<T> GetByIdAsync(string procedurename, IList<System.Data.SqlClient.SqlParameter> sqlParameterCollections = null);
        Task AddAsync(T entity, string procedureName);
        Task UpdateAsync(T entity, string procedureName);
        Task DeleteAsync(int id, string procedureName);
        Task<DataSet> GetDataSet(string procedureName, IList<System.Data.SqlClient.SqlParameter> sqlparameterCollection = null);
        Task<string> GetDataSetAsString(string procedureName, string[] tableNames, List<RelationTable> relationTables = null, List<System.Data.SqlClient.SqlParameter> sqlparameterCollection = null);
        #endregion
    }

    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly string _connectionString;
        public GenericRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        #region Query Based
        public IEnumerable<T> GetAll()
        {
            var entities = new List<T>();
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand($"SELECT * FROM {typeof(T).Name}", connection);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var entity = Activator.CreateInstance<T>();
                        foreach (var prop in typeof(T).GetProperties())
                        {
                            prop.SetValue(entity, reader[prop.Name]);
                        }
                        entities.Add(entity);
                    }
                }
            }
            return entities;
        }

        public T GetById(int id)
        {
            T entity = null;
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand($"SELECT * FROM {typeof(T).Name}s WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", id);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        entity = Activator.CreateInstance<T>();
                        foreach (var prop in typeof(T).GetProperties())
                        {
                            prop.SetValue(entity, reader[prop.Name]);
                        }
                    }
                }
            }
            return entity;
        }

        public void Add(T entity)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand($"INSERT INTO {typeof(T).Name}s VALUES (@Name, @Price)", connection);
                foreach (var prop in typeof(T).GetProperties())
                {
                    command.Parameters.AddWithValue($"@{prop.Name}", prop.GetValue(entity));
                }
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void Update(T entity)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand($"UPDATE {typeof(T).Name}s SET Name = @Name, Price = @Price WHERE Id = @Id", connection);
                foreach (var prop in typeof(T).GetProperties())
                {
                    command.Parameters.AddWithValue($"@{prop.Name}", prop.GetValue(entity));
                }
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void Delete(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand($"DELETE FROM {typeof(T).Name}s WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", id);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }
        #endregion
        #region Procedure Based
        /// <summary>
        /// 
        /// </summary>
        /// <param name="procedureName"></param>
        /// <param name="sqlparameterCollection"></param>
        /// <returns></returns>
        public async Task<DataSet> GetDataSet(string procedureName, IList<System.Data.SqlClient.SqlParameter> sqlparameterCollection = null)
        {
            DataSet dataSet = new DataSet();
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand(procedureName, connection)
                {
                    CommandType = CommandType.StoredProcedure,
                    CommandTimeout = 0
                };
                if (sqlparameterCollection != null)
                {
                    if (sqlparameterCollection.Count > 0)
                    {
                        foreach (var param in sqlparameterCollection)
                            command.Parameters.Add(param);
                    }
                }
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                await connection.OpenAsync();
                adapter.Fill(dataSet, "Table");
            }
            return dataSet;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="procedureName"></param>
        /// <param name="tableNames"></param>
        /// <param name="relationTables"></param>
        /// <param name="sqlparameterCollection"></param>
        /// <returns></returns>
        public async Task<string> GetDataSetAsString(string procedureName,
                                                     string[] tableNames,
                                                     List<RelationTable> relationTables = null,
                                                     List<System.Data.SqlClient.SqlParameter> sqlparameterCollection = null)
        {
            DataSet dataSet = new DataSet();
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand(procedureName, connection)
                {
                    CommandType = CommandType.StoredProcedure,
                    CommandTimeout = 0
                };
                if (sqlparameterCollection != null)
                {
                    if (sqlparameterCollection.Count > 0)
                    {
                        foreach (var param in sqlparameterCollection)
                            command.Parameters.Add(param);
                    }
                }
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                await connection.OpenAsync();
                adapter.Fill(dataSet, "Table");
                if (tableNames != null)
                    await dataSet.AssignTablesNameAsync<DataSet>(tableNames);
                if (relationTables != null)
                    await dataSet.BuildRelationBetweenTablesAsync<DataSet>(relationTables);
                return await dataSet.ConvertDataSetToJsonAsync<DataSet>();
            }
            return string.Empty;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="procedurename"></param>
        /// <param name="sqlParameterCollections"></param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> GetAllAsync(string procedurename, IList<System.Data.SqlClient.SqlParameter> sqlParameterCollections = null)
        {
            var entities = new List<T>();
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand(procedurename, connection)
                {
                    CommandType = CommandType.StoredProcedure,
                    CommandTimeout = 0

                };
                foreach (var param in sqlParameterCollections)
                    command.Parameters.Add(param);
                await connection.OpenAsync();
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var entity = Activator.CreateInstance<T>();
                        foreach (var prop in typeof(T).GetProperties())
                        {
                            prop.SetValue(entity, reader[prop.Name]);
                        }
                        entities.Add(entity);
                    }
                }
            }
            return entities;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="procedureName"></param>
        /// <param name="sqlParameterCollections"></param>
        /// <returns></returns>

        public async Task<T> GetByIdAsync(string procedureName, IList<System.Data.SqlClient.SqlParameter> sqlParameterCollections)
        {
            T entity = null;
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand(procedureName, connection)
                {
                    CommandType = CommandType.StoredProcedure,
                    CommandTimeout = 0
                };
                foreach (var param in sqlParameterCollections)
                    command.Parameters.Add(param);
                await connection.OpenAsync();
                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        entity = Activator.CreateInstance<T>();
                        foreach (var prop in typeof(T).GetProperties())
                        {
                            prop.SetValue(entity, reader[prop.Name]);
                        }
                    }
                }
            }
            return entity;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="procedureName"></param>
        /// <returns></returns>
        public async Task AddAsync(T entity, string procedureName)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand(procedureName, connection)
                {
                    CommandType = CommandType.StoredProcedure
                    ,
                    CommandTimeout = 0
                };
                command.Parameters.Add(new SqlParameter { ParameterName = "@JsonString", Value = JsonConvert.SerializeObject(entity), Direction = ParameterDirection.Input, DbType = DbType.String });
                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="procedureName"></param>
        /// <returns></returns>
        public async Task UpdateAsync(T entity, string procedureName)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand(procedureName, connection)
                {
                    CommandType = CommandType.StoredProcedure,
                    CommandTimeout = 0
                };
                command.Parameters.Add(new SqlParameter { ParameterName = "@JsonString", Value = JsonConvert.SerializeObject(entity), Direction = ParameterDirection.Input, DbType = DbType.String });
                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="procedureName"></param>
        /// <returns></returns>
        public async Task DeleteAsync(int id, string procedureName)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand(procedureName, connection)
                {
                    CommandType = CommandType.StoredProcedure,
                    CommandTimeout = 0
                };
                command.Parameters.AddWithValue("@Id", id);
                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
            }
        }
        #endregion
    }
}
