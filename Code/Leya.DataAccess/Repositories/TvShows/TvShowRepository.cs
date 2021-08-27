/// Written by: Yulia Danilova
/// Creation Date: 20th of November, 2020
/// Purpose: Tv show repository for the bridge-through between the generic storage medium and storage medium for Tv Shows
#region ========================================================================= USING =====================================================================================
using System.Linq;
using System.Threading.Tasks;
using Leya.DataAccess.Common.Enums;
using Leya.DataAccess.Common.Models;
using Leya.DataAccess.StorageAccess;
using Leya.DataAccess.Common.Models.TvShows;
using Leya.DataAccess.Common.Models.Seasons;
#endregion

namespace Leya.DataAccess.Repositories.TvShows
{
    internal sealed class TvShowRepository : ITvShowRepository
    {
        #region ============================================================== FIELD MEMBERS ================================================================================
        private readonly IDataAccess dataAccess;
        #endregion

        #region ================================================================== CTOR =====================================================================================
        /// <summary>
        /// Overload C-tor
        /// </summary>
        /// <param name="dataAccess">The injected data access to use</param>
        public TvShowRepository(IDataAccess dataAccess)
        {
            this.dataAccess = dataAccess;
        }
        #endregion

        #region ================================================================= METHODS ===================================================================================
        /// <summary>
        /// Deletes all tv shows from the storage medium
        /// </summary>
        /// <returns>The result of deleting the tv shows, wrapped in a generic API container of type <see cref="ApiResponse"/></returns>
        public async Task<ApiResponse> DeleteAllAsync()
        {
            // delete all tv shows and their associated data 
            dataAccess.OpenTransaction();
            ApiResponse deleteTvShows = await dataAccess.DeleteAsync(EntityContainers.TvShows);
            ApiResponse deleteEpisodes = await dataAccess.DeleteAsync(EntityContainers.Episodes);
            ApiResponse deleteTvShowRatings = await dataAccess.DeleteAsync(EntityContainers.TvShowRatings);
            ApiResponse deleteTvShowGenres = await dataAccess.DeleteAsync(EntityContainers.TvShowGenre);
            ApiResponse deleteTvShowActors = await dataAccess.DeleteAsync(EntityContainers.TvShowActors);
            ApiResponse deleteSeasons = await dataAccess.DeleteAsync(EntityContainers.Seasons);
            ApiResponse deleteTvShowResume = await dataAccess.DeleteAsync(EntityContainers.TvShowResume);
            ApiResponse deleteEpisodeActors = await dataAccess.DeleteAsync(EntityContainers.EpisodeActors);
            ApiResponse deleteEpisodeCredits = await dataAccess.DeleteAsync(EntityContainers.EpisodeCredits);
            ApiResponse deleteEpisodeGenres = await dataAccess.DeleteAsync(EntityContainers.EpisodeGenre);
            ApiResponse deleteEpisodeRatings = await dataAccess.DeleteAsync(EntityContainers.EpisodeRatings);
            dataAccess.CloseTransaction();
            // check if any of the queries resulted in an error
            if (string.IsNullOrEmpty(deleteTvShows.Error) && string.IsNullOrEmpty(deleteEpisodes.Error) && string.IsNullOrEmpty(deleteTvShowRatings.Error)
                && string.IsNullOrEmpty(deleteTvShowGenres.Error) && string.IsNullOrEmpty(deleteTvShowActors.Error) && string.IsNullOrEmpty(deleteSeasons.Error)
                && string.IsNullOrEmpty(deleteTvShowResume.Error) && string.IsNullOrEmpty(deleteEpisodeActors.Error) && string.IsNullOrEmpty(deleteEpisodeCredits.Error)
                && string.IsNullOrEmpty(deleteEpisodeGenres.Error) && string.IsNullOrEmpty(deleteEpisodeRatings.Error))
                return deleteEpisodeRatings;
            else
                return new ApiResponse() { Error = "Error deleting all tv shows!" };
        }

