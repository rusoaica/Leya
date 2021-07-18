/// Written by: Yulia Danilova
/// Creation Date: 02nd of November, 2020
/// Purpose: Episode repository interface for the bridge-through between the generic storage medium and storage medium for Episode
#region ========================================================================= USING =====================================================================================
using System.Threading.Tasks;
using Leya.DataAccess.Common.Models;
using Leya.DataAccess.Repositories.Common;
using Leya.DataAccess.Common.Models.Episodes;
#endregion

namespace Leya.DataAccess.Repositories.Episodes
{
    public interface IEpisodeRepository : IRepository<EpisodeEntity>
    {
        #region ================================================================= METHODS ===================================================================================
        /// <summary>
        /// Updates the IsWatched status of an episode identified by <paramref name="episodeId"/> in the storage medium
        /// </summary>
        /// <param name="episodeId">The id of the episode whose status will be updated</param>
        /// <param name="isWatched">The IsWatched status to be set</param>
        /// <returns>The result of updating the IsWatched status, wrapped in a generic API container of type <see cref="ApiResponse"/></returns>
        Task<ApiResponse> UpdateIsWatchedStatusAsync(int episodeId, bool isWatched);

        /// <summary>
        /// Updates the IsFavorite status of an episode identified by <paramref name="episodeId"/> in the storage medium
        /// </summary>
        /// <param name="episodeId">The id of the episode whose status will be updated</param>
        /// <param name="isFavorite">The IsFavorite status to be set</param>
        /// <returns>The result of updating the IsFavorite status, wrapped in a generic API container of type <see cref="ApiResponse"/></returns>
        Task<ApiResponse> UpdateIsFavoriteStatusAsync(int episodeId, bool isFavorite);

        /// <summary>
        /// Gets an episode whose tv show's id is identified by <paramref name="tvShowId"/> and its season is identified by <paramref name="seasonId"/>, and its associated data from the storage medium
        /// </summary>
        /// <param name="tvShowId">The id of the tv show of the episode to get</param>
        /// <param name="seasonId">The id of the season of the episode to get</param>
        /// <returns>The episode whose tv show's id is identified by <paramref name="tvShowId"/> and its season is identified by <paramref name="seasonId"/>, 
        /// wrapped in a generic API container of type <see cref="ApiResponse{EpisodeEntity}"/></returns>
        Task<ApiResponse<EpisodeEntity>> GetByIdAsync(int tvShowId, int seasonId);
        #endregion
    }
}
