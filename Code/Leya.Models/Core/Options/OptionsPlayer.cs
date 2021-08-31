/// Written by: Yulia Danilova
/// Creation Date: 26th of November, 2020
/// Purpose: Business model for player options
#region ========================================================================= USING =====================================================================================
using System.Threading.Tasks;
using Leya.Models.Common.Broadcasting;
using Leya.Infrastructure.Notification;
using Leya.Infrastructure.Configuration;
#endregion

namespace Leya.Models.Core.Options
{
    public class OptionsPlayer : NotifyPropertyChanged, IOptionsPlayer
    {
        #region ============================================================== FIELD MEMBERS ================================================================================
        private readonly IAppConfig appConfig;
        private readonly INotificationService notificationService;
        #endregion

        #region ================================================================ PROPERTIES =================================================================================
        private string playerPath = string.Empty;
        public string PlayerPath
        {
            get { return playerPath; }
            set { playerPath = value; Notify(); }
        }

        private string playerArguments = string.Empty;
        public string PlayerArguments
        {
            get { return playerArguments; }
            set { playerArguments = value; Notify(); }
        }

        private bool usesFullScreenArgument;
        public bool UsesFullScreenArgument
        {
            get { return usesFullScreenArgument; }
            set { usesFullScreenArgument = value; Notify(); }
        }

        private bool isSingleInstanceArgument;
        public bool IsSingleInstanceArgument
        {
            get { return isSingleInstanceArgument; }
            set { isSingleInstanceArgument = value; Notify(); }
        }

        private bool enquesFilesInSingleInstanceModeArgument;
        public bool EnquesFilesInSingleInstanceModeArgument
        {
            get { return enquesFilesInSingleInstanceModeArgument; }
            set { enquesFilesInSingleInstanceModeArgument = value; Notify(); }
        }

        private bool isAlwaysOnTopArgument;
        public bool IsAlwaysOnTopArgument
        {
            get { return isAlwaysOnTopArgument; }
            set { isAlwaysOnTopArgument = value; Notify(); }
        }

        private bool usesAutoscaleArgument;
        public bool UsesAutoscaleArgument
        {
            get { return usesAutoscaleArgument; }
            set { usesAutoscaleArgument = value; Notify(); }
        }

        private bool resumesFromLastTimeIndexArgument;
        public bool ResumesFromLastTimeIndexArgument
        {
            get { return resumesFromLastTimeIndexArgument; }
            set { resumesFromLastTimeIndexArgument = value; Notify(); }
        }

        private bool repeatsPlaylistArgument;
        public bool RepeatsPlaylistArgument
        {
            get { return repeatsPlaylistArgument; }
            set { repeatsPlaylistArgument = value; Notify(); }
        }

        private bool shufflesPlaylistArgument;
        public bool ShufflesPlaylistArgument
        {
            get { return shufflesPlaylistArgument; }
            set { shufflesPlaylistArgument = value; Notify(); }
        }

        private bool playAndStopArgument;     
        public bool PlayAndStopArgument
        {
            get { return playAndStopArgument; }
            set { playAndStopArgument = value; Notify(); }
        }
        #endregion

        #region ================================================================== CTOR =====================================================================================
        /// <summary>
        /// Overload C-tor
        /// </summary>
        /// <param name="appConfig">The injected application's configuration</param>
        /// <param name="notificationService">The injected service used for displaying notifications</param>
        public OptionsPlayer(IAppConfig appConfig, INotificationService notificationService)
        {
            this.appConfig = appConfig;
            this.notificationService = notificationService;
        }
        #endregion

        #region ================================================================= METHODS ===================================================================================
        /// <summary>
        /// Enables or disables quiting when playback ends for the media player
        /// </summary>
        public void ChangePlayAndStopArgument(bool param)
        {
            if (param)
            {
                PlayerArguments = PlayerArguments.Replace(" --no-play-and-exit ", string.Empty);
                PlayerArguments += " --play-and-exit ";
            }
            else
            {
                PlayerArguments = PlayerArguments.Replace(" --play-and-exit ", string.Empty);
                PlayerArguments += " --no-play-and-exit ";
            }
        }

        /// <summary>
        /// Enables or disables shuffling playlist playback for the media player
        /// </summary>
        public void ChangeShuffleArgument(bool param)
        {
            if (param)
            {
                PlayerArguments = PlayerArguments.Replace(" --no-random ", string.Empty);
                PlayerArguments += " --random ";
            }
            else
            {
                PlayerArguments = PlayerArguments.Replace(" --random ", string.Empty);
                PlayerArguments += " --no-random ";
            }
        }

        /// <summary>
        /// Enables or disables repeating playlist playback for the media player
        /// </summary>
        public void ChangeRepeatArgument(bool param)
        {
            if (param)
            {
                PlayerArguments = PlayerArguments.Replace(" --no-repeat ", string.Empty);
                PlayerArguments += " --repeat ";
            }
            else
            {
                PlayerArguments = PlayerArguments.Replace(" --repeat ", string.Empty);
                PlayerArguments += " --no-repeat ";
            }
        }

