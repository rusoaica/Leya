/// Written by: Yulia Danilova
/// Creation Date: 17th of July, 2021
/// Purpose: Business model for media library video player

#region ========================================================================= USING =====================================================================================

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Leya.Models.Core.MediaLibrary;
using Leya.Models.Common.Models.Media;
using Leya.Models.Common.Models.Movies;
using Leya.Models.Common.Models.TvShows;
using Leya.Infrastructure.Configuration;
using Leya.Infrastructure.Enums;
using Leya.Models.Common.Broadcasting;
using Leya.Models.Common.Models.Artists;
using Timer = System.Timers.Timer;
#endregion

namespace Leya.Models.Core.Player
{
    public class MediaPlayer : NotifyPropertyChanged, IMediaPlayer
    {
        #region ============================================================== FIELD MEMBERS ================================================================================

        public event Func<bool, Task> PlaybackChanged;

        private TcpClient mediaPlayerTcpClient;
        
        //private Socket mediaPlayerSocket;
        private Process mediaPlayerProcess;

        private readonly IAppConfig appConfig;
        private IntPtr vlcProcessHandle = IntPtr.Zero;
        private Timer PlaybackTimer = new Timer(1000); 
        #endregion

        #region ================================================================ PROPERTIES =================================================================================

        private bool isMediaPlaying;
        public bool IsMediaPlaying
        {
            get { return isMediaPlaying; }
        }

        public bool ShufflesPlaylistArgument { get { return appConfig.Settings.ShufflesPlaylistArgument; } }
        public bool RepeatsPlaylistArgument { get { return appConfig.Settings.RepeatsPlaylistArgument; } }
        
        private int mediaLength;
        public int MediaLength
        {
            get { return mediaLength; }
            set { mediaLength = value; Notify(); }
        }

        private int currentPlaybackPosition;
        public int CurrentPlaybackPosition
        {
            get { return currentPlaybackPosition; }
            set { currentPlaybackPosition = value; Notify(); }
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
            PlaybackTimer.Elapsed += PlaybackTimerOnElapsed;
        }

        /// <summary>
        /// Destructor
        /// </summary>
        ~MediaPlayer()
        {
            // shut down the remote controlled vlc interface connection, then kill the vlc process itself
            QuitMediaPlayerAsync().Wait();
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
                .First(e => e.Id == media.EpisodeOrSongId);
            SeasonEntity season = mediaLibrary.Library.TvShows.SelectMany(t => t.Seasons)
                .First(s => s.Id == media.SeasonOrAlbumId);
            TvShowEntity tvShow = mediaLibrary.Library.TvShows.First(t => t.Id == season.TvShowId);
            MediaTypeSourceEntity mediaSource = mediaLibrary.Library.MediaTypes.SelectMany(mt => mt.MediaTypeSources)
                .First(mts => mts.Id == tvShow.MediaTypeSourceId);
            string path = mediaSource.MediaSourcePath + Path.DirectorySeparatorChar + season.Title + Path.DirectorySeparatorChar + episode.NamedTitle;
            await PlayVideoItemAsync(path, mediaLibrary, media);
        }

        /// <summary>
        /// Plays all the episodes in <paramref name="playlist"/>
        /// </summary>
        /// <param name="mediaLibrary">The media library containing the episodes to be played</param>
        /// <param name="playlist">A collection of episode paths</param>
        public async Task PlayEpisodesListAsync(IMediaLibrary mediaLibrary, IEnumerable<string> playlist)
        {
            if (playlist == null)
                throw new ArgumentNullException("The playlist cannot be null!");
            if (playlist == null || playlist.Count() == 0)
                throw new ArgumentException("The playlist cannot be empty!");
            //await PlayVideoItemAsync(playlist.First(), mediaLibrary);
        }

        /// <summary>
        /// Plays a movie identified by <paramref name="media"/>
        /// </summary>
        /// <param name="mediaLibrary">The media library containing the movie to be played</param>
        /// <param name="media">The movie to be played</param>
        public async Task PlayMovieAsync(IMediaLibrary mediaLibrary, IMediaEntity media)
        {
            // reconstruct the path of the movie to be played
            MovieEntity movie = mediaLibrary.Library.Movies.First(m => m.Id == media.Id);
            MediaTypeSourceEntity mediaSource = mediaLibrary.Library.MediaTypes.SelectMany(mt => mt.MediaTypeSources)
                .First(mts => mts.Id == movie.MediaTypeSourceId);
            string path = mediaSource.MediaSourcePath + Path.DirectorySeparatorChar + movie.NamedTitle;
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
                .First(s => s.Id == media.EpisodeOrSongId);
            AlbumEntity album = mediaLibrary.Library.Artists.SelectMany(a => a.Albums)
                .First(a => a.Id == media.SeasonOrAlbumId);
            ArtistEntity artist = mediaLibrary.Library.Artists.First(a => a.Id == album.ArtistId);
            MediaTypeSourceEntity mediaSource = mediaLibrary.Library.MediaTypes.SelectMany(mt => mt.MediaTypeSources)
                .First(mts => mts.Id == artist.MediaTypeSourceId);
            string path = mediaSource.MediaSourcePath + Path.DirectorySeparatorChar + album.Title + Path.DirectorySeparatorChar + song.NamedTitle;
            await PlayVideoItemAsync(path, mediaLibrary, media);
        }

