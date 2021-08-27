/// Written by: Yulia Danilova
/// Creation Date: 24th of November, 2020
/// Purpose: Movie repository for the bridge-through between the generic storage medium and storage medium for Movies
#region ========================================================================= USING =====================================================================================
using System.Threading.Tasks;
using Leya.DataAccess.Common.Enums;
using Leya.DataAccess.StorageAccess;
using Leya.DataAccess.Common.Models;
using Leya.DataAccess.Common.Models.Movies;
#endregion

namespace Leya.DataAccess.Repositories.Movies
{
    internal sealed class MovieRepository : IMovieRepository
    {
        #region ============================================================== FIELD MEMBERS ================================================================================
        private readonly IDataAccess dataAccess;
        #endregion

        #region ================================================================== CTOR =====================================================================================
        /// <summary>
        /// Overload C-tor
        /// </summary>
        /// <param name="dataAccess">The injected data access to use</param>
        public MovieRepository(IDataAccess dataAccess)
        {
            this.dataAccess = dataAccess;
        }
        #endregion

        #region ================================================================= METHODS ===================================================================================
        /// <summary>
        /// Deletes all movies and their associated data from the storage medium
        /// </summary>
        /// <returns>The result of deleting the movies, wrapped in a generic API container of type <see cref="ApiResponse"/></returns>
        public async Task<ApiResponse> DeleteAllAsync()
        {
            // deletes all movies and their associated data
            dataAccess.OpenTransaction();
            ApiResponse deleteMovies = await dataAccess.DeleteAsync(EntityContainers.Movies);
            ApiResponse deleteMovieStreamDetails = await dataAccess.DeleteAsync(EntityContainers.MovieStreamDetails);
            ApiResponse deleteMovieRatings = await dataAccess.DeleteAsync(EntityContainers.MovieRatings);
            ApiResponse deleteMovieActors = await dataAccess.DeleteAsync(EntityContainers.MovieActors);
            ApiResponse deleteMovieResume = await dataAccess.DeleteAsync(EntityContainers.MovieResume);
            ApiResponse deleteMovieCredits = await dataAccess.DeleteAsync(EntityContainers.MovieCredits);
            ApiResponse deleteMovieAudioInfo = await dataAccess.DeleteAsync(EntityContainers.MovieAudioInfo);
            ApiResponse deleteMovieFileInfo = await dataAccess.DeleteAsync(EntityContainers.MovieFileInfo);
            ApiResponse deleteMovieVideoInfo = await dataAccess.DeleteAsync(EntityContainers.MovieVideoInfo);
            ApiResponse deleteMovieTags = await dataAccess.DeleteAsync(EntityContainers.MovieTags);
            ApiResponse deleteMovieSubtitles = await dataAccess.DeleteAsync(EntityContainers.MovieSubtitles);
            ApiResponse deleteMovieCountries = await dataAccess.DeleteAsync(EntityContainers.MovieCountry);
            ApiResponse deleteMovieDirectors = await dataAccess.DeleteAsync(EntityContainers.MovieDirectors);
            dataAccess.CloseTransaction();
            // check is any of the queries resulted in an error
            if (string.IsNullOrEmpty(deleteMovies.Error) && string.IsNullOrEmpty(deleteMovieStreamDetails.Error) && string.IsNullOrEmpty(deleteMovieRatings.Error)
                && string.IsNullOrEmpty(deleteMovieActors.Error) && string.IsNullOrEmpty(deleteMovieResume.Error) && string.IsNullOrEmpty(deleteMovieCredits.Error)
                && string.IsNullOrEmpty(deleteMovieAudioInfo.Error) && string.IsNullOrEmpty(deleteMovieFileInfo.Error) && string.IsNullOrEmpty(deleteMovieVideoInfo.Error)
                && string.IsNullOrEmpty(deleteMovieTags.Error) && string.IsNullOrEmpty(deleteMovieSubtitles.Error) && string.IsNullOrEmpty(deleteMovieCountries.Error) && string.IsNullOrEmpty(deleteMovieDirectors.Error))
                return deleteMovieDirectors;
            else
                return new ApiResponse() { Error = "Error deleting all movies!" };
        }

