/// Written by: Yulia Danilova
/// Creation Date: 26th of November, 2020
/// Purpose: Interface business model for player options
#region ========================================================================= USING =====================================================================================
using System;
using System.Threading.Tasks;
#endregion

namespace Leya.Models.Core.Options
{
    public interface IOptionsPlayer
    {
        #region ============================================================== FIELD MEMBERS ================================================================================
        event Action<string> PropertyChanged;
        #endregion

        #region ================================================================ PROPERTIES =================================================================================
        string PlayerPath { get; set; }
        string PlayerArguments { get; set; }
        bool PlayAndStopArgument { get; set; }
        bool IsAlwaysOnTopArgument { get; set; }
        bool UsesAutoscaleArgument { get; set; }
        bool UsesFullScreenArgument { get; set; }
        bool RepeatsPlaylistArgument { get; set; }
        bool ShufflesPlaylistArgument { get; set; }
        bool IsSingleInstanceArgument { get; set; }
        bool ResumesFromLastTimeIndexArgument { get; set; }
        bool EnquesFilesInSingleInstanceModeArgument { get; set; }
        #endregion

        #region ================================================================= METHODS ===================================================================================
        /// <summary>
        /// Enables or disables quiting when playback ends for the media player
        /// </summary>
        void ChangePlayAndStopArgument(bool param);

        /// <summary>
        /// Enables or disables shuffling playlist playback for the media player
        /// </summary>
        void ChangeShuffleArgument(bool param);

        /// <summary>
        /// Enables or disables repeating playlist playback for the media player
        /// </summary>
        void ChangeRepeatArgument(bool param);

        /// <summary>
        /// Enables or disables autoscale argument for the media player
        /// </summary>
        void ChangeAutoscaleArgument(bool param);

        /// <summary>
        /// Enables or disables always on top argument for the media player
        /// </summary>
        void ChangeAlwaysOnTopArgument(bool param);

        /// <summary>
        /// Enables or disables enqueue of files into playlist when in single instance mode
        /// </summary>
        void ChangeEnqueueFilesInSingleInstanceModeArgument(bool param);

        /// <summary>
        /// Enables or disables single instance argument for the media player
        /// </summary>
        void ChangeSingleInstanceArgument(bool param);

        /// <summary>
        /// Enables or disables fullscreen argument for the media player
        /// </summary>
        void ChangeFullScreenArgument(bool param);
        
        /// <summary>
        /// Updates the application's configurations for the media player
        /// </summary>
        Task UpdatePlayerOptionsAsync();

        /// <summary>
        /// Gets the media player options from the application's configuration
        /// </summary>
        void GetPlayerOptions();
        #endregion
    }
}