        /// <summary>
        /// Plays a video item
        /// </summary>
        /// <param name="path">The path on the disk for the video file</param>
        /// <param name="mediaLibrary">The media library containing the video item to be played</param>
        /// <param name="media">An optional media view list item that initiated the video playback</param>
        private async Task PlayVideoItemAsync(string path, IMediaLibrary mediaLibrary, IMediaEntity media)
        {
            if (!string.IsNullOrEmpty(appConfig.Settings.PlayerPath))
            {
                if (vlcProcessHandle == IntPtr.Zero)
                {
                    await RunVlcProcessAsync();
                    // hacky, just waits for the process to be fully initialized before attempting to connect to it..
                    await Task.Delay(500);
                    await ConnectToVlcProcessAsync();
                }
                // play the media
                await SendVlcCommandAsync("add", path);
                await Task.Delay(500);
                // get the duration of the media element
                await SendVlcCommandAsync("get_length");
                string duration = await ReceiveVlcOutputAsync();
                MediaLength = (int)double.Parse(duration.Substring(0, duration.IndexOf("\r\n")));
                isMediaPlaying = true;
                PlaybackTimer.Start();
            }
            else
                throw new InvalidOperationException("No media player has been configured in Player Settings!");
        }

        /// <summary>
        /// Vlc command for getting information about the current stream 
        /// </summary>
        /// <returns>A string representing information about the current stream</returns>
        public async Task<string> GetInfoAsync()
        {
            await SendVlcCommandAsync("info");
            return await ReceiveVlcOutputAsync();
        }

        /// <summary>
        /// Stops media playback
        /// </summary>
        public async Task StopPlaybackAsync()
        {
            await SendVlcCommandAsync("stop");
            isMediaPlaying = false;
            PlaybackChanged?.Invoke(false);
        }

        /// <summary>
        /// Displays information about the current media
        /// </summary>
        /// <returns>A string representing the information about the current media</returns>
        public async Task<string> ShowMediaInfoAsync()
        {
            await SendVlcCommandAsync("status");
            return await ReceiveVlcOutputAsync();
        }
        
        /// <summary>
        /// Togles the media playback
        /// </summary>
        public async Task TogglePlayAsync()
        {
            await SendVlcCommandAsync("status");
            isMediaPlaying = (await ReceiveVlcOutputAsync()).Contains("state playing");
            if (isMediaPlaying)
                await SendVlcCommandAsync("pause");
            else
                await SendVlcCommandAsync("play");
        }

        /// <summary>
        /// Navigates to the previous media chapter
        /// </summary>
        public async Task GoToPreviousMediaChapterAsync()
        {
            await SendVlcCommandAsync("chapter_p");
        }
        
        /// <summary>
        /// Navigates to the next media chapter
        /// </summary>
        public async Task GoToNextMediaChapterAsync()
        {
            await SendVlcCommandAsync("chapter_n");
        }

        /// <summary>
        /// Sets the current playcback position
        /// </summary>
        public async Task SetPlaybackPositionAsync()
        {
            await SendVlcCommandAsync("seek", currentPlaybackPosition.ToString());
        }
        
        /// <summary>
        /// Quits the media player
        /// </summary>
        public async Task QuitMediaPlayerAsync()
        {
            await ShutDownVlcRemoteControlAsync();
            await ShutDownVlcProcessAsync();
        }
        
        /// <summary>
        /// Sends a command on the tcp client connected to the remote controlled Vlc interface
        /// </summary>
        /// <param name="command">The command to be sent</param>
        /// <param name="param">Optional parameters to send along with the command</param>
        private async Task SendVlcCommandAsync(string command, string param = null)
        {
            // flush old stuff
            await ReceiveVlcOutputAsync(); 
            // add any optional parameters sent
            if (param != null) 
                command += " " + param;
            byte[] message = Encoding.ASCII.GetBytes(command + Environment.NewLine);
            await mediaPlayerTcpClient.GetStream().WriteAsync(message,0, message.Length);
            mediaPlayerTcpClient.GetStream().FlushAsync();
        }

