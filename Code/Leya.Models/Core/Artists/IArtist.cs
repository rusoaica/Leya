/// Written by: Yulia Danilova
/// Creation Date: 07th of July, 2021
/// Purpose: Interface business model for artists
#region ========================================================================= USING =====================================================================================
using System.Threading.Tasks;
using Leya.Models.Common.Models.Artists;
using Leya.Models.Common.Models.Media;
#endregion

namespace Leya.Models.Core.Artists
{
    public interface IArtist
    {
        #region ================================================================ PROPERTIES =================================================================================
        ArtistEntity[] Artists { get; set; }
        #endregion

        #region ================================================================= METHODS ===================================================================================
        /// <summary>
        /// Gets all artists from the storage medium
        /// </summary>
        Task GetAllAsync();

        /// <summary>
        /// Saves an artist in the storage medium
        /// </summary>
        /// <param name="mediaTypeSource">The media type source of the artist</param>
        /// <param name="mediaTypeId">The media type id of the artist</param>
        Task SaveAsync(MediaTypeSourceEntity mediaTypeSource, int mediaTypeId);

        /// <summary>
        /// Deletes all artists from the storage medium
        /// </summary>
        Task DeleteAllAsync();

        /// <summary>
        /// Updates the IsListened status of an artist identified by <paramref name="artistId"/> in the storage medium
        /// </summary>
        /// <param name="artistId">The id of the artist whose status will be updated</param>
        /// <param name="isListened">The IsListened status to be set</param>
        Task UpdateIsListenedStatusAsync(int artistId, bool? isListened);

        /// <summary>
        /// Updates the IsFavorite status of an artist identified by <paramref name="artistId"/> in the storage medium
        /// </summary>
        /// <param name="artistId">The id of the artist whose status will be updated</param>
        /// <param name="isFavorite">The IsFavorite status to be set</param>
        Task UpdateIsFavoriteStatusAsync(int artistId, bool isFavorite);
        #endregion
    }
}
