/// Written by: Yulia Danilova
/// Creation Date: 04th of July, 2021
/// Purpose: Code behind for the FileSaveDialogV user control
#region ========================================================================= USING =====================================================================================
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using Leya.ViewModels.Common.Models;
using Leya.Infrastructure.Configuration;
using System.Windows.Controls.Primitives;
using Leya.ViewModels.Common.Dialogs.FileSave;
#endregion

namespace Leya.Views.Common.Dialogs
{
    /// <summary>
    /// Interaction logic for FileSaveDialogV.xaml
    /// </summary>
    public partial class FileSaveDialogV : Window, IFileSaveDialogView
    {
        #region ============================================================== FIELD MEMBERS ================================================================================
        private readonly IAppConfig appConfig;
        #endregion

        #region ================================================================== CTOR =====================================================================================
        /// <summary>
        /// Overload C-tor
        /// </summary>
        /// <param name="appConfig">The injected application's configuration</param>
        public FileSaveDialogV(IAppConfig appConfig)
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
            FileSaveDialogVM viewmodel = (FileSaveDialogVM)DataContext;
            // get a list of all selected items in the directories listview
            FileSystemEntity selectedItem = (sender as ListView).SelectedItems.Cast<FileSystemEntity>().FirstOrDefault();
            // only files should be allowed in the selection (not drives or folders)
            if (selectedItem != null && selectedItem.DirType == 0)
            {
                viewmodel.Filename = Path.GetFileName(selectedItem.Path);
                if (grdPreview.Visibility == Visibility.Visible)
                    viewmodel.PreviewFile();
            }
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
                await (DataContext as FileSaveDialogVM).CreateNewFolder_Command.ExecuteAsync();
            }
            else if (e.Key == Key.Escape)
                HideNewFolderModalBox(sender, e);
        }

        /// <summary>
        /// Handles window's SizeChanged event
        /// </summary>
        private async void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            appConfig.Settings.DialogsHeight = Height;
            appConfig.Settings.DialogsWidth = Width;
            await appConfig.UpdateConfigurationAsync();
        }

        /// <summary>
        /// Handles separator's DragCompleted event
        /// </summary>
        private async void Separator_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            appConfig.Settings.NavigationPanelWidth = grdContainer.ColumnDefinitions[0].Width.Value;
            appConfig.Settings.DirectoriesPanelWidth = grdContainer.ColumnDefinitions[2].Width.Value;
            await appConfig.UpdateConfigurationAsync();
        }

        /// <summary>
        /// Handles window's ContentRendered event
        /// </summary>
        private void Window_ContentRendered(object sender, System.EventArgs e)
        {
            if (string.IsNullOrEmpty((DataContext as FileSaveDialogVM).InitialFolder) && !string.IsNullOrEmpty(appConfig.Settings.LastDirectory))
                (DataContext as FileSaveDialogVM).InitialFolder = appConfig.Settings.LastDirectory;
            // set the selection mode
            lstDirectories.SelectionMode = SelectionMode.Single;
        }

        /// <summary>
        /// Handles the preview panel toggle checkbox's CheckedChanged event
        /// </summary>
        private void TogglePreviewPanel_CheckChanged(object sender, RoutedEventArgs e)
        {
            if ((sender as CheckBox).IsChecked == true)
                grdPreview.Visibility = Visibility.Visible;
            else
                grdPreview.Visibility = Visibility.Collapsed;
        }
        #endregion
    }
}
