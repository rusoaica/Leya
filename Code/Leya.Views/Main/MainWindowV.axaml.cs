/// Written by: Yulia Danilova
/// Creation Date: 09th of November, 2020
/// Purpose: View code behind for the MainWindow window
#region ========================================================================= USING =====================================================================================
using System;
using Avalonia;
using System.IO;
using System.Linq;
using Avalonia.Input;
using Avalonia.Styling;
using Avalonia.Controls;
using Leya.Views.Startup;
using Avalonia.Animation;
using Avalonia.Markup.Xaml;
using Leya.ViewModels.Main;
using System.Threading.Tasks;
using Avalonia.Interactivity;
using Leya.Infrastructure.Enums;
using System.Collections.Generic;
using Leya.Infrastructure.Dialog;
using Leya.Models.Common.Models.Media;
using Leya.Infrastructure.Notification;
using Leya.Models.Common.Models.Common;
using Leya.Infrastructure.Configuration;
#endregion

namespace Leya.Views.Main
{
    public partial class MainWindowV : Window, IMainWindowView
    {
        #region ============================================================== FIELD MEMBERS ================================================================================
        private Grid grdWindowDrag;
        private readonly Image imgBanner;
        private readonly Grid grdDescription;
        private readonly Grid grdTransitionMask;
        private readonly AutoCompleteBox acbSearchMediaLibrary;
        private readonly IAppConfig appConfig;
        private readonly IFileBrowserService fileBrowserService;
        private readonly INotificationService notificationService;
        private readonly IFolderBrowserService folderBrowserService;
        private Animation? opacityFadeInAnimation;
        private Animation? opacityFadeOutAnimation;
        private readonly Clock animationPlaybackClock = new Clock();
        private bool isWindowLoaded;
        private bool isWindowDragged;
        #endregion

        #region ================================================================== CTOR =====================================================================================
        /// <summary>
        /// Default C-tor
        /// </summary>
        public MainWindowV()
        {
        }

        /// <summary>
        /// Overload C-tor
        /// </summary>
        /// <param name="folderBrowserService">Injected folder browser service</param>
        /// <param name="fileBrowserService">Injected file browser service</param>
        /// <param name="appConfig">Injected application's configuration service</param>
        /// <param name="notificationService">Injected notification service</param>
        public MainWindowV(IFolderBrowserService folderBrowserService, IFileBrowserService fileBrowserService, IAppConfig appConfig, INotificationService notificationService)
        {
            AvaloniaXamlLoader.Load(this);
#if DEBUG
            this.AttachDevTools();
#endif
            imgBanner = this.FindControl<Image>("imgBanner");
            grdDescription = this.FindControl<Grid>("grdDescription");
            grdTransitionMask = this.FindControl<Grid>("grdTransitionMask");
            acbSearchMediaLibrary = this.FindControl<AutoCompleteBox>("acbSearchMediaLibrary");
         
            grdTransitionMask.Clock = animationPlaybackClock;
            CreateOpacityFadeInAnimation();
            CreateOpacityFadeOutAnimation();
            this.appConfig = appConfig;
            this.fileBrowserService = fileBrowserService;
            this.notificationService = notificationService;
            this.folderBrowserService = folderBrowserService;
        }
        #endregion

        #region ================================================================= METHODS =================================================================================== 
        /// <summary>
        /// Creates the animation used in the mask fade in
        /// </summary>
        private void CreateOpacityFadeInAnimation()
        {
            opacityFadeInAnimation = new Animation()
            {
                SpeedRatio = 1d,
                FillMode = FillMode.Forward,
                Delay = TimeSpan.FromSeconds(0),
                Duration = TimeSpan.FromSeconds(0.5),
                PlaybackDirection = PlaybackDirection.Alternate,
                DelayBetweenIterations = TimeSpan.FromSeconds(0),
                IterationCount = new IterationCount(1, IterationType.Many),
                Children =
                {
                    new KeyFrame()
                    {
                        KeyTime = TimeSpan.FromSeconds(0),
                        Setters = { new Setter(OpacityProperty, 0.0), }
                    },
                    new KeyFrame()
                    {
                        KeyTime = TimeSpan.FromSeconds(0.5),
                        Setters = { new Setter(OpacityProperty, 1.0), }
                    }
                }
            };
        }

