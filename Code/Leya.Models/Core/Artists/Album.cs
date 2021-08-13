/// Written by: Yulia Danilova
/// Creation Date: 07th of July, 2021
/// Purpose: Business model for albums
#region ========================================================================= USING =====================================================================================
using System;
using System.Linq;
using Leya.DataAccess;
using System.Threading.Tasks;
using Leya.Models.Common.Infrastructure;
using Leya.Models.Common.Models.Artists;
using Leya.DataAccess.Repositories.Albums;
#endregion

namespace Leya.Models.Core.Artists
{
    public class Album : IAlbum
    {
        #region ============================================================== FIELD MEMBERS ================================================================================
        private readonly ISong songs;
        private readonly IAlbumRepository albumRepository;
        #endregion

        #region ================================================================ PROPERTIES =================================================================================
        public AlbumEntity[] Albums { get; set; }
        #endregion

        #region ================================================================== CTOR =====================================================================================
        /// <summary>
        /// Overload C-tor
        /// <param name="unitOfWork">Injected unit of work for interacting with the data access layer repositories</param>
        /// <param name="songs">Injected song business model</param>
        /// </summary>
        public Album(IUnitOfWork unitOfWork, ISong songs)
        {
            this.songs = songs;
            albumRepository = unitOfWork.GetRepository<IAlbumRepository>();
        }
        #endregion

        #region ================================================================= METHODS ===================================================================================
        /// <summary>
        /// Gets the seasons from the storage medium
        /// </summary>
        public async Task GetAlbumsAsync()
        {
            await Task.Run(async () =>
            {
                // get all albums
                var result = await albumRepository.GetAllAsync();
                // get all songs
                await songs.GetSongsAsync();
                if (string.IsNullOrEmpty(result.Error))
                {
                    Albums = Services.AutoMapper.Map<AlbumEntity[]>(result.Data);
                    // for each album, assign its songs
                    foreach (AlbumEntity album in Albums)
                        album.Songs = songs.Songs.Where(s => s.AlbumId == album.Id).ToArray();
                }
                else
                    throw new InvalidOperationException("Error getting the albums from the repository: " + result.Error);
            });
        }

        /// <summary>
        /// Updates the IsListened status of an album identified by <paramref name="albumId"/> in the storage medium
        /// </summary>
        /// <param name="albumId">The id of the album whose status will be updated</param>
        /// <param name="isListened">The IsListened status to be set</param>
        public async Task UpdateIsListenedStatusAsync(int albumId, bool isListened)
        {
            var result = await albumRepository.UpdateIsListenedStatusAsync(albumId, isListened);
            if (!string.IsNullOrEmpty(result.Error))
                throw new InvalidOperationException("Error updating the IsListened status of the album: " + result.Error);
        }

        /// <summary>
        /// Updates the IsFavorite status of an album identified by <paramref name="albumId"/> in the storage medium
        /// </summary>
        /// <param name="albumId">The id of the album whose status will be updated</param>
        /// <param name="isFavorite">The IsFavorite status to be set</param>
        public async Task UpdateIsFavoriteStatusAsync(int albumId, bool isFavorite)
        {
            var result = await albumRepository.UpdateIsFavoriteStatusAsync(albumId, isFavorite);
            if (!string.IsNullOrEmpty(result.Error))
                throw new InvalidOperationException("Error updating the IsFavorite status of the album: " + result.Error);
        }
        #endregion
    }
}
