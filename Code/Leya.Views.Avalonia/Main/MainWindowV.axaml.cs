/// Written by: Yulia Danilova
/// Creation Date: 09th of November, 2020
/// Purpose: View code behind for the MainWindow window
#region ========================================================================= USING =====================================================================================
using Avalonia;
using Avalonia.Animation;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.LogicalTree;
using Avalonia.Markup.Xaml;
using Avalonia.Markup.Xaml.Styling;
using Avalonia.Media;
using Avalonia.Styling;
using Avalonia.Threading;
using Avalonia.VisualTree;
using Leya.Infrastructure.Configuration;
using Leya.Infrastructure.Dialog;
using Leya.Infrastructure.Enums;
using Leya.Models.Common.Models.Media;
using Leya.ViewModels.Common.Models.Media;
using Leya.ViewModels.Common.ViewFactory;
using Leya.ViewModels.Main;
using Leya.Views.Common.Controls;
using Leya.Views.Common.Styles;
using Leya.Views.Startup;
using Leya.ViewsAvalonia;
using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Avalonia.Controls.Templates;
#endregion

namespace Leya.Views.Main
{
    public partial class MainWindowV : Window, IMainWindowView
    {
        private Animation? opacityFadeInAnimation;
        private Animation? opacityFadeOutAnimation;
        private Clock animationPlaybackClock = new Clock();

        #region ============================================================== FIELD MEMBERS ================================================================================
        Grid grdTransitionMask;
        Grid grdActors;
        Grid grdWindowDrag;
        Grid grdDescription;
        Image imgBanner;
        private readonly IFolderBrowserService folderBrowserService;
        private readonly IAppConfig appConfig;
        private static string lastUsedPath;
        private bool isWindowLoaded;
        private bool isWindowDragged;
        //private readonly IViewFactory viewFactory;
        private string currentLibraryName = string.Empty;
        #endregion

        #region ================================================================== CTOR =====================================================================================
        /// <summary>
        /// Default C-tor
        /// </summary>
        public MainWindowV()
        {
        }

        public MainWindowV(IFolderBrowserService folderBrowserService, IAppConfig appConfig)
        {
            AvaloniaXamlLoader.Load(this);
#if DEBUG
            this.AttachDevTools();
#endif
            grdTransitionMask = this.FindControl<Grid>("grdTransitionMask");
            grdActors = this.FindControl<Grid>("grdActors");
            imgBanner = this.FindControl<Image>("imgBanner");
            grdDescription = this.FindControl<Grid>("grdDescription");
            
            grdTransitionMask.Clock = animationPlaybackClock;
            CreateOpacityFadeInAnimation();
            CreateOpacityFadeOutAnimation();
            this.folderBrowserService = folderBrowserService;
            this.appConfig = appConfig;
            //opacityAnimation.RunAsync(grdMask, animationPlaybackClock);
            //animationPlaybackClock.PlayState = PlayState.Run;
        }
        #endregion

