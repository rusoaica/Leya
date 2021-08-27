/// Written by: Yulia Danilova
/// Creation Date: 09th of December, 2020
/// Purpose: Artist repository interface for the bridge-through between the generic storage medium and storage medium for Artists
#region ========================================================================= USING =====================================================================================
using System.Threading.Tasks;
using Leya.DataAccess.Common.Models;
using Leya.DataAccess.Repositories.Common;
using Leya.DataAccess.Common.Models.Artists;
#endregion

namespace Leya.DataAccess.Repositories.Artists
{
    public interface IArtistRepository : IRepository<ArtistEntity>
    {
        #region ================================================================= METHODS ===================================================================================
        /// <summary>
        /// Updates the IsListened status of an artist identified by <paramref name="artistId"/> in the storage medium
        /// </summary>
        /// <param name="artistId">The id of the artist whose status will be updated</param>
        /// <param name="isListened">The IsListened status to be set</param>
        /// <returns>The result of updating the IsListened status, wrapped in a generic API container of type <see cref="ApiResponse"/></returns>
        Task<ApiResponse> UpdateIsListenedStatusAsync(int artistId, bool? isListened);

        /// <summary>
        /// Updates the IsFavorite status of an artist identified by <paramref name="artistId"/> in the storage medium
        /// </summary>
        /// <param name="artistId">The id of the artist whose status will be updated</param>
        /// <param name="isFavorite">The IsFavorite status to be set</param>
        /// <returns>The result of updating the IsFavorite status, wrapped in a generic API container of type <see cref="ApiResponse"/></returns>
        Task<ApiResponse> UpdateIsFavoriteStatusAsync(int artistId, bool isFavorite);
        #endregion
    }
}
