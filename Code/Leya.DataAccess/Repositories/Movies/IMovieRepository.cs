/// Written by: Yulia Danilova
/// Creation Date: 02nd of December, 2020
/// Purpose: Movie repository interface for the bridge-through between the generic storage medium and storage medium for Movies
#region ========================================================================= USING =====================================================================================
using System.Threading.Tasks;
using Leya.DataAccess.Common.Models;
using Leya.DataAccess.Repositories.Common;
using Leya.DataAccess.Common.Models.Movies;
#endregion

namespace Leya.DataAccess.Repositories.Movies
{
    public interface IMovieRepository : IRepository<MovieEntity>
    {
        #region ================================================================= METHODS ===================================================================================
        /// <summary>
        /// Updates the IsWatched status of a movie identified by <paramref name="movieId"/> in the storage medium
        /// </summary>
        /// <param name="movieId">The id of the movie whose status will be updated</param>
        /// <param name="isWatched">The IsWatched status to be set</param>
        /// <returns>The result of updating the IsWatched status, wrapped in a generic API container of type <see cref="ApiResponse"/></returns>
        Task<ApiResponse> UpdateIsWatchedStatusAsync(int movieId, bool isWatched);

        /// <summary>
        /// Updates the IsFavorite status of a movie identified by <paramref name="movieId"/> in the storage medium
        /// </summary>
        /// <param name="movieId">The id of the movie whose status will be updated</param>
        /// <param name="isFavorite">The IsFavorite status to be set</param>
        /// <returns>The result of updating the IsFavorite status, wrapped in a generic API container of type <see cref="ApiResponse"/></returns>
        Task<ApiResponse> UpdateIsFavoriteStatusAsync(int movieId, bool isFavorite);
        #endregion
    }
}