        /// <summary>
        /// Receives output from the tcp client connected to the remote controlled Vlc interface
        /// </summary>
        /// <returns>A string representing the vlc output, if any</returns>
        private async Task<string> ReceiveVlcOutputAsync()
        {
            DateTime now = DateTime.Now;
            StringBuilder sb = new StringBuilder();
            bool hasData = false;
            while (DateTime.Now < now.AddSeconds(2))
            {
                // re-create the string output from the bytes received, if available
                while (mediaPlayerTcpClient.Available > 0)
                {
                    byte[] message = new byte[1024];
                    int streamByte = await mediaPlayerTcpClient.GetStream().ReadAsync(message, 0, 1024);
                    foreach (byte b in message)
                        sb.Append((char)b);
                    hasData = true;
                    // hacky: assumes only 1024 bytes of info from VLC, always (could there be more?)
                    break;
                }
                if (hasData)
                    break;
            }
            if (sb.Length > 0)
                return sb.ToString().Trim();
            return null;
        }
        
        /// <summary>
        /// Starts the vlc process as a remote controlled interface
        /// </summary>
        private async Task RunVlcProcessAsync()
        {
            // do not spawn a new vlc process if there is already one started by the application!
            if (vlcProcessHandle != IntPtr.Zero) 
                return;
            IPAddress ipAddress = GetLocalIpAddress();
            mediaPlayerProcess = new Process();
            mediaPlayerProcess.EnableRaisingEvents = true;
            // the vlc executable's path should be assigned in the application's configuration
            mediaPlayerProcess.StartInfo.FileName = appConfig.Settings.PlayerPath;
            // start the vlc process with a remote controlled interface
            mediaPlayerProcess.StartInfo.Arguments = " --intf rc --rc-host=" + ipAddress + ":10001 --rc-fake-tty --no-video-deco --no-embedded-video " + appConfig.Settings.PlayerArguments; 
            mediaPlayerProcess.StartInfo.RedirectStandardInput = true;
            mediaPlayerProcess.StartInfo.RedirectStandardError = true;
            mediaPlayerProcess.StartInfo.RedirectStandardOutput = true;
            mediaPlayerProcess.Start();
            mediaPlayerProcess.BeginOutputReadLine();
            mediaPlayerProcess.BeginErrorReadLine();
            vlcProcessHandle = mediaPlayerProcess.Handle;
            mediaPlayerProcess.OutputDataReceived += new DataReceivedEventHandler((s, e) => { Trace.WriteLine(e.Data); });
            await PlaybackChanged?.Invoke(true);
            // mediaPlayerProcess.Exited += async (s, e) => { };
            mediaPlayerProcess.ErrorDataReceived += new DataReceivedEventHandler(async (s, e) =>
            {
                // check if the user closes the vlc output, in which case the vlc process is "zombified" (still exists, but in an errored state)
                if (e.Data != null && e.Data.Contains("X server failure"))
                    await QuitMediaPlayerAsync();
            });
        }

        /// <summary>
        /// Connects a socket to the address and port where the remote controlled vlc process listens for commands
        /// </summary>
        private async Task ConnectToVlcProcessAsync()
        {
            // if the socket is already connected, shut it down first (could still be connected to a zombified vlc process!)
            if (mediaPlayerTcpClient != null && mediaPlayerTcpClient.Connected)
                await ShutDownVlcRemoteControlAsync();
            IPAddress ipAddress = GetLocalIpAddress();
            mediaPlayerTcpClient = new TcpClient(ipAddress.ToString(), 10001);
        }
        
        /// <summary>
        /// Closes the remote controlled vlc interface and the port connected to it
        /// </summary>
        private async Task ShutDownVlcRemoteControlAsync()
        {
            // release the socket connection
            await SendVlcCommandAsync("shutdown");
            mediaPlayerTcpClient.Close();
        }

        /// <summary>
        /// Closes the vlc process
        /// </summary>
        private async Task ShutDownVlcProcessAsync()
        {
            MediaLength = 0;
            CurrentPlaybackPosition = 0;
            PlaybackTimer.Stop();
            await PlaybackChanged?.Invoke(false);
            // reset current's vlc process handle and kill it (normal close is ignored!)
            vlcProcessHandle = IntPtr.Zero;
            mediaPlayerProcess.CancelErrorRead();
            mediaPlayerProcess.CancelOutputRead();
            mediaPlayerProcess.Kill();
        }
        
        /// <summary>
        /// Gets the first local IPv4 address
        /// </summary>
        /// <returns>An IPv4 address representing the first local address</returns>
        /// <exception cref="Exception">Exception thrown when no IPv4 adapter is found in the system</exception>
        private IPAddress GetLocalIpAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                    return ip;
            throw new  Exception("No network adapters with an IPv4 address in the system!");
        }
        #endregion


        private void PlaybackTimerOnElapsed(object sender, ElapsedEventArgs e)
        {
            if (isMediaPlaying)
                CurrentPlaybackPosition++;
        }
    }
}