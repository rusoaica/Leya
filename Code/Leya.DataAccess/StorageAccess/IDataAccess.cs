/// Written by: Yulia Danilova
/// Creation Date: 12th of June, 2021
/// Purpose: Interface for getting or saving data to and from a generic storage medium
#region ========================================================================= USING =====================================================================================
using System.Threading.Tasks;
using Leya.DataAccess.Common.Enums;
using Leya.DataAccess.Common.Models;
#endregion

namespace Leya.DataAccess.StorageAccess
{
    internal interface IDataAccess
    {
        #region ================================================================= METHODS ===================================================================================
        /// <summary>
        /// Opens a transaction
        /// </summary>
        void OpenTransaction();

        /// <summary>
        /// Closes a transaction, rolls back changes if the transaction was faulty
        /// </summary>
        void CloseTransaction();

        /// <summary>
        /// Deletes data from the storage medium
        /// </summary>
        /// <param name="container">The storage container from which to delete data</param>
        /// <param name="filter">Used for conditional deletes, specifies an object whose properties are used for the conditions</param>
        /// <returns>An <see cref="ApiResponse"/> instance containing the count of affected entries, or the provided error, in case of failure</returns>
        Task<ApiResponse> DeleteAsync(EntityContainers container, dynamic filter = null);

        /// <summary>
        /// Updates data in the storage medium
        /// </summary>
        /// <param name="container">The storage container in which to update the data</param>
        /// <param name="values">The values to be updated</param>
        /// <param name="condition">The condition for the entries to be updated</param>
        /// <param name="conditionValue">The value of the condition for the entries to be updated</param>
        /// <returns>An <see cref="ApiResponse"/> instance containing the count of affected entries, or the provided error, in case of failure</returns>
        Task<ApiResponse> UpdateAsync(EntityContainers container, string values, string condition, string conditionValue);

        /// <summary>
        /// Saves data of type <typeparamref name="TEntity"/> in the storage medium
        /// </summary>
        /// <typeparam name="TEntity">The type of the model to be saved</typeparam>
        /// <param name="container">The storage container in which to insert data</param>
        /// <param name="model">The model to be saved</param>
        /// <returns>An <see cref="ApiResponse{TEntity}"/> instance containing the id of the inserted data, or the provided error, in case of failure</returns>
        Task<ApiResponse<TEntity>> InsertAsync<TEntity>(EntityContainers container, TEntity model) where TEntity : IStorageEntity, new();

        /// <summary>
        /// Selects data of type <typeparamref name="TEntity"/> from the storage medium
        /// </summary>
        /// <typeparam name="TEntity">The type of the model to get from the storage medium</typeparam>
        /// <param name="container">The storage container from which to select the data</param>
        /// <param name="columns">The columns to take from <paramref name="container"/></param>
        /// <param name="filter">Used for conditional selects, specifies an object whose properties are used for the conditions</param>
        /// <returns>An <see cref="ApiResponse{TEntity}"/> instance containing the requested data from the storage medium, or the provided error, in case of failure</returns>
        Task<ApiResponse<TEntity>> SelectAsync<TEntity>(EntityContainers container, string columns = null, dynamic filter = null) where TEntity : IStorageEntity;
        #endregion
    }
}
