/// Written by: Yulia Danilova
/// Creation Date: 09th of September, 2021
/// Purpose: Business model for system options
#region ========================================================================= USING =====================================================================================
using System;
using System.Threading.Tasks;
using Leya.Models.Common.Broadcasting;
using Leya.Infrastructure.Notification;
using Leya.Infrastructure.Configuration;
#endregion

namespace Leya.Models.Core.Options
{
    public class OptionsSystem : NotifyPropertyChanged, IOptionsSystem
    {
        #region ============================================================== FIELD MEMBERS ================================================================================
        public static event Action<int> ThemeChanged;

        private readonly IAppConfig appConfig;
        private readonly INotificationService notificationService;
        #endregion

        #region ================================================================ PROPERTIES =================================================================================
        private bool usesDatabaseForStorage = true; 
        public bool UsesDatabaseForStorage
        {
            get { return usesDatabaseForStorage; }
            set { usesDatabaseForStorage = value; Notify(); }
        }
        #endregion

        #region ================================================================== CTOR =====================================================================================
        /// <summary>
        /// Overload C-tor
        /// </summary>
        /// <param name="appConfig">The injected application's configuration</param>
        /// <param name="notificationService">The injected service used for displaying notifications</param>
        public OptionsSystem(IAppConfig appConfig, INotificationService notificationService)
        {
            this.appConfig = appConfig;
            this.notificationService = notificationService;
        }
        #endregion

        #region ================================================================= METHODS ===================================================================================
        /// <summary>
        /// Gets the system options from the application's configuration
        /// </summary>        
        public void GetSystemOptions()
        {
            UsesDatabaseForStorage = appConfig.Settings.UsesDatabaseForStorage;
        }

        /// <summary>
        /// Updates the application's configurations for the system options
        /// </summary>
        public async Task UpdateSystemOptionsAsync()
        {
            appConfig.Settings.UsesDatabaseForStorage = UsesDatabaseForStorage;
            await appConfig.UpdateConfigurationAsync();
            await notificationService.ShowAsync("System settings have been updated!", "LEYA - Success");
        }
        #endregion
    }
}
