/// Written by: Yulia Danilova
/// Creation Date: 14th of May, 2021
/// Purpose: Model for strongly typed application configuration values

namespace Leya.Models.Common.Models.Configuration
{
    public class SettingsM
    {
        #region =============================================================== PROPERTIES ==================================================================================
        public string PlayerPath { get; set; }
        public string WeatherUrl { get; set; }
        public string SelectedTheme { get; set; }
        public string PlayerArguments { get; set; }
        public string BackgroundImagePath { get; set; }
        public bool PlayAndStopArgument { get; set; }
        public bool UsesAutoscaleArgument { get; set; }
        public bool IsAlwaysOnTopArgument { get; set; }
        public bool RepeatsPlaylistArgument { get; set; }
        public bool ShufflesPlaylistArgument { get; set; }
        public bool UsesFullScreenArgument { get; set; } = true;
        public bool IsSingleInstanceArgument { get; set; } = true;
        public bool EnquesFilesInSingleInstanceModeArgument { get; set; }
        public bool ResumesFromLastTimeIndexArgument { get; set; } = true;
        public double DisplayOffset { get; set; }
        #endregion
    }
}
