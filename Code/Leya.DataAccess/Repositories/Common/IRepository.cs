/// Written by: Yulia Danilova
/// Creation Date: 11th of June, 2021
/// Purpose: Interface for interaction with a generic persistance medium
#region ========================================================================= USING =====================================================================================
using System.Threading.Tasks;
using Leya.DataAccess.Common.Models;
using Leya.DataAccess.StorageAccess;
#endregion

namespace Leya.DataAccess.Repositories.Common
{
    public interface IRepository<TEntity> where TEntity : IStorageEntity
    {
        #region ================================================================= METHODS ===================================================================================
        /// <summary>
        /// Deletes all entities from the storage medium
        /// </summary>
        /// <returns>The result of deleting the entities, wrapped in a generic API container of type <see cref="ApiResponse"/></returns>
        Task<ApiResponse> DeleteAllAsync();

        /// <summary>
        /// Deletes an entity identified by <paramref name="id"/> from the storage medium
        /// </summary>
        /// <param name="id">The id of the entity to be deleted</param>
        /// <returns>The result of deleting the entity, wrapped in a generic API container of type <see cref="ApiResponse"/></returns>
        Task<ApiResponse> DeleteByIdAsync(int id);

        /// <summary>
        /// Gets all data of type <typeparamref name="TEntity"/> from the storage medium
        /// </summary>
        /// <returns>A list of <typeparamref name="TEntity"/>, wrapped in a generic API container of type <see cref="ApiResponse{TEntity}"/></returns>
        Task<ApiResponse<TEntity>> GetAllAsync();

        /// <summary>
        /// Updates <paramref name="entity"/> in the storage medium
        /// </summary>
        /// <param name="entity">The entity that will be updated</param>
        /// <returns>The result of updating the <paramref name="entity"/>, wrapped in a generic API container of type <see cref="ApiResponse"/></returns>
        Task<ApiResponse> UpdateAsync(TEntity entity);

        /// <summary>
        /// Gets an entity of type <typeparamref name="TEntity"/> identified by <paramref name="id"/> from the storage medium
        /// </summary>
        /// <param name="id">The id of the entity to get</param>
        /// <returns>An entity of type <typeparamref name="TEntity"/> identified by <paramref name="id"/>, wrapped in a generic API container of type <see cref="ApiResponse{TEntity}"/></returns>
        Task<ApiResponse<TEntity>> GetByIdAsync(int id);

        /// <summary>
        /// Saves an entity of type <typeparamref name="TEntity"/> in the storage medium
        /// </summary>
        /// <param name="entity">The entity to be saved</param>
        /// <returns>The result of saving <paramref name="entity"/>, wrapped in a generic API container of type <see cref="ApiResponse{TEntity}"/></returns>
        Task<ApiResponse<TEntity>> InsertAsync(TEntity entity);
        #endregion
    }
}
