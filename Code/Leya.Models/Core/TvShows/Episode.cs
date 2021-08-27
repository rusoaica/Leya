/// Written by: Yulia Danilova
/// Creation Date: 05th of July, 2021
/// Purpose: Business model for episodes
#region ========================================================================= USING =====================================================================================
using System;
using Leya.DataAccess;
using System.Threading.Tasks;
using Leya.Models.Common.Infrastructure;
using Leya.Models.Common.Models.TvShows;
using Leya.DataAccess.Repositories.Episodes;
#endregion

namespace Leya.Models.Core.TvShows
{
    public class Episode : IEpisode
    {
        #region ============================================================== FIELD MEMBERS ================================================================================
        private readonly IEpisodeRepository episodeRepository;
        #endregion

        #region ================================================================ PROPERTIES =================================================================================
        public EpisodeEntity[] Episodes { get; set; }
        #endregion

        #region ================================================================== CTOR =====================================================================================
        /// <summary>
        /// Overload C-tor
        /// <param name="unitOfWork">Injected unit of work for interacting with the data access layer repositories</param>
        /// </summary>
        public Episode(IUnitOfWork unitOfWork)
        {
            episodeRepository = unitOfWork.GetRepository<IEpisodeRepository>();
        }
        #endregion

        #region ================================================================= METHODS ===================================================================================
        /// <summary>
        /// Gets the episodes from the storage medium
        /// </summary>
        public async Task GetAllAsync()
        {
            var result = await episodeRepository.GetAllAsync();
            if (string.IsNullOrEmpty(result.Error))
                Episodes = Services.AutoMapper.Map<EpisodeEntity[]>(result.Data);
            else
                throw new InvalidOperationException("Error getting the episodes from the repository: " + result.Error);
        }

        /// <summary>
        /// Saves <paramref name="episodeEntity"/> in the storage medium
        /// </summary>
        /// <param name="episodeEntity">The episode to be saved</param>
        public async Task SaveAsync(EpisodeEntity episodeEntity)
        {
            var result = await episodeRepository.InsertAsync(episodeEntity.ToStorageEntity());
            if (!string.IsNullOrEmpty(result.Error))            
                throw new InvalidOperationException("Error inserting the episode in the storage medium!");
        }

        /// <summary>
        /// Updates the IsWatched status of an episode identified by <paramref name="episodeId"/> in the storage medium
        /// </summary>
        /// <param name="episodeId">The id of the episode whose status will be updated</param>
        /// <param name="isWatched">The IsWatched status to be set</param>
        public async Task UpdateIsWatchedStatusAsync(int episodeId, bool? isWatched)
        {
            var result = await episodeRepository.UpdateIsWatchedStatusAsync(episodeId, isWatched);
            if (!string.IsNullOrEmpty(result.Error))
                throw new InvalidOperationException("Error updating the IsWatched status of the episode: " + result.Error);
        }

        /// <summary>
        /// Updates the IsFavorite status of an episode identified by <paramref name="episodeId"/> in the storage medium
        /// </summary>
        /// <param name="episodeId">The id of the episode whose status will be updated</param>
        /// <param name="isFavorite">The IsFavorite status to be set</param>
        public async Task UpdateIsFavoriteStatusAsync(int episodeId, bool isFavorite)
        {
            var result = await episodeRepository.UpdateIsFavoriteStatusAsync(episodeId, isFavorite);
            if (!string.IsNullOrEmpty(result.Error))
                throw new InvalidOperationException("Error updating the IsFavorite status of the episode: " + result.Error);
        }
        #endregion
    }
}
