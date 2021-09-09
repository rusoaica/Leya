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
using Leya.Models.Common.Models.Media;
using System.IO;
using Newtonsoft.Json;
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
        /// Gets all tv shows from the storage medium
        /// </summary>
        public async Task GetAllAsync()
        {
            await Task.Run(async () =>
            {
                // get all tv shows
                var result = await tvShowRepository.GetAllAsync();
                // get all seasons
                await seasons.GetAllAsync();
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
        /// Deletes all tv shows from the storage medium
        /// </summary>
        public async Task DeleteAllAsync()
        {
            var result = await tvShowRepository.DeleteAllAsync();
            if (!string.IsNullOrEmpty(result.Error))
                throw new InvalidOperationException("Error deleting the tv shows from the repository: " + result.Error);
        }

        /// <summary>
        /// Saves a TV Show in the storage medium
        /// </summary>
        /// <param name="mediaTypeSource">The media type source of the TV show</param>
        /// <param name="mediaTypeId">The media type id of the TV show</param>
        public async Task SaveAsync(MediaTypeSourceEntity mediaTypeSource, int mediaTypeId)
        {
            // read the tv show details, if any
            if (File.Exists(mediaTypeSource.MediaSourcePath + Path.DirectorySeparatorChar + "tvshow.nfo"))
            {
                using (StreamReader tvShowStream = new StreamReader(mediaTypeSource.MediaSourcePath + Path.DirectorySeparatorChar + "tvshow.nfo"))
                {
                    // deserialize the tv show info json
                    TvShowEntity tvShowEntity = JsonConvert.DeserializeObject<TvShowEntity>(await tvShowStream.ReadToEndAsync());
                    // assign the media type source 
                    tvShowEntity.MediaTypeSourceId = mediaTypeSource.Id;
                    tvShowEntity.MediaTypeId = mediaTypeId;
                    // store the tv show storage entity for later, it contains the ids of its seasons, no reason to ask the storage for them yet again
                    var tvShowStorageEntity = tvShowEntity.ToStorageEntity();
                    // save the tv show
                    var result = await tvShowRepository.InsertAsync(tvShowStorageEntity);
                    if (!string.IsNullOrEmpty(result.Error))
                        throw new InvalidOperationException("Error inserting the tv show in the repository: " + result.Error);
                    else
                    {
                        // iterate all the seasons and save their episodes by reading their info json files
                        foreach (SeasonEntity season in tvShowEntity.Seasons)
                        {
                            foreach (string episodePath in Directory.EnumerateFiles(mediaTypeSource.MediaSourcePath + Path.DirectorySeparatorChar + season.Title).Where(f => f.EndsWith(".nfo")))
                            {
                                using (StreamReader episodeStream = new StreamReader(episodePath))
                                {
                                    // deserialize the episode info json
                                    EpisodeEntity episodeEntity = JsonConvert.DeserializeObject<EpisodeEntity>(episodeStream.ReadToEnd());
                                    // assign the media type source 
                                    episodeEntity.TvShowId = result.Data[0].Id;
                                    episodeEntity.SeasonId = tvShowStorageEntity.Seasons.Where(s => s.Number == season.Number).First().Id;
                                    // save the episode
                                    await seasons.SaveEpisodeAsync(episodeEntity);
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Updates the IsWatched status of a tv show identified by <paramref name="tvShowId"/> in the storage medium
        /// </summary>
        /// <param name="tvShowId">The id of the tv show whose status will be updated</param>
        /// <param name="isWatched">The IsWatched status to be set</param>
        public async Task UpdateIsWatchedStatusAsync(int tvShowId, bool? isWatched)
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
