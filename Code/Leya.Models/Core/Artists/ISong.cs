/// Written by: Yulia Danilova
/// Creation Date: 07th of July, 2021
/// Purpose: Interface business model for songs
#region ========================================================================= USING =====================================================================================
using System.Threading.Tasks;
using Leya.Models.Common.Models.Artists;
#endregion

namespace Leya.Models.Core.Artists
{
    public interface ISong
    {
        #region ================================================================ PROPERTIES =================================================================================
        SongEntity[] Songs { get; set; }
        #endregion

        #region ================================================================= METHODS ===================================================================================
        /// <summary>
        /// Gets the songs from the storage medium
        /// </summary>
        Task GetSongsAsync();

        /// <summary>
        /// Updates the IsListened status of a song identified by <paramref name="songId"/> in the storage medium
        /// </summary>
        /// <param name="songId">The id of the song whose status will be updated</param>
        /// <param name="isListened">The IsListened status to be set</param>
        Task UpdateIsListenedStatusAsync(int songId, bool isListened);

        /// <summary>
        /// Updates the IsFavorite status of a song identified by <paramref name="songId"/> in the storage medium
        /// </summary>
        /// <param name="songId">The id of the song whose status will be updated</param>
        /// <param name="isFavorite">The IsFavorite status to be set</param>
        Task UpdateIsFavoriteStatusAsync(int songId, bool isFavorite);
        #endregion
    }
}
