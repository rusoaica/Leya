/// Written by: Yulia Danilova
/// Creation Date: 10th of December, 2020
/// Purpose: Album repository for the bridge-through between the generic storage medium and strage medium for Albums
#region ========================================================================= USING =====================================================================================
using System.Threading.Tasks;
using Leya.DataAccess.Common.Enums;
using Leya.DataAccess.StorageAccess;
using Leya.DataAccess.Common.Models;
using Leya.DataAccess.Common.Models.Songs;
using Leya.DataAccess.Common.Models.Albums;
#endregion

namespace Leya.DataAccess.Repositories.Albums
{
    internal sealed class AlbumRepository : IAlbumRepository
    {
        #region ============================================================== FIELD MEMBERS ================================================================================
        private readonly IDataAccess dataAccess;
        #endregion

        #region ================================================================== CTOR =====================================================================================
        /// <summary>
        /// Overload C-tor
        /// </summary>
        /// <param name="dataAccess">The injected data access to use</param>
        public AlbumRepository(IDataAccess dataAccess)
        {
            this.dataAccess = dataAccess;
        }
        #endregion

        #region ================================================================= METHODS ===================================================================================
        /// <summary>
        /// Deletes all albums from the storage medium
        /// </summary>
        /// <returns>The result of deleting the albums, wrapped in a generic API container of type <see cref="ApiResponse"/></returns>
        public async Task<ApiResponse> DeleteAllAsync()
        {
            return await dataAccess.DeleteAsync(EntityContainers.Albums);
        }

        /// <summary>
        /// Deletes an album identified by <paramref name="id"/> from the storage medium
        /// </summary>
        /// <param name="id">The id of the album to be deleted</param>
        /// <returns>The result of deleting the album, wrapped in a generic API container of type <see cref="ApiResponse"/></returns>
        public async Task<ApiResponse> DeleteByIdAsync(int id)
        {
            dataAccess.OpenTransaction();
            // get all the songs belonging to the specified album
            ApiResponse<SongEntity> songs = await dataAccess.SelectAsync<SongEntity>(EntityContainers.Songs,
                  "Id, ArtistId, Title, NamedTitle, Rating, Length, AlbumOrder, IsListened, IsFavorite", new { AlbumId = id });
            // delete the album
            ApiResponse deleteAlbum = await dataAccess.DeleteAsync(EntityContainers.Albums, new { Id = id });
            // delete each song of the album and collect errors, if any
            if (songs.Data != null)
                deleteAlbum.Error = (await dataAccess.DeleteAsync(EntityContainers.Songs, new { AlbumId = songs.Data[0].Id }))?.Error ?? deleteAlbum.Error;
            dataAccess.CloseTransaction();
            // check is any of the queries resulted in an error
            if (string.IsNullOrEmpty(deleteAlbum.Error))
                return deleteAlbum;
            else
                return new ApiResponse() { Error = "Error deleting the album!" };
        }

        /// <summary>
        /// Gets all albums from the storage medium
        /// </summary>
        /// <returns>A list of albums, wrapped in a generic API container of type <see cref="ApiResponse{AlbumEntity}"/></returns>
        public async Task<ApiResponse<AlbumEntity>> GetAllAsync()
        {
            return await dataAccess.SelectAsync<AlbumEntity>(EntityContainers.Albums, "Id, ArtistId, Title, NamedTitle, Year, Description, Rating, IsListened, IsFavorite");
        }

        /// <summary>
        /// Gets the album identified by <paramref name="id"/> from the storage medium
        /// </summary>
        /// <param name="id">The Id of the album to get</param>
        /// <returns>An album identified by <paramref name="id"/>, wrapped in a generic API container of type <see cref="ApiResponse{AlbumEntity}"/></returns>
        public async Task<ApiResponse<AlbumEntity>> GetByIdAsync(int id)
        {
            return await dataAccess.SelectAsync<AlbumEntity>(EntityContainers.Albums, "Id, ArtistId, Title, NamedTitle, Year, Description, Rating, IsListened, IsFavorite", new { Id = id });
        }

        /// <summary>
        /// Saves an album in the storage medium
        /// </summary>
        /// <param name="entity">The album to be saved</param>
        /// <returns>The result of saving <paramref name="entity"/>, wrapped in a generic API container of type <see cref="ApiResponse{AlbumEntity}"/></returns>
        public async Task<ApiResponse<AlbumEntity>> InsertAsync(AlbumEntity entity)
        {
            return await dataAccess.InsertAsync(EntityContainers.Albums, entity);
        }

        /// <summary>
        /// Updates <paramref name="entity"/> in the storage medium
        /// </summary>
        /// <param name="entity">The entity that will be updated</param>
        /// <returns>The result of updating <paramref name="entity"/>, wrapped in a generic API container of type <see cref="ApiResponse"/></returns>
        public async Task<ApiResponse> UpdateAsync(AlbumEntity entity)
        {
            return await dataAccess.UpdateAsync(EntityContainers.Albums,
                "ArtistId = '" + entity.ArtistId +
                "', Title = '" + entity.Title +
                "', NamedTitle = '" + entity.NamedTitle +
                "', Year = '" + entity.Year +
                "', Description = '" + entity.Description +
                "', Rating = '" + entity.Rating +
                "', IsListened = '" + entity.IsListened +
                "', IsFavorite = '" + entity.IsFavorite + "'",
                "Id", "'" + entity.Id + "'");
        }

        /// <summary>
        /// Updates the IsListened status of an album identified by <paramref name="albumId"/> in the storage medium
        /// </summary>
        /// <param name="albumId">The id of the album whose status will be updated</param>
        /// <param name="isListened">The IsListened status to be set</param>
        /// <returns>The result of updating the IsListened status, wrapped in a generic API container of type <see cref="ApiResponse"/></returns>
        public async Task<ApiResponse> UpdateIsListenedStatusAsync(int albumId, bool? isListened)
        {
            return await dataAccess.UpdateAsync(EntityContainers.Albums, "IsListened = '" + (isListened != null ? isListened.ToString() : "Null") + "'", "Id", "'" + albumId + "'");
        }

        /// <summary>
        /// Updates the IsFavorite status of an album identified by <paramref name="AlbumId"/> in the storage medium
        /// </summary>
        /// <param name="AlbumId">The id of the album whose status will be updated</param>
        /// <param name="isFavorite">The IsFavorite status to be set</param>
        /// <returns>The result of updating the IsFavorite status, wrapped in a generic API container of type <see cref="ApiResponse"/></returns>
        public async Task<ApiResponse> UpdateIsFavoriteStatusAsync(int AlbumId, bool isFavorite)
        {
            return await dataAccess.UpdateAsync(EntityContainers.Albums, "IsFavorite = '" + isFavorite + "'", "Id", "'" + AlbumId + "'");
        }
        #endregion
    }
}
