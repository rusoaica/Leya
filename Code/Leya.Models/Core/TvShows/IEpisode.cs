/// Written by: Yulia Danilova
/// Creation Date: 05th of July, 2021
/// Purpose: Interface business model for episodes
#region ========================================================================= USING =====================================================================================
using System.Threading.Tasks;
using Leya.Models.Common.Models.TvShows;
#endregion

namespace Leya.Models.Core.TvShows
{
    public interface IEpisode
    {
        #region ================================================================ PROPERTIES =================================================================================
        EpisodeEntity[] Episodes { get; set; }
        #endregion

        #region ================================================================= METHODS ===================================================================================
        /// <summary>
        /// Gets the tv shows from the storage medium
        /// </summary>
        Task GetAllAsync();

        /// <summary>
        /// Saves <paramref name="episodeEntity"/> in the storage medium
        /// </summary>
        /// <param name="episodeEntity">The episode to be saved</param>
        Task SaveAsync(EpisodeEntity episodeEntity);

        /// <summary>
        /// Updates the IsWatched status of an episode identified by <paramref name="episodeId"/> in the storage medium
        /// </summary>
        /// <param name="episodeId">The id of the episode whose status will be updated</param>
        /// <param name="isWatched">The IsWatched status to be set</param>
        Task UpdateIsWatchedStatusAsync(int episodeId, bool? isWatched);

        /// <summary>
        /// Updates the IsFavorite status of an episode identified by <paramref name="episodeId"/> in the storage medium
        /// </summary>
        /// <param name="episodeId">The id of the episode whose status will be updated</param>
        /// <param name="isFavorite">The IsFavorite status to be set</param>
        Task UpdateIsFavoriteStatusAsync(int episodeId, bool isFavorite);
        #endregion
    }
}
