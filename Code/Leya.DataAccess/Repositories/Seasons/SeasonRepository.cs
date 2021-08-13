/// Written by: Yulia Danilova
/// Creation Date: 22nd of November, 2020
/// Purpose: Seasons repository interface for the bridge-through between the generic storage medium and storage medium for Seasons
#region ========================================================================= USING =====================================================================================
using System.Threading.Tasks;
using Leya.DataAccess.Common.Enums;
using Leya.DataAccess.StorageAccess;
using Leya.DataAccess.Common.Models;
using Leya.DataAccess.Common.Models.Seasons;
using Leya.DataAccess.Common.Models.Episodes;
#endregion

namespace Leya.DataAccess.Repositories.Seasons
{
    internal sealed class SeasonRepository : ISeasonRepository
    {
        #region ============================================================== FIELD MEMBERS ================================================================================
        private readonly IDataAccess dataAccess;
        #endregion

        #region ================================================================== CTOR =====================================================================================
        /// <summary>
        /// Overload C-tor
        /// </summary>
        /// <param name="dataAccess">The injected data access to use</param>
        public SeasonRepository(IDataAccess dataAccess)
        {
            this.dataAccess = dataAccess;
        }
        #endregion

        #region ================================================================= METHODS ===================================================================================
        /// <summary>
        /// Deletes all seasons from the storage medium
        /// </summary>
        /// <returns>The result of deleting the seasons, wrapped in a generic API container of type <see cref="ApiResponse"/></returns>
        public async Task<ApiResponse> DeleteAllAsync()
        {
            // delete all seasons and their associated data 
            dataAccess.OpenTransaction();
            ApiResponse deleteSeasons = await dataAccess.DeleteAsync(EntityContainers.Seasons);
            ApiResponse deleteEpisodeActors = await dataAccess.DeleteAsync(EntityContainers.EpisodeActors);
            ApiResponse deleteEpisodeCredits = await dataAccess.DeleteAsync(EntityContainers.EpisodeCredits);
            ApiResponse deleteEpisodeGenres = await dataAccess.DeleteAsync(EntityContainers.EpisodeGenre);
            ApiResponse deleteEpisodeRatings = await dataAccess.DeleteAsync(EntityContainers.EpisodeRatings);
            dataAccess.CloseTransaction();
            // check if any of the queries resulted in an error
            if (string.IsNullOrEmpty(deleteSeasons.Error) && string.IsNullOrEmpty(deleteEpisodeActors.Error) && string.IsNullOrEmpty(deleteEpisodeCredits.Error) &&
                string.IsNullOrEmpty(deleteEpisodeGenres.Error) && string.IsNullOrEmpty(deleteEpisodeRatings.Error))
                return deleteSeasons;
            else
                return new ApiResponse() { Error = "Error deleting all seasons!" };
        }

        /// <summary>
        /// Deletes a season identified by <paramref name="id"/> from the storage medium
        /// </summary>
        /// <param name="id">The id of the season to be deleted</param>
        /// <returns>The result of deleting the season, wrapped in a generic API container of type <see cref="ApiResponse"/></returns>
        public async Task<ApiResponse> DeleteByIdAsync(int id)
        {
            dataAccess.OpenTransaction();
            // get all the episodes belonging to the specified season
            ApiResponse<EpisodeEntity> episodes = await dataAccess.SelectAsync<EpisodeEntity>(EntityContainers.Episodes,
                  "Id, TvShowId, Title, NamedTitle, Episode, Synopsis, Runtime, MPAA, LastPlayed, ImDbId, TvDbId, TmDbId, Director, Year, Aired, IsWatched, IsFavorite", new { SeasonId = id });
            // delete the season and its associated data 
            ApiResponse deleteSeason = await dataAccess.DeleteAsync(EntityContainers.Seasons, new { Id = id });
            // for each episode, delete its associated data and collect errors, if any
            if (episodes.Data != null)
            {
                await Task.Run(async () =>
                {
                    for (int i = 0; i < episodes.Data.Length; i++)
                    {
                        deleteSeason.Error = (await dataAccess.DeleteAsync(EntityContainers.Episodes, new { SeasonId = episodes.Data[0].Id }))?.Error ?? deleteSeason.Error;
                        deleteSeason.Error = (await dataAccess.DeleteAsync(EntityContainers.EpisodeActors, new { SeasonId = episodes.Data[0].Id }))?.Error ?? deleteSeason.Error;
                        deleteSeason.Error = (await dataAccess.DeleteAsync(EntityContainers.EpisodeCredits, new { SeasonId = episodes.Data[0].Id }))?.Error ?? deleteSeason.Error;
                        deleteSeason.Error = (await dataAccess.DeleteAsync(EntityContainers.EpisodeGenre, new { SeasonId = episodes.Data[0].Id }))?.Error ?? deleteSeason.Error;
                        deleteSeason.Error = (await dataAccess.DeleteAsync(EntityContainers.EpisodeRatings, new { SeasonId = episodes.Data[0].Id }))?.Error ?? deleteSeason.Error;
                    }
                });
            }

            dataAccess.CloseTransaction();
            // check if the query resulted in an error
            if (string.IsNullOrEmpty(deleteSeason.Error))
                return deleteSeason;
            else
                return new ApiResponse() { Error = "Error deleting the season!" };
        }

