/// Written by: Yulia Danilova
/// Creation Date: 09th of November, 2020
/// Purpose: View code behind for the MainWindow window
#region ========================================================================= USING =====================================================================================
using Avalonia;
using Avalonia.Animation;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Markup.Xaml.Styling;
using Avalonia.Media;
using Avalonia.Styling;
using Avalonia.Threading;
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
using System.Runtime.InteropServices;
using System.Threading.Tasks;
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
        private readonly IFolderBrowserService folderBrowserService;
        private static string lastUsedPath;
        private const int ABM_GETTASKBARPOS = 5;
        //private readonly IViewFactory viewFactory;
        //private readonly IFolderBrowserService folderBrowserService;
        private string currentLibraryName = string.Empty;

        [DllImport("shell32.dll")]
        private static extern IntPtr SHAppBarMessage(int msg, ref APPBARDATA data);
        private struct APPBARDATA
        {
            public int cbSize;
            public IntPtr hWnd;
            public int uCallbackMessage;
            public int uEdge;
            public RECT rc;
            public IntPtr lParam;
        }

        private struct RECT
        {
            public int left, top, right, bottom;
        }
        #endregion

        #region ================================================================== CTOR =====================================================================================
        /// <summary>
        /// Default C-tor
        /// </summary>
        public MainWindowV()
        {
        }

        public MainWindowV(IFolderBrowserService folderBrowserService)
        {
            AvaloniaXamlLoader.Load(this);
#if DEBUG
            this.AttachDevTools();
#endif
            grdTransitionMask = this.FindControl<Grid>("grdTransitionMask");
       
            grdTransitionMask.Clock = animationPlaybackClock;
            CreateOpacityFadeInAnimation();
            CreateOpacityFadeOutAnimation();
            this.folderBrowserService = folderBrowserService;
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
                await(DataContext as MainWindowVM).NavigateMediaLibraryDownAsync(mediaEntity);
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
            //// get the size of the structure that contains the information about the system appbar message
            //APPBARDATA _data = new APPBARDATA();
            //_data.cbSize = Marshal.SizeOf(_data);
            //// send an appbar message to the system and retrieve the bounding rectangle of the Windows taskbar
            //SHAppBarMessage(ABM_GETTASKBARPOS, ref _data);
            //int _screen_index = 1;
            //double _horizontal_offset = 0;
            //// get the sum of the horizontal resolutions of all displays with a lower index than the index on which the window is displayed
            //// (this will take the horizontal width by taking into account windows scaling)
            //for (int i = 0; i < _screen_index; i++)
            //    _horizontal_offset += WinForms.Screen.AllScreens[i].Bounds.Width / (GetDpiForScreen(i).Width / 96);
            //// get the current screen
            //WinForms.Screen _screen = WinForms.Screen.AllScreens[_screen_index];
            //Top = _screen.Bounds.Top;
            //// the left position of the window starts at the horizontal offset (Edit: replaced by user setting!)
            //Left = MainWindowVM.config.Settings.DisplayOffset; // = _horizontal_offset;
            //Width = _screen.Bounds.Width / (GetDpiForScreen(_screen_index).Width / 96);
            //// for height, take into account the taskbar too
            //Height = _screen.Bounds.Height / (GetDpiForScreen(_screen_index).Height / 96) - ((_data.rc.bottom - _data.rc.top) / (GetDpiForScreen(_screen_index).Height / 96));
            //// if there is a weather.com specified in settings, get the weather info from that link
            //if (!string.IsNullOrEmpty(MainWindowVM.config.Settings.WeatherUrl))
            //    await (DataContext as MainWindowVM).GetWeatherInfoAsync(MainWindowVM.config.Settings.WeatherUrl);
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
        ///// Handles media SizeChanged event
        ///// </summary>
        //private void MediaSizeChanged(object sender, SizeChangedEventArgs e)
        //{
        //    ListView listview = sender as ListView;
        //    GridView gridview = listview.View as GridView;
        //    double width = listview.ActualWidth - 35;
        //    gridview.Columns[0].Width = width * 0.10;
        //    gridview.Columns[1].Width = width * 0.63;
        //    gridview.Columns[2].Width = width * 0.10;
        //    gridview.Columns[3].Width = width * 0.08;
        //    gridview.Columns[4].Width = width * 0.08;
        //}

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
        /// Handle's main menu's items Click event
        /// </summary>
        /// <param name="sender">The main menu item that initiated the Click event</param>
        private async void MainMenu_Click(MediaTypeEntity sender)
        {
            grdTransitionMask.IsVisible = true;
            await opacityFadeInAnimation.RunAsync(grdTransitionMask, animationPlaybackClock);
            (DataContext as MainWindowVM).BeginMainMenuNavigation(sender);
        }

        private void MediaItem_PointerReleased(object? sender, PointerReleasedEventArgs e)
        {
            IMediaEntity mediaEntity = (sender as Label).Tag as IMediaEntity;
        }

        /// <summary>
        /// Handles NavigateUp MouseUp event
        /// </summary>
        private async void NavigateUp_PointerReleased(object? sender, PointerReleasedEventArgs e)
        {
            // start the fade in animation of the black mask
            grdTransitionMask.IsVisible = true;
            await opacityFadeInAnimation.RunAsync(grdTransitionMask, animationPlaybackClock);
            await(DataContext as MainWindowVM).NavigateMediaLibraryUpAsync();
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

        ///// <summary>
        ///// Handles AddNewMedia button Click event
        ///// </summary>
        //private void AddNewMedia_Click(object sender, RoutedEventArgs e)
        //{
        //    grdMediaSources.Visibility = Visibility.Visible;
        //}
        #endregion
    }
}
