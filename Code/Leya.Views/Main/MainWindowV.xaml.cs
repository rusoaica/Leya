/// Written by: Yulia Danilova
/// Creation Date: 09th of November, 2020
/// Purpose: View code behind for the MainWindow window
#region ========================================================================= USING =====================================================================================
using System;
using System.IO;
using System.Windows;
using Leya.ViewModels;
using Leya.Views.Common;
//using Leya.Models.Media;
using System.Windows.Media;
using Leya.ViewModels.Main;
using System.Windows.Input;
using System.Configuration;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Collections.Generic;
using System.Windows.Media.Imaging;
//using Leya.ViewModels.Common.Enums;
using System.Runtime.InteropServices;
//using WinForms = System.Windows.Forms;
using Leya.Views.Common.Dispatcher;
using Leya.Infrastructure.Enums;
using Leya.Views.Options;
using Leya.Models.Common.Models.Media;
using System.Windows.Media.Animation;
using Leya.ViewModels.Common.ViewFactory;
using Leya.ViewModels.Options;
using Leya.Views.Register;
using Leya.ViewModels.Register;
using Leya.Infrastructure.Dialog;
#endregion

namespace Leya.Views.Main
{
    /// <summary>
    /// Interaction logic for MainWindowV.xaml
    /// </summary>
    public partial class MainWindowV : Window, IMainWindowView
    {
        #region ============================================================== FIELD MEMBERS ================================================================================
        private static string lastUsedPath;
        private const int ABM_GETTASKBARPOS = 5;
        private readonly IViewFactory viewFactory;
        private readonly IFolderBrowserService folderBrowserService;
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
        public MainWindowV(IViewFactory viewFactory, IFolderBrowserService folderBrowserService)
        {
            InitializeComponent();
            this.viewFactory = viewFactory;
            this.folderBrowserService = folderBrowserService;
            //// get the saved last used path in dialogs such as open/save file/folder
            //lastUsedPath = Views.Properties.Settings.Default.LastSelectedFolder;
            //if (string.IsNullOrEmpty(lastUsedPath) || !Directory.Exists(lastUsedPath))
            //    lastUsedPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            //if (!string.IsNullOrEmpty(MainWindowVM.config.Settings.BackgroundImagePath) && File.Exists(MainWindowVM.config.Settings.BackgroundImagePath))
            //    imgBackground.Source = new BitmapImage(new Uri(MainWindowVM.config.Settings.BackgroundImagePath));
        }
        #endregion

