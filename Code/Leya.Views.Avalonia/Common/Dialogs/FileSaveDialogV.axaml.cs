/// Written by: Yulia Danilova
/// Creation Date: 04th of July, 2021
/// Purpose: Code behind for the FileSaveDialogV user control
#region ========================================================================= USING =====================================================================================
using System;
using Avalonia;
using System.IO;
using System.Linq;
using Avalonia.Input;
using Avalonia.Controls;
using Leya.Views.Startup;
using Avalonia.VisualTree;
using Avalonia.Markup.Xaml;
using System.Threading.Tasks;
using Avalonia.Interactivity;
using Leya.ViewModels.Common.Models;
using Leya.Models.Common.Models.Common;
using Leya.Infrastructure.Configuration;
using Leya.ViewModels.Common.Dialogs.FileSave;
#endregion

namespace Leya.Views.Common.Dialogs
{
    public partial class FileSaveDialogV : Window, IFileSaveDialogView
    {
        #region ============================================================== FIELD MEMBERS ================================================================================
        private readonly IAppConfig appConfig;
        private readonly Grid modalGrid;
        private readonly Grid grdPreview;
        private readonly Grid grdContainer;
        private readonly Border grdNewFolder;
        private readonly ListBox lstDirectories;
        private readonly GridSplitter spPreview;
        private readonly TextBox txtNewFolderName;
        private ScrollViewer directoriesScrollViewer;
        #endregion

        #region ================================================================== CTOR =====================================================================================
        /// <summary>
        /// Default C-tor, stupidly and mandatorily required by Avalonia's designer. Does nothing...
        /// </summary>
        public FileSaveDialogV()
        {

        }

        /// <summary>
        /// Overload C-tor
        /// </summary>
        /// <param name="appConfig">The injected application's configuration</param>
        public FileSaveDialogV(IAppConfig appConfig)
        {
            AvaloniaXamlLoader.Load(this);
#if DEBUG
            this.AttachDevTools();
#endif
            this.appConfig = appConfig;

            modalGrid = this.FindControl<Grid>("modalGrid");
            grdPreview = this.FindControl<Grid>("grdPreview");
            grdContainer = this.FindControl<Grid>("grdContainer");
            grdNewFolder = this.FindControl<Border>("grdNewFolder");
            spPreview = this.FindControl<GridSplitter>("spPreview");
            lstDirectories = this.FindControl<ListBox>("lstDirectories");
            txtNewFolderName = this.FindControl<TextBox>("txtNewFolderName");

            // set the size of the dialog to the last bounds saved in the application's configuration file
            Width = appConfig.Settings.DialogsWidth;
            Height = appConfig.Settings.DialogsHeight;
            // set the position of the grid splitter to the last position saved in application's configuration file
            grdContainer.ColumnDefinitions[0].Width = new GridLength(appConfig.Settings.NavigationPanelWidth, GridUnitType.Star);
            grdContainer.ColumnDefinitions[2].Width = new GridLength(appConfig.Settings.DirectoriesPanelWidth, GridUnitType.Star);
            grdContainer.ColumnDefinitions[4].Width = new GridLength(appConfig.Settings.PreviewPanelWidth, GridUnitType.Star);
            // avalonia has no "SizeChanged" event, subscribe to changes to the Bounds property with a method
            BoundsProperty.Changed.AddClassHandler<Window>((s, e) => Window_SizeChanged());
        }
        #endregion

        #region ================================================================= METHODS ===================================================================================
        /// <summary>
        /// Shows the current window as a modal dialog
        /// </summary>
        public async Task<bool?> ShowDialog()
        {
            return await ShowDialog<bool?>(StartupV.Instance);
        }
        #endregion

        #region ============================================================= EVENT HANDLERS ================================================================================
        /// <summary>
        /// Handles window's Opened event
        /// </summary>
        private void Window_Opened(object? sender, EventArgs e)
        {
            if (string.IsNullOrEmpty((DataContext as FileSaveDialogVM).InitialFolder) && !string.IsNullOrEmpty(appConfig.Settings.LastDirectory))
                (DataContext as FileSaveDialogVM).InitialFolder = appConfig.Settings.LastDirectory;
            // set the selection mode
            lstDirectories.SelectionMode = SelectionMode.Single;
            directoriesScrollViewer = lstDirectories.FindDescendantOfType<ScrollViewer>();
            (DataContext as FileSaveDialogVM).ClosingView += (s, e) => Close();
        }

        /// <summary>
        /// Handles Favorite's Checked and Unchecked events
        /// </summary>
        private async void Favorite_CheckedChanged(object? sender, RoutedEventArgs e)
        {
            (DataContext as FileSaveDialogVM).IsFavoritePath = (sender as CheckBox).IsChecked == true;
            await (DataContext as FileSaveDialogVM).SetIsFavoritePathAsync_Command.ExecuteAsync();
        }

