/// Written by: Yulia Danilova
/// Creation Date: 22nd of November, 2020
/// Purpose: Episode repository for the bridge-through between the generic storage medium and storage medium for Episode
#region ========================================================================= USING =====================================================================================
using System.Threading.Tasks;
using Leya.DataAccess.Common.Enums;
using Leya.DataAccess.StorageAccess;
using Leya.DataAccess.Common.Models;
using Leya.DataAccess.Common.Models.Episodes;
#endregion

namespace Leya.DataAccess.Repositories.Episodes
{
    internal sealed class EpisodeRepository : IEpisodeRepository
    {
        #region ============================================================== FIELD MEMBERS ================================================================================
        private readonly IDataAccess dataAccess;
        #endregion

        #region ================================================================== CTOR =====================================================================================
        /// <summary>
        /// Overload C-tor
        /// </summary>
        /// <param name="dataAccess">The injected data access to use</param>
        public EpisodeRepository(IDataAccess dataAccess)
        {
            this.dataAccess = dataAccess;
        }
        #endregion

        #region ================================================================= METHODS ===================================================================================
        /// <summary>
        /// Deletes all episodes from the storage medium
        /// </summary>
        /// <returns>The result of deleting the episodes, wrapped in a generic API container of type <see cref="ApiResponse"/></returns>
        public async Task<ApiResponse> DeleteAllAsync()
        {
            // delete all episodes and their associated data
            dataAccess.OpenTransaction();
            ApiResponse deleteEpisodes = await dataAccess.DeleteAsync(EntityContainers.Episodes);
            ApiResponse deleteEpisodeActors = await dataAccess.DeleteAsync(EntityContainers.EpisodeActors);
            ApiResponse deleteEpisodeCredits = await dataAccess.DeleteAsync(EntityContainers.EpisodeCredits);
            ApiResponse deleteEpisodeGenres = await dataAccess.DeleteAsync(EntityContainers.EpisodeGenre);
            ApiResponse deleteEpisodeRatings = await dataAccess.DeleteAsync(EntityContainers.EpisodeRatings);
            dataAccess.CloseTransaction();
            // check if any of the queries resulted in an error
            if (string.IsNullOrEmpty(deleteEpisodes.Error) && string.IsNullOrEmpty(deleteEpisodeActors.Error) && string.IsNullOrEmpty(deleteEpisodeCredits.Error) &&
                string.IsNullOrEmpty(deleteEpisodeGenres.Error) && string.IsNullOrEmpty(deleteEpisodeRatings.Error))
                return deleteEpisodes;
            else
                return new ApiResponse() { Error = "Error deleting all episodes!" };
        }

        /// <summary>
        /// Deletes an episode identified by <paramref name="id"/> from the storage medium
        /// </summary>
        /// <param name="id">The id of the episode to be deleted</param>
        /// <returns>The result of deleting the episode, wrapped in a generic API container of type <see cref="ApiResponse"/></returns>
        public async Task<ApiResponse> DeleteByIdAsync(int id)
        {
            // delete the episode and its associated data
            dataAccess.OpenTransaction();
            ApiResponse deleteEpisode = await dataAccess.DeleteAsync(EntityContainers.Episodes, new { Id = id });
            ApiResponse deleteEpisodeActors = await dataAccess.DeleteAsync(EntityContainers.EpisodeActors, new { EpisodeId = id });
            ApiResponse deleteEpisodeCredits = await dataAccess.DeleteAsync(EntityContainers.EpisodeCredits, new { EpisodeId = id });
            ApiResponse deleteEpisodeGenres = await dataAccess.DeleteAsync(EntityContainers.EpisodeGenre, new { EpisodeId = id });
            ApiResponse deleteEpisodeRatings = await dataAccess.DeleteAsync(EntityContainers.EpisodeRatings, new { EpisodeId = id });
            dataAccess.CloseTransaction();
            // check if any of the queries resulted in an error
            if (string.IsNullOrEmpty(deleteEpisode.Error) && string.IsNullOrEmpty(deleteEpisodeActors.Error) && string.IsNullOrEmpty(deleteEpisodeCredits.Error) &&
                string.IsNullOrEmpty(deleteEpisodeGenres.Error) && string.IsNullOrEmpty(deleteEpisodeRatings.Error))
                return deleteEpisode;
            else
                return new ApiResponse() { Error = "Error deleting the episode!" };
        }