        /// <summary>
        /// Deletes a tv show identified by <paramref name="id"/> from the storage medium
        /// </summary>
        /// <param name="id">The id of the tv show to be deleted</param>
        /// <returns>The result of deleting the tv show, wrapped in a generic API container of type <see cref="ApiResponse"/></returns>
        public async Task<ApiResponse> DeleteByIdAsync(int id)
        {
            // delete the tv show and its associated data 
            dataAccess.OpenTransaction();
            ApiResponse deleteTvShow = await dataAccess.DeleteAsync(EntityContainers.TvShows, new { Id = id });
            ApiResponse deleteEpisode = await dataAccess.DeleteAsync(EntityContainers.Episodes, new { TvShowId = id });
            ApiResponse deleteTvShowRating = await dataAccess.DeleteAsync(EntityContainers.TvShowRatings, new { TvShowId = id });
            ApiResponse deleteTvShowGenre = await dataAccess.DeleteAsync(EntityContainers.TvShowGenre, new { TvShowId = id });
            ApiResponse deleteTvShowActors = await dataAccess.DeleteAsync(EntityContainers.TvShowActors, new { TvShowId = id });
            ApiResponse deleteSeasons = await dataAccess.DeleteAsync(EntityContainers.Seasons, new { TvShowId = id });
            ApiResponse deleteTvShowResume = await dataAccess.DeleteAsync(EntityContainers.TvShowResume, new { TvShowId = id }); 
            ApiResponse deleteEpisodeActors = await dataAccess.DeleteAsync(EntityContainers.EpisodeActors, new { EpisodeId = id });
            ApiResponse deleteEpisodeCredits = await dataAccess.DeleteAsync(EntityContainers.EpisodeCredits, new { EpisodeId = id });
            ApiResponse deleteEpisodeGenres = await dataAccess.DeleteAsync(EntityContainers.EpisodeGenre, new { EpisodeId = id });
            ApiResponse deleteEpisodeRatings = await dataAccess.DeleteAsync(EntityContainers.EpisodeRatings, new { EpisodeId = id });
            dataAccess.CloseTransaction();
            // check is any of the queries resulted in an error
            if (string.IsNullOrEmpty(deleteTvShow.Error) && string.IsNullOrEmpty(deleteEpisode.Error) && string.IsNullOrEmpty(deleteTvShowRating.Error)
                && string.IsNullOrEmpty(deleteTvShowGenre.Error) && string.IsNullOrEmpty(deleteTvShowActors.Error) && string.IsNullOrEmpty(deleteSeasons.Error)
                && string.IsNullOrEmpty(deleteTvShowResume.Error) && string.IsNullOrEmpty(deleteEpisodeActors.Error) && string.IsNullOrEmpty(deleteEpisodeCredits.Error)
                && string.IsNullOrEmpty(deleteEpisodeGenres.Error) && string.IsNullOrEmpty(deleteEpisodeRatings.Error))
                return deleteEpisodeRatings;
            else
                return new ApiResponse() { Error = "Error deleting the tv show!" };
        }

