/// Written by: Yulia Danilova
/// Creation Date: 02nd of December, 2020
/// Purpose: Interface for getting or saving data to and from an ORM database 
/// Remarks: The connection string is injected from the composition root!

#region ========================================================================= USING =====================================================================================
using Dapper;
using System;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Collections.Generic;
using Leya.DataAccess.Common.Enums;
using Leya.DataAccess.Common.Models;
using Leya.DataAccess.Common.Attributes;
#endregion

namespace Leya.DataAccess.StorageAccess
{
    internal sealed class SqlDataAccess : IDataAccess
    {
        #region ============================================================== FIELD MEMBERS ================================================================================
        private bool isTransactionFaulty;
        private IDbTransaction dbTransaction;
        private readonly IDbConnection dbConnection;
        #endregion

        #region ================================================================ PROPERTIES =================================================================================
        public string ConnectionString { get; set; }
        #endregion

        #region ================================================================== CTOR =====================================================================================
        /// <summary>
        /// Overload C-tor
        /// </summary>
        /// <param name="dbConnection">The injected database connection to use</param>
        internal SqlDataAccess(IDbConnection dbConnection)
        {
            this.dbConnection = dbConnection;
            // map the string type to the bool database type (save booleans as strings, not integers)
            SqlMapper.AddTypeMap(typeof(bool), DbType.String);
        }

        /// <summary>
        /// Default Destructor
        /// </summary>
        ~SqlDataAccess()
        {
            dbConnection?.Dispose();
        }
        #endregion

        #region ================================================================= METHODS ===================================================================================
        /// <summary>
        /// Selects data of type <typeparamref name="TEntity"/> from the database
        /// </summary>
        /// <typeparam name="TEntity">The type of the model to get from the database</typeparam>
        /// <param name="table">The table from which to select the data</param>
        /// <param name="columns">The columns to take from <paramref name="table"/></param>
        /// <param name="filter">Used for conditional selects, specifies an object whose properties are used for the conditions (SELECT ... FROM ... WHERE ...)</param>
        /// <returns>An <see cref="ApiResponse{TEntity}"/> instance containing the requested data from the database, or the provided error, in case of failure</returns>
        public async Task<ApiResponse<TEntity>> SelectAsync<TEntity>(EntityContainers table, string columns = null, object filter = null) where TEntity : IStorageEntity
        {
            ApiResponse<TEntity> serializedData = new ApiResponse<TEntity>();
            try
            {
                // check if the connection string was previously assigned, and if not, assign it
                if (string.IsNullOrEmpty(dbConnection.ConnectionString))
                    dbConnection.ConnectionString = ConnectionString;
                // reconstruct the SQL query from the parameters
                string query = "SELECT " + (string.IsNullOrEmpty(columns) ? "*" : columns) + " FROM " + table + (filter != null ? " WHERE " : string.Empty);
                // if a filter was specified, add the properties and their values from filter as the WHERE clauses of the SQL query
                Dictionary<string, object> args = null;
                if (filter != null)
                {
                    // cannot use lambda expressions as arguments to dynamically dispatched operations, so, declare it externally, for readability
                    Func<PropertyInfo, bool> predicate = e => e.GetValue(filter) != null;
                    // get the properties of the filter object
                    PropertyInfo[] filters = filter.GetType()
                                                   .GetProperties()
                                                   .Where(e => predicate(e))
                                                   .ToArray();
                    args = new Dictionary<string, object>();
                    int i = 0;
                    // reconstruct the WHERE clause part of the string from the properties of filter, using named parameters
                    foreach (PropertyInfo f in filters)
                    {
                        query += f.Name + " = @" + f.Name;
                        args.Add("@" + f.Name, f.GetValue(filter));
                        if (filters.Length > 1 && i < filters.Length - 1)
                        {
                            query += " AND ";
                            i++;
                        }
                    }
                }
                // SQLite is case sensitive on comparisons
                if (dbConnection.GetType().Name == "SQLiteConnection")
                    query += filter != null ? " COLLATE NOCASE" : string.Empty;
                query += ";";
#if DEBUG
                // Console.WriteLine(query);
#endif
                // execute the SQL query
                TEntity[] temp = (await dbConnection.QueryAsync<TEntity>(query, args ?? null)).ToArray();
                // construct the API response and return it
                serializedData.Data = temp.Length > 0 ? temp : null;
                serializedData.Count = temp.Length;
            }
            catch (Exception ex)
            {
                // mark the transaction as faulty, so it is rolled back
                isTransactionFaulty = true;
                // in case of exceptions, populate the Error property of the API response
                serializedData.Error = ex.Message;
                Console.WriteLine(ex.Message);
            }
            return serializedData;
        }

