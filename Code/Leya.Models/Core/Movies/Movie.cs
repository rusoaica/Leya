/// Written by: Yulia Danilova
/// Creation Date: 07th of July, 2021
/// Purpose: Business model for movies
#region ========================================================================= USING =====================================================================================
using System;
using Leya.DataAccess;
using System.Threading.Tasks;
using Leya.Models.Common.Models.Movies;
using Leya.Models.Common.Infrastructure;
using Leya.DataAccess.Repositories.Movies;
#endregion

namespace Leya.Models.Core.Movies
{
    public class Movie : IMovie
    {
        #region ============================================================== FIELD MEMBERS ================================================================================
        private readonly IMovieRepository movieRepository;
        #endregion

        #region ================================================================ PROPERTIES =================================================================================
        public MovieEntity[] Movies { get; set; }
        #endregion

        #region ================================================================== CTOR =====================================================================================
        /// <summary>
        /// Overload C-tor
        /// <param name="unitOfWork">Injected unit of work for interacting with the data access layer repositories</param>
        /// </summary>
        public Movie(IUnitOfWork unitOfWork)
        {
            movieRepository = unitOfWork.GetRepository<IMovieRepository>();
        }
        #endregion

        #region ================================================================= METHODS ===================================================================================
        /// <summary>
        /// Gets the movies from the storage medium
        /// </summary>
        public async Task GetMoviesAsync()
        {
            var result = await movieRepository.GetAllAsync();
            if (string.IsNullOrEmpty(result.Error))
                Movies = Services.AutoMapper.Map<MovieEntity[]>(result.Data);
            else
                throw new InvalidOperationException("Error getting the movies from the repository: " + result.Error);
        }

        /// <summary>
        /// Updates the IsWatched status of a movie identified by <paramref name="movieId"/> in the storage medium
        /// </summary>
        /// <param name="movieId">The id of the movie whose status will be updated</param>
        /// <param name="isWatched">The IsWatched status to be set</param>
        public async Task UpdateIsWatchedStatusAsync(int movieId, bool isWatched)
        {
            var result = await movieRepository.UpdateIsWatchedStatusAsync(movieId, isWatched);
            if (!string.IsNullOrEmpty(result.Error))
                throw new InvalidOperationException("Error updating the IsWatched status of the movie: " + result.Error);
        }

        /// <summary>
        /// Updates the IsFavorite status of a movie identified by <paramref name="movieId"/> in the storage medium
        /// </summary>
        /// <param name="movieId">The id of the movie whose status will be updated</param>
        /// <param name="isFavorite">The IsFavorite status to be set</param>
        public async Task UpdateIsFavoriteStatusAsync(int movieId, bool isFavorite)
        {
            var result = await movieRepository.UpdateIsFavoriteStatusAsync(movieId, isFavorite);
            if (!string.IsNullOrEmpty(result.Error))
                throw new InvalidOperationException("Error updating the IsFavorite status of the movie: " + result.Error);
        }
        #endregion
    }
}
