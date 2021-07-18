/// Written by: Yulia Danilova
/// Creation Date: 12th of November, 2020
/// Purpose: User repository for the bridge-through between the generic storage medium and storage medium for Users
#region ========================================================================= USING =====================================================================================
using System.Threading.Tasks;
using Leya.DataAccess.Common.Enums;
using Leya.DataAccess.StorageAccess;
using Leya.DataAccess.Common.Models;
using Leya.DataAccess.Common.Models.Users;
#endregion

namespace Leya.DataAccess.Repositories.Users
{
    internal sealed class UserRepository : IUserRepository
    {
        #region ============================================================== FIELD MEMBERS ================================================================================
        private readonly IDataAccess dataAccess;
        #endregion

        #region ================================================================== CTOR =====================================================================================
        /// <summary>
        /// Overload C-tor
        /// </summary>
        /// <param name="dataAccess">The injected data access to use</param>
        public UserRepository(IDataAccess dataAccess)
        {
            this.dataAccess = dataAccess;
        }
        #endregion

        #region ================================================================= METHODS ===================================================================================
        /// <summary>
        /// Updates the password for <paramref name="entity"/> in the storage medium
        /// </summary>
        /// <param name="entity">The user whose password will be updated</param>
        /// <returns>The result of updating the password of <paramref name="entity"/>, wrapped in a generic API container of type <see cref="ApiResponse"/></returns>
        public async Task<ApiResponse> ChangePasswordAsync(UserEntity entity)
        {
            return await dataAccess.UpdateAsync(EntityContainers.Users, "Password = '" + entity.Password + "'", "Id", "'" + entity.Id + "'");
        }

        /// <summary>
        /// Deletes all users from the storage medium
        /// </summary>
        /// <returns>The result of deleting the users, wrapped in a generic API container of type <see cref="ApiResponse"/></returns>
        public async Task<ApiResponse> DeleteAllAsync()
        {
            // delete all users
            ApiResponse deleteUsers = await dataAccess.DeleteAsync(EntityContainers.Users);
            // check if the query resulted in an error
            if (string.IsNullOrEmpty(deleteUsers.Error))
                return deleteUsers;
            else
                return new ApiResponse() { Error = "Error deleting all users!" };
        }

        /// <summary>
        /// Deletes a user identified by <paramref name="id"/> from the storage medium
        /// </summary>
        /// <param name="id">The id of the user to be deleted</param>
        /// <returns>The result of deleting the user, wrapped in a generic API container of type <see cref="ApiResponse"/></returns>
        public async Task<ApiResponse> DeleteByIdAsync(int id)
        {
            // delete the user
            ApiResponse user = await dataAccess.DeleteAsync(EntityContainers.Users, new { Id = id });
            dataAccess.CloseTransaction();
            // check if any of the query returned an error
            if (string.IsNullOrEmpty(user.Error))
                return user;
            else
                return new ApiResponse() { Error = "Error deleting the user!" };
        }

        /// <summary>
        /// Gets all users from the storage medium
        /// </summary>
        /// <returns>A list of users, wrapped in a generic API container of type <see cref="ApiResponse{UserEntity}"/></returns>
        public async Task<ApiResponse<UserEntity>> GetAllAsync()
        {
            return await dataAccess.SelectAsync<UserEntity>(EntityContainers.Users, "Id, Username, Password, SecurityQuestion, SecurityAnswer, Created");
        }

        /// <summary>
        /// Gets the user identified by <paramref name="id"/> from the storage medium
        /// </summary>
        /// <param name="id">The Id of the user to get</param>
        /// <returns>A user identified by <paramref name="id"/>, wrapped in a generic API container of type <see cref="ApiResponse{UserEntity}"/></returns>
        public async Task<ApiResponse<UserEntity>> GetByIdAsync(int id)
        {
            return await dataAccess.SelectAsync<UserEntity>(EntityContainers.Users, "Id, Username, Password, SecurityQuestion, SecurityAnswer, Created", new { Id = id });
        }

        /// <summary>
        /// Gets the user identified by <paramref name="username"/> from the storage medium
        /// </summary>
        /// <param name="username">The Id of the user to get</param>
        /// <returns>A user identified by <paramref name="username"/>, wrapped in a generic API container of type <see cref="ApiResponse{UserEntity}"/></returns>
        public async Task<ApiResponse<UserEntity>> GetByUsernameAsync(string username)
        {
            return await dataAccess.SelectAsync<UserEntity>(EntityContainers.Users, "Id, Username, Password, SecurityQuestion, SecurityAnswer, Created", new { Username = username });
        }

        /// <summary>
        /// Saves a user in the storage medium
        /// </summary>
        /// <param name="entity">The user to be saved</param>
        /// <returns>The result of saving <paramref name="entity"/>, wrapped in a generic API container of type <see cref="ApiResponse{UserEntity}"/></returns>
        public async Task<ApiResponse<UserEntity>> InsertAsync(UserEntity entity)
        {
            return await dataAccess.InsertAsync(EntityContainers.Users, entity);
        }

        /// <summary>
        /// Updates <paramref name="entity"/> in the storage medium
        /// </summary>
        /// <param name="entity">The entity that will be updated</param>
        /// <returns>The result of updating <paramref name="entity"/>, wrapped in a generic API container of type <see cref="ApiResponse"/></returns>
        public async Task<ApiResponse> UpdateAsync(UserEntity entity)
        {
            return await dataAccess.UpdateAsync(EntityContainers.Users,
                "Username = '" + entity.Username +
                "', Password = '" + entity.Password +
                "', SecurityQuestion = '" + entity.SecurityQuestion +
                "', SecurityAnswer = '" + entity.SecurityAnswer + "'",
                "Id", "'" + entity.Id + "'");
        }
        #endregion
    }
}