        /// <summary>
        /// Saves data of type <typeparamref name="TEntity"/> in the database
        /// </summary>
        /// <typeparam name="TEntity">The type of the model to save in the database</typeparam>
        /// <param name="container">The table in which to insert data</param>
        /// <param name="model">The model to be saved</param>
        /// <returns>An <see cref="ApiResponse{TEntity}"/> instance containing the id of the inserted data from the database, or the provided error, in case of failure</returns>
        public async Task<ApiResponse<TEntity>> InsertAsync<TEntity>(EntityContainers container, TEntity model) where TEntity : IStorageEntity, new()
        {
            ApiResponse<TEntity> serializedData = new ApiResponse<TEntity>();
            try
            {
                // check if the connection string was previously assigned, and if not, assign it
                if (string.IsNullOrEmpty(dbConnection.ConnectionString))
                    dbConnection.ConnectionString = ConnectionString;
                // reconstruct the SQL query from the properties of the provided model
                string query = "INSERT INTO " + container + " (\n\t" + GetColumns(model) + ")\nVALUES (\n\t" + GetParameters(model) + ");";
                // also select last inserted row id
                if (dbConnection.GetType().Name == "SQLiteConnection")
                    query += "\nSELECT last_insert_rowid();";
                if (dbConnection.GetType().Name == "MySqlConnection")
                    query += "\nSELECT last_insert_id();";
#if DEBUG
                //Console.WriteLine(query);
#endif
                // execute the SQL query and construct the API response with the returned id of the inserted data
                serializedData.Data = new TEntity[] { new TEntity() { Id = await dbConnection.ExecuteScalarAsync<int>(query, model) } };
                serializedData.Count = 1;
            }
            catch (Exception ex)
            {
                // mark the transaction as faulty, so it is rolled back
                isTransactionFaulty = true;
                // in case of exceptions, populate the Error property of the API response
                serializedData.Error = ex.Message;
                Console.WriteLine(ex.Message);
            }
            return serializedData;
        }
        
        /// <summary>
        /// Updates data in the database
        /// </summary>
        /// <param name="table">The table in which to update the data</param>
        /// <param name="values">The values to be updated</param>
        /// <param name="condition">The condition for the rows to be updated</param>
        /// <param name="conditionValue">The value of the condition for the rows to be updated</param>
        /// <returns>An <see cref="ApiResponse"/> instance containing the count of affected rows in the database, or the provided error, in case of failure</returns>
        public async Task<ApiResponse> UpdateAsync(EntityContainers table, string values, string condition, string conditionValue)
        {
            ApiResponse serializedData = new ApiResponse();
            try
            {
                // check if the connection string was previously assigned, and if not, assign it
                if (string.IsNullOrEmpty(dbConnection.ConnectionString))
                    dbConnection.ConnectionString = ConnectionString;
                // reconstruct the SQL query from the parameters
                string query = "UPDATE " + table + " SET " + values + " WHERE " + condition + " = " + conditionValue + ";";
#if DEBUG
                //Console.WriteLine(query);
#endif
                // execute the SQL query and construct the API response with the number of rows affected
                serializedData.Count = await dbConnection.ExecuteAsync(query);
            }
            catch (Exception ex)
            {
                // mark the transaction as faulty, so it is rolled back
                isTransactionFaulty = true;
                // in case of exceptions, populate the Error property of the API response
                serializedData.Error = ex.Message;
                Console.WriteLine(ex.Message);
            }
            return serializedData;
        }

        /// <summary>
        /// Deletes data from the database
        /// </summary>
        /// <param name="table">The table from which to delete data</param>
        /// <param name="filter">Used for conditional deletes, specifies an object whose properties are used for the conditions (DELETE FROM ... WHERE ...)</param>
        /// <returns>An <see cref="ApiResponse"/> instance containing the count of affected rows in the database, or the provided error, in case of failure</returns>
        public async Task<ApiResponse> DeleteAsync(EntityContainers table, object filter = null)
        {
            // check if the connection string was previously assigned, and if not, assign it
            if (string.IsNullOrEmpty(dbConnection.ConnectionString))
                dbConnection.ConnectionString = ConnectionString;
            ApiResponse serializedData = new ApiResponse();
            try
            {
                // reconstruct the SQL query from the parameters
                string query = "DELETE FROM " + table + (filter != null ? " WHERE " : string.Empty);
                Dictionary<string, object> args = null;
                // if a filter was specified, add the properties and their values from filter as the WHERE clauses of the SQL query
                if (filter != null)
                {
                    // cannot use lambda expressions as arguments to dynamically dispatched operations, so, declare it externally, for readability
                    Func<PropertyInfo, bool> predicate = e => e.GetValue(filter) != null;
                    // get the properties of the filter object
                    PropertyInfo[] filters = filter.GetType()
                                                  .GetProperties()
                                                  .Where(e => predicate(e))
                                                  .ToArray();
                    args = new Dictionary<string, object>();
                    int i = 0;
                    // reconstruct the WHERE clause part of the string from the properties of filter, using named parameters
                    foreach (PropertyInfo f in filters)
                    {
                        query += f.Name + " = @" + f.Name;
                        args.Add("@" + f.Name, f.GetValue(filter));
                        if (filters.Length > 1 && i < filters.Length - 1)
                        {
                            query += " AND ";
                            i++;
                        }
                    }
                }
                // SQLite is case sensitive on comparisons
                if (dbConnection.GetType().Name == "SQLiteConnection")
                    query += filter != null ? " COLLATE NOCASE" : string.Empty;
                query += ";";
#if DEBUG
                //Console.WriteLine(query);
#endif
                // execute the SQL query and construct the API response with the number of rows affected
                serializedData.Count = await dbConnection.ExecuteAsync(query, args ?? null);
            }
            catch (Exception ex)
            {
                // mark the transaction as faulty, so it is rolled back
                isTransactionFaulty = true;
                // in case of exceptions, populate the Error property of the API response
                serializedData.Error = ex.Message;
                Console.WriteLine(ex.Message);
            }
            return serializedData;
        }