        /// <summary>
        /// Gets all tv shows and their associated data from the storage medium
        /// </summary>
        /// <returns>A list of tv shows and their associated data, wrapped in a generic API container of type <see cref="ApiResponse{TvShowEntity}"/></returns>
        public async Task<ApiResponse<TvShowEntity>> GetAllAsync()
        {
            dataAccess.OpenTransaction();
            // get the tv shows data
            ApiResponse<TvShowEntity> tvShows = await dataAccess.SelectAsync<TvShowEntity>(EntityContainers.TvShows,
                "Id, MediaTypeSourceId, MediaTypeId, TvShowNamedTitle, TvShowTitle, NumberOfSeasons, NumberOfEpisodes, Synopsis, TagLine, Runtime, MPAA, " +
                "LastPlayed, ImDbId, TvDbId, TmDbId, Aired, IsEnded, Trailer, Studio, IsWatched, IsFavorite, Created");
            if (tvShows.Data != null)
            {
                await Task.Run(async () =>
                {
                    for (int i = 0; i < tvShows.Data.Length; i++)
                    {
                        // get the data associated with the tv show
                        tvShows.Data[i].Ratings = (await dataAccess.SelectAsync<TvShowRatingEntity>(EntityContainers.TvShowRatings, "Id, TvShowId, Name, Max, Value, Votes", new { TvShowId = tvShows.Data[i].Id })).Data;
                        tvShows.Data[i].Genres = (await dataAccess.SelectAsync<TvShowGenreEntity>(EntityContainers.TvShowGenre, "Genre, TvShowId, Id", new { TvShowId = tvShows.Data[i].Id })).Data;
                        tvShows.Data[i].Actors = (await dataAccess.SelectAsync<TvShowActorEntity>(EntityContainers.TvShowActors, "Id, TvShowId, Name, Role, `Order`, Thumb", new { TvShowId = tvShows.Data[i].Id })).Data;
                        //tvShows.Data[i].Seasons = (await dataAccess.SelectAsync<SeasonEntity>(EntityContainers.Seasons, "Id, TvShowId, SeasonNumber, SeasonName, IsWatched, IsFavorite, Year, Synopsis, Premiered", new { TvShowId = tvShows.Data[i].Id })).Data;
                        tvShows.Data[i].Resume = (await dataAccess.SelectAsync<TvShowResumeEntity>(EntityContainers.TvShowResume, "TvShowId, SeasonId, EpisodeId, Position, Total", new { TvShowId = tvShows.Data[i].Id, SeasonId = 0, EpisodeId = 0 })).Data?[0];
                    }
                });
            }
            dataAccess.CloseTransaction();
            return tvShows;
        }

        /// <summary>
        /// Gets a tv show whose media type source's id is identified by <paramref name="id"/> and its associated data from the storage medium
        /// </summary>
        /// <param name="id">The id of the media type source of the tv show to get</param>
        /// <returns>The tv show whose media type source's id is identified by <paramref name="id"/>, wrapped in a generic API container of type <see cref="ApiResponse{TvShowEntity}"/></returns>
        public async Task<ApiResponse<TvShowEntity>> GetByIdAsync(int id)
        {
            // get the tv show data
            dataAccess.OpenTransaction();
            ApiResponse<TvShowEntity> _output = await dataAccess.SelectAsync<TvShowEntity>(EntityContainers.TvShows,
                "Id, MediaTypeSourceId, MediaTypeId, TvShowNamedTitle, TvShowTitle, NumberOfSeasons, NumberOfEpisodes, Synopsis, TagLine, Runtime, MPAA, " +
                "LastPlayed, ImDbId, TvDbId, TmDbId, Aired, IsEnded, Trailer, Studio, IsWatched, IsFavorite, Created", new { Id = id });
            if (_output.Data != null)
            {
                await Task.Run(async () =>
                {
                    // get the data assoviated with the tv show
                    _output.Data[0].Ratings = (await dataAccess.SelectAsync<TvShowRatingEntity>(EntityContainers.TvShowRatings, "Id, TvShowId, Name, Max, Value, Votes", new { TvShowId = _output.Data[0].Id })).Data;
                    _output.Data[0].Genres = (await dataAccess.SelectAsync<TvShowGenreEntity>(EntityContainers.TvShowGenre, "Genre, TvShowId, Id", new { TvShowId = _output.Data[0].Id })).Data;
                    _output.Data[0].Actors = (await dataAccess.SelectAsync<TvShowActorEntity>(EntityContainers.TvShowActors, "Id, TvShowId, Name, Role, `Order`, Thumb", new { TvShowId = _output.Data[0].Id })).Data;
                    //_output.Data[0].Seasons = (await dataAccess.SelectAsync<SeasonEntity>(EntityContainers.Seasons, "Id, TvShowId, SeasonNumber, SeasonName, IsWatched, IsFavorite, Year, Synopsis, Premiered", new { TvShowId = _output.Data[0].Id })).Data;
                    _output.Data[0].Resume = (await dataAccess.SelectAsync<TvShowResumeEntity>(EntityContainers.TvShowResume, "TvShowId, SeasonId, EpisodeId, Position, Total", new { TvShowId = _output.Data[0].Id, SeasonId = 0, EpisodeId = 0 })).Data?[0];
                });
            }
            dataAccess.CloseTransaction();
            return _output;
        }

