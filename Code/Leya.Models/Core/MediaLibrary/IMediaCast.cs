/// Written by: Yulia Danilova
/// Creation Date: 29th of August, 2021
/// Purpose: Interface business model for media library cast
#region ========================================================================= USING =====================================================================================
using System.Collections.Generic;
using Leya.Models.Common.Models.Media;
#endregion

namespace Leya.Models.Core.MediaLibrary
{
    public interface IMediaCast
    {
        #region ================================================================= METHODS ===================================================================================
        /// <summary>
        /// Gets the list of actors of an episode
        /// </summary>
        /// <param name="mediaLibrary">The library from which to take the episode for which to get the list of actors</param>
        /// <param name="media">The navigation media entity containing the id of the episode for which to get the list of actors</param>
        /// <returns>An enumeration of actors of an episode</returns>
        IEnumerable<CastEntity> ShowEpisodeMediaCastAsync(IMediaLibrary mediaLibrary, IMediaEntity media);

        /// <summary>
        /// Gets the list of actors of a tv show episodes
        /// </summary>
        /// <param name="mediaLibrary">The library from which to take the tv show episodes for which to get the list of actors</param>
        /// <param name="media">The navigation media entity containing the id of the tv show for which to get the list of actors</param>
        /// <returns>An enumeration of actors of a tv show</returns>
        IEnumerable<CastEntity> ShowTvShowMediaCastAsync(IMediaLibrary mediaLibrary, IMediaEntity media);

        /// <summary>
        /// Gets the list of actors of a movie
        /// </summary>
        /// <param name="mediaLibrary">The library from which to take the movie for which to get the list of actors</param>
        /// <param name="media">The navigation media entity containing the id of the movie for which to get the list of actors</param>
        /// <returns>An enumeration of actors of a movie</returns>
        IEnumerable<CastEntity> ShowMovieMediaCastAsync(IMediaLibrary mediaLibrary, IMediaEntity media);
        #endregion
    }
}
