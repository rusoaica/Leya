/// Written by: Yulia Danilova
/// Creation Date: 20th of November, 2020
/// Purpose: Tv show repository interface for the bridge-through between the generic storage medium and storage medium for Tv Shows
#region ========================================================================= USING =====================================================================================
using System.Threading.Tasks;
using Leya.DataAccess.Common.Models;
using Leya.DataAccess.Repositories.Common;
using Leya.DataAccess.Common.Models.TvShows;
#endregion

namespace Leya.DataAccess.Repositories.TvShows
{
    public interface ITvShowRepository : IRepository<TvShowEntity>
    {
        #region ================================================================= METHODS ===================================================================================
        /// <summary>
        /// Updates the IsWatched status of a tv show identified by <paramref name="tvShowId"/> in the storage medium
        /// </summary>
        /// <param name="tvShowId">The id of the tv show whose status will be updated</param>
        /// <param name="isWatched">The IsWatched status to be set</param>
        /// <returns>The result of updating the IsWatched status, wrapped in a generic API container of type <see cref="ApiResponse"/></returns>
        Task<ApiResponse> UpdateIsWatchedStatusAsync(int tvShowId, bool? isWatched);

        /// <summary>
        /// Updates the IsFavorite status of a tv show identified by <paramref name="tvShowId"/> in the storage medium
        /// </summary>
        /// <param name="tvShowId">The id of the tv show whose status will be updated</param>
        /// <param name="isFavorite">The IsFavorite status to be set</param>
        /// <returns>The result of updating the IsFavorite status, wrapped in a generic API container of type <see cref="ApiResponse"/></returns>
        Task<ApiResponse> UpdateIsFavoriteStatusAsync(int tvShowId, bool isFavorite);
        #endregion
    }
}