        /// <summary>
        /// Gets all episodes and their associated data from the storage medium
        /// </summary>
        /// <returns>A list of episodes and their associated data, wrapped in a generic API container of type <see cref="ApiResponse{EpisodeEntity}"/></returns>
        public async Task<ApiResponse<EpisodeEntity>> GetAllAsync()
        {
            dataAccess.OpenTransaction();
            // get the episodes data
            ApiResponse<EpisodeEntity> episodes = await dataAccess.SelectAsync<EpisodeEntity>(EntityContainers.Episodes,
                "Id, TvShowId, Title, NamedTitle, SeasonId, Episode, Synopsis, Runtime, MPAA, LastPlayed, ImDbId, TvDbId, TmDbId, Director, Year, Aired, IsWatched, IsFavorite");
            if (episodes.Data != null)
            {
                await Task.Run(async () =>
                {
                    for (int i = 0; i < episodes.Data.Length; i++)
                    {
                        // get the data associated with the episode
                        episodes.Data[i].Ratings = (await dataAccess.SelectAsync<EpisodeRatingEntity>(EntityContainers.EpisodeRatings, "Id, EpisodeId, Name, Max, Value, Votes", new { EpisodeId = episodes.Data[i].Id })).Data;
                        episodes.Data[i].Genres = (await dataAccess.SelectAsync<EpisodeGenreEntity>(EntityContainers.EpisodeGenre, "Genre, EpisodeId, Id", new { EpisodeId = episodes.Data[i].Id })).Data;
                        episodes.Data[i].Actors = (await dataAccess.SelectAsync<EpisodeActorEntity>(EntityContainers.EpisodeActors, "Id, EpisodeId, Name, Role, `Order`, Thumb", new { EpisodeId = episodes.Data[i].Id })).Data;
                        episodes.Data[i].Credits = (await dataAccess.SelectAsync<EpisodeCreditEntity>(EntityContainers.EpisodeCredits, "Credit, EpisodeId, Id", new { EpisodeId = episodes.Data[i].Id })).Data;
                    }
                });
            }
            dataAccess.CloseTransaction();
            return episodes;
        }

        /// <summary>
        /// Gets an episode whose id is identified by <paramref name="id"/> and its associated data from the storage medium
        /// </summary>
        /// <param name="id">The id of the episode to get</param>
        /// <returns>The episode whose id is identified by <paramref name="id"/>, wrapped in a generic API container of type <see cref="ApiResponse{EpisodeEntity}"/></returns>
        public async Task<ApiResponse<EpisodeEntity>> GetByIdAsync(int id)
        {
            dataAccess.OpenTransaction();
            // get the episode data
            ApiResponse<EpisodeEntity> episode = await dataAccess.SelectAsync<EpisodeEntity>(EntityContainers.Episodes,
                "Id, TvShowId, Title, SeasonId, NamedTitle, Episode, Synopsis, Runtime, MPAA, LastPlayed, ImDbId, TvDbId, TmDbId, Director, Year, Aired, IsWatched, IsFavorite",
                new { Id = id });
            if (episode.Data != null)
            {
                await Task.Run(async () =>
                {
                    // get the data associated with the episode
                    episode.Data[0].Ratings = (await dataAccess.SelectAsync<EpisodeRatingEntity>(EntityContainers.EpisodeRatings, "Id, EpisodeId, Name, Max, Value, Votes", new { EpisodeId = episode.Data[0].Id })).Data;
                    episode.Data[0].Genres = (await dataAccess.SelectAsync<EpisodeGenreEntity>(EntityContainers.EpisodeGenre, "Genre, EpisodeId, Id", new { EpisodeId = episode.Data[0].Id })).Data;
                    episode.Data[0].Actors = (await dataAccess.SelectAsync<EpisodeActorEntity>(EntityContainers.EpisodeActors, "Id, EpisodeId, Name, Role, `Order`, Thumb", new { EpisodeId = episode.Data[0].Id })).Data;
                    episode.Data[0].Credits = (await dataAccess.SelectAsync<EpisodeCreditEntity>(EntityContainers.EpisodeCredits, "Credit, EpisodeId, Id", new { EpisodeId = episode.Data[0].Id })).Data;
                });
            }
            dataAccess.CloseTransaction();
            return episode;
        }