        /// <summary>
        /// Saves a tv show and its associated data in the storage medium
        /// </summary>
        /// <param name="entity">The tv show to be saved</param>
        /// <returns>The result of saving <paramref name="entity"/>, wrapped in a generic API container of type <see cref="ApiResponse{TvShowEntity}"/></returns>
        public async Task<ApiResponse<TvShowEntity>> InsertAsync(TvShowEntity entity)
        {
            dataAccess.OpenTransaction();
            // insert the tv show
            ApiResponse<TvShowEntity> tvShow = await dataAccess.InsertAsync(EntityContainers.TvShows, entity);
            if (tvShow.Data != null && tvShow.Data.Length > 0)
            {
                await Task.Run(async () =>
                {
                    // insert the ratings
                    foreach (TvShowRatingEntity rating in entity?.Ratings ?? Enumerable.Empty<object>())
                    {
                        // assign the tv show id value to the inserted tv show output id
                        rating.TvShowId = tvShow.Data[0].Id;
                        await dataAccess.InsertAsync(EntityContainers.TvShowRatings, rating);
                    }
                    // insert the genre
                    foreach (TvShowGenreEntity genre in entity?.Genres ?? Enumerable.Empty<object>())
                    {
                        genre.TvShowId = tvShow.Data[0].Id;
                        await dataAccess.InsertAsync(EntityContainers.TvShowGenre, genre);
                    }
                    // insert the actors
                    foreach (TvShowActorEntity actor in entity?.Actors ?? Enumerable.Empty<object>())
                    {
                        // assign the tv show id value to the inserted tv show output id
                        actor.TvShowId = tvShow.Data[0].Id;
                        await dataAccess.InsertAsync(EntityContainers.TvShowActors, actor);
                    }
                    // insert the seasons
                    foreach (SeasonEntity season in entity?.Seasons ?? Enumerable.Empty<object>())
                    {
                        // assign the tv show id value to the inserted tv show output id
                        season.TvShowId = tvShow.Data[0].Id;
                        season.Id = (await dataAccess.InsertAsync(EntityContainers.Seasons, season)).Data[0].Id;
                    }
                    // assign the tv show id value to the inserted tv show output id
                    if (entity.Resume != null)
                    {
                        entity.Resume.TvShowId = tvShow.Data[0].Id;
                        // insert the tv show resume data
                        await dataAccess.InsertAsync(EntityContainers.TvShowResume, entity.Resume);
                    }
                });
            }
            dataAccess.CloseTransaction();
            return tvShow;
        }

