/// Written by: Yulia Danilova
/// Creation Date: 17th of July, 2021
/// Purpose: Interface business model for media library video player
#region ========================================================================= USING =====================================================================================
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Leya.Models.Core.MediaLibrary;
using Leya.Models.Common.Models.Media;
#endregion

namespace Leya.Models.Core.Player
{
    public interface IMediaPlayer
    {
        #region ============================================================== FIELD MEMBERS ================================================================================
        event Func<bool, Task> PlaybackChanged;
        event Action<string> PropertyChanged;
        #endregion

        #region ================================================================ PROPERTIES =================================================================================
        bool ShufflesPlaylistArgument { get; }
        bool RepeatsPlaylistArgument { get; }
        int MediaLength { get; set; }
        int CurrentPlaybackPosition { get; set; }
        #endregion

        #region ================================================================= METHODS ===================================================================================
        /// <summary>
        /// Plays a tv show episode identified by <paramref name="media"/>
        /// </summary>
        /// <param name="mediaLibrary">The media library containing the episode to be played</param>
        /// <param name="media">The episode to be played</param>
        Task PlayEpisodeAsync(IMediaLibrary mediaLibrary, IMediaEntity media);

        /// <summary>
        /// Plays all the episodes in <paramref name="playlist"/>
        /// </summary>
        /// <param name="mediaLibrary">The media library containing the episodes to be played</param>
        /// <param name="playlist">A collection of episode paths</param>
        Task PlayEpisodesListAsync(IMediaLibrary mediaLibrary, IEnumerable<string> playlist);
        
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

        /// <summary>
        /// Displays information about the current media
        /// </summary>
        /// <returns>A string representing the information about the current media</returns>
        Task<string> ShowMediaInfoAsync();
        
        /// <summary>
        /// Vlc command for getting information about the current stream 
        /// </summary>
        /// <returns>A string representing information about the current stream</returns>
        Task<string> GetInfoAsync();

        /// <summary>
        /// Togles the media playback
        /// </summary>
        Task TogglePlayAsync();

        /// <summary>
        /// Stops media playback
        /// </summary>
        Task StopPlaybackAsync();

        /// <summary>
        /// Navigates to the previous media chapter
        /// </summary>
        Task GoToPreviousMediaChapterAsync();
        
        /// <summary>
        /// Navigates to the next media chapter
        /// </summary>
        Task GoToNextMediaChapterAsync();

        /// <summary>
        /// Sets the current playcback position
        /// </summary>
        Task SetPlaybackPositionAsync();
        
        /// <summary>
        /// Quits the media player
        /// </summary>
        Task QuitMediaPlayerAsync();
        #endregion
    }
}