        /// <summary>
        /// Gets an episode whose tv show's id is identified by <paramref name="tvShowId"/> and its season is identified by <paramref name="seasonId"/>, and its associated data from the storage medium
        /// </summary>
        /// <param name="tvShowId">The id of the tv show of the episode to get</param>
        /// <param name="seasonId">The id of the season of the episode to get</param>
        /// <returns>The episode whose tv show's id is identified by <paramref name="tvShowId"/> and its season is identified by <paramref name="seasonId"/>, 
        /// wrapped in a generic API container of type <see cref="ApiResponse{EpisodeEntity}"/></returns>
        public async Task<ApiResponse<EpisodeEntity>> GetByIdAsync(int tvShowId, int seasonId)
        {
            dataAccess.OpenTransaction();
            // get the episode data
            ApiResponse<EpisodeEntity> episode = await dataAccess.SelectAsync<EpisodeEntity>(EntityContainers.Episodes,
                "Id, TvShowId, Title, NamedTitle, Episode, Synopsis, Runtime, MPAA, LastPlayed, ImDbId, TvDbId, TmDbId, Director, Year, Aired, IsWatched, IsFavorite",
                new { TvShowId = tvShowId, SeasonId = seasonId });
            if (episode.Data != null)
            {
                await Task.Run(async () =>
                {
                    // get the data associated with the episode
                    episode.Data[0].Ratings = (await dataAccess.SelectAsync<EpisodeRatingEntity>(EntityContainers.EpisodeRatings, "Id, EpisodeId, Name, Max, Value, Votes", new { EpisodeId = episode.Data[0].Id })).Data;
                    episode.Data[0].Genres = (await dataAccess.SelectAsync<EpisodeGenreEntity>(EntityContainers.EpisodeGenre, "Genre, EpisodeId, Id", new { EpisodeId = episode.Data[0].Id })).Data;
                    episode.Data[0].Actors = (await dataAccess.SelectAsync<EpisodeActorEntity>(EntityContainers.EpisodeActors, "Id, EpisodeId, Name, Role, `Order`, Thumb", new { EpisodeId = episode.Data[0].Id })).Data;
                    episode.Data[0].Credits = (await dataAccess.SelectAsync<EpisodeCreditEntity>(EntityContainers.EpisodeCredits, "Credit, EpisodeId, Id", new { EpisodeId = episode.Data[0].Id })).Data;
                });
            }
            dataAccess.CloseTransaction();
            return episode;
        }

        /// <summary>
        /// Saves an episode and its associated data in the storage medium
        /// </summary>
        /// <param name="entity">The episode to be saved</param>
        /// <returns>The result of saving <paramref name="entity"/>, wrapped in a generic API container of type <see cref="ApiResponse{EpisodeEntity}"/></returns>
        public async Task<ApiResponse<EpisodeEntity>> InsertAsync(EpisodeEntity entity)
        {
            dataAccess.OpenTransaction();
            // insert the episode
            ApiResponse<EpisodeEntity> episode = await dataAccess.InsertAsync(EntityContainers.Episodes, entity);
            if (episode.Data != null && episode.Data.Length > 0)
            {
                await Task.Run(async () =>
                {
                    // insert the ratings
                    if (entity.Ratings != null)
                    {
                        foreach (EpisodeRatingEntity rating in entity.Ratings)
                        {
                            rating.EpisodeId = episode.Data[0].Id;
                            await dataAccess.InsertAsync(EntityContainers.EpisodeRatings, rating);
                        }
                    }
                    // insert the genres
                    if (entity.Genres != null)
                    {
                        foreach (EpisodeGenreEntity genre in entity.Genres)
                        {
                            genre.EpisodeId = episode.Data[0].Id;
                            await dataAccess.InsertAsync(EntityContainers.EpisodeGenre, genre);
                        }
                    }
                    // insert the credits
                    if (entity.Credits != null)
                    {
                        foreach (EpisodeCreditEntity credit in entity.Credits)
                        {
                            credit.EpisodeId = episode.Data[0].Id;
                            await dataAccess.InsertAsync(EntityContainers.EpisodeCredits, credit);
                        }
                    }
                    // insert the actors
                    if (entity.Actors != null)
                    {
                        foreach (EpisodeActorEntity actor in entity.Actors)
                        {
                            actor.EpisodeId = episode.Data[0].Id;
                            await dataAccess.InsertAsync(EntityContainers.EpisodeActors, actor);
                        }
                    }
                });
            }
            dataAccess.CloseTransaction();
            return episode;
        }

