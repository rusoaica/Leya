/// Written by: Yulia Danilova
/// Creation Date: 18th of July, 2021
/// Purpose: Interface business model for media library common states
#region ========================================================================= USING =====================================================================================
using System.Threading.Tasks;
using Leya.Models.Common.Models.Media;
#endregion

namespace Leya.Models.Core.MediaLibrary
{
    public interface IMediaState
    {
        #region ================================================================= METHODS ===================================================================================
        /// <summary>
        /// Updates the IsWatched status for <paramref name="media"/>
        /// </summary>
        /// <param name="mediaLibrary">The media library containing the episode whose IsWatched status is updated</param>
        /// <param name="media">The media item for which to update the IsWatched status</param>
        Task SetEpisodeIsWatchedStatusAsync(IMediaLibrary mediaLibrary, IMediaEntity media);

        /// <summary>
        /// Updates the IsWatched status for <paramref name="media"/>
        /// </summary>
        /// <param name="mediaLibrary">The media library containing the season whose IsWatched status is updated</param>
        /// <param name="media">The media item for which to update the IsWatched status</param>
        Task SetSeasonIsWatchedStatusAsync(IMediaLibrary mediaLibrary, IMediaEntity media);

        /// <summary>
        /// Updates the IsWatched status for <paramref name="media"/>
        /// </summary>
        /// <param name="mediaLibrary">The media library containing the tv show whose IsWatched status is updated</param>
        /// <param name="media">The media item for which to update the IsWatched status</param>
        Task SetTvShowIsWatchedStatusAsync(IMediaLibrary mediaLibrary, IMediaEntity media);

        /// <summary>
        /// Updates the IsWatched status for <paramref name="media"/>
        /// </summary>
        /// <param name="mediaLibrary">The media library containing the movie whose IsWatched status is updated</param>
        /// <param name="media">The media item for which to update the IsWatched status</param>
        Task SetMovieIsWatchedStatusAsync(IMediaLibrary mediaLibrary, IMediaEntity media);

        /// <summary>
        /// Updates the IsListened status for <paramref name="media"/>
        /// </summary>
        /// <param name="mediaLibrary">The media library containing the song whose IsListened status is updated</param>
        /// <param name="media">The media item for which to update the IsListened status</param>
        Task SetSongIsListenedStatusAsync(IMediaLibrary mediaLibrary, IMediaEntity media);

        /// <summary>
        /// Updates the IsListened status for <paramref name="media"/>
        /// </summary>
        /// <param name="mediaLibrary">The media library containing the album whose IsListened status is updated</param>
        /// <param name="media">The media item for which to update the IsListened status</param>
        Task SetAlbumIsListenedStatusAsync(IMediaLibrary mediaLibrary, IMediaEntity media);

        /// <summary>
        /// Updates the IsListened status for <paramref name="media"/>
        /// </summary>
        /// <param name="mediaLibrary">The media library containing the artist whose IsListened status is updated</param>
        /// <param name="media">The media item for which to update the IsListened status</param>
        Task SetArtistIsListenedStatusAsync(IMediaLibrary mediaLibrary, IMediaEntity media);

        /// <summary>
        /// Updates the IsFavorite status for <paramref name="media"/>
        /// </summary>
        /// <param name="mediaLibrary">The media library containing the episode whose IsFavorite status is updated</param>
        /// <param name="media">The media item for which to update the IsFavorite status</param>
        Task SetEpisodeIsFavoriteStatusAsync(IMediaLibrary mediaLibrary, IMediaEntity media);

        /// <summary>
        /// Updates the IsFavorite status for <paramref name="media"/>
        /// </summary>
        /// <param name="mediaLibrary">The media library containing the season whose IsFavorite status is updated</param>
        /// <param name="media">The media item for which to update the IsFavorite status</param>
        Task SetSeasonIsFavoriteStatusAsync(IMediaLibrary mediaLibrary, IMediaEntity media);

        /// <summary>
        /// Updates the IsFavorite status for <paramref name="media"/>
        /// </summary>
        /// <param name="mediaLibrary">The media library containing the tv show whose IsFavorite status is updated</param>
        /// <param name="media">The media item for which to update the IsFavorite status</param>
        Task SetTvShowIsFavoriteStatusAsync(IMediaLibrary mediaLibrary, IMediaEntity media);

        /// <summary>
        /// Updates the IsFavorite status for <paramref name="media"/>
        /// </summary>
        /// <param name="mediaLibrary">The media library containing the movie whose IsFavorite status is updated</param>
        /// <param name="media">The media item for which to update the IsFavorite status</param>
        Task SetMovieIsFavoriteStatusAsync(IMediaLibrary mediaLibrary, IMediaEntity media);

        /// <summary>
        /// Updates the IsFavorite status for <paramref name="media"/>
        /// </summary>
        /// <param name="mediaLibrary">The media library containing the song whose IsFavorite status is updated</param>
        /// <param name="media">The media item for which to update the IsFavorite status</param>
        Task SetSongIsFavoriteStatusAsync(IMediaLibrary mediaLibrary, IMediaEntity media);

        /// <summary>
        /// Updates the IsFavorite status for <paramref name="media"/>
        /// </summary>
        /// <param name="mediaLibrary">The media library containing the album whose IsFavorite status is updated</param>
        /// <param name="media">The media item for which to update the IsFavorite status</param>
        Task SetAlbumIsFavoriteStatusAsync(IMediaLibrary mediaLibrary, IMediaEntity media);

        /// <summary>
        /// Updates the IsFavorite status for <paramref name="media"/>
        /// </summary>
        /// <param name="mediaLibrary">The media library containing the artist whose IsFavorite status is updated</param>
        /// <param name="media">The media item for which to update the IsFavorite status</param>
        Task SetArtistIsFavoriteStatusAsync(IMediaLibrary mediaLibrary, IMediaEntity media);
        #endregion
    }
}