        /// <summary>
        /// Deletes a movie identified by <paramref name="id"/> from the storage medium
        /// </summary>
        /// <param name="id">The id of the movie to be deleted</param>
        /// <returns>The result of deleting the movie, wrapped in a generic API container of type <see cref="ApiResponse"/></returns>
        public async Task<ApiResponse> DeleteByIdAsync(int id)
        {
            // deletes the movie and its associated data
            dataAccess.OpenTransaction();
            ApiResponse deleteMovies = await dataAccess.DeleteAsync(EntityContainers.Movies, new { Id = id });
            ApiResponse deleteMovieStreamDetails = await dataAccess.DeleteAsync(EntityContainers.MovieStreamDetails, new { MovieId = id });
            ApiResponse deleteMovieRatings = await dataAccess.DeleteAsync(EntityContainers.MovieRatings, new { MovieId = id });
            ApiResponse deleteMovieActors = await dataAccess.DeleteAsync(EntityContainers.MovieActors, new { MovieId = id });
            ApiResponse deleteMovieResume = await dataAccess.DeleteAsync(EntityContainers.MovieResume, new { MovieId = id });
            ApiResponse deleteMovieCredits = await dataAccess.DeleteAsync(EntityContainers.MovieCredits, new { MovieId = id });
            ApiResponse deleteMovieAudioInfo = await dataAccess.DeleteAsync(EntityContainers.MovieAudioInfo, new { MovieId = id });
            ApiResponse deleteMovieFileInfo = await dataAccess.DeleteAsync(EntityContainers.MovieFileInfo, new { MovieId = id });
            ApiResponse deleteMovieVideoInfo = await dataAccess.DeleteAsync(EntityContainers.MovieVideoInfo, new { MovieId = id });
            ApiResponse deleteMovieTags = await dataAccess.DeleteAsync(EntityContainers.MovieTags, new { MovieId = id });
            ApiResponse deleteMovieSubtitles = await dataAccess.DeleteAsync(EntityContainers.MovieSubtitles, new { MovieId = id });
            ApiResponse deleteMovieCountries = await dataAccess.DeleteAsync(EntityContainers.MovieCountry, new { MovieId = id });
            ApiResponse deleteMovieDirectors = await dataAccess.DeleteAsync(EntityContainers.MovieDirectors, new { MovieId = id });
            dataAccess.CloseTransaction();
            // check is any of the queries resulted in an error
            if (string.IsNullOrEmpty(deleteMovies.Error) && string.IsNullOrEmpty(deleteMovieStreamDetails.Error) && string.IsNullOrEmpty(deleteMovieRatings.Error) && 
                string.IsNullOrEmpty(deleteMovieActors.Error) && string.IsNullOrEmpty(deleteMovieResume.Error) && string.IsNullOrEmpty(deleteMovieCredits.Error) && 
                string.IsNullOrEmpty(deleteMovieAudioInfo.Error) && string.IsNullOrEmpty(deleteMovieFileInfo.Error) && string.IsNullOrEmpty(deleteMovieVideoInfo.Error) && 
                string.IsNullOrEmpty(deleteMovieTags.Error) && string.IsNullOrEmpty(deleteMovieSubtitles.Error) && string.IsNullOrEmpty(deleteMovieCountries.Error) && 
                string.IsNullOrEmpty(deleteMovieDirectors.Error)) 
                return deleteMovieDirectors;
            else
                return new ApiResponse() { Error = "Error deleting the movie!" };
        }

