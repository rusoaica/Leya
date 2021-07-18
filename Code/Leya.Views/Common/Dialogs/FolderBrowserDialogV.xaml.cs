﻿/// Written by: Yulia Danilova
/// Creation Date: 27th of June, 2021
/// Purpose: Code behind for the FolderBrowseDialogV user control
#region ========================================================================= USING =====================================================================================
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System.Collections.Generic;
using Leya.ViewModels.Common.Models;
using Leya.Infrastructure.Configuration;
using System.Windows.Controls.Primitives;
using Leya.ViewModels.Common.Dialogs.FolderBrowser;
#endregion

namespace Leya.Views.Common.Dialogs
{
    /// <summary>
    /// Interaction logic for FolderBrowseDialog.xaml
    /// </summary>
    public partial class FolderBrowserDialogV : Window, IFolderBrowserDialogView
    {
        #region ============================================================== FIELD MEMBERS ================================================================================
        private readonly IAppConfig appConfig;
        #endregion

        #region ================================================================== CTOR =====================================================================================
        /// <summary>
        /// Overload C-tor
        /// </summary>
        /// <param name="appConfig">The injected application's configuration</param>
        public FolderBrowserDialogV(IAppConfig appConfig)
        {
            InitializeComponent();
            this.appConfig = appConfig;

            Width = appConfig.Settings.DialogsWidth;
            Height = appConfig.Settings.DialogsHeight;
            grdContainer.ColumnDefinitions[0].Width = new GridLength(appConfig.Settings.NavigationPanelWidth, GridUnitType.Star);
            grdContainer.ColumnDefinitions[2].Width = new GridLength(appConfig.Settings.DirectoriesPanelWidth, GridUnitType.Star);
        }
        #endregion

        #region ============================================================= EVENT HANDLERS ================================================================================
        /// <summary>
        /// Handles SelectionChanged event of the directories listview
        /// </summary>
        private void Directories_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FolderBrowserDialogVM viewmodel = (FolderBrowserDialogVM)DataContext;
            // get a list of all selected items in the directories listview
            List<FileSystemEntity> selectedItems = (sender as ListView).SelectedItems.Cast<FileSystemEntity>().ToList();
            // only directories should be allowed in the selection (not drives, etc)
            if (selectedItems.Select(e => e.DirType).All(e => e == 2))
                viewmodel.SelectedDirectories = selectedItems.Count > 0 ? "\"" + string.Join("\" \"", selectedItems.Select(e => e.Path).ToArray()) + "\"" : string.Empty;
            else
                viewmodel.SelectedDirectories = string.Empty;
        }

        /// <summary>
        /// Hides the modal box for entering a new folder name
        /// </summary>
        private void HideNewFolderModalBox(object sender, RoutedEventArgs e)
        {
            grdNewFolder.Visibility = Visibility.Collapsed;
            modalGrid.Visibility = Visibility.Collapsed;
            modalBg.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Shows the modal box for entering a new folder name
        /// </summary>
        private void ShowNewFolderModalBox(object sender, RoutedEventArgs e)
        {
            grdNewFolder.Visibility = Visibility.Visible;
            modalGrid.Visibility = Visibility.Visible;
            modalBg.Visibility = Visibility.Visible;
            txtNewFolderName.Focus();
        }

        /// <summary>
        /// Handles the KeyUp event of the new folder name textbox
        /// </summary>
        private async void NewFolderName_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && !string.IsNullOrEmpty((sender as TextBox).Text))
            {
                HideNewFolderModalBox(sender, e);
                await (DataContext as FolderBrowserDialogVM).CreateNewFolder_Command.ExecuteAsync();
            }
            else if (e.Key == Key.Escape)
                HideNewFolderModalBox(sender, e);
        }

        /// <summary>
        /// Handles window's SizeChanged event
        /// </summary>
        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            appConfig.Settings.DialogsHeight = Height;
            appConfig.Settings.DialogsWidth = Width;
            appConfig.UpdateConfiguration();
        }

        /// <summary>
        /// Handles separator's DragCompleted event
        /// </summary>
        private void Separator_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            appConfig.Settings.NavigationPanelWidth = grdContainer.ColumnDefinitions[0].Width.Value;
            appConfig.Settings.DirectoriesPanelWidth = grdContainer.ColumnDefinitions[2].Width.Value;
            appConfig.UpdateConfiguration();
        }

        /// <summary>
        /// Handles window's ContentRendered event
        /// </summary>
        private void Window_ContentRendered(object sender, System.EventArgs e)
        {
            if (string.IsNullOrEmpty((DataContext as FolderBrowserDialogVM).InitialFolder) && !string.IsNullOrEmpty(appConfig.Settings.LastDirectory))
                (DataContext as FolderBrowserDialogVM).InitialFolder = appConfig.Settings.LastDirectory;
        }
        #endregion
    }
}