/// Written by: Yulia Danilova
/// Creation Date: 02nd of December, 2020
/// Purpose: User repository interface for the bridge-through between the generic storage medium and storage medium for Users
#region ========================================================================= USING =====================================================================================
using System.Threading.Tasks;
using Leya.DataAccess.Common.Models;
using Leya.DataAccess.Common.Models.Users;
using Leya.DataAccess.Repositories.Common;
#endregion

namespace Leya.DataAccess.Repositories.Users
{
    public interface IUserRepository : IRepository<UserEntity>
    {
        #region ================================================================= METHODS ===================================================================================
        /// <summary>
        /// Gets the user identified by <paramref name="username"/> from the storage medium
        /// </summary>
        /// <param name="username">The Id of the user to get</param>
        /// <returns>A user identified by <paramref name="username"/>, wrapped in a generic API container of type <see cref="ApiResponse{UserEntity}"/></returns>
        Task<ApiResponse<UserEntity>> GetByUsernameAsync(string username);

        /// <summary>
        /// Updates the password for <paramref name="entity"/> in the storage medium
        /// </summary>
        /// <param name="entity">The user whose password will be updated</param>
        /// <returns>The result of updating the password of <paramref name="entity"/>, wrapped in a generic API container of type <see cref="ApiResponse"/></returns>
        Task<ApiResponse> ChangePasswordAsync(UserEntity entity);
        #endregion
    }
}
