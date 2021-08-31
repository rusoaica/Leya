/// Written by: Yulia Danilova
/// Creation Date: 28th of August, 2021
/// Purpose: Business model for options

namespace Leya.Models.Core.Options
{
    public class AppOptions : IAppOptions
    {
        #region ================================================================ PROPERTIES =================================================================================
        public IOptionsMedia OptionsMedia { get; }
        public IOptionsPlayer OptionsPlayer { get; }
        public IOptionsInterface OptionsInterface { get; }
        #endregion

        #region ================================================================== CTOR =====================================================================================
        /// <summary>
        /// Overload C-tor
        /// </summary>
        /// <param name="optionsMedia">The injected media options</param>
        /// <param name="optionsPlayer">The injected media player options</param>
        /// <param name="optionsInterface">The injected user interface options</param>
        public AppOptions(IOptionsMedia optionsMedia, IOptionsPlayer optionsPlayer, IOptionsInterface optionsInterface)
        {
            OptionsMedia = optionsMedia;
            OptionsPlayer = optionsPlayer;
            OptionsInterface = optionsInterface;
        }
        #endregion
    }
}