        /// <summary>
        /// Enables or disables autoscale argument for the media player
        /// </summary>
        public void ChangeAutoscaleArgument(bool param)
        {
            if (param)
            {
                PlayerArguments = PlayerArguments.Replace(" --no-autoscale ", string.Empty);
                PlayerArguments += " --autoscale ";
            }
            else
            {
                PlayerArguments = PlayerArguments.Replace(" --autoscale ", string.Empty);
                PlayerArguments += " --no-autoscale ";
            }
        }

        /// <summary>
        /// Enables or disables always on top argument for the media player
        /// </summary>
        public void ChangeAlwaysOnTopArgument(bool param)
        {
            if (param)
            {
                PlayerArguments = PlayerArguments.Replace(" --no-video-on-top ", string.Empty);
                PlayerArguments += " --video-on-top ";
            }
            else
            {
                PlayerArguments = PlayerArguments.Replace(" --video-on-top ", string.Empty);
                PlayerArguments += " --no-video-on-top ";
            }
        }

        /// <summary>
        /// Enables or disables enqueue of files into playlist when in single instance mode
        /// </summary>
        public void ChangeEnqueueFilesInSingleInstanceModeArgument(bool param)
        {
            if (param)
            {
                PlayerArguments = PlayerArguments.Replace(" --no-playlist-enqueue ", string.Empty);
                PlayerArguments += " --playlist-enqueue ";
            }
            else
            {
                PlayerArguments = PlayerArguments.Replace(" --playlist-enqueue ", string.Empty);
                PlayerArguments += " --no-playlist-enqueue ";
            }
        }

        /// <summary>
        /// Enables or disables single instance argument for the media player
        /// </summary>
        public void ChangeSingleInstanceArgument(bool param)
        {
            if (param)
            {
                PlayerArguments += " --one-instance ";
                PlayerArguments = PlayerArguments.Replace(" --no-one-instance ", string.Empty);
            }
            else
            {
                PlayerArguments = PlayerArguments.Replace(" --one-instance ", string.Empty);
                PlayerArguments += " --no-one-instance ";
            }
        }

        /// <summary>
        /// Enables or disables fullscreen argument for the media player
        /// </summary>
        public void ChangeFullScreenArgument(bool param)
        {
            if (param)
            {
                PlayerArguments = PlayerArguments.Replace(" --no-fullscreen ", string.Empty);
                PlayerArguments += " --fullscreen ";
            }
            else
            {
                PlayerArguments = PlayerArguments.Replace(" --fullscreen ", string.Empty);
                PlayerArguments += " --no-fullscreen ";
            }
        }

        /// <summary>
        /// Updates the application's configurations for the media player
        /// </summary>
        public async Task UpdatePlayerOptionsAsync()
        {
            appConfig.Settings.PlayerPath = PlayerPath;
            appConfig.Settings.PlayerArguments = PlayerArguments;
            appConfig.Settings.PlayAndStopArgument = PlayAndStopArgument;
            appConfig.Settings.IsAlwaysOnTopArgument = IsAlwaysOnTopArgument;
            appConfig.Settings.UsesAutoscaleArgument = UsesAutoscaleArgument;
            appConfig.Settings.UsesFullScreenArgument = UsesFullScreenArgument;
            appConfig.Settings.RepeatsPlaylistArgument = RepeatsPlaylistArgument;
            appConfig.Settings.ShufflesPlaylistArgument = ShufflesPlaylistArgument;
            appConfig.Settings.IsSingleInstanceArgument = IsSingleInstanceArgument;
            appConfig.Settings.ResumesFromLastTimeIndexArgument = ResumesFromLastTimeIndexArgument;
            appConfig.Settings.EnquesFilesInSingleInstanceModeArgument = EnquesFilesInSingleInstanceModeArgument;
            await appConfig.UpdateConfigurationAsync();
            await notificationService.ShowAsync("Player settings have been updated!", "LEYA - Success");
        }

        /// <summary>
        /// Gets the media player options from the application's configuration
        /// </summary>
        public void GetPlayerOptions()
        {
            if (!string.IsNullOrEmpty(appConfig.Settings.PlayerPath))
            {
                PlayerPath = appConfig.Settings.PlayerPath;
                PlayerArguments = appConfig.Settings.PlayerArguments;
                ShufflesPlaylistArgument = appConfig.Settings.ShufflesPlaylistArgument;
                RepeatsPlaylistArgument = appConfig.Settings.RepeatsPlaylistArgument;
                UsesFullScreenArgument = appConfig.Settings.UsesFullScreenArgument;
                UsesAutoscaleArgument = appConfig.Settings.UsesAutoscaleArgument;
                IsAlwaysOnTopArgument = appConfig.Settings.IsAlwaysOnTopArgument;
                IsSingleInstanceArgument = appConfig.Settings.IsSingleInstanceArgument;
                PlayAndStopArgument = appConfig.Settings.PlayAndStopArgument;
                ResumesFromLastTimeIndexArgument = appConfig.Settings.ResumesFromLastTimeIndexArgument;
                EnquesFilesInSingleInstanceModeArgument = appConfig.Settings.EnquesFilesInSingleInstanceModeArgument;
            }
        }
        #endregion
    }
}