        /// <summary>
        /// Gets all movies and their associated data from the storage medium
        /// </summary>
        /// <returns>A list of movies and their associated data, wrapped in a generic API container of type <see cref="ApiResponse{MovieEntity}"/></returns>
        public async Task<ApiResponse<MovieEntity>> GetAllAsync()
        {
            dataAccess.OpenTransaction();
            // get the movie data
            ApiResponse<MovieEntity> movies = await dataAccess.SelectAsync<MovieEntity>(EntityContainers.Movies,
                "Id, MediaTypeSourceId, MediaTypeId, MovieTitle, OriginalTitle, NamedTitle, Synopsis, TagLine, Runtime, MPAA, LastPlayed, ImDbId, " +
                "TmDbId, `Set`, Premiered, IsEnded, Studio, Trailer, TvShowLink, IsWatched, IsFavorite, Created");
            if (movies.Data != null)
            {
                await Task.Run(async () =>
                {
                    for (int i = 0; i < movies.Data.Length; i++)
                    {
                        // get the data associated with the movie
                        movies.Data[i].Ratings = (await dataAccess.SelectAsync<MovieRatingEntity>(EntityContainers.MovieRatings, "Id, MovieId, Name, Max, Value, Votes", new { MovieId = movies.Data[i].Id })).Data;
                        movies.Data[i].Genre = (await dataAccess.SelectAsync<MovieGenreEntity>(EntityContainers.MovieGenre, "Genre, MovieId, Id", new { MovieId = movies.Data[i].Id })).Data;
                        movies.Data[i].Country = (await dataAccess.SelectAsync<MovieCountryEntity>(EntityContainers.MovieCountry, "Country, MovieId, Id", new { MovieId = movies.Data[i].Id })).Data;
                        movies.Data[i].Credits = (await dataAccess.SelectAsync<MovieCreditEntity>(EntityContainers.MovieCredits, "Credit, MovieId, Id", new { MovieId = movies.Data[i].Id })).Data;
                        movies.Data[i].Director = (await dataAccess.SelectAsync<MovieDirectorEntity>(EntityContainers.MovieDirectors, "Director, MovieId, Id", new { MovieId = movies.Data[i].Id })).Data;
                        movies.Data[i].Tags = (await dataAccess.SelectAsync<MovieTagEntity>(EntityContainers.MovieTags, "Tag, MovieId, Id", new { MovieId = movies.Data[i].Id })).Data;
                        movies.Data[i].FileInfo = (await dataAccess.SelectAsync<FileInfoEntity>(EntityContainers.MovieFileInfo, "MovieId, Id", new { MovieId = movies.Data[i].Id })).Data[0];
                        movies.Data[i].FileInfo.StreamDetails = (await dataAccess.SelectAsync<StreamDetailEntity>(EntityContainers.MovieStreamDetails, "MovieId, Id", new { MovieId = movies.Data[i].Id })).Data[0];
                        movies.Data[i].FileInfo.StreamDetails.Audio = (await dataAccess.SelectAsync<AudioEntity>(EntityContainers.MovieAudioInfo, "Id, MovieId, Codec, Language, Channels", new { MovieId = movies.Data[i].Id })).Data[0];
                        movies.Data[i].FileInfo.StreamDetails.Video = (await dataAccess.SelectAsync<VideoEntity>(EntityContainers.MovieVideoInfo, "Id, MovieId, Codec, Aspect, Width, Height, Is3D", new { MovieId = movies.Data[i].Id })).Data[0];
                        movies.Data[i].FileInfo.StreamDetails.Subtitle = (await dataAccess.SelectAsync<SubtitleEntity>(EntityContainers.MovieSubtitles, "Id, MovieId, Language", new { MovieId = movies.Data[i].Id })).Data[0];
                        movies.Data[i].Actors = (await dataAccess.SelectAsync<MovieActorEntity>(EntityContainers.MovieActors, "Id, MovieId, Name, Role, `Order`, Thumb", new { MovieId = movies.Data[i].Id })).Data;
                        movies.Data[i].Resume = (await dataAccess.SelectAsync<MovieResumeEntity>(EntityContainers.MovieResume, "Id, MovieId, Position, Total", new { MovieId = movies.Data[i].Id })).Data[0];
                    }
                });
            }
            dataAccess.CloseTransaction();
            return movies;
        }