        /// <summary>
        /// Creates the animation used in the mask fade out
        /// </summary>
        private void CreateOpacityFadeOutAnimation()
        {
            opacityFadeOutAnimation = new Animation()
            {
                SpeedRatio = 1d,
                FillMode = FillMode.Forward,
                Delay = TimeSpan.FromSeconds(0),
                Duration = TimeSpan.FromSeconds(1),
                PlaybackDirection = PlaybackDirection.Alternate,
                DelayBetweenIterations = TimeSpan.FromSeconds(0),
                IterationCount = new IterationCount(1, IterationType.Many),
                Children =
                {
                    new KeyFrame()
                    {
                        KeyTime = TimeSpan.FromSeconds(0),
                        Setters = { new Setter(OpacityProperty, 1.0), }
                    },
                    new KeyFrame()
                    {
                        KeyTime = TimeSpan.FromSeconds(1),
                        Setters = { new Setter(OpacityProperty, 0.0), }
                    }
                }
            };
        }

        /// <summary>
        /// Displays the main menu via a fading animation
        /// </summary>
        private async Task AnimateToMainMenuAsync()
        {
            if ((DataContext as MainWindowVM).CurrentNavigationLevel != NavigationLevel.None)
            {
                // start the fade in animation of the black mask
                grdTransitionMask.IsVisible = true;
                await opacityFadeInAnimation.RunAsync(grdTransitionMask, animationPlaybackClock);
                await (DataContext as MainWindowVM).ExitMediaLibrary();
            }
        }

