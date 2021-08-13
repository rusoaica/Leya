/// Written by: Yulia Danilova
/// Creation Date: 17th of July, 2021
/// Purpose: Interface business model for media library video player
#region ========================================================================= USING =====================================================================================
using System.Threading.Tasks;
using Leya.Models.Core.MediaLibrary;
using Leya.Models.Common.Models.Media;
#endregion

namespace Leya.Models.Core.Player
{
    public interface IMediaPlayer
    {
        #region ================================================================= METHODS ===================================================================================
        /// <summary>
        /// Plays a tv show episode identified by <paramref name="media"/>
        /// </summary>
        /// <param name="mediaLibrary">The media library containing the episode to be played</param>
        /// <param name="media">The episode to be played</param>
        Task PlayEpisodeAsync(IMediaLibrary mediaLibrary, IMediaEntity media);

        /// <summary>
        /// Plays a movie identified by <paramref name="media"/>
        /// </summary>
        /// <param name="mediaLibrary">The media library containing the movie to be played</param>
        /// <param name="media">The movie to be played</param>
        Task PlayMovieAsync(IMediaLibrary mediaLibrary, IMediaEntity media);

        /// <summary>
        /// Plays a song identified by <paramref name="media"/>
        /// </summary>
        /// <param name="mediaLibrary">The media library containing the song to be played</param>
        /// <param name="media">The song to be played</param>
        Task PlaySongAsync(IMediaLibrary mediaLibrary, IMediaEntity media);
        #endregion
    }
}