        /// <summary>
        /// Gets all seasons from the storage medium
        /// </summary>
        /// <returns>A list of seasons, wrapped in a generic API container of type <see cref="ApiResponse{SeasonEntity}"/></returns>
        public async Task<ApiResponse<SeasonEntity>> GetAllAsync()
        {
            return await dataAccess.SelectAsync<SeasonEntity>(EntityContainers.Seasons, "Id, TvShowId, SeasonNumber, SeasonName, IsWatched, IsFavorite, Year, Synopsis, Premiered");
        }

        /// <summary>
        /// Gets the season identified by <paramref name="id"/> from the storage medium
        /// </summary>
        /// <param name="id">The Id of the season to get</param>
        /// <returns>A season identified by <paramref name="id"/>, wrapped in a generic API container of type <see cref="ApiResponse{SeasonEntity}"/></returns>
        public async Task<ApiResponse<SeasonEntity>> GetByIdAsync(int id)
        {
            return await dataAccess.SelectAsync<SeasonEntity>(EntityContainers.Seasons, "Id, TvShowId, SeasonNumber, SeasonName, IsWatched, IsFavorite, Year, Synopsis, Premiered", new { Id = id });
        }

        /// <summary>
        /// Saves a season in the storage medium
        /// </summary>
        /// <param name="entity">The season to be saved</param>
        /// <returns>The result of saving <paramref name="entity"/>, wrapped in a generic API container of type <see cref="ApiResponse{SeasonEntity}"/></returns>
        public async Task<ApiResponse<SeasonEntity>> InsertAsync(SeasonEntity entity)
        {
            return await dataAccess.InsertAsync(EntityContainers.Seasons, entity);
        }

        /// <summary>
        /// Updates <paramref name="entity"/> in the storage medium
        /// </summary>
        /// <param name="entity">The entity that will be updated</param>
        /// <returns>The result of updating <paramref name="entity"/>, wrapped in a generic API container of type <see cref="ApiResponse"/></returns>
        public async Task<ApiResponse> UpdateAsync(SeasonEntity entity)
        {
            return await dataAccess.UpdateAsync(EntityContainers.Seasons,
                "TvShowId = '" + entity.TvShowId +
                "', Year = '" + entity.Year +
                "', SeasonNumber = '" + entity.SeasonNumber +
                "', SeasonName = '" + entity.SeasonName +
                "', IsWatched = '" + entity.IsWatched +
                "', IsFavorite = '" + entity.IsFavorite +
                "', Synopsis = '" + entity.Synopsis +
                "', Premiered = '" + entity.Premiered + "'",
                "Id", "'" + entity.Id + "'");
        }

        /// <summary>
        /// Updates the IsWatched status of a season identified by <paramref name="seasonId"/> in the storage medium
        /// </summary>
        /// <param name="seasonId">The id of the season whose status will be updated</param>
        /// <param name="isWatched">The IsWatched status to be set</param>
        /// <returns>The result of updating the IsWatched status, wrapped in a generic API container of type <see cref="ApiResponse"/></returns>
        public async Task<ApiResponse> UpdateIsWatchedStatusAsync(int seasonId, bool isWatched)
        {
            return await dataAccess.UpdateAsync(EntityContainers.Seasons, "IsWatched = '" + isWatched + "'", "Id", "'" + seasonId + "'");
        }

        /// <summary>
        /// Updates the IsFavorite status of a season identified by <paramref name="seasonId"/> in the storage medium
        /// </summary>
        /// <param name="seasonId">The id of the season whose status will be updated</param>
        /// <param name="isFavorite">The IsFavorite status to be set</param>
        /// <returns>The result of updating the IsFavorite status, wrapped in a generic API container of type <see cref="ApiResponse"/></returns>
        public async Task<ApiResponse> UpdateIsFavoriteStatusAsync(int seasonId, bool isFavorite)
        {
            return await dataAccess.UpdateAsync(EntityContainers.Seasons, "IsFavorite = '" + isFavorite + "'", "Id", "'" + seasonId + "'");
        }
        #endregion
    }
}
