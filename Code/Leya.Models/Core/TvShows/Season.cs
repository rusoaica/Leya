/// Written by: Yulia Danilova
/// Creation Date: 06th of July, 2021
/// Purpose: Business model for seasons
#region ========================================================================= USING =====================================================================================
using System;
using System.Linq;
using Leya.DataAccess;
using System.Threading.Tasks;
using Leya.Models.Common.Infrastructure;
using Leya.Models.Common.Models.TvShows;
using Leya.DataAccess.Repositories.Seasons;
#endregion

namespace Leya.Models.Core.TvShows
{
    public class Season : ISeason
    {
        #region ============================================================== FIELD MEMBERS ================================================================================
        private readonly IEpisode episodes;
        private readonly ISeasonRepository seasonRepository;
        #endregion

        #region ================================================================ PROPERTIES =================================================================================
        public SeasonEntity[] Seasons { get; set; }
        #endregion

        #region ================================================================== CTOR =====================================================================================
        /// <summary>
        /// Overload C-tor
        /// <param name="unitOfWork">Injected unit of work for interacting with the data access layer repositories</param>
        /// <param name="episodes">Injected episode business model</param>
        /// </summary>
        public Season(IUnitOfWork unitOfWork, IEpisode episodes)
        {
            this.episodes = episodes;
            seasonRepository = unitOfWork.GetRepository<ISeasonRepository>();
        }
        #endregion

        #region ================================================================= METHODS ===================================================================================
        /// <summary>
        /// Gets the seasons from the storage medium
        /// </summary>
        public async Task GetAllAsync()
        {
            await Task.Run(async() =>
            {
                // get all seasons
                var result = await seasonRepository.GetAllAsync();
                // get all episodes
                await episodes.GetAllAsync();
                if (string.IsNullOrEmpty(result.Error))
                {
                    Seasons = Services.AutoMapper.Map<SeasonEntity[]>(result.Data);
                    // for each season, assign its episodes
                    foreach (SeasonEntity season in Seasons)
                        season.Episodes = episodes.Episodes.Where(s => s.SeasonId == season.Id).ToArray();
                }
                else
                    throw new InvalidOperationException("Error getting the seasons from the repository: " + result.Error);
            });
        }

        /// <summary>
        /// Saves <paramref name="episodeEntity"/> in the storage medium
        /// </summary>
        /// <param name="episodeEntity">The episode to be saved</param>
        public async Task SaveEpisodeAsync(EpisodeEntity episodeEntity)
        {
            await episodes.SaveAsync(episodeEntity);
        }

        /// <summary>
        /// Updates the IsWatched status of a season identified by <paramref name="seasonId"/> in the storage medium
        /// </summary>
        /// <param name="seasonId">The id of the season whose status will be updated</param>
        /// <param name="isWatched">The IsWatched status to be set</param>
        public async Task UpdateIsWatchedStatusAsync(int seasonId, bool? isWatched)
        {
            var result = await seasonRepository.UpdateIsWatchedStatusAsync(seasonId, isWatched);
            if (!string.IsNullOrEmpty(result.Error))
                throw new InvalidOperationException("Error updating the IsWatched status of the season: " + result.Error);
        }

        /// <summary>
        /// Updates the IsFavorite status of a season identified by <paramref name="seasonId"/> in the storage medium
        /// </summary>
        /// <param name="seasonId">The id of the season whose status will be updated</param>
        /// <param name="isFavorite">The IsFavorite status to be set</param>
        public async Task UpdateIsFavoriteStatusAsync(int seasonId, bool isFavorite)
        {
            var result = await seasonRepository.UpdateIsFavoriteStatusAsync(seasonId, isFavorite);
            if (!string.IsNullOrEmpty(result.Error))
                throw new InvalidOperationException("Error updating the IsFavorite status of the season: " + result.Error);
        }
        #endregion
    }
}