        /// <summary>
        /// Handles Favorite's Checked and Unchecked events
        /// </summary>
        private async void Extensions_SelectionChanged(object? sender, SelectionChangedEventArgs e)
        {
            (DataContext as FileSaveDialogVM).SelectedExtensionFilter = (sender as ComboBox).SelectedItem as SearchEntity;
            await (DataContext as FileSaveDialogVM).SelectedExtensionChangedAsync_Command.ExecuteAsync();
        }

        /// <summary>
        /// Handles SelectionChanged event of the directories listview
        /// </summary>
        private void Directories_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FileSaveDialogVM viewmodel = (FileSaveDialogVM)DataContext;
            // get a list of all selected items in the directories listview
            FileSystemEntity selectedItem = (sender as ListBox).SelectedItems.Cast<FileSystemEntity>().FirstOrDefault();
            // only files should be allowed in the selection (not drives or folders)
            if (selectedItem != null && selectedItem.DirType == 0)
            {
                viewmodel.Filename = Path.GetFileName(selectedItem.Path);
                if (grdPreview.IsVisible)
                    viewmodel.PreviewFile();
            }
        }

        /// <summary>
        /// Hides the modal box for entering a new folder name
        /// </summary>
        private void HideNewFolderModalBox(object sender, RoutedEventArgs e)
        {
            grdNewFolder.IsVisible = false;
            modalGrid.IsVisible = false;
        }

        /// <summary>
        /// Shows the modal box for entering a new folder name
        /// </summary>
        private void ShowNewFolderModalBox(object sender, RoutedEventArgs e)
        {
            grdNewFolder.IsVisible = true;
            modalGrid.IsVisible = true;
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
        private async void Window_SizeChanged()
        {
            appConfig.Settings.DialogsHeight = Height;
            appConfig.Settings.DialogsWidth = Width;
            await appConfig.UpdateConfigurationAsync();
        }

        /// <summary>
        /// Handles Directories separator's DragCompleted event
        /// </summary>
        private async void SeparatorDirectories_DragCompleted(object sender, VectorEventArgs e)
        {
            appConfig.Settings.NavigationPanelWidth = grdContainer.ColumnDefinitions[0].Width.Value;
            appConfig.Settings.DirectoriesPanelWidth = grdContainer.ColumnDefinitions[2].Width.Value;
            await appConfig.UpdateConfigurationAsync();
        }

        /// <summary>
        /// Handles Preview separator's DragCompleted event
        /// </summary>
        private async void SeparatorPreview_DragCompleted(object sender, VectorEventArgs e)
        {
            if (grdPreview.IsVisible)
            {
                appConfig.Settings.DirectoriesPanelWidth = grdContainer.ColumnDefinitions[2].Width.Value;
                appConfig.Settings.PreviewPanelWidth = grdContainer.ColumnDefinitions[4].Width.Value;
                await appConfig.UpdateConfigurationAsync();
            }
        }

        /// <summary>
        /// Handles the preview panel toggle checkbox's CheckedChanged event
        /// </summary>
        private void TogglePreviewPanel_CheckChanged(object sender, RoutedEventArgs e)
        {
            if ((sender as CheckBox).IsChecked == true)
            {
                grdContainer.ColumnDefinitions[2].Width = new GridLength(appConfig.Settings.DirectoriesPanelWidth, GridUnitType.Star);
                grdContainer.ColumnDefinitions[4].Width = new GridLength(appConfig.Settings.PreviewPanelWidth, GridUnitType.Star);
                grdContainer.ColumnDefinitions[4].MinWidth = 75;
                grdPreview.IsVisible = true;
                spPreview.IsVisible = true;
            }
            else
            {
                grdContainer.ColumnDefinitions[2].Width = new GridLength(appConfig.Settings.DirectoriesPanelWidth + appConfig.Settings.PreviewPanelWidth, GridUnitType.Star);
                grdContainer.ColumnDefinitions[4].Width = new GridLength(0, GridUnitType.Star);
                grdContainer.ColumnDefinitions[4].MinWidth = 0;
                grdPreview.IsVisible = false;
                spPreview.IsVisible = false;
            }
        }

        /// <summary>
        /// Handles Directories listbox's PointerWheelChanged event
        /// </summary>
        private void Directories_PointerWheelChanged(object? sender, PointerWheelEventArgs e)
        {
            directoriesScrollViewer.Offset = new Vector(directoriesScrollViewer.Offset.X - (e.Delta.Y * 50), directoriesScrollViewer.Offset.Y);
        }
        #endregion
    }
}
