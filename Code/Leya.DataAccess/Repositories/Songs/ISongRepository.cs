/// Written by: Yulia Danilova
/// Creation Date: 10th of December, 2020
/// Purpose: Song repository interface for the bridge-through between the generic storage medium and storage medium for Song
#region ========================================================================= USING =====================================================================================
using System.Threading.Tasks;
using Leya.DataAccess.Common.Models;
using Leya.DataAccess.Common.Models.Songs;
using Leya.DataAccess.Repositories.Common;
#endregion

namespace Leya.DataAccess.Repositories.Songs
{
    public interface ISongRepository : IRepository<SongEntity>
    {
        #region ================================================================= METHODS ===================================================================================
        /// <summary>
        /// Gets a song whose artist id is identified by <paramref name="artistId"/> and whose album id is identified by <paramref name="albumId"/> 
        /// and its associated data from the database
        /// </summary>
        /// <param name="artistId">The id of the artist of the song to get</param>
        /// <param name="albumId">The id of the album of the song to get</param>
        /// <returns>The song whose artist's id is identified by <paramref name="artistId"/> and album's id is identified by <paramref name="albumId"/></returns>
        Task<ApiResponse<SongEntity>> GetByIdAsync(int artistId, int albumId);

        /// <summary>
        /// Updates the IsListened status of a song identified by <paramref name="songId"/> in the storage medium
        /// </summary>
        /// <param name="songId">The id of the song whose status will be updated</param>
        /// <param name="isListened">The IsListened status to be set</param>
        /// <returns>The result of updating the IsListened status, wrapped in a generic API container of type <see cref="ApiResponse"/></returns>
        Task<ApiResponse> UpdateIsListenedStatusAsync(int songId, bool? isListened);

        /// <summary>
        /// Updates the IsFavorite status of a song identified by <paramref name="songId"/> in the storage medium
        /// </summary>
        /// <param name="songId">The id of the song whose status will be updated</param>
        /// <param name="isFavorite">The IsFavorite status to be set</param>
        /// <returns>The result of updating the IsFavorite status, wrapped in a generic API container of type <see cref="ApiResponse"/></returns>
        Task<ApiResponse> UpdateIsFavoriteStatusAsync(int songId, bool isFavorite);
        #endregion
    }
}
