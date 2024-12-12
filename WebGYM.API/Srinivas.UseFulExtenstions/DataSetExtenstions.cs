using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Srinivas.UseFulExtenstions
{
    
    public static class DataSetExtenstions
    {

        /// <summary>
        /// Assiging tablenames to dataset
        /// </summary>
        /// <typeparam name="DataSet"></typeparam>
        /// <param name="dataSet"></param>
        /// <param name="tableNames"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public async static Task AssignTablesNameAsync<DataSet>(this System.Data.DataSet dataSet, string[] tableNames)
        {
            if (tableNames == null)
                throw new ArgumentNullException("table Names should not be empty.");
            int count = 0;
            foreach (string tableName in tableNames)
            {
                dataSet.Tables[count].TableName = tableName;
                count++;
            }
        }
        /// <summary>
        /// Assiging tablenames to dataset
        /// </summary>
        /// <typeparam name="DataSet"></typeparam>
        /// <param name="dataSet"></param>
        /// <param name="tableNames"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void AssignTablesName<DataSet>(this System.Data.DataSet dataSet, string[] tableNames)
        {
            if (tableNames == null)
                throw new ArgumentNullException("table Names should not be empty.");
            int count = 0;
            foreach (string tableName in tableNames)
            {
                dataSet.Tables[count].TableName = tableName;
                count++;
            }
        }
        /// <summary>
        /// Building relation between tables and assigning null or default values to respective columns.
        /// then if we convert data set to json then we can get the column names in json string.
        /// </summary>
        /// <typeparam name="DataSet"></typeparam>
        /// <param name="result"></param>
        /// <param name="objRelationList"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void BuildRelationBetweenTables<DataSet>(this System.Data.DataSet result, List<RelationTable> objRelationList)
        {
            if (objRelationList == null)
                throw new ArgumentNullException("Relation Table List Should not be empty");
            foreach (RelationTable objRelation in objRelationList)
            {
                if (!result.Relations.Equals(objRelation.RelationName))
                {
                    DataRelation tablesRelation = new DataRelation(
                       objRelation.RelationName,
                        result.Tables[objRelation.ParentTableName].Columns[objRelation.ParentTableKeycolumn],
                        result.Tables[objRelation.ChaildTableName].Columns[objRelation.ChaildTableKeycolumn], true);
                    tablesRelation.Nested = true;
                    result.Relations.Add(tablesRelation);
                }
            }
            foreach (DataTable table in result.Tables)
            {
                foreach (DataColumn column in table.Columns)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        if (row.IsNull(column))
                        {
                            if (column.DataType == typeof(int) || column.DataType == typeof(long) ||
                                column.DataType == typeof(short) || column.DataType == typeof(float) ||
                                column.DataType == typeof(double) || column.DataType == typeof(decimal))
                                row[column] = 0;
                            else if (column.DataType == typeof(string))
                                row[column] = "";
                            else if (column.DataType == typeof(DateTime))
                                row[column] = DateTime.MinValue;
                            // row[column] = Nullable<DateTime>;
                            else if (column.DataType == typeof(bool))
                                row[column] = false;
                            else if (column.DataType == typeof(byte))
                                row[column] = (byte)0;
                            else if (column.DataType == typeof(Guid))
                                row[column] = Guid.Empty;
                            // Add more data types as needed
                        }
                    }
                }
            }
            //return result==null?new DataSet(): result;
        }
        /// <summary>
        /// Building relation between tables and assigning null or default values to respective columns.
        /// then if we convert data set to json then we can get the column names in json string.
        /// </summary>
        /// <typeparam name="DataSet"></typeparam>
        /// <param name="result"></param>
        /// <param name="objRelationList"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public async static Task BuildRelationBetweenTablesAsync<DataSet>(this System.Data.DataSet result, List<RelationTable> objRelationList)
        {
            if (objRelationList == null)
                throw new ArgumentNullException("Relation Table List Should not be empty");
            foreach (RelationTable objRelation in objRelationList)
            {
                if (!result.Relations.Equals(objRelation.RelationName))
                {
                    DataRelation tablesRelation = new DataRelation(
                       objRelation.RelationName,
                        result.Tables[objRelation.ParentTableName].Columns[objRelation.ParentTableKeycolumn],
                        result.Tables[objRelation.ChaildTableName].Columns[objRelation.ChaildTableKeycolumn], true);
                    tablesRelation.Nested = true;
                    result.Relations.Add(tablesRelation);
                }
            }
            foreach (DataTable table in result.Tables)
            {
                foreach (DataColumn column in table.Columns)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        if (row.IsNull(column))
                        {
                            if (column.DataType == typeof(int) || column.DataType == typeof(long) ||
                                column.DataType == typeof(short) || column.DataType == typeof(float) ||
                                column.DataType == typeof(double) || column.DataType == typeof(decimal))
                                row[column] = 0;
                            else if (column.DataType == typeof(string))
                                row[column] = "";
                            else if (column.DataType == typeof(DateTime))
                                row[column] = DateTime.MinValue;
                            // row[column] = Nullable<DateTime>;
                            else if (column.DataType == typeof(bool))
                                row[column] = false;
                            else if (column.DataType == typeof(byte))
                                row[column] = (byte)0;
                            else if (column.DataType == typeof(Guid))
                                row[column] = Guid.Empty;
                            // Add more data types as needed
                        }
                    }
                }
            }
            //return result==null?new DataSet(): result;
        }
        /// <summary>
        /// Convert Data set to Json string.
        /// </summary>
        /// <typeparam name="DataSet"></typeparam>
        /// <param name="dataSet"></param>
        /// <returns></returns>
        public async static Task<string> ConvertDataSetToJsonAsync<DataSet>(this System.Data.DataSet dataSet)
        {
            var jsonResult = new JObject();
            foreach (DataTable table in dataSet.Tables)
            {
                var tableArray = new JArray();
                foreach (DataRow row in table.Rows)
                {
                    var rowObject = new JObject();
                    foreach (DataColumn column in table.Columns)
                    {
                        rowObject[column.ColumnName] = JToken.FromObject(row[column]);
                    }
                    // Add related tables
                    foreach (DataRelation relation in table.ChildRelations)
                    {
                        var childArray = new JArray();
                        foreach (DataRow childRow in row.GetChildRows(relation))
                        {
                            var childObject = new JObject();
                            foreach (DataColumn column in childRow.Table.Columns)
                            {
                                childObject[column.ColumnName] = JToken.FromObject(childRow[column]);
                            }
                            childArray.Add(childObject);
                        }
                        rowObject[relation.RelationName] = childArray;
                    }
                    tableArray.Add(rowObject);
                }
                jsonResult[table.TableName] = tableArray;
            }
            return await Task.FromResult<string>(JsonConvert.SerializeObject(jsonResult, Formatting.Indented));
        }
        /// <summary>
        /// Convert Data set to Json string.
        /// </summary>
        /// <typeparam name="DataSet"></typeparam>
        /// <param name="dataSet"></param>
        /// <returns></returns>
        public static string ConvertDataSetToJson<DataSet>(this System.Data.DataSet dataSet)
        {
            var jsonResult = new JObject();
            foreach (DataTable table in dataSet.Tables)
            {
                var tableArray = new JArray();
                foreach (DataRow row in table.Rows)
                {
                    var rowObject = new JObject();
                    foreach (DataColumn column in table.Columns)
                    {
                        rowObject[column.ColumnName] = JToken.FromObject(row[column]);
                    }
                    // Add related tables
                    foreach (DataRelation relation in table.ChildRelations)
                    {
                        var childArray = new JArray();
                        foreach (DataRow childRow in row.GetChildRows(relation))
                        {
                            var childObject = new JObject();
                            foreach (DataColumn column in childRow.Table.Columns)
                            {
                                childObject[column.ColumnName] = JToken.FromObject(childRow[column]);
                            }
                            childArray.Add(childObject);
                        }
                        rowObject[relation.RelationName] = childArray;
                    }
                    tableArray.Add(rowObject);
                }
                jsonResult[table.TableName] = tableArray;
            }
            return JsonConvert.SerializeObject(jsonResult, Formatting.Indented);
        }
    }
}
