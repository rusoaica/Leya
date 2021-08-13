/// Written by: Yulia Danilova
/// Creation Date: 05th of July, 2021
/// Purpose: Interface business model for tv shows
#region ========================================================================= USING =====================================================================================
using System.Threading.Tasks;
using Leya.Models.Common.Models.TvShows;
#endregion

namespace Leya.Models.Core.TvShows
{
    public interface ITvShow
    {
        #region ================================================================ PROPERTIES =================================================================================
        TvShowEntity[] TvShows { get; set; }
        #endregion

        #region ================================================================= METHODS ===================================================================================
        /// <summary>
        /// Gets the tv shows from the storage medium
        /// </summary>
        Task GetTvShowsAsync();

        /// <summary>
        /// Updates the IsWatched status of a tv show identified by <paramref name="tvShowId"/> in the storage medium
        /// </summary>
        /// <param name="tvShowId">The id of the tv show whose status will be updated</param>
        /// <param name="isWatched">The IsWatched status to be set</param>
        Task UpdateIsWatchedStatusAsync(int tvShowId, bool isWatched);

        /// <summary>
        /// Updates the IsFavorite status of a tv show identified by <paramref name="tvShowId"/> in the storage medium
        /// </summary>
        /// <param name="tvShowId">The id of the tv show whose status will be updated</param>
        /// <param name="isFavorite">The IsFavorite status to be set</param>
        Task UpdateIsFavoriteStatusAsync(int tvShowId, bool isFavorite);
        #endregion
    }
}