        /// <summary>
        /// Gets a movie whose media type source's id is identified by <paramref name="id"/> and its associated data from the storage medium
        /// </summary>
        /// <param name="id">The id of the media type source of the movie to get</param>
        /// <returns>The movie whose media type source's id is identified by <paramref name="id"/>, wrapped in a generic API container of type <see cref="ApiResponse{MovieEntity}"/></returns>
        public async Task<ApiResponse<MovieEntity>> GetByIdAsync(int id)
        {
            dataAccess.OpenTransaction();
            // get the movie data
            ApiResponse<MovieEntity> _output = await dataAccess.SelectAsync<MovieEntity>(EntityContainers.Movies,
                "Id, MediaTypeSourceId, MediaTypeId, MovieTitle, OriginalTitle, NamedTitle, Synopsis, TagLine, Runtime, MPAA, LastPlayed, ImDbId, " +
                "TmDbId, Set, Premiered, IsEnded, Studio, Trailer, TvShowLink, IsWatched, IsFavorite, Created", new { MediaTypeSourceId = id });
            if (_output.Data != null && _output.Data.Length > 0)
            {
                await Task.Run(async () =>
                {
                    // get the data associated with the movie
                    _output.Data[0].Ratings = (await dataAccess.SelectAsync<MovieRatingEntity>(EntityContainers.MovieRatings, "Id, MovieId, Name, Max, Value, Votes", new { MovieId = _output.Data[0].Id })).Data;
                    _output.Data[0].Genre = (await dataAccess.SelectAsync<MovieGenreEntity>(EntityContainers.MovieGenre, "Genre, MovieId, Id", new { MovieId = _output.Data[0].Id })).Data;
                    _output.Data[0].Country = (await dataAccess.SelectAsync<MovieCountryEntity>(EntityContainers.MovieCountry, "Country, MovieId, Id", new { MovieId = _output.Data[0].Id })).Data;
                    _output.Data[0].Credits = (await dataAccess.SelectAsync<MovieCreditEntity>(EntityContainers.MovieCredits, "Credit, MovieId, Id", new { MovieId = _output.Data[0].Id })).Data;
                    _output.Data[0].Director = (await dataAccess.SelectAsync<MovieDirectorEntity>(EntityContainers.MovieDirectors, "Director, MovieId, Id", new { MovieId = _output.Data[0].Id })).Data;
                    _output.Data[0].Tags = (await dataAccess.SelectAsync<MovieTagEntity>(EntityContainers.MovieTags, "Tag, MovieId, Id", new { MovieId = _output.Data[0].Id })).Data;
                    _output.Data[0].FileInfo = (await dataAccess.SelectAsync<FileInfoEntity>(EntityContainers.MovieFileInfo, "MovieId, Id", new { MovieId = _output.Data[0].Id })).Data[0];
                    _output.Data[0].FileInfo.StreamDetails = (await dataAccess.SelectAsync<StreamDetailEntity>(EntityContainers.MovieStreamDetails, "MovieId, Id", new { MovieId = _output.Data[0].Id })).Data[0];
                    _output.Data[0].FileInfo.StreamDetails.Audio = (await dataAccess.SelectAsync<AudioEntity>(EntityContainers.MovieAudioInfo, "Id, MovieId, Codec, Language, Channels", new { MovieId = _output.Data[0].Id })).Data[0];
                    _output.Data[0].FileInfo.StreamDetails.Video = (await dataAccess.SelectAsync<VideoEntity>(EntityContainers.MovieVideoInfo, "Id, MovieId, Codec, Aspect, Width, Height, Is3D", new { MovieId = _output.Data[0].Id })).Data[0];
                    _output.Data[0].FileInfo.StreamDetails.Subtitle = (await dataAccess.SelectAsync<SubtitleEntity>(EntityContainers.MovieSubtitles, "Id, MovieId, Language", new { MovieId = _output.Data[0].Id })).Data[0];
                    _output.Data[0].Actors = (await dataAccess.SelectAsync<MovieActorEntity>(EntityContainers.MovieActors, "Id, MovieId, Name, Role, `Order`, Thumb", new { MovieId = _output.Data[0].Id })).Data;
                    _output.Data[0].Resume = (await dataAccess.SelectAsync<MovieResumeEntity>(EntityContainers.MovieResume, "Id, MovieId, Position, Total", new { MovieId = _output.Data[0].Id })).Data[0];
                });
            }
            dataAccess.CloseTransaction();
            return _output;
        }

