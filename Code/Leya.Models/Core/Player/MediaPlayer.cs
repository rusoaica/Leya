/// Written by: Yulia Danilova
/// Creation Date: 17th of July, 2021
/// Purpose: Business model for media library video player
#region ========================================================================= USING =====================================================================================
using System;
using System.Linq;
using System.Diagnostics;
using System.Threading.Tasks;
using Leya.Models.Core.MediaLibrary;
using Leya.Models.Common.Models.Media;
using Leya.Models.Common.Models.Movies;
using Leya.Models.Common.Models.TvShows;
using Leya.Infrastructure.Configuration;
using Leya.Models.Common.Models.Artists;
#endregion

namespace Leya.Models.Core.Player
{
    public class MediaPlayer : IMediaPlayer
    {
        #region ============================================================== FIELD MEMBERS ================================================================================
        private readonly IAppConfig appConfig;
        #endregion

        #region ================================================================ PROPERTIES =================================================================================
        private bool isMediaPlaying;
        public bool IsMediaPlaying 
        { 
            get { return isMediaPlaying; } 
        }
        #endregion

        #region ================================================================== CTOR =====================================================================================
        /// <summary>
        /// Overload C-tor
        /// <param name="appConfig">The injected application's configuration to use</param>
        /// </summary>
        public MediaPlayer(IAppConfig appConfig)
        {
            this.appConfig = appConfig;
        }
        #endregion

        #region ================================================================= METHODS ===================================================================================
        /// <summary>
        /// Plays a tv show episode identified by <paramref name="media"/>
        /// </summary>
        /// <param name="mediaLibrary">The media library containing the episode to be played</param>
        /// <param name="media">The episode to be played</param>
        public async Task PlayEpisodeAsync(IMediaLibrary mediaLibrary, IMediaEntity media)
        {
            // reconstruct the path of the episode to be played
            EpisodeEntity episode = mediaLibrary.Library.TvShows.SelectMany(t => t.Seasons)
                                                                .SelectMany(s => s.Episodes)
                                                                .Where(e => e.Id == media.EpisodeOrSongId)
                                                                .First();
            SeasonEntity season = mediaLibrary.Library.TvShows.SelectMany(t => t.Seasons)
                                                              .Where(s => s.Id == media.SeasonOrAlbumId)
                                                              .First();
            TvShowEntity tvShow = mediaLibrary.Library.TvShows.Where(t => t.Id == season.TvShowId)
                                                              .First();
            MediaTypeSourceEntity mediaSource = mediaLibrary.Library.MediaTypes.SelectMany(mt => mt.MediaTypeSources)
                                                                               .Where(mts => mts.Id == tvShow.MediaTypeSourceId)
                                                                               .First();
            string path = mediaSource.MediaSourcePath + @"\" + season.SeasonName + @"\" + episode.NamedTitle;
            await PlayVideoItemAsync(path, mediaLibrary, media);
        }

        /// <summary>
        /// Plays a movie identified by <paramref name="media"/>
        /// </summary>
        /// <param name="mediaLibrary">The media library containing the movie to be played</param>
        /// <param name="media">The movie to be played</param>
        public async Task PlayMovieAsync(IMediaLibrary mediaLibrary, IMediaEntity media)
        {
            // reconstruct the path of the movie to be played
            MovieEntity movie = mediaLibrary.Library.Movies.Where(m => m.Id == media.Id)
                                                           .First();
            MediaTypeSourceEntity mediaSource = mediaLibrary.Library.MediaTypes.SelectMany(mt => mt.MediaTypeSources)
                                                                               .Where(mts => mts.Id == movie.MediaTypeSourceId)
                                                                               .First();
            string path = mediaSource.MediaSourcePath + @"\" + movie.NamedTitle;
            await PlayVideoItemAsync(path, mediaLibrary, media);
        }

        /// <summary>
        /// Plays a song identified by <paramref name="media"/>
        /// </summary>
        /// <param name="mediaLibrary">The media library containing the song to be played</param>
        /// <param name="media">The song to be played</param>
        public async Task PlaySongAsync(IMediaLibrary mediaLibrary, IMediaEntity media)
        {
            // reconstruct the path of the song to be played
            SongEntity song = mediaLibrary.Library.Artists.SelectMany(a => a.Albums)
                                                          .SelectMany(a => a.Songs)
                                                          .Where(s => s.Id == media.EpisodeOrSongId)
                                                          .First();
            AlbumEntity album = mediaLibrary.Library.Artists.SelectMany(a => a.Albums)
                                                            .Where(a => a.Id == media.SeasonOrAlbumId)
                                                            .First();
            ArtistEntity artist = mediaLibrary.Library.Artists.Where(a => a.Id == album.ArtistId)
                                                              .First();
            MediaTypeSourceEntity mediaSource = mediaLibrary.Library.MediaTypes.SelectMany(mt => mt.MediaTypeSources)
                                                                               .Where(mts => mts.Id == artist.MediaTypeSourceId)
                                                                               .First();
            string path = mediaSource.MediaSourcePath + @"\" + album.Title + @"\" + song.NamedTitle;
            await PlayVideoItemAsync(path, mediaLibrary, media);
        }

        /// <summary>
        /// Plays a video item
        /// </summary>
        /// <param name="path">The path on the disk for the video file</param>
        /// <param name="media">An optional media view list item that initiated the video playback</param>
#pragma warning disable CS1998
        private async Task PlayVideoItemAsync(string path, IMediaLibrary mediaLibrary, IMediaEntity media)
#pragma warning restore CS1998
        {
            if (!string.IsNullOrEmpty(appConfig.Settings.PlayerPath))
            {
                // start the media player and play the media
                Process mediaPlayerProcess = new Process();
                mediaPlayerProcess.EnableRaisingEvents = true;
                mediaPlayerProcess.StartInfo.FileName = appConfig.Settings.PlayerPath;
                mediaPlayerProcess.StartInfo.Arguments = appConfig.Settings.PlayerArguments + " " + "\"" + path + "\"";
                //mediaPlayerProcess.StartInfo.RedirectStandardOutput = true;
                //mediaPlayerProcess.BeginOutputReadLine();
                //mediaPlayerProcess.OutputDataReceived += (sender, e) =>
                //{

                //};
                mediaPlayerProcess.Exited += async (sender, e) =>
                {
                    // mark episode as watched when playback ends
                    media.IsWatched = true;
                    await mediaLibrary.MediaState.SetEpisodeIsWatchedStatusAsync(mediaLibrary, media);
                    isMediaPlaying = false;
                };
                mediaPlayerProcess.Start();
                isMediaPlaying = true;
            }
            else
                throw new InvalidOperationException("No media player has been configured in Player Settings!");
        }
        #endregion
    }
}
