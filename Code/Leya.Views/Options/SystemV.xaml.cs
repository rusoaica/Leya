/// Written by: Yulia Danilova
/// Creation Date: 18th of November, 2020
/// Purpose: View code behind for the MainWindow window
#region ========================================================================= USING =====================================================================================
using System;
using System.Windows;
using Leya.ViewModels;
using Leya.Views.Common;
using Leya.Views.Startup;
using System.Diagnostics;
using System.Windows.Input;
using System.Windows.Controls;
using Leya.Views.Common.Dispatcher;
using Leya.ViewModels.Common.ViewFactory;
using Leya.ViewModels.Options;
#endregion

namespace Leya.Views.Options
{
    /// <summary>
    /// Interaction logic for SystemV.xaml
    /// </summary>
    public partial class SystemV : Page, ISystemView
    {
        #region ============================================================== FIELD MEMBERS ================================================================================
        private readonly IViewFactory viewFactory;
        #endregion

        #region ================================================================== CTOR =====================================================================================
        /// <summary>
        /// Default C-tor
        /// </summary>
        public SystemV(IViewFactory viewFactory)
        {
            InitializeComponent();
            this.viewFactory = viewFactory;
            grdOptions.Visibility = Visibility.Collapsed;
        }
        #endregion

        #region ============================================================= EVENT HANDLERS ================================================================================
        /// <summary>
        /// Handles OptionsMenu MouseUp event
        /// </summary>
        private void OptionsMenu_MouseUp(object sender, MouseButtonEventArgs e)
        {
            grdOptionsMenu.Visibility = Visibility.Collapsed;
            grdOptions.Visibility = Visibility.Visible;
            switch ((sender as Control).Tag.ToString())
            {
                case "Media":
                    navOptions.Source = new Uri(@"pack://application:,,,/Leya;component/Options/OptionsMediaV.xaml", UriKind.Absolute);
                    break;
                case "Interface":
                    navOptions.Source = new Uri(@"pack://application:,,,/Leya;component/Options/OptionsInterfaceV.xaml", UriKind.Absolute);
                    break;
                case "Player":
                    navOptions.Source = new Uri(@"pack://application:,,,/Leya;component/Options/OptionsPlayerV.xaml", UriKind.Absolute);
                    break;
                case "System Info":
                    navOptions.Source = new Uri(@"pack://application:,,,/Leya;component/Options/OptionsSystemInfoV.xaml", UriKind.Absolute);
                    break;
                case "System":
                    navOptions.Source = new Uri(@"pack://application:,,,/Leya;component/Options/OptionsSystemV.xaml", UriKind.Absolute);
                    break;
            }
        }

        /// <summary>
        /// Handles Back MouseUp event
        /// </summary>
        private void Back_MouseUp(object sender, MouseButtonEventArgs e)
        {
            grdOptionsMenu.Visibility = Visibility.Visible;
            grdOptions.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Handles Security MouseUp event
        /// </summary>
        private void Security_MouseUp(object sender, MouseButtonEventArgs e)
        {
            //bool isOpened = false;
            //// iterate all opened windows
            //foreach (object window in Application.Current.Windows)
            //{
            //    // check if a Change Password view is already opened
            //    if (window is ChangePasswordV)
            //    {
            //        // brind the Change Password window to front
            //        ((Window)window).WindowState = WindowState.Normal;
            //        ((Window)window).Activate();
            //        isOpened = true;
            //        break;
            //    }
            //}
            //// if no Change Password window is opened, open a new one
            //if (!isOpened)
            //    new ChangePasswordV().Show();
        }

        /// <summary>
        ///  Handles LogginMenu MouseUp event
        /// </summary>
        private void LoggingMenu_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Process.Start(AppDomain.CurrentDomain.BaseDirectory + @"Logs");
        }
        #endregion
    }
}