        /// <summary>
        /// Saves a movie and its associated data in the storage medium
        /// </summary>
        /// <param name="entity">The movie to be saved</param>
        /// <returns>The result of saving <paramref name="entity"/>, wrapped in a generic API container of type <see cref="ApiResponse{MovieEntity}"/></returns>
        public async Task<ApiResponse<MovieEntity>> InsertAsync(MovieEntity entity)
        {
            dataAccess.OpenTransaction();
            // insert the movie
            ApiResponse<MovieEntity> movie = await dataAccess.InsertAsync(EntityContainers.Movies, entity);
            await Task.Run(async () =>
            {
                // insert the ratings
                foreach (MovieRatingEntity rating in entity.Ratings)
                {
                    rating.MovieId = movie.Data[0].Id;
                    await dataAccess.InsertAsync(EntityContainers.MovieRatings, rating);
                }
                // insert the genre
                foreach (MovieGenreEntity genre in entity.Genre)
                {
                    genre.MovieId = movie.Data[0].Id;
                    await dataAccess.InsertAsync(EntityContainers.MovieGenre, genre);
                }
                // insert the country
                foreach (MovieCountryEntity country in entity.Country)
                {
                    country.MovieId = movie.Data[0].Id;
                    await dataAccess.InsertAsync(EntityContainers.MovieCountry, country);
                }
                // insert the credits
                foreach (MovieCreditEntity credit in entity.Credits)
                {
                    credit.MovieId = movie.Data[0].Id;
                    await dataAccess.InsertAsync(EntityContainers.MovieCredits, credit);
                }
                // insert the director
                foreach (MovieDirectorEntity director in entity.Director)
                {
                    director.MovieId = movie.Data[0].Id;
                    await dataAccess.InsertAsync(EntityContainers.MovieDirectors, director);
                }
                // insert the tags
                foreach (MovieTagEntity tag in entity.Tags)
                {
                    tag.MovieId = movie.Data[0].Id;
                    await dataAccess.InsertAsync(EntityContainers.MovieTags, tag);
                }
                entity.FileInfo.MovieId = movie.Data[0].Id;
                // insert the file info
                await dataAccess.InsertAsync(EntityContainers.MovieFileInfo, entity.FileInfo);
                entity.FileInfo.StreamDetails.MovieId = movie.Data[0].Id;
                // insert the movie stream details data
                await dataAccess.InsertAsync(EntityContainers.MovieStreamDetails, entity.FileInfo.StreamDetails);
                entity.FileInfo.StreamDetails.Audio.MovieId = movie.Data[0].Id;
                // insert the movie audio data
                await dataAccess.InsertAsync(EntityContainers.MovieAudioInfo, entity.FileInfo.StreamDetails.Audio);
                entity.FileInfo.StreamDetails.Video.MovieId = movie.Data[0].Id;
                // insert the movie video data
                await dataAccess.InsertAsync(EntityContainers.MovieVideoInfo, entity.FileInfo.StreamDetails.Video);
                entity.FileInfo.StreamDetails.Subtitle.MovieId = movie.Data[0].Id;
                // insert the movie subtitle data
                await dataAccess.InsertAsync(EntityContainers.MovieSubtitles, entity.FileInfo.StreamDetails.Subtitle);
                // insert the actors
                foreach (MovieActorEntity actor in entity.Actors)
                {
                    actor.MovieId = movie.Data[0].Id;
                    await dataAccess.InsertAsync(EntityContainers.MovieActors, actor);
                }
                entity.Resume.MovieId = movie.Data[0].Id;
                // insert the movie resume data
                await dataAccess.InsertAsync(EntityContainers.MovieResume, entity.Resume);
            });
            dataAccess.CloseTransaction();
            return movie;
        }

