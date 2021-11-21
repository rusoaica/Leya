/// Written by: Yulia Danilova
/// Creation Date: 14th of May, 2021
/// Purpose: Model for strongly typed application configuration values
#region ========================================================================= USING =====================================================================================
using System.Collections.Generic;
#endregion

namespace Leya.Infrastructure.Configuration
{
    public class SettingsEntity
    {
        #region =============================================================== PROPERTIES ==================================================================================
        public int DialogNavigationFilterSelectedIndex { get; set; }
        public int? MainWindowWidth { get; set; }
        public int? MainWindowHeight { get; set; }
        public int? MainWindowPositionX { get; set; }
        public int? MainWindowPositionY { get; set; }
        public int? StartupWindowPositionX { get; set; }
        public int? StartupWindowPositionY { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string PlayerPath { get; set; }
        public string WeatherUrl { get; set; }
        public string LastDirectory { get; set; }
        public string SelectedTheme { get; set; }
        public string PlayerArguments { get; set; }
        public string BackgroundImagePath { get; set; }
        public double DialogsWidth { get; set; }
        public double DialogsHeight { get; set; }
        public double PreviewPanelWidth { get; set; }
        public double NavigationPanelWidth { get; set; }
        public double DirectoriesPanelWidth { get; set; }
        public bool Autologin { get; set; }
        public bool PlayAndStopArgument { get; set; }
        public bool RememberCredentials { get; set; }
        public bool UsesAutoscaleArgument { get; set; }
        public bool IsAlwaysOnTopArgument { get; set; }
        public bool UsesDatabaseForStorage { get; set; }
        public bool RepeatsPlaylistArgument { get; set; }
        public bool ShufflesPlaylistArgument { get; set; }
        public bool UsesFullScreenArgument { get; set; } = true;
        public bool IsSingleInstanceArgument { get; set; } = true;
        public bool ContinuousPlaybackForEpisodes { get; set; } = true;
        public bool EnquesFilesInSingleInstanceModeArgument { get; set; }
        public bool ResumesFromLastTimeIndexArgument { get; set; } = true;
        public List<string> FavoritePaths { get; set; }
        #endregion
    }
}
