/// Written by: Yulia Danilova
/// Creation Date: 10th of December, 2020
/// Purpose: Album repository interface for the bridge-through between the generic storage medium and storage medium for Albums
#region ========================================================================= USING =====================================================================================
using System.Threading.Tasks;
using Leya.DataAccess.Common.Models;
using Leya.DataAccess.Repositories.Common;
using Leya.DataAccess.Common.Models.Albums;
#endregion

namespace Leya.DataAccess.Repositories.Albums
{
    public interface IAlbumRepository : IRepository<AlbumEntity>
    {
        #region ================================================================= METHODS ===================================================================================
        /// <summary>
        /// Updates the IsListened status of an album identified by <paramref name="albumId"/> in the storage medium
        /// </summary>
        /// <param name="albumId">The id of the album whose status will be updated</param>
        /// <param name="isListened">The IsListened status to be set</param>
        /// <returns>The result of updating the IsListened status, wrapped in a generic API container of type <see cref="ApiResponse"/></returns>
        Task<ApiResponse> UpdateIsListenedStatusAsync(int albumId, bool isListened);

        /// <summary>
        /// Updates the IsFavorite status of an album identified by <paramref name="albumId"/> in the storage medium
        /// </summary>
        /// <param name="albumId">The id of the album whose status will be updated</param>
        /// <param name="isFavorite">The IsFavorite status to be set</param>
        /// <returns>The result of updating the IsFavorite status, wrapped in a generic API container of type <see cref="ApiResponse"/></returns>
        Task<ApiResponse> UpdateIsFavoriteStatusAsync(int albumId, bool isFavorite);
        #endregion
    }
}
