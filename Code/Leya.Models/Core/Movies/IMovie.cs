/// Written by: Yulia Danilova
/// Creation Date: 07th of July, 2021
/// Purpose: Interface business model for movies
#region ========================================================================= USING =====================================================================================
using System.Threading.Tasks;
using Leya.Models.Common.Models.Movies;
#endregion

namespace Leya.Models.Core.Movies
{
    public interface IMovie
    {
        #region ================================================================ PROPERTIES =================================================================================
        MovieEntity[] Movies { get; set; }
        #endregion

        #region ================================================================= METHODS ===================================================================================
        /// <summary>
        /// Gets the tv shows from the storage medium
        /// </summary>
        Task GetMoviesAsync();

        /// <summary>
        /// Updates the IsWatched status of a movie identified by <paramref name="movieId"/> in the storage medium
        /// </summary>
        /// <param name="movieId">The id of the movie whose status will be updated</param>
        /// <param name="isWatched">The IsWatched status to be set</param>
        Task UpdateIsWatchedStatusAsync(int movieId, bool isWatched);

        /// <summary>
        /// Updates the IsFavorite status of a movie identified by <paramref name="movieId"/> in the storage medium
        /// </summary>
        /// <param name="movieId">The id of the movie whose status will be updated</param>
        /// <param name="isFavorite">The IsFavorite status to be set</param>
        Task UpdateIsFavoriteStatusAsync(int movieId, bool isFavorite);
        #endregion
    }
}