        #region ================================================================= METHODS =================================================================================== 
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
                // WPF bug - due to virtualization, sometimes _parameter can be automatically set to "DisconnectedItem", throwing exception
                await (DataContext as MainWindowVM).NavigateMediaLibraryDownAsync(mediaEntity);
            }
            else
                await (DataContext as MainWindowVM).NavigateMediaLibraryDownAsync(mediaEntity);
        }

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

        private void SlideScoreTitle(object sender, EventArgs e)
        {
            App.styles.SetTheme(App.styles.CurrentTheme switch
            {
                Themes.Dark => Themes.Light,
                Themes.Light => Themes.Dark,
                _ => throw new ArgumentOutOfRangeException(nameof(App.styles.CurrentTheme))
            });
            ff = !ff;
        }
        bool ff;

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
        /// Gets the DPI value for the screen identified by <paramref name="_screenIndex"/>
        /// </summary>
        /// <param name="_screenIndex">The screen for which to take the DPI</param>
        /// <returns>The DPI of the screen identified by <paramref name="_screenIndex"/></returns>
        //private Size GetDpiForScreen(int _screenIndex)
        //{
        //    WinForms.Screen.AllScreens[_screenIndex].GetDpi(DpiType.Effective, out uint _x, out uint _y);
        //    return new Size(_x, _y);
        //}
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
            isWindowLoaded = true;
            BoundsProperty.Changed.AddClassHandler<Window>((s, e) => Window_SizeChanged());


            // Avalonia bug: for some reason, neither of these work for finding a child control inside a template
            //grdWindowDrag = this.FindControl<Grid>("grdWindowDrag");
            //grdWindowDrag = ((IControl)this.GetVisualChildren().FirstOrDefault())?.FindControl<Grid>("grdWindowDrag");

            grdWindowDrag = (((VisualChildren[0] as Grid).Children[0] as DockPanel).Children[0] as Grid).Children[1] as Grid;
            grdWindowDrag.PointerPressed += (s, e) => isWindowDragged = true;
            grdWindowDrag.PointerReleased += (s, e) => isWindowDragged = false;
        }

        /// <summary>
        /// Handles window's SizeChanged event
        /// </summary>
        private async void Window_SizeChanged()
        {
            appConfig.Settings.MainWindowHeight = (int)Height;
            appConfig.Settings.MainWindowWidth = (int)Width;
            await appConfig.UpdateConfigurationAsync();
            if (imgBanner.Bounds.Width > 0 && grdDescription.Bounds.Width > 0)
            {
                imgBanner.Width = grdDescription.Bounds.Width - 30;
                imgBanner.Height = (grdDescription.Bounds.Width - 30) / 5.4;
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
            if (e.Key == Key.Escape && (DataContext as MainWindowVM).CurrentNavigationLevel != NavigationLevel.None)
            {
                // start the fade in animation of the black mask
                grdTransitionMask.IsVisible = true;
                await opacityFadeInAnimation.RunAsync(grdTransitionMask, animationPlaybackClock);
                await (DataContext as MainWindowVM).ExitMediaLibrary();
                // for some reason, the window closes when escape is pressed, so keep it handled
                e.Handled = true;
            }
        }

        ///// <summary>
        ///// Handles Banner SizeChanged event
        ///// </summary>
        //private void Banner_SizeChanged(object sender, SizeChangedEventArgs e)
        //{
        //    // keep the banner's aspect ratio
        //    imgBanner.Height = imgBanner.ActualWidth / 5.4;
        //}

        ///// <summary>
        ///// Handles MediaItem MouseUp event
        ///// </summary>
        //private void MediaItem_MouseUp(object sender, MouseButtonEventArgs e)
        //{
        //    try
        //    {
        //        // despite the selected item having a binding set in XAML, we also assign it manually here, so that the element gets
        //        // selected even when the user clicks on other controls that are on top of the listview item (the title label, etc)
        //        DependencyObject source = (DependencyObject)e.OriginalSource;
        //        while ((source != null) && !(source is ListViewItem))
        //            source = VisualTreeHelper.GetParent(source);
        //        // WPF bug - due to virtualization, sometimes _parameter can be automatically set to "DisconnectedItem", throwing exception
        //        if (source != null && !source.ToString().Contains("{DisconnectedItem}"))
        //            (DataContext as MainWindowVM).SourceMediaSelectedItem = ((IMediaEntity)((ListViewItem)source).Content);
        //    }
        //    catch { }
        //}

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

        ///// <summary>
        ///// Handles ShowCast MouseUp event
        ///// </summary>
        //private void ShowCast_MouseUp(object sender, MouseButtonEventArgs e)
        //{
        //    //NavigationLevel _current_navigation_level = (DataContext as MainWindowVM).CurrentNavigationLevel;
        //    //if (_current_navigation_level == NavigationLevel.Episode || _current_navigation_level == NavigationLevel.Movie)
        //    //    grdActors.Visibility = Visibility.Visible;
        //}

        ///// <summary>
        ///// Handles HideCast MouseUp event
        ///// </summary>
        //private void HideCast_MouseUp(object sender, MouseButtonEventArgs e)
        //{
        //    grdActors.Visibility = Visibility.Collapsed;
        //}

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
        /// Handles media items PointerReleased event
        /// </summary>
        private void MediaItem_PointerReleased(object? sender, PointerReleasedEventArgs e)
        {
            (DataContext as MainWindowVM).SourceMediaSelectedItem = (sender as Label).Tag as IMediaEntity;
        }

        /// <summary>
        /// Handles media item IsWatched Click event
        /// </summary>
        private async void IsWatched_Click(object? sender, RoutedEventArgs e)
        {
            if ((sender as CheckBox).Tag != null && (sender as CheckBox).Tag is IMediaEntity mediaEntity)
            {
                mediaEntity.IsWatched = (sender as CheckBox).IsChecked;
                await(DataContext as MainWindowVM).SetIsWatchedStatusAsync_Command.ExecuteAsync((sender as CheckBox).Tag as IMediaEntity);
            }
        }

        /// <summary>
        /// Handles media item IsWatched Click event
        /// </summary>
        private async void IsFavorite_Click(object? sender, RoutedEventArgs e)
        {
            if ((sender as CheckBox).Tag != null && (sender as CheckBox).Tag is IMediaEntity mediaEntity)
            {
                mediaEntity.IsFavorite = (sender as CheckBox).IsChecked == true;
                await (DataContext as MainWindowVM).SetIsFavoriteStatusAsync_Command.ExecuteAsync((sender as CheckBox).Tag as IMediaEntity);
            }
        }

        /// <summary>
        /// Handles ShowCast PointerReleased event
        /// </summary>
        private void ShowCast_PointerReleased(object? sender, PointerReleasedEventArgs e)
        {
            NavigationLevel currentNavigationLevel = (DataContext as MainWindowVM).CurrentNavigationLevel;
            if (currentNavigationLevel == NavigationLevel.Episode || currentNavigationLevel == NavigationLevel.Movie)
                grdActors.IsVisible = true;
        }

        /// <summary>
        /// Handles NavigateUp MouseUp event
        /// </summary>
        private async void NavigateUp_PointerReleased(object? sender, PointerReleasedEventArgs e)
        {
            // start the fade in animation of the black mask
            grdTransitionMask.IsVisible = true;
            await opacityFadeInAnimation.RunAsync(grdTransitionMask, animationPlaybackClock);
            await (DataContext as MainWindowVM).NavigateMediaLibraryUpAsync();
        }

        public async Task<bool?> ShowDialog()
        {
            return await ShowDialog<bool?>(StartupV.Instance);
        }
        #endregion

        //#endregion

        ///// <summary>
        ///// Handles OptionsMenu MouseUp event
        ///// </summary>
        //private void OptionsMenu_MouseUp(object sender, MouseButtonEventArgs e)
        //{
        //    grdOptionsMenu.Visibility = Visibility.Collapsed;
        //    switch ((sender as Control).Tag.ToString())
        //    {
        //        case "Media":
        //            //grdMediaTypesOptions.Visibility = Visibility.Visible;
        //            //(DataContext as MainWindowVM).GetMediaTypes();
        //            break;
        //        case "Interface":
        //            //navOptions.Source = new Uri(@"pack://application:,,,/Leya;component/Options/OptionsInterfaceV.xaml", UriKind.Absolute);
        //            break;
        //        case "Player":
        //            //navOptions.Source = new Uri(@"pack://application:,,,/Leya;component/Options/OptionsPlayerV.xaml", UriKind.Absolute);
        //            break;
        //        case "System Info":
        //            //navOptions.Source = new Uri(@"pack://application:,,,/Leya;component/Options/OptionsSystemInfoV.xaml", UriKind.Absolute);
        //            break;
        //        case "System":
        //            //navOptions.Source = new Uri(@"pack://application:,,,/Leya;component/Options/OptionsSystemV.xaml", UriKind.Absolute);
        //            break;
        //    }
        //}

        ///// <summary>
        ///// Handles Security MouseUp event
        ///// </summary>
        //private void Security_MouseUp(object sender, MouseButtonEventArgs e)
        //{
        //    bool isOpened = false;
        //    // iterate all opened windows
        //    foreach (object window in Application.Current.Windows)
        //    {
        //        // check if a Change Password view is already opened
        //        if (window is ChangePasswordV)
        //        {
        //            // brind the Change Password window to front
        //            ((Window)window).WindowState = WindowState.Normal;
        //            ((Window)window).Activate();
        //            isOpened = true;
        //            break;
        //        }
        //    }
        //    // if no Change Password window is opened, open a new one
        //    if (!isOpened)
        //        viewFactory.CreateView<IChangePasswordView>().ShowDialog();
        //}

        //#region Options Media
        ///// <summary>
        ///// Handles Media SizeChanged event
        ///// </summary>
        //private void Media_SizeChanged(object sender, SizeChangedEventArgs e)
        //{
        //    ListView listView = sender as ListView;
        //    GridView gridView = listView.View as GridView;
        //    double workingWidth = listView.ActualWidth - 35;
        //    gridView.Columns[0].Width = workingWidth * 0.75;
        //    gridView.Columns[1].Width = workingWidth * 0.20;
        //    gridView.Columns[2].Width = workingWidth * 0.05;
        //}

        ///// <summary>
        ///// Handles SourceMedia SizeChanged event
        ///// </summary>
        //private void SourceMedia_SizeChanged(object sender, SizeChangedEventArgs e)
        //{
        //    ListView listView = sender as ListView;
        //    GridView gridView = listView.View as GridView;
        //    double workingWidth = listView.ActualWidth - 35;
        //    gridView.Columns[0].Width = workingWidth * 0.95;
        //    gridView.Columns[1].Width = workingWidth * 0.05;
        //}

        /// <summary>
        /// Handles the Browse button Click event
        /// </summary>
        private async void Browse_Click(object sender, RoutedEventArgs e)
        {
            // display the open folder dialog
            folderBrowserService.AllowMultiselection = true;
            if (await folderBrowserService.Show() == NotificationResult.OK)
            {
                string filename = folderBrowserService.SelectedDirectories;
                // invoke the view model's method for adding a new media source
                if (!string.IsNullOrEmpty(filename))
                    await (DataContext as MainWindowVM).AddMediaSourceAsync_Command.ExecuteAsync(filename);
            }
        }
        #endregion
    }
}