        /// <summary>
        /// Gets the names of the public properties of <paramref name="model"/> and formats them as a string used in SQL queries
        /// </summary>
        /// <typeparam name="TEntity">The type of <paramref name="model"/></typeparam>
        /// <param name="model">A database model containing the public properties used for getting or saving data in a database table</param>
        /// <returns>A formatted string used in SQL queries, composed of the names of the public properties of <paramref name="model"/></returns>
        internal string GetColumns<TEntity>(TEntity model)
        {
            // get a list of all public properties of the provided model, but ignore those with the IgnoreOnInsert attribute
            IEnumerable<PropertyInfo> properties = model.GetType()
                                                        .GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance)
                                                        .Where(e => e.GetCustomAttributes<IgnoreOnInsertAttribute>()
                                                        .Count() == 0);
            // concatenate all the above properties in a single string, separated by comas, and escape them as SQL column names
            return string.Join(",\n\t", properties.Select(e => "`" + e.Name + "`"));
        }

        /// <summary>
        /// Gets the values of the public properties of <paramref name="model"/> and formats them as a string used in SQL queries
        /// </summary>
        /// <typeparam name="TEntity">The type of <paramref name="model"/></typeparam>
        /// <param name="model">A database model containing the public properties whose values are used for saving data in a database table</param>
        /// <returns>A formatted string used in SQL queries, composed of the columns of the public properties of <paramref name="model"/></returns>
        internal string GetParameters<TEntity>(TEntity model)
        {
            // get a list of all public properties of the provided model, but ignore those with the IgnoreOnInsert attribute
            IEnumerable<PropertyInfo> properties = model.GetType()
                                                        .GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance)
                                                        .Where(e => e.GetCustomAttributes<IgnoreOnInsertAttribute>()
                                                        .Count() == 0);
            // concatenate all the above properties in a single string, separated by comas, and escape them as SQL column parameters
            return string.Join(",\n\t", properties.Select(e => "@" + e.Name));
        }

        /// <summary>
        /// Opens an SQL transaction
        /// </summary>
        public void OpenTransaction()
        {
            // check if the connection string was previously assigned, and if not, assign it
            if (string.IsNullOrEmpty(dbConnection.ConnectionString))
                dbConnection.ConnectionString = ConnectionString;
            // check if the database connection was previously opened, and it not, open it
            if (dbConnection.State == ConnectionState.Closed)
            {
                dbConnection.Open();
                if (dbConnection.GetType().Name == "SQLiteConnection")
                {
                    // set SQLite WAL mode, for increased insert performance: https://sqlite.org/wal.html
                    using (IDbCommand command = dbConnection.CreateCommand())
                    {
                        command.CommandText = "PRAGMA journal_mode = WAL; PRAGMA synchronous = NORMAL;";
                        command.ExecuteNonQuery();
                    }
                }
            }
            // begin the transaction
            dbTransaction = dbConnection.BeginTransaction();
        }

        /// <summary>
        /// Closes an SQL transaction, rolls back changes if the transaction was faulty
        /// </summary>
        public void CloseTransaction()
        {
            // try and commit the transaction changes, if it's not marked as faulty
            try
            {
                if (!isTransactionFaulty)
                    dbTransaction?.Commit();
                else
                {
                    isTransactionFaulty = false;
                    dbTransaction?.Rollback();
                }
            }
            catch (Exception ex)
            {
                // if the transaction failed, rollback the changes
                dbTransaction?.Rollback();
                Console.WriteLine(ex.Message);
            }
        }
        #endregion
    }
}
