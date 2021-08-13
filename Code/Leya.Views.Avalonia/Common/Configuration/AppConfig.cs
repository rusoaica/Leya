/// Written by: Yulia Danilova
/// Creation Date: 14th of May, 2021
/// Purpose: Model for strongly typed application configuration values
#region ========================================================================= USING =====================================================================================
using System.IO;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Leya.Infrastructure.Enums;
using System.Collections.Generic;
using Leya.Infrastructure.Notification;
using Leya.Infrastructure.Configuration;
#endregion

namespace Leya.Views.Common.Configuration
{
    internal sealed class AppConfig : IAppConfig
    {
        #region ============================================================== FIELD MEMBERS ================================================================================
        private readonly INotificationService notificationService;
        #endregion

        #region =============================================================== PROPERTIES ==================================================================================
        public Dictionary<string, string> ConnectionStrings { get; set; } = new Dictionary<string, string>();
        public SettingsEntity Settings { get; set; } = new SettingsEntity();
        [JsonIgnore]
        public string ConfigurationFilePath { get; internal set; }
        #endregion

        #region ================================================================== CTOR =====================================================================================
        /// <summary>
        /// Overload C-tor
        /// </summary>
        /// <param name="notificationService">The injected notification service to use</param>
        public AppConfig(INotificationService notificationService)
        {
            this.notificationService = notificationService;
        }
        #endregion

        #region ================================================================= METHODS ===================================================================================
        /// <summary>
        /// Saves the application's configuration settings
        /// </summary>
        public async Task UpdateConfigurationAsync()
        {
            if (!string.IsNullOrEmpty(ConfigurationFilePath) && File.Exists(ConfigurationFilePath))
                File.WriteAllText(ConfigurationFilePath, JsonConvert.SerializeObject(this, Formatting.Indented));
            else
                await notificationService.ShowAsync("Application's configuration file was not found!", "LEYA - Error", NotificationButton.OK, NotificationImage.Error);
        }
        #endregion
    }
}
