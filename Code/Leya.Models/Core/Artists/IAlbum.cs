/// Written by: Yulia Danilova
/// Creation Date: 07th of July, 2021
/// Purpose: Interface business model for albums
#region ========================================================================= USING =====================================================================================
using System.Threading.Tasks;
using Leya.Models.Common.Models.Artists;
#endregion

namespace Leya.Models.Core.Artists
{
    public interface IAlbum
    {
        #region ================================================================ PROPERTIES =================================================================================
        AlbumEntity[] Albums { get; set; }
        #endregion

        #region ================================================================= METHODS ===================================================================================
        /// <summary>
        /// Gets the albums from the storage medium
        /// </summary>
        Task GetAlbumsAsync();

        /// <summary>
        /// Saves <paramref name="songEntity"/> in the storage medium
        /// </summary>
        /// <param name="songEntity">The song to be saved</param>
        Task SaveSongAsync(SongEntity songEntity);

        /// <summary>
        /// Updates the IsListened status of an album identified by <paramref name="albumId"/> in the storage medium
        /// </summary>
        /// <param name="albumId">The id of the album whose status will be updated</param>
        /// <param name="isListened">The IsListened status to be set</param>
        Task UpdateIsListenedStatusAsync(int albumId, bool? isListened);

        /// <summary>
        /// Updates the IsFavorite status of an album identified by <paramref name="albumId"/> in the storage medium
        /// </summary>
        /// <param name="albumId">The id of the album whose status will be updated</param>
        /// <param name="isFavorite">The IsFavorite status to be set</param>
        Task UpdateIsFavoriteStatusAsync(int albumId, bool isFavorite);
        #endregion
    }
}
