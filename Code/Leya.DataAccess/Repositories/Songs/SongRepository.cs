/// Written by: Yulia Danilova
/// Creation Date: 10th of December, 2020
/// Purpose: Song repository for the bridge-through between the generic storage medium and storage medium for Song
#region ========================================================================= USING =====================================================================================
using System.Threading.Tasks;
using Leya.DataAccess.Common.Enums;
using Leya.DataAccess.StorageAccess;
using Leya.DataAccess.Common.Models;
using Leya.DataAccess.Common.Models.Songs;
#endregion

namespace Leya.DataAccess.Repositories.Songs
{
    internal sealed class SongRepository : ISongRepository
    {
        #region ============================================================== FIELD MEMBERS ================================================================================
        private readonly IDataAccess dataAccess;
        #endregion

        #region ================================================================== CTOR =====================================================================================
        /// <summary>
        /// Overload C-tor
        /// </summary>
        /// <param name="dataAccess">The injected data access to use</param>
        public SongRepository(IDataAccess dataAccess)
        {
            this.dataAccess = dataAccess;
        }
        #endregion

        #region ================================================================= METHODS ===================================================================================
        /// <summary>
        /// Deletes all songs from the storage medium
        /// </summary>
        /// <returns>The result of deleting the songs, wrapped in a generic API container of type <see cref="ApiResponse"/></returns>
        public async Task<ApiResponse> DeleteAllAsync()
        {
            // delete all songs
            ApiResponse deleteSongs = await dataAccess.DeleteAsync(EntityContainers.Songs);
            // check if the query resulted in an error
            if (string.IsNullOrEmpty(deleteSongs.Error))
                return deleteSongs;
            else
                return new ApiResponse() { Error = "Error deleting all songs!" };
        }

        /// <summary>
        /// Deletes a song identified by <paramref name="id"/> from the storage medium
        /// </summary>
        /// <param name="id">The id of the song to be deleted</param>
        /// <returns>The result of deleting the song, wrapped in a generic API container of type <see cref="ApiResponse"/></returns>
        public async Task<ApiResponse> DeleteByIdAsync(int id)
        {
            // delete the song
            ApiResponse deleteSong = await dataAccess.DeleteAsync(EntityContainers.Songs, new { Id = id });
            // check if the quey resulted in an error
            if (string.IsNullOrEmpty(deleteSong.Error))
                return deleteSong;
            else
                return new ApiResponse() { Error = "Error deleting the song!" };
        }

        /// <summary>
        /// Gets all songs from the storage medium
        /// </summary>
        /// <returns>A list of songs, wrapped in a generic API container of type <see cref="ApiResponse{SongEntity}"/></returns>
        public async Task<ApiResponse<SongEntity>> GetAllAsync()
        {
            return await dataAccess.SelectAsync<SongEntity>(EntityContainers.Songs, "Id, ArtistId, Title, NamedTitle, Rating, Length, AlbumOrder, IsListened, IsFavorite");
        }

        /// <summary>
        /// Gets a song whose id is identified by <paramref name="id"/> from the storage medium
        /// </summary>
        /// <param name="id">The id of the song to get</param>
        /// <returns>The song whose id is identified by <paramref name="id"/>, wrapped in a generic API container of type <see cref="ApiResponse{SongEntity}"/></returns>
        public async Task<ApiResponse<SongEntity>> GetByIdAsync(int id)
        {
            return await dataAccess.SelectAsync<SongEntity>(EntityContainers.Songs, "Id, ArtistId, Title, NamedTitle, Rating, Length, AlbumOrder, IsListened, IsFavorite", new { Id = id });
        }

        /// <summary>
        /// Gets a song whose artist id is identified by <paramref name="artistId"/> and whose album id is identified by <paramref name="albumId"/> from the storage medium
        /// </summary>
        /// <param name="artistId">The id of the artist of the song to get</param>
        /// <param name="albumId">The id of the album of the song to get</param>
        /// <returns>The song whose artist's id is identified by <paramref name="artistId"/> and album's id is identified by <paramref name="albumId"/></returns>
        public async Task<ApiResponse<SongEntity>> GetByIdAsync(int artistId, int albumId)
        {
            // get the song data
            ApiResponse<SongEntity> song = await dataAccess.SelectAsync<SongEntity>(EntityContainers.Songs, "Id, ArtistId, Title, NamedTitle, Rating, Length, AlbumOrder, IsListened, IsFavorite",
                new { ArtistId = artistId, AlbumId = albumId });
            if (song.Data != null)
            {
                song.Data[0].AlbumId = albumId;
                song.Data[0].ArtistId = artistId;
            }
            return song;
        }

        /// <summary>
        /// Saves a song and its associated data in the storage medium
        /// </summary>
        /// <param name="entity">The song to be saved</param>
        /// <returns>The result of saving <paramref name="entity"/>, wrapped in a generic API container of type <see cref="ApiResponse{SongEntity}"/></returns>
        public async Task<ApiResponse<SongEntity>> InsertAsync(SongEntity entity)
        {
            return await dataAccess.InsertAsync(EntityContainers.Songs, entity);
        }

        /// <summary>
        /// Updates <paramref name="entity"/> and its additional info in the storage medium
        /// </summary>
        /// <param name="entity">The entity that will be updated</param>
        /// <returns>The result of updating <paramref name="entity"/>, wrapped in a generic API container of type <see cref="ApiResponse"/></returns>
        public async Task<ApiResponse> UpdateAsync(SongEntity entity)
        {
            return await dataAccess.UpdateAsync(EntityContainers.Songs,
                "ArtistId = '" + entity.ArtistId +
                "', AlbumId = '" + entity.AlbumId +
                "', Title = '" + entity.Title +
                "', NamedTitle = '" + entity.NamedTitle +
                "', Rating = '" + entity.Rating +
                "', Length = '" + entity.Length +
                "', AlbumOrder = '" + entity.AlbumOrder +
                "', IsListened = '" + entity.IsListened +
                "', IsFavorite = '" + entity.IsFavorite + "'",
                "Id", "'" + entity.Id + "'");
        }

        /// <summary>
        /// Updates the IsListened status of a song identified by <paramref name="songId"/> in the storage medium
        /// </summary>
        /// <param name="songId">The id of the song whose status will be updated</param>
        /// <param name="isListened">The IsListened status to be set</param>
        /// <returns>The result of updating the IsListened status, wrapped in a generic API container of type <see cref="ApiResponse"/></returns>
        public async Task<ApiResponse> UpdateIsListenedStatusAsync(int songId, bool? isListened)
        {
            return await dataAccess.UpdateAsync(EntityContainers.Songs, "IsListened = '" + (isListened != null ? isListened.ToString() : "Null") + "'", "Id", "'" + songId + "'");
        }

        /// <summary>
        /// Updates the IsFavorite status of a song identified by <paramref name="songId"/> in the storage medium
        /// </summary>
        /// <param name="songId">The id of the song whose status will be updated</param>
        /// <param name="isFavorite">The IsFavorite status to be set</param>
        /// <returns>The result of updating the IsFavorite status, wrapped in a generic API container of type <see cref="ApiResponse"/></returns>
        public async Task<ApiResponse> UpdateIsFavoriteStatusAsync(int songId, bool isFavorite)
        {
            return await dataAccess.UpdateAsync(EntityContainers.Songs, "IsFavorite = '" + isFavorite + "'", "Id", "'" + songId + "'");
        }
        #endregion
    }
}
