/// Written by: Yulia Danilova
/// Creation Date: 14th of May, 2021
/// Purpose: Interface for the application's configuration
#region ========================================================================= USING =====================================================================================
using System.Collections.Generic;
#endregion

namespace Leya.Infrastructure.Configuration
{
    public interface IAppConfig
    {
        #region ================================================================ PROPERTIES =================================================================================
        string ConfigurationFilePath { get; }
        SettingsEntity Settings { get; set; }
        Dictionary<string, string> ConnectionStrings { get; set; }
        #endregion

        #region ================================================================= METHODS ===================================================================================
        /// <summary>
        /// Saves the application's configuration settings
        /// </summary>
        void UpdateConfiguration();
        #endregion
    }
}
