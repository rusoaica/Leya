/// Written by: Yulia Danilova
/// Creation Date: 05th of July, 2021
/// Purpose: Interface business model for tv show seasons
#region ========================================================================= USING =====================================================================================
using System.Threading.Tasks;
using Leya.Models.Common.Models.TvShows;
#endregion

namespace Leya.Models.Core.TvShows
{
    public interface ISeason
    {
        #region ================================================================ PROPERTIES =================================================================================
        SeasonEntity[] Seasons { get; set; }
        #endregion

        #region ================================================================= METHODS ===================================================================================
        /// <summary>
        /// Gets the tv shows from the storage medium
        /// </summary>
        Task GetSeasonsAsync();

        /// <summary>
        /// Updates the IsWatched status of a season identified by <paramref name="seasonId"/> in the storage medium
        /// </summary>
        /// <param name="seasonId">The id of the season whose status will be updated</param>
        /// <param name="isWatched">The IsWatched status to be set</param>
        Task UpdateIsWatchedStatusAsync(int seasonId, bool isWatched);

        /// <summary>
        /// Updates the IsFavorite status of a season identified by <paramref name="seasonId"/> in the storage medium
        /// </summary>
        /// <param name="seasonId">The id of the season whose status will be updated</param>
        /// <param name="isFavorite">The IsFavorite status to be set</param>
        Task UpdateIsFavoriteStatusAsync(int seasonId, bool isFavorite);
        #endregion
    }
}
