/// Written by: Yulia Danilova
/// Creation Date: 18th of November, 2020
/// Purpose: View code behind for the OptionsMedia page
#region ========================================================================= USING =====================================================================================
using System.Windows;
using Leya.Views.Main;
using Leya.ViewModels.Main;
using System.Windows.Media;
using Leya.ViewModels.Options;
using System.Windows.Controls;
using System.Threading.Tasks;
using Leya.ViewModels.Common.ViewFactory;
#endregion

namespace Leya.Views.Options
{
    /// <summary>
    /// Interaction logic for OptionsMediaV.xaml
    /// </summary>
    public partial class OptionsMediaV : Window, IOptionsMediaView
    {
        #region ============================================================== FIELD MEMBERS ================================================================================
        private Window mainWindow;
        private readonly IOptionsMediaVM optionsMediaVM;
        #endregion

        #region ================================================================== CTOR =====================================================================================
        /// <summary>
        /// Default C-tor
        /// </summary>
        public OptionsMediaV()
        {
            InitializeComponent();
            this.optionsMediaVM = optionsMediaVM;
        }
        #endregion

        #region ============================================================= EVENT HANDLERS ================================================================================
        /// <summary>
        /// Handles the ValidationChanged event of viewmodel
        /// </summary>
        private void OptionsMediaV_ValidationChanged(bool _isValid)
        {
            // if the current page's view model validation is invalid, display the help button on the main window of the application
            MainWindowVM mainWindowVM = mainWindow.DataContext as MainWindowVM;
            mainWindowVM.WindowHelp = (DataContext as OptionsMediaVM).WindowHelp;
            mainWindowVM.IsHelpButtonVisible = !_isValid;
        }

        /// <summary>
        /// Handles Media SizeChanged event
        /// </summary>
        private void Media_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ListView _list_view = sender as ListView;
            GridView _grid_view = _list_view.View as GridView;
            double _working_width = _list_view.ActualWidth - 35;
            _grid_view.Columns[0].Width = _working_width * 0.75;
            _grid_view.Columns[1].Width = _working_width * 0.20;
            _grid_view.Columns[2].Width = _working_width * 0.05;
        }

        /// <summary>
        /// Handles SourceMedia SizeChanged event
        /// </summary>
        private void SourceMedia_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ListView _list_view = sender as ListView;
            GridView _grid_view = _list_view.View as GridView;
            double _working_width = _list_view.ActualWidth - 35;
            _grid_view.Columns[0].Width = _working_width * 0.95;
            _grid_view.Columns[1].Width = _working_width * 0.05;
        }

        /// <summary>
        /// Handles the Cancel button Click event
        /// </summary>
        private void CloseAddMedia_Click(object sender, RoutedEventArgs e)
        {
            // hide the panel for media type sources
            grdBackground.Visibility = Visibility.Collapsed;
            (mainWindow.DataContext as MainWindowVM).IsHelpButtonVisible = false;
        }

        /// <summary>
        /// Handles the Browse button Click event
        /// </summary>
        private void Browse_MouseUp(object sender, RoutedEventArgs e)
        {
            //// display the open folder dialog
            //string _filename = MainWindowV.ShowOpenFolderDialog();
            //// invoke the view model's method for adding a new media source
            //if (!string.IsNullOrEmpty(_filename))
            //    (DataContext as OptionsMediaVM).AddMediaSource_Command.ExecuteSync(_filename);
        }

        /// <summary>
        /// Handles AddNewMedia button Click event
        /// </summary>
        private void AddNewMedia_Click(object sender, RoutedEventArgs e)
        {
            grdBackground.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Handles MediaType MouseDoubleClick event
        /// </summary>
        private void MediaType_MouseDoucleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            // only display the panel for media type sources if the double click happened on a listview item of media types listview
            DependencyObject _original_source = (DependencyObject)e.OriginalSource;
            while ((_original_source != null) && !(_original_source is ListViewItem))
                _original_source = VisualTreeHelper.GetParent(_original_source);
            if (_original_source != null)
                grdBackground.Visibility = Visibility.Visible;
        }
        #endregion

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            optionsMediaVM.ValidationChanged += OptionsMediaV_ValidationChanged;
            // get and store the main window of the application, so we can later use its title bar's help button
            foreach (object window in Application.Current.Windows)
                if (window is MainWindowV wnd)
                    mainWindow = wnd;
            grdBackground.Visibility = Visibility.Collapsed;
        }

        Task<bool?> IView.ShowDialog()
        {
            throw new System.NotImplementedException();
        }
    }
}
