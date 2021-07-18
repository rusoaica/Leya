/// Written by: Yulia Danilova
/// Creation Date: 11th of November, 2020
/// Purpose: Code behind for the StartupV view
#region ========================================================================= USING =====================================================================================
using System;
using System.Windows;
using System.Windows.Controls;
using Leya.ViewModels.Startup;
using Leya.Infrastructure.Security;
using Leya.Infrastructure.Configuration;
#endregion

namespace Leya.Views.Startup
{
    /// <summary>
    /// Interaction logic for StartupV.xaml
    /// </summary>
    public partial class StartupV : Window, IStartupView
    {
        #region ============================================================== FIELD MEMBERS ================================================================================
        private readonly IAppConfig appConfig;
        #endregion

        #region ================================================================== CTOR =====================================================================================
        /// <summary>
        /// Overload C-tor
        /// <param name="appConfig">The injected application's configuration to use</param>
        /// </summary>
        public StartupV(IAppConfig appConfig)
        {
            InitializeComponent();
            this.appConfig = appConfig;
        }
        #endregion

        #region ============================================================= EVENT HANDLERS ================================================================================
        /// <summary>
        /// Handles PasswordChanged event
        /// </summary>
        private void PasswordChanged(object sender, RoutedEventArgs e)
        {
            // since there is no data binding for PasswordBox, update property value here
            // this doesn't break MVVM patterns, since it's still View binding related code
            (DataContext as StartupVM).Password = ((PasswordBox)sender).SecurePassword;
        }

        /// <summary>
        /// Handles Window ContentRendered event
        /// </summary>
        private void Window_ContentRendered(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(appConfig.Settings.Password))
                txtPassword.Password = Crypto.Decrypt(appConfig.Settings.Password);
            if (!string.IsNullOrEmpty(appConfig.Settings.SelectedTheme))
            {
                string theme = appConfig.Settings.SelectedTheme;
                Application.Current.Resources.MergedDictionaries.Clear();
                ResourceDictionary dictionary = new ResourceDictionary();
                dictionary.Source = new Uri("/Common/Styles/" + theme + ".xaml", UriKind.Relative);
                Application.Current.Resources.MergedDictionaries.Add(dictionary);
            }
        }
        #endregion
    }
}