        /// <summary>
        /// Updates <paramref name="entity"/> and its additional info in the storage medium
        /// </summary>
        /// <param name="entity">The entity that will be updated</param>
        /// <returns>The result of updating <paramref name="entity"/>, wrapped in a generic API container of type <see cref="ApiResponse"/></returns>
        public async Task<ApiResponse> UpdateAsync(EpisodeEntity entity)
        {
            dataAccess.OpenTransaction();
            // update the episode
            ApiResponse episode = await dataAccess.UpdateAsync(EntityContainers.Episodes,
                "TvShowId = '" + entity.TvShowId +
                "', SeasonId = '" + entity.SeasonId +
                "', Title = '" + entity.Title +
                "', NamedTitle = '" + entity.NamedTitle +
                "', Episode = '" + entity.Episode +
                "', Synopsis = '" + entity.Synopsis +
                "', Runtime = '" + entity.Runtime +
                "', MPAA = '" + entity.MPAA +
                "', LastPlayed = '" + entity.LastPlayed +
                "', ImDbId = '" + entity.ImDbId +
                "', TvDbId = '" + entity.TvDbId +
                "', TmDbId = '" + entity.TmDbId +
                "', Director = '" + entity.Director +
                "', Year = '" + entity.Year +
                "', Aired = '" + entity.Aired +
                "', IsWatched = '" + entity.IsWatched +
                "', IsFavorite = '" + entity.IsFavorite + "'",
                "Id", "'" + entity.Id + "'");
            // update the data associated with the episode
            await Task.Run(async () =>
            {
                foreach (EpisodeRatingEntity rating in entity.Ratings)
                {
                    await dataAccess.UpdateAsync(EntityContainers.EpisodeRatings,
                    "Name = '" + rating.Name +
                    "', Max = '" + rating.Max +
                    "', Value = '" + rating.Value +
                    "', Votes = '" + rating.Votes + "'",
                    "EpisodeId", "'" + entity.Id + "'");
                }
                foreach (EpisodeGenreEntity genre in entity.Genres)
                    await dataAccess.UpdateAsync(EntityContainers.EpisodeGenre, "Genre = '" + genre.Genre + "'", "EpisodeId", "'" + entity.Id + "'");
                foreach (EpisodeCreditEntity credit in entity.Credits)
                    await dataAccess.UpdateAsync(EntityContainers.EpisodeCredits, "Credit = '" + credit.Credit + "'", "EpisodeId", "'" + entity.Id + "'");
                foreach (EpisodeActorEntity actor in entity.Actors)
                {
                    await dataAccess.UpdateAsync(EntityContainers.EpisodeActors,
                    "Name = '" + actor.Name +
                    "', Role = '" + actor.Role +
                    "', Order = '" + actor.Order +
                    "', Thumb = '" + actor.Thumb + "'",
                    "EpisodeId", "'" + entity.Id + "'");
                }
            });
            dataAccess.CloseTransaction();
            return episode;
        }

        /// <summary>
        /// Updates the IsWatched status of an episode identified by <paramref name="episodeId"/> in the storage medium
        /// </summary>
        /// <param name="episodeId">The id of the episode whose status will be updated</param>
        /// <param name="isWatched">The IsWatched status to be set</param>
        /// <returns>The result of updating the IsWatched status, wrapped in a generic API container of type <see cref="ApiResponse"/></returns>
        public async Task<ApiResponse> UpdateIsWatchedStatusAsync(int episodeId, bool? isWatched)
        {
            return await dataAccess.UpdateAsync(EntityContainers.Episodes, "IsWatched = '" + (isWatched != null ? isWatched.ToString() : "Null") + "'", "Id", "'" + episodeId + "'");
        }

        /// <summary>
        /// Updates the IsFavorite status of an episode identified by <paramref name="episodeId"/> in the storage medium
        /// </summary>
        /// <param name="episodeId">The id of the episode whose status will be updated</param>
        /// <param name="isFavorite">The IsFavorite status to be set</param>
        /// <returns>The result of updating the IsFavorite status, wrapped in a generic API container of type <see cref="ApiResponse"/></returns>
        public async Task<ApiResponse> UpdateIsFavoriteStatusAsync(int episodeId, bool isFavorite)
        {
            return await dataAccess.UpdateAsync(EntityContainers.Episodes, "IsFavorite = '" + isFavorite + "'", "Id", "'" + episodeId + "'");
        }
        #endregion
    }
}
