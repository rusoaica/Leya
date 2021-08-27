/// Written by: Yulia Danilova
/// Creation Date: 07th of July, 2021
/// Purpose: Business model for songs
#region ========================================================================= USING =====================================================================================
using System;
using Leya.DataAccess;
using System.Threading.Tasks;
using Leya.Models.Common.Infrastructure;
using Leya.Models.Common.Models.Artists;
using Leya.DataAccess.Repositories.Songs;
#endregion

namespace Leya.Models.Core.Artists
{
    public class Song : ISong
    {
        #region ============================================================== FIELD MEMBERS ================================================================================
        private readonly ISongRepository songRepository;
        #endregion

        #region ================================================================ PROPERTIES =================================================================================
        public SongEntity[] Songs { get; set; }
        #endregion

        #region ================================================================== CTOR =====================================================================================
        /// <summary>
        /// Overload C-tor
        /// <param name="unitOfWork">Injected unit of work for interacting with the data access layer repositories</param>
        /// </summary>
        public Song(IUnitOfWork unitOfWork)
        {
            songRepository = unitOfWork.GetRepository<ISongRepository>();
        }
        #endregion

        #region ================================================================= METHODS ===================================================================================
        /// <summary>
        /// Gets the songs from the storage medium
        /// </summary>
        public async Task GetAllAsync()
        {
            var result = await songRepository.GetAllAsync();
            if (string.IsNullOrEmpty(result.Error))
                Songs = Services.AutoMapper.Map<SongEntity[]>(result.Data);
            else
                throw new InvalidOperationException("Error getting the songs from the repository: " + result.Error);
        }

        /// <summary>
        /// Saves <paramref name="songEntity"/> in the storage medium
        /// </summary>
        /// <param name="songEntity">The song to be saved</param>
        public async Task SaveAsync(SongEntity songEntity)
        {
            var result = await songRepository.InsertAsync(songEntity.ToStorageEntity());
            if (!string.IsNullOrEmpty(result.Error))
                throw new InvalidOperationException("Error inserting the song in the storage medium!");
        }

        /// <summary>
        /// Updates the IsListened status of a song identified by <paramref name="songId"/> in the storage medium
        /// </summary>
        /// <param name="songId">The id of the song whose status will be updated</param>
        /// <param name="isListened">The IsListened status to be set</param>
        public async Task UpdateIsListenedStatusAsync(int songId, bool? isListened)
        {
            var result = await songRepository.UpdateIsListenedStatusAsync(songId, isListened);
            if (!string.IsNullOrEmpty(result.Error))
                throw new InvalidOperationException("Error updating the IsListened status of the song: " + result.Error);
        }

        /// <summary>
        /// Updates the IsFavorite status of a song identified by <paramref name="songId"/> in the storage medium
        /// </summary>
        /// <param name="songId">The id of the song whose status will be updated</param>
        /// <param name="isFavorite">The IsFavorite status to be set</param>
        public async Task UpdateIsFavoriteStatusAsync(int songId, bool isFavorite)
        {
            var result = await songRepository.UpdateIsFavoriteStatusAsync(songId, isFavorite);
            if (!string.IsNullOrEmpty(result.Error))
                throw new InvalidOperationException("Error updating the IsFavorite status of the song: " + result.Error);
        }
        #endregion
    }
}
