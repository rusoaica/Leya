/// Written by: Yulia Danilova
/// Creation Date: 11th of November, 2020
/// Purpose: Code behind for the StartupV view
#region ========================================================================= USING =====================================================================================
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System.Threading.Tasks;
using Leya.ViewModels.Startup;
using System;
using Leya.Infrastructure.Configuration;
#endregion

namespace Leya.Views.Startup
{
    public partial class StartupV : Window, IStartupView
    {
        #region ============================================================== FIELD MEMBERS ================================================================================
        private readonly IAppConfig appConfig;
        private bool isWindowLoaded;
        #endregion

        #region ================================================================ PROPERTIES =================================================================================
        public static StartupV Instance { get; private set; }
        #endregion

        #region ================================================================== CTOR =====================================================================================
        /// <summary>
        /// Default C-tor
        /// </summary>
        public StartupV()
        {
        }

        /// <summary>
        /// Default C-tor
        /// </summary>
        public StartupV(IAppConfig appConfig)
        {
            AvaloniaXamlLoader.Load(this);
            Instance = this;
#if DEBUG
            this.AttachDevTools();
            this.appConfig = appConfig;
#endif
        }
        #endregion

        #region ================================================================= METHODS ===================================================================================
        /// <summary>
        /// Shows the current window as a modal dialog
        /// </summary>
        public async Task<bool?> ShowDialog()
        {
            return await ShowDialog<bool?>(Instance);
        }
        #endregion

        #region ============================================================= EVENT HANDLERS ================================================================================
        /// <summary>
        /// Handles window's Opened event
        /// </summary>
        private void Window_Opened(object? sender, EventArgs e)
        {
            (DataContext as StartupVM).ShowingView += (s, e) => Show();
            (DataContext as StartupVM).HidingView += (s, e) => Hide();
            if (appConfig.Settings.StartupWindowPositionX != null)
                Position = Position.WithX((int)appConfig.Settings.StartupWindowPositionX);
            if (appConfig.Settings.StartupWindowPositionY != null)
                Position = Position.WithY((int)appConfig.Settings.StartupWindowPositionY);
            isWindowLoaded = true;

        }

        /// <summary>
        /// Handles Window's PositionChanged event
        /// </summary>
        private async void Window_PositionChanged(object? sender, PixelPointEventArgs e)
        {
            // do not allow the application's configuration to be updated with the new position
            // unless the window is loaded and the user is the one changing it
            if (isWindowLoaded)
            {
                appConfig.Settings.StartupWindowPositionX = Position.X;
                appConfig.Settings.StartupWindowPositionY = Position.Y;
                await appConfig.UpdateConfigurationAsync();
            }
        }
        #endregion
    }
}
