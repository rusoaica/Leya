/// Written by: Yulia Danilova
/// Creation Date: 02nd of December, 2020
/// Purpose: Seasons repository interface for the bridge-through between the generic storage medium and storage medium for Seasons
#region ========================================================================= USING =====================================================================================
using System.Threading.Tasks;
using Leya.DataAccess.Common.Models;
using Leya.DataAccess.Repositories.Common;
using Leya.DataAccess.Common.Models.Seasons;
#endregion

namespace Leya.DataAccess.Repositories.Seasons
{
    public interface ISeasonRepository : IRepository<SeasonEntity>
    {
        #region ================================================================= METHODS ===================================================================================
        /// <summary>
        /// Updates the IsWatched status of a season identified by <paramref name="seasonId"/> in the storage medium
        /// </summary>
        /// <param name="seasonId">The id of the season whose status will be updated</param>
        /// <param name="isWatched">The IsWatched status to be set</param>
        /// <returns>The result of updating the IsWatched status, wrapped in a generic API container of type <see cref="ApiResponse"/></returns>
        Task<ApiResponse> UpdateIsWatchedStatusAsync(int seasonId, bool? isWatched);

        /// <summary>
        /// Updates the IsFavorite status of a season identified by <paramref name="seasonId"/> in the storage medium
        /// </summary>
        /// <param name="seasonId">The id of the season whose status will be updated</param>
        /// <param name="isFavorite">The IsFavorite status to be set</param>
        /// <returns>The result of updating the IsFavorite status, wrapped in a generic API container of type <see cref="ApiResponse"/></returns>
        Task<ApiResponse> UpdateIsFavoriteStatusAsync(int seasonId, bool isFavorite);
        #endregion
    }
}