        #region ================================================================= METHODS =================================================================================== 
        /// <summary>
        /// Finds an UI visual children
        /// </summary>
        /// <typeparam name="T">The type of children to find</typeparam>
        /// <param name="dependencyObject">The object whose children will be searched</param>
        /// <returns>One or more children of <paramref name="dependencyObject"/>, of type <typeparamref name="T"/></returns>
        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject dependencyObject) where T : DependencyObject
        {
            if (dependencyObject != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(dependencyObject); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(dependencyObject, i);
                    if (child != null && child is T type)
                        yield return type;
                    foreach (T childOfChild in FindVisualChildren<T>(child))
                        yield return childOfChild;
                }
            }
        }

        /// <summary>
        /// Finds an UI visual parent
        /// </summary>
        /// <typeparam name="T">The type of parent to find</typeparam>
        /// <param name="dependencyObject">The object whose parent will be searched</param>
        /// <returns>One or more parents of <paramref name="dependencyObject"/>, of type <typeparamref name="T"/></returns>
        public static T FindVisualParent<T>(FrameworkElement dependencyObject) where T : FrameworkElement
        {
            if (dependencyObject != null)
            {
                DependencyObject parent = VisualTreeHelper.GetParent(dependencyObject);
                if (parent != null && !(parent is T))
                    return FindVisualParent<T>((FrameworkElement)parent);
                return (T)parent;
            }
            else
                return null;
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
        /// Handles the Window ContentRendered event
        /// </summary>
        private async void Window_ContentRendered(object sender, EventArgs e)
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
        /// Handles main menu's scrollviewer PreviewMouseWheel event
        /// </summary>
        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta < 0) // mouse wheel down
            {
                // while the scrollviewer's horizontal offset minus the mouse wheel's delta (which is negative) is still greater than 0, scroll towards right
                if (scrMainMenu.HorizontalOffset - e.Delta > 0)
                    scrMainMenu.ScrollToHorizontalOffset(scrMainMenu.HorizontalOffset - e.Delta);
                else
                    scrMainMenu.ScrollToLeftEnd();
            }
            else // mouse wheel up
            {
                // while the scrollviewer's extend width is still greater than the scrollviewer's horizontal offset minus the mouse wheel's delta (which is positive), scroll towards left
                if (scrMainMenu.ExtentWidth > scrMainMenu.HorizontalOffset - e.Delta)
                    scrMainMenu.ScrollToHorizontalOffset(scrMainMenu.HorizontalOffset - e.Delta);
                else
                    scrMainMenu.ScrollToRightEnd();
            }
            UpdateLayout();
        }

        /// <summary>
        /// Handles media SizeChanged event
        /// </summary>
        private void MediaSizeChanged(object sender, SizeChangedEventArgs e)
        {
            ListView listview = sender as ListView;
            GridView gridview = listview.View as GridView;
            double width = listview.ActualWidth - 35;
            gridview.Columns[0].Width = width * 0.10;
            gridview.Columns[1].Width = width * 0.63;
            gridview.Columns[2].Width = width * 0.10;
            gridview.Columns[3].Width = width * 0.08;
            gridview.Columns[4].Width = width * 0.08;
        }

        /// <summary>
        /// Handles Banner SizeChanged event
        /// </summary>
        private void Banner_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            // keep the banner's aspect ratio
            imgBanner.Height = imgBanner.ActualWidth / 5.4;
        }

        /// <summary>
        /// Handles MediaItem MouseUp event
        /// </summary>
        private void MediaItem_MouseUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                // despite the selected item having a binding set in XAML, we also assign it manually here, so that the element gets
                // selected even when the user clicks on other controls that are on top of the listview item (the title label, etc)
                DependencyObject source = (DependencyObject)e.OriginalSource;
                while ((source != null) && !(source is ListViewItem))
                    source = VisualTreeHelper.GetParent(source);
                // WPF bug - due to virtualization, sometimes _parameter can be automatically set to "DisconnectedItem", throwing exception
                if (source != null && !source.ToString().Contains("{DisconnectedItem}"))
                    (DataContext as MainWindowVM).SourceMediaSelectedItem = ((IMediaEntity)((ListViewItem)source).Content);
            }
            catch { }
        }

        /// <summary>
        /// Handles MainMenu RequestBringIntoView event
        /// </summary>
        private void MainMenu_RequestBringIntoView(object sender, RequestBringIntoViewEventArgs e)
        {
            e.Handled = true;
        }

        /// <summary>
        /// Handles MediaContextMenu ContextMenuOpening event
        /// </summary>
        private void MediaContextMenu_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            // the media context menu only makes sense on a listview item, make sure there is one selected
            if (lstMedia.SelectedItem == null)
                cntMedia.IsOpen = false;
        }

        /// <summary>
        /// Handles ShowCast MouseUp event
        /// </summary>
        private void ShowCast_MouseUp(object sender, MouseButtonEventArgs e)
        {
            //NavigationLevel _current_navigation_level = (DataContext as MainWindowVM).CurrentNavigationLevel;
            //if (_current_navigation_level == NavigationLevel.Episode || _current_navigation_level == NavigationLevel.Movie)
            //    grdActors.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Handles HideCast MouseUp event
        /// </summary>
        private void HideCast_MouseUp(object sender, MouseButtonEventArgs e)
        {
            grdActors.Visibility = Visibility.Collapsed;
        }

        #region Navigation
        /// <summary>
        /// ~~~Handles the viewmodel's Navigated event
        /// </summary>
        private void MainWindowV_Navigated()
        {
            // start the fade out animation of the black mask
            DoubleAnimation animation = new DoubleAnimation
            {
                From = 1,
                To = 0,
                Duration = TimeSpan.FromSeconds(1),
                FillBehavior = FillBehavior.Stop
            };
            animation.Completed += (s, e) =>
            {
                grdMask.Visibility = Visibility.Collapsed;
            };
            grdMask.BeginAnimation(OpacityProperty, animation);
        }

        /// <summary>
        /// ~~~Handles window KeyUp event
        /// </summary>
        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            // if the user presses Escape, exit any view and return to main menu
            if (e.Key == Key.Escape && (DataContext as MainWindowVM).CurrentNavigationLevel != NavigationLevel.None)
            {
                // start the fade in animation of the black mask
                grdMask.Visibility = Visibility.Visible;
                DoubleAnimation animation = new DoubleAnimation
                {
                    From = 0,
                    To = 1,
                    Duration = TimeSpan.FromSeconds(0.5),
                    FillBehavior = FillBehavior.HoldEnd
                };
                animation.Completed += async (s, a) =>
                {
                    await (DataContext as MainWindowVM).ExitMediaLibrary();
                };
                grdMask.BeginAnimation(OpacityProperty, animation);
                // for some reason, the window closes when escape is pressed, so keep it handled
                e.Handled = true;
            }
        }

        /// <summary>
        /// ~~~Handles NavigateUp MouseUp event
        /// </summary>
        private void NavigateUp_MouseUp(object sender, MouseButtonEventArgs e)
        {
            // start the fade in animation of the black mask
            grdMask.Visibility = Visibility.Visible;
            DoubleAnimation animation = new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromSeconds(0.5),
                FillBehavior = FillBehavior.HoldEnd
            };
            animation.Completed += async (s, a) =>
            {
                await (DataContext as MainWindowVM).NavigateMediaLibraryUpAsync();
            };
            grdMask.BeginAnimation(OpacityProperty, animation);
        }

        /// <summary>
        /// ~~~Handles the MenuItem's MouseDown event
        /// </summary>
        private void MenuItem_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // start the fade in animation of the black mask
            grdMask.Visibility = Visibility.Visible;
            DoubleAnimation animation = new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromSeconds(0.5),
                FillBehavior = FillBehavior.HoldEnd
            };
            animation.Completed += (s, a) =>
            {
                // get the parent listviewitem of the double clicked element
                DependencyObject source = (DependencyObject)e.OriginalSource;
                while ((source != null) && !(source is ListViewItem))
                    source = VisualTreeHelper.GetParent(source);
                // WPF bug - due to virtualization, sometimes _parameter can be automatically set to "DisconnectedItem", throwing exception
                if (source != null && !source.ToString().Contains("{DisconnectedItem}"))
                    (DataContext as MainWindowVM).BeginMainMenuNavigation(((MediaTypeEntity)((ListViewItem)source).Content));
            };
            grdMask.BeginAnimation(OpacityProperty, animation);
        }

        /// <summary>
        /// ~~~Handles MediaItem's MouseDoubleClick event
        /// </summary>
        private async void MediaItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            NavigationLevel navigationLevel = (DataContext as MainWindowVM).CurrentNavigationLevel;
            // get the parent listviewitem of the double clicked element
            DependencyObject source = (DependencyObject)e.OriginalSource;
            while ((source != null) && !(source is ListViewItem))
                source = VisualTreeHelper.GetParent(source);
            if (navigationLevel != NavigationLevel.Episode && navigationLevel != NavigationLevel.Song && navigationLevel != NavigationLevel.Movie)
            {
                // start the fade in animation of the black mask
                grdMask.Visibility = Visibility.Visible;
                DoubleAnimation animation = new DoubleAnimation
                {
                    From = 0,
                    To = 1,
                    Duration = TimeSpan.FromSeconds(0.5),
                    FillBehavior = FillBehavior.HoldEnd
                };
                animation.Completed += async (s, a) =>
                {
                    // WPF bug - due to virtualization, sometimes _parameter can be automatically set to "DisconnectedItem", throwing exception
                    if (source != null && !source.ToString().Contains("{DisconnectedItem}"))
                        await (DataContext as MainWindowVM).NavigateMediaLibraryDownAsync(((IMediaEntity)((ListViewItem)source).Content));
                };
                grdMask.BeginAnimation(OpacityProperty, animation);
            }
            else
            {
                if (source != null && !source.ToString().Contains("{DisconnectedItem}"))
                    await (DataContext as MainWindowVM).NavigateMediaLibraryDownAsync(((IMediaEntity)((ListViewItem)source).Content));
            }
        }
        #endregion

        #endregion

        /// <summary>
        /// Handles OptionsMenu MouseUp event
        /// </summary>
        private void OptionsMenu_MouseUp(object sender, MouseButtonEventArgs e)
        {
            grdOptionsMenu.Visibility = Visibility.Collapsed;
            switch ((sender as Control).Tag.ToString())
            {
                case "Media":
                    //grdMediaOptions.Visibility = Visibility.Visible;
                    //(DataContext as MainWindowVM).GetMediaTypes();
                    break;
                case "Interface":
                    //navOptions.Source = new Uri(@"pack://application:,,,/Leya;component/Options/OptionsInterfaceV.xaml", UriKind.Absolute);
                    break;
                case "Player":
                    //navOptions.Source = new Uri(@"pack://application:,,,/Leya;component/Options/OptionsPlayerV.xaml", UriKind.Absolute);
                    break;
                case "System Info":
                    //navOptions.Source = new Uri(@"pack://application:,,,/Leya;component/Options/OptionsSystemInfoV.xaml", UriKind.Absolute);
                    break;
                case "System":
                    //navOptions.Source = new Uri(@"pack://application:,,,/Leya;component/Options/OptionsSystemV.xaml", UriKind.Absolute);
                    break;
            }
        }

        /// <summary>
        /// Handles Security MouseUp event
        /// </summary>
        private async void Security_MouseUp(object sender, MouseButtonEventArgs e)
        {
            bool isOpened = false;
            // iterate all opened windows
            foreach (object window in Application.Current.Windows)
            {
                // check if a Change Password view is already opened
                if (window is ChangePasswordV)
                {
                    // brind the Change Password window to front
                    ((Window)window).WindowState = WindowState.Normal;
                    ((Window)window).Activate();
                    isOpened = true;
                    break;
                }
            }
            // if no Change Password window is opened, open a new one
            if (!isOpened)
                await viewFactory.CreateView<IChangePasswordView>().ShowDialog();
        }

        #region Options Media
        /// <summary>
        /// Handles Media SizeChanged event
        /// </summary>
        private void Media_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ListView listView = sender as ListView;
            GridView gridView = listView.View as GridView;
            double workingWidth = listView.ActualWidth - 35;
            gridView.Columns[0].Width = workingWidth * 0.75;
            gridView.Columns[1].Width = workingWidth * 0.20;
            gridView.Columns[2].Width = workingWidth * 0.05;
        }

        /// <summary>
        /// Handles SourceMedia SizeChanged event
        /// </summary>
        private void SourceMedia_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ListView listView = sender as ListView;
            GridView gridView = listView.View as GridView;
            double workingWidth = listView.ActualWidth - 35;
            gridView.Columns[0].Width = workingWidth * 0.95;
            gridView.Columns[1].Width = workingWidth * 0.05;
        }

        /// <summary>
        /// Handles MediaType MouseDoubleClick event
        /// </summary>
        private void MediaType_MouseDoucleClick(object sender, MouseButtonEventArgs e)
        {
            // only display the panel for media type sources if the double click happened on a listview item of media types listview
            DependencyObject originalSource = (DependencyObject)e.OriginalSource;
            while ((originalSource != null) && !(originalSource is ListViewItem))
                originalSource = VisualTreeHelper.GetParent(originalSource);
            if (originalSource != null)
                grdBackground.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Handles the Browse button Click event
        /// </summary>
        private async void Browse_MouseUp(object sender, RoutedEventArgs e)
        {
            // display the open folder dialog
            if (await folderBrowserService.Show() == NotificationResult.OK)
            {
                string filename = folderBrowserService.SelectedDirectories;
                // invoke the view model's method for adding a new media source
                if (!string.IsNullOrEmpty(filename))
                    await (DataContext as MainWindowVM).AddMediaSourceAsync_Command.ExecuteAsync(filename);
            }
        }

        /// <summary>
        /// Handles AddNewMedia button Click event
        /// </summary>
        private void AddNewMedia_Click(object sender, RoutedEventArgs e)
        {
            grdBackground.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Handles the Cancel button Click event
        /// </summary>
        private void CloseAddMedia_Click(object sender, RoutedEventArgs e)
        {
            // hide the panel for media type sources
            grdBackground.Visibility = Visibility.Collapsed;
            (DataContext as MainWindowVM).IsHelpButtonVisible = false;
        }

        Task<bool?> IView.ShowDialog()
        {
            throw new NotImplementedException();
        }

        public Task<TResult> ShowDialog<TResult>()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