        /// <summary>
        /// Updates <paramref name="entity"/> and its additional info in the storage medium
        /// </summary>
        /// <param name="entity">The entity that will be updated</param>
        /// <returns>The result of updating <paramref name="entity"/>, wrapped in a generic API container of type <see cref="ApiResponse"/></returns>
        public async Task<ApiResponse> UpdateAsync(MovieEntity entity)
        {
            dataAccess.OpenTransaction();
            // update the movie
            ApiResponse movie = await dataAccess.UpdateAsync(EntityContainers.Movies,
                "MediaTypeSourceId = '" + entity.MediaTypeSourceId +
                "', MediaTypeId = '" + entity.MediaTypeId +
                "', MovieTitle = '" + entity.MovieTitle +
                "', OriginalTitle = '" + entity.OriginalTitle +
                "', NamedTitle = '" + entity.NamedTitle +
                "', Synopsis = '" + entity.Synopsis +
                "', Tagline = '" + entity.Tagline +
                "', Runtime = '" + entity.Runtime +
                "', MPAA = '" + entity.MPAA +
                "', LastPlayed = '" + entity.LastPlayed +
                "', ImDbId = '" + entity.ImDbId +
                "', TmDbId = '" + entity.TmDbId +
                "', Set = '" + entity.Set +
                "', Premiered = '" + entity.Premiered +
                "', IsEnded = '" + entity.IsEnded +
                "', Studio = '" + entity.Studio +
                "', Trailer = '" + entity.Trailer +
                "', TvShowLink = '" + entity.TvShowLink +
                "', IsWatched = '" + entity.IsWatched +
                "', IsFavorite = '" + entity.IsFavorite + "'",
                "Id", "'" + entity.Id + "'");
            // update the data associated with the movie
            await Task.Run(async () =>
            {
                foreach (MovieRatingEntity rating in entity.Ratings)
                {
                    await dataAccess.UpdateAsync(EntityContainers.MovieRatings, 
                    "Name = '" + rating.Name +
                    "', Max = '" + rating.Max +
                    "', Value = '" + rating.Value +
                    "', Votes = '" + rating.Votes + "'",
                    "MovieId", "'" + entity.Id + "'");
                }
                foreach (MovieGenreEntity genre in entity.Genre)
                    await dataAccess.UpdateAsync(EntityContainers.MovieGenre, "Genre = '" + genre.Genre + "'", "MovieId", "'" + entity.Id + "'");
                foreach (MovieCountryEntity country in entity.Country)
                    await dataAccess.UpdateAsync(EntityContainers.MovieCountry, "Country = '" + country.Country + "'", "MovieId", "'" + entity.Id + "'");
                foreach (MovieCreditEntity credit in entity.Credits)
                    await dataAccess.UpdateAsync(EntityContainers.MovieCredits, "Credit = '" + credit.Credit + "'", "MovieId", "'" + entity.Id + "'");
                foreach (MovieDirectorEntity director in entity.Director)
                    await dataAccess.UpdateAsync(EntityContainers.MovieDirectors, "Director = '" + director.Director + "'", "MovieId", "'" + entity.Id + "'");
                foreach (MovieTagEntity tag in entity.Tags)
                    await dataAccess.UpdateAsync(EntityContainers.MovieDirectors, "Tag = '" + tag.Tag + "'", "MovieId", "'" + entity.Id + "'");
                foreach (MovieActorEntity actor in entity.Actors)
                {
                    await dataAccess.UpdateAsync(EntityContainers.MovieActors,
                    "Name = '" + actor.Name +
                    "', Role = '" + actor.Role +
                    "', Order = '" + actor.Order +
                    "', Thumb = '" + actor.Thumb + "'",
                    "MovieId", "'" + entity.Id + "'");
                }
                // update the movie audio data
                await dataAccess.UpdateAsync(EntityContainers.MovieAudioInfo, 
                    "Codec = '" + entity.FileInfo.StreamDetails.Audio.Codec +
                    "', Language = '" + entity.FileInfo.StreamDetails.Audio.Language +
                    "', Channels = '" + entity.FileInfo.StreamDetails.Audio.Channels + "'", "MovieId", "'" + entity.Id + "'");
                // update the movie video data
                await dataAccess.UpdateAsync(EntityContainers.MovieVideoInfo,
                   "Codec = '" + entity.FileInfo.StreamDetails.Video.Codec +
                   "', Aspect = '" + entity.FileInfo.StreamDetails.Video.Aspect +
                   "', Aspect = '" + entity.FileInfo.StreamDetails.Video.Aspect +
                   "', Width = '" + entity.FileInfo.StreamDetails.Video.Width +
                   "', Height = '" + entity.FileInfo.StreamDetails.Video.Height +
                   "', Is3D = '" + entity.FileInfo.StreamDetails.Video.Is3D + "'", "MovieId", "'" + entity.Id + "'");
                // update the movie subtitle data
                await dataAccess.UpdateAsync(EntityContainers.MovieSubtitles,
                   "Language = '" + entity.FileInfo.StreamDetails.Subtitle.Language + "'", "MovieId", "'" + entity.Id + "'");
                // update the movie resume data
                await dataAccess.UpdateAsync(EntityContainers.MovieResume,
                    "Position = '" + entity.Resume.Position + "', Total = '" + entity.Resume.Total + "'", "MovieId", "'" + entity.Id + "'");
            });
            dataAccess.CloseTransaction();
            return movie;
        }