        /// <summary>
        /// Updates <paramref name="entity"/> and its additional info in the storage medium
        /// </summary>
        /// <param name="entity">The entity that will be updated</param>
        /// <returns>The result of updating <paramref name="entity"/>, wrapped in a generic API container of type <see cref="ApiResponse"/></returns>
        public async Task<ApiResponse> UpdateAsync(TvShowEntity entity)
        {
            dataAccess.OpenTransaction();
            // update the tv show
            ApiResponse tvShow = await dataAccess.UpdateAsync(EntityContainers.TvShows,
                "MediaTypeSourceId = '" + entity.MediaTypeSourceId +
                "', MediaTypeId = '" + entity.MediaTypeId +
                "', TvShowTitle = '" + entity.TvShowTitle +
                "', TvShowNamedTitle = '" + entity.TvShowNamedTitle +
                "', NumberOfSeasons = '" + entity.NumberOfSeasons +
                "', NumberOfEpisodes = '" + entity.NumberOfEpisodes +
                "', Synopsis = '" + entity.Synopsis +
                "', TagLine = '" + entity.TagLine +
                "', Runtime = '" + entity.Runtime +
                "', MPAA = '" + entity.MPAA +
                "', LastPlayed = '" + entity.LastPlayed +
                "', ImDbId = '" + entity.ImDbId +
                "', TvDbId = '" + entity.TvDbId +
                "', TmDbId = '" + entity.TmDbId +
                "', Aired = '" + entity.Aired +
                "', IsEnded = '" + entity.IsEnded +
                "', Trailer = '" + entity.Trailer +
                "', Studio = '" + entity.Studio +
                "', IsWatched = '" + entity.IsWatched +
                "', IsFavorite = '" + entity.IsFavorite + "'",
                "Id", "'" + entity.Id + "'");
            // update the data associated with the tv show
            await Task.Run(async () =>
            {
                foreach (TvShowRatingEntity rating in entity.Ratings)
                {
                    tvShow.Error = (await dataAccess.UpdateAsync(EntityContainers.TvShowRatings,
                    "Name = '" + rating.Name +
                    "', Max = '" + rating.Max +
                    "', Value = '" + rating.Value +
                    "', Votes = '" + rating.Votes + "'",
                    "Id", "'" + rating.Id + "'"))?.Error ?? tvShow.Error;
                }
                foreach (TvShowGenreEntity genre in entity.Genres)
                    tvShow.Error = (await dataAccess.UpdateAsync(EntityContainers.TvShowGenre, "Genre = '" + genre.Genre + "'", "Id", "'" + genre.Id + "'"))?.Error ?? tvShow.Error;
                foreach (TvShowActorEntity actor in entity.Actors)
                {
                    tvShow.Error = (await dataAccess.UpdateAsync(EntityContainers.TvShowActors,
                    "Name = '" + actor.Name +
                    "', Role = '" + actor.Role +
                    "', `Order` = '" + actor.Order +
                    "', Thumb = '" + actor.Thumb + "'",
                    "Id", "'" + actor.Id + "'"))?.Error ?? tvShow.Error;
                }
                //foreach (SeasonEntity season in entity.Seasons)
                //{
                //    tvShow.Error = (await dataAccess.UpdateAsync(EntityContainers.Seasons,
                //    "Year = '" + season.Year +
                //    "', SeasonNumber = '" + season.SeasonNumber +
                //    "', SeasonName = '" + season.SeasonName +
                //    "', IsWatched = '" + season.IsWatched +
                //    "', IsFavorite = '" + season.IsFavorite +
                //    "', Synopsis = '" + season.Synopsis +
                //    "', Premiered = '" + season.Premiered + "'",
                //    "Id", "'" + season.Id + "'"))?.Error ?? tvShow.Error;
                //}
            });
            dataAccess.CloseTransaction();
            return tvShow;
        }

        /// <summary>
        /// Updates the IsWatched status of a tv show identified by <paramref name="tvShowId"/> in the storage medium
        /// </summary>
        /// <param name="tvShowId">The id of the tv show whose status will be updated</param>
        /// <param name="isWatched">The IsWatched status to be set</param>
        /// <returns>The result of updating the IsWatched status, wrapped in a generic API container of type <see cref="ApiResponse"/></returns>
        public async Task<ApiResponse> UpdateIsWatchedStatusAsync(int tvShowId, bool? isWatched)
        {
            return await dataAccess.UpdateAsync(EntityContainers.TvShows, "IsWatched = '" + (isWatched != null ? isWatched.ToString() : "Null") + "'", "Id", "'" + tvShowId + "'");
        }

        /// <summary>
        /// Updates the IsFavorite status of a tv show identified by <paramref name="tvShowId"/> in the storage medium
        /// </summary>
        /// <param name="tvShowId">The id of the tv show whose status will be updated</param>
        /// <param name="isFavorite">The IsFavorite status to be set</param>
        /// <returns>The result of updating the IsFavorite status, wrapped in a generic API container of type <see cref="ApiResponse"/></returns>
        public async Task<ApiResponse> UpdateIsFavoriteStatusAsync(int tvShowId, bool isFavorite)
        {
            return await dataAccess.UpdateAsync(EntityContainers.TvShows, "IsFavorite = '" + isFavorite + "'", "Id", "'" + tvShowId + "'");
        }
        #endregion
    }
}