        /// <summary>
        /// Escapes backslash
        /// </summary>
        /// <param name="text">The string to be escaped</param>
        /// <returns>The escaped string</returns>
        public static string EscapeQuotations(string text)
        {
            if (!string.IsNullOrEmpty(text))
                return text.Contains(@"\\") ? EscapeQuotations(text.Replace(@"\\", @"\")) : text;
            else
                return text;
        }

        /// <summary>
        /// Shows the current window as a modal dialog
        /// </summary>
        public async Task<bool?> ShowDialogAsync()
        {
            return await ShowDialog<bool?>(StartupV.Instance);
        }
        #endregion

        #region ============================================================= EVENT HANDLERS ================================================================================
        /// <summary>
        /// Handles the Window's Opened event
        /// </summary>
        private async void Window_Opened(object? sender, EventArgs e)
        {
            // subscribe to the navigation event
            (DataContext as MainWindowVM).Navigated += MainWindowV_Navigated;
            // set the size and position to the values saved in the application's configuration, if any
            if (appConfig.Settings.MainWindowPositionX != null)
                Position = Position.WithX((int)appConfig.Settings.MainWindowPositionX);
            if (appConfig.Settings.MainWindowPositionY != null)
                Position = Position.WithY((int)appConfig.Settings.MainWindowPositionY);
            if (appConfig.Settings.MainWindowWidth != null)
                Width = (int)appConfig.Settings.MainWindowWidth;
            if (appConfig.Settings.MainWindowHeight != null)
                Height = (int)appConfig.Settings.MainWindowHeight;
            if (!string.IsNullOrEmpty(appConfig.Settings.BackgroundImagePath) && File.Exists(appConfig.Settings.BackgroundImagePath))
                (DataContext as MainWindowVM).BackgroundImagePath = appConfig.Settings.BackgroundImagePath;
            isWindowLoaded = true;
            BoundsProperty.Changed.AddClassHandler<Window>((s, e) => Window_SizeChanged());

            // Avalonia bug: for some reason, neither of these work for finding a child control inside a template
            //grdWindowDrag = this.FindControl<Grid>("grdWindowDrag");
            //grdWindowDrag = ((IControl)this.GetVisualChildren().FirstOrDefault())?.FindControl<Grid>("grdWindowDrag");

            grdWindowDrag = (((VisualChildren[0] as Grid).Children[0] as DockPanel).Children[0] as Grid).Children[1] as Grid;
            grdWindowDrag.PointerPressed += (s, e) => isWindowDragged = true;
            grdWindowDrag.PointerReleased += (s, e) => isWindowDragged = false;
            try
            {
                if (!string.IsNullOrEmpty(appConfig.Settings.WeatherUrl))
                    await (DataContext as MainWindowVM).GetWeatherInfoAsync(appConfig.Settings.WeatherUrl);
            }
            catch (Exception ex)
            {
                await notificationService.ShowAsync("Error getting the weather info from www.weather.com!\n" + ex.Message, "LEYA - Error", NotificationButton.OK, NotificationImage.Error);
            }
            acbSearchMediaLibrary.ItemFilter = new AutoCompleteFilterPredicate<object>(SearchMediaLibraryCustomFilter);
        }

        /// <summary>
        /// Handles window's SizeChanged event
        /// </summary>
        private async void Window_SizeChanged()
        {
            if (isWindowLoaded)
            {
                // store the new window size in the application's configuration file
                appConfig.Settings.MainWindowHeight = (int)Height;
                appConfig.Settings.MainWindowWidth = (int)Width;
                await appConfig.UpdateConfigurationAsync();
                // maintain aspect ratio of the banner image
                if (imgBanner.Bounds.Width > 0 && grdDescription.Bounds.Width > 0)
                {
                    imgBanner.Width = grdDescription.Bounds.Width - 30;
                    imgBanner.Height = (grdDescription.Bounds.Width - 30) / 5.4;
                }
            }
        }

        /// <summary>
        /// Handles Window's PositionChanged event
        /// </summary>
        private async void Window_PositionChanged(object? sender, PixelPointEventArgs e)
        {
            // do not allow the application's configuration to be updated with the new position
            // unless the window is loaded and the user is the one changing it
            if (isWindowLoaded && isWindowDragged)
            {
                appConfig.Settings.MainWindowPositionX = Position.X;
                appConfig.Settings.MainWindowPositionY = Position.Y;
                await appConfig.UpdateConfigurationAsync();
            }
        }

        /// <summary>
        /// Handles window KeyUp event
        /// </summary>
        private async void Window_KeyUp(object? sender, KeyEventArgs e)
        {
            // if the user presses Escape, exit any view and return to main menu
            if (e.Key == Key.Escape)
            {
                await AnimateToMainMenuAsync();
                // for some reason, the window closes when escape is pressed, so keep it handled
                e.Handled = true;
            }
        }

        /// <summary>
        /// Handles MediaItem's DoubleTapped event
        /// </summary>
        private async void MediaItem_DoubleTapped(object? sender, RoutedEventArgs e)
        {
            NavigationLevel navigationLevel = (DataContext as MainWindowVM).CurrentNavigationLevel;
            // get the parent listbox item of the double tapped element
            IMediaEntity mediaEntity = (sender as Label).Tag as IMediaEntity;
            if (navigationLevel is not NavigationLevel.Episode and not NavigationLevel.Song and not NavigationLevel.Movie)
            {
                // start the fade in animation of the black mask
                grdTransitionMask.IsVisible = true;
                await opacityFadeInAnimation.RunAsync(grdTransitionMask, animationPlaybackClock);
            }
            await (DataContext as MainWindowVM).NavigateMediaLibraryDownAsync(mediaEntity);
        }

        /// <summary>
        /// Handles advanced search results DoubleTapped event
        /// </summary>
        private async void AdvancedSearchResultItem_DoubleTapped(object? sender, RoutedEventArgs e)
        {
            // start the fade in animation of the black mask
            grdTransitionMask.IsVisible = true;
            await opacityFadeInAnimation.RunAsync(grdTransitionMask, animationPlaybackClock);
            // proceed to display the media list containing the double clicked search result
            await (DataContext as MainWindowVM).OpenAdvancedSearchResult((sender as Label).Tag as AdvancedSearchResultEntity);
        }

        /// <summary>
        /// Provides a custom filter for media library search
        /// </summary>
        /// <param name="search">The term to be searched</param>
        /// <param name="item">The control to be searched for <paramref name="search"/></param>
        /// <returns>True if <paramref name="search"/> was found on <paramref name="item"/>, False otherwise</returns>
        private bool SearchMediaLibraryCustomFilter(string search, object item)
        {
            FilterEntity element = item as FilterEntity;
            // ignore casing
            search = search.ToLower().Trim();
            // check if at least one search criteria is met
            bool hasEpisode = element.ChildTitle.ToLower().Contains(search);
            bool hasTvShow = (element.ParentTitle?.ToString().ToLower().Contains(search) ?? false);
            bool hasTags = element.Tags?.Where(t => t.ToLower().Contains(search)).Count() > 0;
            bool hasGenres = element?.Genres.Where(g => g.ToLower().Contains(search)).Count() > 0;
            bool hasActors = element?.Members.Where(a => a.ToLower().Contains(search)).Count() > 0;
            bool hasRoles = (element?.Roles).Where(r => r.ToLower().Contains(search)).Count() > 0;
            return hasEpisode || hasTvShow || hasTags || hasGenres || hasActors || hasRoles;
        }

        /// <summary>
        /// Handles the media source browse button's Click event
        /// </summary>
        private async void Browse_Click(object sender, RoutedEventArgs e)
        {
            // display the open folder dialog
            folderBrowserService.AllowMultiselection = true;
            if (await folderBrowserService.ShowAsync() == NotificationResult.OK)
            {
                string filename = folderBrowserService.SelectedDirectories;
                // invoke the view model's method for adding a new media source
                if (!string.IsNullOrEmpty(filename))
                    await (DataContext as MainWindowVM).AddMediaSourceAsync_Command.ExecuteAsync(filename);
            }
        }

        /// <summary>
        /// Handles the player path browse button's Click event
        /// </summary>
        private async void BrowsePlayerPath_Click(object sender, RoutedEventArgs e)
        {
            // does not apply on Linux!
            // fileBrowserService.Filter = new List<string>() { ".exe" };
            if (await fileBrowserService.ShowAsync() == NotificationResult.OK && fileBrowserService.SelectedFiles != null)
            {
                (DataContext as MainWindowVM).PlayerPath = fileBrowserService.SelectedFiles.Length > 3 ? fileBrowserService.SelectedFiles.Substring(1, fileBrowserService.SelectedFiles.Length - 2) : null;
                (DataContext as MainWindowVM).UpdatePlayerOptionsAsync_Command.RaiseCanExecuteChanged();
            }
        }

        /// <summary>
        /// Handles the background image path browse button's Click event
        /// </summary>
        private async void BrowseBackgroundImage_Click(object sender, RoutedEventArgs e)
        {
            if (await fileBrowserService.ShowAsync() == NotificationResult.OK && fileBrowserService.SelectedFiles != null)
            {
                fileBrowserService.Filter = new List<string>() { ".jpg", "jpeg", "png", "bmp", "webp" };
                (DataContext as MainWindowVM).BackgroundImagePath = fileBrowserService.SelectedFiles.Length > 3 ? fileBrowserService.SelectedFiles.Substring(1, fileBrowserService.SelectedFiles.Length - 2) : null;
                (DataContext as MainWindowVM).UpdatePlayerOptionsAsync_Command.RaiseCanExecuteChanged(); // ?
            }
        }

        /// <summary>
        /// Handles the media seach Back button's Click event
        /// </summary>
        private async void DisplayMainMenu_Click(object sender, RoutedEventArgs e)
        {
            await AnimateToMainMenuAsync();
        }

        #region Navigation
        /// <summary>
        /// Handles the viewmodel's Navigated event
        /// </summary>
        private async void MainWindowV_Navigated()
        {
            await opacityFadeOutAnimation.RunAsync(grdTransitionMask, animationPlaybackClock);
            grdTransitionMask.IsVisible = false;
            grdTransitionMask.IsVisible = false;
        }

        /// <summary>
        /// Handles main menu's items Click event
        /// </summary>
        /// <param name="sender">The main menu item that initiated the Click event</param>
        private async void MainMenu_Click(MediaTypeEntity sender)
        {
            grdTransitionMask.IsVisible = true;
            await opacityFadeInAnimation.RunAsync(grdTransitionMask, animationPlaybackClock);
            (DataContext as MainWindowVM).BeginMainMenuNavigation(sender);
        }

        /// <summary>
        /// Handles main menu's items MouseEnter event
        /// </summary>
        /// <param name="sender">The main menu item that initiated the MouseEnter event</param>
        private void MainMenu_MouseEnter(MediaTypeEntity sender)
        {
            (DataContext as MainWindowVM).DisplayMediaTypeInfo_Command.ExecuteSync(sender);
        }

        /// <summary>
        /// Handles main menu's items MouseLeave event
        /// </summary>
        private void MainMenu_MouseLeave()
        {
            (DataContext as MainWindowVM).DisplayMediaTypeInfo_Command.ExecuteSync(null);
        }

        /// <summary>
        /// Handles media items PointerReleased event
        /// </summary>
        private void MediaItem_PointerReleased(object? sender, PointerReleasedEventArgs e)
        {
            (DataContext as MainWindowVM).SourceMediaSelectedItem = (sender as Label).Tag as IMediaEntity;
        }

        /// <summary>
        /// Handles NavigateUp PointerReleased event
        /// </summary>
        private async void NavigateUp_PointerReleased(object? sender, PointerReleasedEventArgs e)
        {
            // start the fade in animation of the black mask
            grdTransitionMask.IsVisible = true;
            await opacityFadeInAnimation.RunAsync(grdTransitionMask, animationPlaybackClock);
            await (DataContext as MainWindowVM).NavigateMediaLibraryUpAsync();
        }
        #endregion

        ///// <summary>
        ///// Handles MainMenu RequestBringIntoView event
        ///// </summary>
        //private void MainMenu_RequestBringIntoView(object sender, RequestBringIntoViewEventArgs e)
        //{
        //    e.Handled = true;
        //}

        ///// <summary>
        ///// Handles MediaContextMenu ContextMenuOpening event
        ///// </summary>
        //private void MediaContextMenu_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        //{
        //    // the media context menu only makes sense on a listview item, make sure there is one selected
        //    if (lstMedia.SelectedItem == null)
        //        cntMedia.IsOpen = false;
        //}
        #endregion
    }
}
