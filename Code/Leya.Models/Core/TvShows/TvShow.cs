/// Written by: Yulia Danilova
/// Creation Date: 05th of July, 2021
/// Purpose: Business model for tv shows
#region ========================================================================= USING =====================================================================================
using System;
using System.Linq;
using Leya.DataAccess;
using System.Threading.Tasks;
using Leya.Models.Common.Infrastructure;
using Leya.Models.Common.Models.TvShows;
using Leya.DataAccess.Repositories.TvShows;
#endregion

namespace Leya.Models.Core.TvShows
{
    public class TvShow : ITvShow
    {
        #region ============================================================== FIELD MEMBERS ================================================================================
        private readonly ISeason seasons;
        private readonly ITvShowRepository tvShowRepository;
        #endregion

        #region ================================================================ PROPERTIES =================================================================================
        public TvShowEntity[] TvShows { get; set; }
        #endregion

        #region ================================================================== CTOR =====================================================================================
        /// <summary>
        /// Overload C-tor
        /// <param name="unitOfWork">Injected unit of work for interacting with the data access layer repositories</param>
        /// <param name="seasons">Injected season business model</param>
        /// </summary>
        public TvShow(IUnitOfWork unitOfWork, ISeason seasons)
        {
            this.seasons = seasons;
            tvShowRepository = unitOfWork.GetRepository<ITvShowRepository>();
        }
        #endregion

        #region ================================================================= METHODS ===================================================================================
        /// <summary>
        /// Gets the tv shows from the storage medium
        /// </summary>
        public async Task GetTvShowsAsync()
        {
            await Task.Run(async () =>
            {
                // get all tv shows
                var result = await tvShowRepository.GetAllAsync();
                // get all seasons
                await seasons.GetSeasonsAsync();
                if (string.IsNullOrEmpty(result.Error))
                {
                    TvShows = Services.AutoMapper.Map<TvShowEntity[]>(result.Data);
                    // for each tv show, assign its seasons
                    foreach (TvShowEntity tvShow in TvShows)
                        tvShow.Seasons = seasons.Seasons.Where(s => s.TvShowId == tvShow.Id).ToArray();
                }
                else
                    throw new InvalidOperationException("Error getting the tv shows from the repository: " + result.Error);
            });
        }

        /// <summary>
        /// Updates the IsWatched status of a tv show identified by <paramref name="tvShowId"/> in the storage medium
        /// </summary>
        /// <param name="tvShowId">The id of the tv show whose status will be updated</param>
        /// <param name="isWatched">The IsWatched status to be set</param>
        public async Task UpdateIsWatchedStatusAsync(int tvShowId, bool isWatched)
        {
            var result = await tvShowRepository.UpdateIsWatchedStatusAsync(tvShowId, isWatched);
            if (!string.IsNullOrEmpty(result.Error))
                throw new InvalidOperationException("Error updating the IsWatched status of the tv show: " + result.Error);
        }

        /// <summary>
        /// Updates the IsFavorite status of a tv show identified by <paramref name="tvShowId"/> in the storage medium
        /// </summary>
        /// <param name="tvShowId">The id of the tv show whose status will be updated</param>
        /// <param name="isFavorite">The IsFavorite status to be set</param>
        public async Task UpdateIsFavoriteStatusAsync(int tvShowId, bool isFavorite)
        {
            var result = await tvShowRepository.UpdateIsFavoriteStatusAsync(tvShowId, isFavorite);
            if (!string.IsNullOrEmpty(result.Error))
                throw new InvalidOperationException("Error updating the IsFavorite status of the tv show: " + result.Error);
        }
        #endregion
    }
}