        /// <summary>
        /// Updates the IsWatched status of a movie identified by <paramref name="movieId"/> in the storage medium
        /// </summary>
        /// <param name="movieId">The id of the movie whose status will be updated</param>
        /// <param name="isWatched">The IsWatched status to be set</param>
        /// <returns>The result of updating the IsWatched status, wrapped in a generic API container of type <see cref="ApiResponse"/></returns>
        public async Task<ApiResponse> UpdateIsWatchedStatusAsync(int movieId, bool? isWatched)
        {
            return await dataAccess.UpdateAsync(EntityContainers.Movies, "IsWatched = '" + (isWatched != null ? isWatched.ToString() : "Null") + "'", "Id", "'" + movieId + "'");
        }

        /// <summary>
        /// Updates the IsFavorite status of a movie identified by <paramref name="movieId"/> in the storage medium
        /// </summary>
        /// <param name="movieId">The id of the movie whose status will be updated</param>
        /// <param name="isFavorite">The IsFavorite status to be set</param>
        /// <returns>The result of updating the IsFavorite status, wrapped in a generic API container of type <see cref="ApiResponse"/></returns>
        public async Task<ApiResponse> UpdateIsFavoriteStatusAsync(int movieId, bool isFavorite)
        {
            return await dataAccess.UpdateAsync(EntityContainers.Movies, "IsFavorite = '" + isFavorite + "'", "Id", "'" + movieId + "'");
        }
        #endregion
    }
}
