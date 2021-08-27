/// Written by: Yulia Danilova
/// Creation Date: 11th of November, 2020
/// Purpose: View Model for the main application Window
#region ========================================================================= USING =====================================================================================
using System;
using System.Linq;
using System.Reflection;
using System.Diagnostics;
using System.Threading.Tasks;
using Leya.ViewModels.Options;
using Leya.Infrastructure.Enums;
using System.Collections.Generic;
using Leya.Models.Core.Navigation;
using Leya.ViewModels.Common.MVVM;
using Leya.Models.Core.MediaLibrary;
using System.Collections.ObjectModel;
using Leya.Models.Common.Models.Media;
using Leya.Infrastructure.Notification;
using Leya.Models.Common.Models.Common;
using Leya.ViewModels.Common.ViewFactory;
using Leya.ViewModels.Common.Models.Media;
using System.IO;
using System.Text.RegularExpressions;
using Leya.Models.Core.Options;
#endregion

namespace Leya.ViewModels.Main
{
    public class MainWindowVM : BaseModel, IMainWindowVM
    {
        #region ============================================================== FIELD MEMBERS ================================================================================
        public event Action Navigated;
        public event Action<bool> ValidationChanged;

        private readonly IViewFactory viewFactory;
        private readonly IMediaLibrary mediaLibrary;
        private readonly IMediaLibraryNavigation mediaLibraryNavigation;
        private readonly IOptionsMedia optionsMedia;
        #endregion

        #region ============================================================= BINDING COMMANDS ==============================================================================
        public IAsyncCommand PlayAllMediaAsync_Command { get; private set; }
        public IAsyncCommand SaveMediaLibraryAsync_Command { get; private set; }
        public IAsyncCommand Search_EnterKeyUpAsync_Command { get; private set; }
        public IAsyncCommand Search_DropDownClosingAsync_Command { get; private set; }
        public IAsyncCommand ViewOpenedAsync_Command { get; private set; }
        public IAsyncCommand<MediaEntity> ShowMediaCastAsync_Command { get; private set; }
        public IAsyncCommand<IMediaEntity> SetIsWatchedStatusAsync_Command { get; private set; }
        public IAsyncCommand<IMediaEntity> SetIsFavoriteStatusAsync_Command { get; private set; }
        public IAsyncCommand<MediaTypeEntity> DeleteMediaTypeAsync_Command { get; private set; }
        public ISyncCommand<MediaTypeEntity> GetMediaTypeSources_Command { get; private set; }
        public ISyncCommand<MediaTypeEntity> DisplayMediaTypeInfo_Command { get; private set; }
        public ISyncCommand<MediaTypeSourceEntity> DeleteMediaTypeSource_Command { get; private set; }
        public ISyncCommand<MediaTypeSourceEntity> OpenMediaSourceLocation_Command { get; private set; }
        public IAsyncCommand<string> AddMediaSourceAsync_Command { get; private set; }
        public ISyncCommand<decimal> DisplayOffset_ValueChanged_Command { get; private set; }
        public SyncCommand ShowMediaOptions_Command { get; private set; }
        public SyncCommand OpenMediaFolder_Command { get; private set; }
        public SyncCommand Filter_EnterKeyUp_Command { get; private set; }
        public SyncCommand SelectedMediaChanged_Command { get; private set; }
        public SyncCommand Filter_DropDownClosing_Command { get; private set; }
        public SyncCommand OpenLoggingDirectory_Command { get; private set; }
        public SyncCommand ResetAddMediaSourceElements_Command { get; private set; }
        public AsyncCommand RefreshMediaSourcesAsync_Command { get; private set; }
        public ISyncCommand ExitMediaTypeSourcesOptions_Command { get; private set; }
        public ISyncCommand ExitMediaTypesOptions_Command { get; private set; }
        //public AutoCompleteFilterPredicate<object> SearchMediaLibrary_Command { get; private set; }
        #endregion

        #region ============================================================ BINDING PROPERTIES ============================================================================= 
        private string version = "1.2.6.1";
        public string Version
        {
            get { return version; }
            set { version = value; Notify(); }
        }

        private string scannerOutput;
        public string ScannerOutput
        {
            get { return scannerOutput; }
            set { scannerOutput = value; Notify(); }
        }

        private string fanart;
        public string Fanart
        {
            get { return fanart; }
            set { fanart = value; Notify(); }
        }

        private string banner = string.Empty;
        public string Banner
        {
            get { return banner; }
            set { banner = value; Notify(); }
        }

        private string poster;
        public string Poster
        {
            get { return poster; }
            set { poster = value; Notify(); IsMediaCoverVisible = value != null; }
        }

        private string tagLine;
        public string TagLine
        {
            get { return tagLine; }
            set { tagLine = value; Notify(); }
        }

        private string mpaa = string.Empty;
        public string MPAA
        {
            get { return mpaa; }
            set { mpaa = value; Notify(); }
        }

        private string mediaCase = "bg-bluray.png";
        public string MediaCase
        {
            get { return mediaCase; }
            set { mediaCase = value; Notify(); }
        }

        private string writers = string.Empty;
        public string Writers
        {
            get { return writers; }
            set { writers = value; Notify(); }
        }

        private string director = string.Empty;
        public string Director
        {
            get { return director; }
            set { director = value; Notify(); }
        }

        private string filterText;
        public string FilterText
        {
            get { return filterText; }
            set { filterText = value; Notify(); }
        }


        private string currentStatus = "Ended";
        public string CurrentStatus
        {
            get { return currentStatus; }
            set { currentStatus = value; Notify(); }
        }

        private string localPath = @"D:\MULTIMEDIA\TV SHOWS\";
        public string LocalPath
        {
            get { return localPath; }
            set { localPath = value; Notify(); }
        }

        private string synopsis = string.Empty;
        public string Synopsis
        {
            get { return synopsis; }
            set { synopsis = value; Notify(); }
        }

        private string genre = string.Empty;
        public string Genre
        {
            get { return genre; }
            set { genre = value; Notify(); }
        }

        private string forecast;
        public string Forecast
        {
            get { return forecast; }
            set { forecast = value; Notify(); }
        }

        private string totalMediaCountType = "TV SHOWS:";
        public string TotalMediaCountType
        {
            get { return totalMediaCountType; }
            set { totalMediaCountType = value; Notify(); }
        }

        private string mediaName;
        public string MediaName
        {
            get { return mediaName; }
            set { mediaName = value?.ToUpper(); Notify(); RefreshMediaSourcesAsync_Command?.RaiseCanExecuteChanged(); }
        }

        private int totalMediaCount = 0;
        public int TotalMediaCount
        {
            get { return totalMediaCount; }
            set { totalMediaCount = value; Notify(); }
        }

        private int totalWatchedCount = 0;
        public int TotalWatchedCount
        {
            get { return totalWatchedCount; }
            set { totalWatchedCount = value; Notify(); }
        }

        private int totalUnwatchedCount = 0;
        public int TotalUnwatchedCount
        {
            get { return totalUnwatchedCount; }
            set { totalUnwatchedCount = value; Notify(); }
        }

        private int numberOfSeasons = 0;
        public int NumberOfSeasons
        {
            get { return numberOfSeasons; }
            set { numberOfSeasons = value; Notify(); }
        }

        private int numberOfUnwatchedEpisodes = 0;
        public int NumberOfUnwatchedEpisodes
        {
            get { return numberOfUnwatchedEpisodes; }
            set { numberOfUnwatchedEpisodes = value; Notify(); }
        }

        private int numberOfEpisodes = 0;
        public int NumberOfEpisodes
        {
            get { return numberOfEpisodes; }
            set { numberOfEpisodes = value; Notify(); }
        }

        private double celsius;
        public double Celsius
        {
            get { return celsius; }
            set { celsius = value; Notify(); }
        }

        private double fahrenheit;
        public double Fahrenheit
        {
            get { return fahrenheit; }
            set { fahrenheit = value; Notify(); }
        }

        private bool isMediaOptionsPanelVisible;
        public bool IsMediaOptionsPanelVisible
        {
            get { return isMediaOptionsPanelVisible; }
            set { isMediaOptionsPanelVisible = value; Notify(); }
        }

        private bool isMainMenuVisible = true;
        public bool IsMainMenuVisible
        {
            get { return isMainMenuVisible; }
            set { isMainMenuVisible = value; Notify(); }
        }

        private bool areActorsVisible = false;
        public bool AreActorsVisible
        {
            get { return areActorsVisible; }
            set { areActorsVisible = value; Notify(); }
        }

        private bool isNumberOfSeasonsVisible;
        public bool IsNumberOfSeasonsVisible
        {
            get { return isNumberOfSeasonsVisible; }
            set { isNumberOfSeasonsVisible = value; Notify(); }
        }

        private bool isScannerTextVisible = true;
        public bool IsScannerTextVisible
        {
            get { return isScannerTextVisible; }
            set { isScannerTextVisible = value; Notify(); }
        }

        private bool isTvShow;
        public bool IsTvShow
        {
            get { return isTvShow; }
            set { isTvShow = value; Notify(); }
        }

        private bool isMediaBluray = true;
        public bool IsMediaBluray
        {
            get { return isMediaBluray; }
            set { isMediaBluray = value; Notify(); }
        }

        private bool isFanartVisible = false;
        public bool IsFanartVisible
        {
            get { return isFanartVisible; }
            set { isFanartVisible = value; Notify(); }
        }

        private bool isMediaCoverVisible = false;
        public bool IsMediaCoverVisible
        {
            get { return isMediaCoverVisible; }
            set { isMediaCoverVisible = value; Notify(); }
        }

        private bool isBackNavigationPossible = false;
        public bool IsBackNavigationPossible
        {
            get { return isBackNavigationPossible; }
            set { isBackNavigationPossible = value; Notify(); }
        }

        private bool isMediaContainerVisible = false;
        public bool IsMediaContainerVisible
        {
            get { return isMediaContainerVisible; }
            set { isMediaContainerVisible = value; Notify(); }
        }

        private bool isMediaTypeSourcesOptionVisible = false;
        public bool IsMediaTypeSourcesOptionVisible
        {
            get { return isMediaTypeSourcesOptionVisible; }
            set { isMediaTypeSourcesOptionVisible = value; Notify(); }
        }

        private bool isMediaTypesOptionVisible;
        public bool IsMediaTypesOptionVisible
        {
            get { return isMediaTypesOptionVisible; }
            set { isMediaTypesOptionVisible = value; Notify(); }
        }

        private bool isOptionsContainerVisible = false;
        public bool IsOptionsContainerVisible
        {
            get { return isOptionsContainerVisible; }
            set { isOptionsContainerVisible = value; Notify(); }
        }

        private bool isMaskBackgroundVisible = false;
        public bool IsMaskBackgroundVisible
        {
            get { return isMaskBackgroundVisible; }
            set { isMaskBackgroundVisible = value; Notify(); }
        }

        private bool isWritersVisible;
        public bool IsWritersVisible
        {
            get { return isWritersVisible; }
            set { isWritersVisible = value; Notify(); }
        }

        private bool isDirectorVisible;
        public bool IsDirectorVisible
        {
            get { return isDirectorVisible; }
            set { isDirectorVisible = value; Notify(); }
        }

        //private bool isMediaContextMenuOpen;
        //public bool IsMediaContextMenuOpen
        //{
        //    get
        //    {
        //        if (SourceMediaSelectedItem == null)
        //            return false;
        //        return isMediaContextMenuOpen;
        //    }
        //    set
        //    {
        //        isMediaContextMenuOpen = value;
        //        if (SourceMediaSelectedItem == null)
        //            isMediaContextMenuOpen = false;
        //        Notify();
        //    }
        //}

        private DateTime premiered = DateTime.Now;
        public DateTime Premiered
        {
            get { return premiered; }
            set { premiered = value; Notify(); }
        }

        private DateTime added = DateTime.Now;
        public DateTime Added
        {
            get { return added; }
            set { added = value; Notify(); }
        }

        private TimeSpan runtime = TimeSpan.FromSeconds(0);
        public TimeSpan Runtime
        {
            get { return runtime; }
            set { runtime = value; Notify(); }
        }

        public DateTime CurrentTime
        {
            get { return DateTime.Now; }
        }

        private ObservableCollection<MediaTypeEntity> sourceMediaCategories = new ObservableCollection<MediaTypeEntity>();
        public ObservableCollection<MediaTypeEntity> SourceMediaCategories
        {
            get { return sourceMediaCategories; }
            set { sourceMediaCategories = value; Notify(); }
        }

        //private ObservableCollection<MediaTypeSourceEntity> sourceMediaCategorySources = new ObservableCollection<MediaTypeSourceEntity>();
        //public ObservableCollection<MediaTypeSourceEntity> SourceMediaCategorySources
        //{
        //    get { return sourceMediaCategorySources; }
        //    set { sourceMediaCategorySources = value; Notify(); RefreshMediaSourcesAsync_Command?.RaiseCanExecuteChanged(); }
        //}

        public ObservableCollection<MediaTypeSourceEntity> SourceMediaCategorySources
        {
            get { return new ObservableCollection<MediaTypeSourceEntity>(optionsMedia.SourceMediaCategorySources); }
            set
            {
                optionsMedia.SourceMediaCategorySources = value.ToList();
                Notify();
                RefreshMediaSourcesAsync_Command?.RaiseCanExecuteChanged();
            }
        }

        private ObservableCollection<MediaTypeEntity> sourceMediaTypes = new ObservableCollection<MediaTypeEntity>();
        public ObservableCollection<MediaTypeEntity> SourceMediaTypes
        {
            get { return sourceMediaTypes; }
            set { sourceMediaTypes = value; Notify(); }
        }

        private ObservableCollection<SearchEntity> sourceMediaCategoryTypes = new ObservableCollection<SearchEntity>();
        public ObservableCollection<SearchEntity> SourceMediaCategoryTypes
        {
            get { return sourceMediaCategoryTypes; }
            set { sourceMediaCategoryTypes = value; Notify(); }
        }

        private ObservableCollection<SearchEntity> sourceActors;
        public ObservableCollection<SearchEntity> SourceActors
        {
            get { return sourceActors; }
            set { sourceActors = value; Notify(); }
        }

        private ObservableCollection<SearchEntity> sourceSearch;
        public ObservableCollection<SearchEntity> SourceSearch
        {
            get { return sourceSearch; }
            set { sourceSearch = value; Notify(); }
        }

        private ObservableCollection<SearchEntity> sourceFilter;
        public ObservableCollection<SearchEntity> SourceFilter
        {
            get { return sourceFilter; }
            set { sourceFilter = value; Notify(); }
        }

        private ObservableCollection<IMediaEntity> sourceMedia;
        public ObservableCollection<IMediaEntity> SourceMedia
        {
            get { return sourceMedia; }
            set { sourceMedia = value; Notify(); }
        }

        private SearchEntity searchSelectedItem;
        public SearchEntity SearchSelectedItem
        {
            get { return searchSelectedItem; }
            set { searchSelectedItem = value; Notify(); }
        }

        private SearchEntity selectedMediaCategoryType;
        public SearchEntity SelectedMediaCategoryType
        {
            get { return selectedMediaCategoryType; }
            set { selectedMediaCategoryType = value; Notify(); RefreshMediaSourcesAsync_Command?.RaiseCanExecuteChanged(); }
        }

        private IMediaEntity sourceMediaSelectedItem;
        public IMediaEntity SourceMediaSelectedItem
        {
            get { return sourceMediaSelectedItem; }
            set { sourceMediaSelectedItem = value; Notify(); SelectedMediaChanged(); }
        }
        #endregion

        #region ================================================================ PROPERTIES =================================================================================
        public NavigationLevel CurrentNavigationLevel { get { return mediaLibraryNavigation.CurrentNavigationLevel; } }
        #endregion

        #region ================================================================== CTOR =====================================================================================
        /// <summary>
        /// Overload C-tor
        /// </summary>
        /// <param name="viewFactory">Injected abstract factory for creating views</param>
        /// <param name="mediaLibrary">Injected media library business model</param>
        /// <param name="mediaLibraryNavigation">Injected media library navigation business model</param>
        /// <param name="notificationService">Injected notification service</param>
        public MainWindowVM(IViewFactory viewFactory, IMediaLibrary mediaLibrary, IMediaLibraryNavigation mediaLibraryNavigation, INotificationService notificationService, IOptionsMedia optionsMedia)
        {
            this.viewFactory = viewFactory;
            this.mediaLibrary = mediaLibrary;
            this.notificationService = notificationService;
            this.mediaLibraryNavigation = mediaLibraryNavigation;
            this.optionsMedia = optionsMedia;
            this.mediaLibrary.MediaTypesLoaded += MediaLibrary_MediaTypesLoaded;
            this.mediaLibrary.LibraryLoaded += MediaLibrary_LibraryLoaded;
            //IsOptionsContainerVisible = true;
            //OpenMediaFolder_Command = new SyncCommand(OpenMediaFolder);
            //Filter_EnterKeyUp_Command = new SyncCommand(Filter_EnterKeyUp);
            //PlayAllMediaAsync_Command = new AsyncCommand(PlayAllMediaAsync);
            SelectedMediaChanged_Command = new SyncCommand(SelectedMediaChanged);
            SaveMediaLibraryAsync_Command = new AsyncCommand(SaveMediaLibraryAsync);
            //Filter_DropDownClosing_Command = new SyncCommand(Filter_DropDownClosing);
            //Search_EnterKeyUpAsync_Command = new AsyncCommand(Search_EnterKeyUpAsync);
            //ShowMediaCastAsync_Command = new AsyncCommand<MediaDisplayEntity>(ShowMediaCastAsync);
            DisplayMediaTypeInfo_Command = new SyncCommand<MediaTypeEntity>(DisplayMediaTypeStatistics);
            SetIsWatchedStatusAsync_Command = new AsyncCommand<IMediaEntity>(SetIsWatchedStatusAsync);
            ViewOpenedAsync_Command = new AsyncCommand(ViewOpenedAsync);
            ExitMediaTypeSourcesOptions_Command = new SyncCommand(ExitMediaTypeSourcesOptions);
            ExitMediaTypesOptions_Command = new SyncCommand(ExitMediaTypesOptions);
            //Search_DropDownClosingAsync_Command = new AsyncCommand(Search_DropDownClosingAsync);
            SetIsFavoriteStatusAsync_Command = new AsyncCommand<IMediaEntity>(SetIsFavoriteStatusAsync);
            //DisplayOffset_ValueChanged_Command = new SyncCommand<decimal>(DisplayOffset_ValueChanged);
            mediaLibraryNavigation.Navigated += InitiateNavigation;
            OpenLoggingDirectory_Command = new SyncCommand(OpenLoggingDirectory);
            // SearchMediaLibrary_Command = new AutoCompleteFilterPredicate<object>(SearchMediaLibrary);
            this.mediaLibraryNavigation.PropertyChanged += DomainModelPropertyChanged;
            this.optionsMedia.PropertyChanged += DomainModelPropertyChanged;
            AddMediaSourceAsync_Command = new AsyncCommand<string>(AddMediaTypeSourceAsync);
            RefreshMediaSourcesAsync_Command = new AsyncCommand(UpdateMediaLibraryAsync, ValidateAddMediaTypeSource);
            ResetAddMediaSourceElements_Command = new SyncCommand(ResetAddMediaSourceElements);
            DeleteMediaTypeAsync_Command = new AsyncCommand<MediaTypeEntity>(DeleteMediaTypeAsync);
            GetMediaTypeSources_Command = new SyncCommand<MediaTypeEntity>(GetMediaTypeSources);
            DeleteMediaTypeSource_Command = new SyncCommand<MediaTypeSourceEntity>(DeleteMediaTypeSource);
            OpenMediaSourceLocation_Command = new SyncCommand<MediaTypeSourceEntity>(optionsMedia.OpenMediaSourceLocation);
            ShowMediaOptions_Command = new SyncCommand(ShowMediaOptions);
            SourceMediaCategoryTypes = new ObservableCollection<SearchEntity>()
            {
                new SearchEntity() { Text = "TV SHOW" },
                new SearchEntity() { Text = "MOVIE" },
                new SearchEntity() { Text = "MUSIC" },
            };
        }


        #endregion

        #region ================================================================= METHODS ===================================================================================
        /// <summary>
        /// Handles the event risen when a domain model property changes its value and needs to notify the outside world of this change.
        /// Used in MVVM to trigger an update of the bounded properties, taking their values from the domain models corresponding properties
        /// </summary>
        /// <param name="propertyName">The name of the domain model property whose value changed</param>
        /// <param name="sender">The name of the class containing the property identified by <paramref name="propertyName"/></param>
        private void DomainModelPropertyChanged(string propertyName, string sender)
        {
            // get the readonly field that contains the property whose value changed and was notified by the event broadcaster
            FieldInfo diField = GetType().GetField(sender, BindingFlags.IgnoreCase | BindingFlags.NonPublic | BindingFlags.Instance);
            // get the value of the above property
            object fieldPropertyValue = diField.GetValue(this).GetType().GetProperty(propertyName).GetValue(diField.GetValue(this));
            // set the value of the property in this class that has the same name as the above property, using the above value
            PropertyInfo viewModelProperty = GetType().GetProperty(propertyName);
            if (viewModelProperty.CanWrite)
            {
                // handle List to Obervable transition
                if (viewModelProperty.PropertyType.IsGenericType && typeof(ObservableCollection<>).IsAssignableFrom(viewModelProperty.PropertyType.GetGenericTypeDefinition())
                    && fieldPropertyValue is System.Collections.IEnumerable collection)
                {
                    // avoid for now: causes recursion it the VM observable directly references the model list!

                    //Type observableGenericType = typeof(ObservableCollection<>).MakeGenericType(viewModelProperty.PropertyType.GetGenericArguments()[0]);
                    //viewModelProperty.SetValue(this, Activator.CreateInstance(observableGenericType, new object[] { collection }));
                }
                else
                    viewModelProperty.SetValue(this, fieldPropertyValue);
            }
        }

        /// <summary>
        /// Updates statistics related to the media library
        /// </summary>
        /// <param name="mediaType">The media type item for which to update the statistics</param>
        private void DisplayMediaTypeStatistics(MediaTypeEntity mediaType)
        {
            if (mediaType != null && mediaType.IsMedia)
            {
                switch (mediaType.MediaType)
                {
                    case "TV SHOW":
                        TotalMediaCountType = "TV SHOWS:";
                        TotalMediaCount = mediaLibrary.Library.TvShows?.Count() ?? 0;
                        TotalWatchedCount = mediaLibrary.Library.TvShows?.Where(t => t.IsWatched == true).Count() ?? 0;
                        TotalUnwatchedCount = mediaLibrary.Library.TvShows?.Where(t => !t.IsWatched == true).Count() ?? 0;
                        break;
                    case "MOVIE":
                        TotalMediaCountType = "MOVIES:";
                        TotalMediaCount = mediaLibrary.Library.Movies?.Count() ?? 0;
                        TotalWatchedCount = mediaLibrary.Library.Movies?.Where(m => m.IsWatched == true).Count() ?? 0;
                        TotalUnwatchedCount = mediaLibrary.Library.Movies?.Where(m => !m.IsWatched == true).Count() ?? 0;
                        break;
                    case "MUSIC":
                        TotalMediaCountType = "ARTISTS:";
                        TotalMediaCount = mediaLibrary.Library.Artists?.Count() ?? 0;
                        TotalWatchedCount = mediaLibrary.Library.Artists?.Where(a => a.IsListened == true).Count() ?? 0;
                        TotalUnwatchedCount = mediaLibrary.Library.Artists?.Where(a => !a.IsListened == true).Count() ?? 0;
                        break;
                }
            }
            else
            {
                TotalMediaCountType = "ALL MEDIA:";
                TotalMediaCount = mediaLibrary.Library.TvShows?.SelectMany(t => t.Seasons)?.SelectMany(s => s.Episodes)?.Count() ?? 0 + mediaLibrary.Library?.Movies?.Count() ?? 0 + mediaLibrary.Library?.Artists?.SelectMany(t => t.Albums)?.SelectMany(s => s.Songs)?.Count() ?? 0;
                TotalWatchedCount = mediaLibrary.Library.TvShows?.SelectMany(t => t.Seasons)?.SelectMany(s => s.Episodes)?.Where(e => e.IsWatched == true)?.Count() ?? 0 + mediaLibrary.Library.Movies?.Where(m => m.IsWatched == true)?.Count() ?? 0;
                TotalUnwatchedCount = mediaLibrary.Library.TvShows?.SelectMany(t => t.Seasons)?.SelectMany(s => s.Episodes)?.Where(e => !e.IsWatched == true)?.Count() ?? 0 + mediaLibrary.Library.Movies?.Where(m => !m.IsWatched == true)?.Count() ?? 0;
            }
        }

        /// <summary>
        /// Updates the IsWatched status for <paramref name="mediaEntity"/>
        /// </summary>
        /// <param name="mediaEntity">The media item for which to update the IsWatched status</param>
        private async Task SetIsWatchedStatusAsync(IMediaEntity mediaEntity)
        {
            try
            {
                if (CurrentNavigationLevel == NavigationLevel.Episode)
                    await mediaLibrary.MediaState.SetEpisodeIsWatchedStatusAsync(mediaLibrary, mediaEntity);
                else if (CurrentNavigationLevel == NavigationLevel.Season)
                    await mediaLibrary.MediaState.SetSeasonIsWatchedStatusAsync(mediaLibrary, mediaEntity);
                else if (CurrentNavigationLevel == NavigationLevel.TvShow)
                    await mediaLibrary.MediaState.SetTvShowIsWatchedStatusAsync(mediaLibrary, mediaEntity);
                else if (CurrentNavigationLevel == NavigationLevel.Movie)
                    await mediaLibrary.MediaState.SetMovieIsWatchedStatusAsync(mediaLibrary, mediaEntity);
                else if (CurrentNavigationLevel == NavigationLevel.Song)
                    await mediaLibrary.MediaState.SetSongIsListenedStatusAsync(mediaLibrary, mediaEntity);
                else if (CurrentNavigationLevel == NavigationLevel.Album)
                    await mediaLibrary.MediaState.SetAlbumIsListenedStatusAsync(mediaLibrary, mediaEntity);
                else if (CurrentNavigationLevel == NavigationLevel.Artist)
                    await mediaLibrary.MediaState.SetArtistIsListenedStatusAsync(mediaLibrary, mediaEntity);
            }
            catch (Exception ex) when (ex is InvalidOperationException || ex is ArgumentException)
            {
                await notificationService.ShowAsync(ex.Message, "LEYA - Error", NotificationButton.OK, NotificationImage.Error);
            }
        }

        /// <summary>
        /// Updates the IsFavorite status for <paramref name="mediaEntity"/>
        /// </summary>
        /// <param name="mediaEntity">The media item for which to update the IsFavorite status</param>
        private async Task SetIsFavoriteStatusAsync(IMediaEntity mediaEntity)
        {
            try
            {
                if (CurrentNavigationLevel == NavigationLevel.Episode)
                    await mediaLibrary.MediaState.SetEpisodeIsFavoriteStatusAsync(mediaLibrary, mediaEntity);
                else if (CurrentNavigationLevel == NavigationLevel.Season)
                    await mediaLibrary.MediaState.SetSeasonIsFavoriteStatusAsync(mediaLibrary, mediaEntity);
                else if (CurrentNavigationLevel == NavigationLevel.TvShow)
                    await mediaLibrary.MediaState.SetTvShowIsFavoriteStatusAsync(mediaLibrary, mediaEntity);
                else if (CurrentNavigationLevel == NavigationLevel.Movie)
                    await mediaLibrary.MediaState.SetMovieIsFavoriteStatusAsync(mediaLibrary, mediaEntity);
                else if (CurrentNavigationLevel == NavigationLevel.Song)
                    await mediaLibrary.MediaState.SetSongIsFavoriteStatusAsync(mediaLibrary, mediaEntity);
                else if (CurrentNavigationLevel == NavigationLevel.Album)
                    await mediaLibrary.MediaState.SetAlbumIsFavoriteStatusAsync(mediaLibrary, mediaEntity);
                else if (CurrentNavigationLevel == NavigationLevel.Artist)
                    await mediaLibrary.MediaState.SetArtistIsFavoriteStatusAsync(mediaLibrary, mediaEntity);
            }
            catch (Exception ex) when (ex is InvalidOperationException || ex is ArgumentException)
            {
                await notificationService.ShowAsync(ex.Message, "LEYA - Error", NotificationButton.OK, NotificationImage.Error);
            }
        }

        /// <summary>
        /// Opens a directory containing the logs of the application
        /// </summary>
        private void OpenLoggingDirectory()
        {
            // TODO: replace with cross platform way of opening folders
            Process.Start(AppDomain.CurrentDomain.BaseDirectory + @"Logs");
        }

        /// <summary>
        /// Updates all the media library in the database
        /// </summary>
        private async Task SaveMediaLibraryAsync()
        {
            if (await notificationService.ShowAsync("Are you sure you want to refresh the media library?\nThis could take a while, depending on the size of your library.", "LEYA", NotificationButton.YesNo, NotificationImage.Question) == NotificationResult.Yes)
            {
                ShowProgressBar();
                IsScannerTextVisible = true;
                try
                {
                    ScannerOutput = "Updating media library...";
                    await mediaLibrary.UpdateMediaLibraryAsync();
                    ScannerOutput = "Loading media library...";
                    await mediaLibrary.GetMediaLibraryAsync();
                    await notificationService.ShowAsync("Media library updated!", "LEYA - Success");
                    ScannerOutput = "";
                    IsScannerTextVisible = false;
                    ScannerOutput = string.Empty;
                    HideProgressBar();
                }
                catch (Exception ex)
                {
                    await notificationService.ShowAsync("Error updating the media library: " + ex.Message, "LEYA - Error");
                    if (!(ex is InvalidOperationException))
                    {
                        IsScannerTextVisible = false;
                        ScannerOutput = string.Empty;
                        HideProgressBar();
                    }
                }
            }
        }

        #region Options Media

        /// <summary>
        /// Refreshes the media library
        /// </summary>
        private async Task UpdateMediaLibraryAsync()
        {
            // do not allow two media items with same name
            if (SourceMediaCategories.Count(mc => mc.MediaName == MediaName) > 0 && !optionsMedia.IsMediaTypeSourceUpdate)
                await notificationService.ShowAsync("The specified media name already exists!", "LEYA - Error", NotificationButton.OK, NotificationImage.Error);
            else
            {
                MediaTypeEntity item = new MediaTypeEntity() { MediaName = MediaName, IsMedia = true, MediaType = SelectedMediaCategoryType.Text, Id = optionsMedia.IsMediaTypeSourceUpdate ? optionsMedia.Id : 0 };
                // if we are updating an already existing media type, delete it and its sources before re-inserting them (might contain modifications)
                if (optionsMedia.IsMediaTypeSourceUpdate)
                    await DeleteMediaTypeAsync(item);
                // add the new media type and get the returned row id
                int newMediaId = await optionsMedia.SaveMediaTypeAsync(item);
                if (newMediaId != -1)
                    await optionsMedia.SaveMediaLibrarySourcesAsync(newMediaId);
                // refresh the list of media types
                await optionsMedia.RefreshMediaTypes(mediaLibrary);
                GetMediaTypes();
                optionsMedia.IsMediaTypeSourceUpdate = false;
                ExitMediaTypeSourcesOptions();
                await notificationService.ShowAsync("Media library updated!", "LEYA - Success");
            }
        }

        /// <summary>
        /// Resets the fields required to create a new media type to their default values
        /// </summary>
        private void ResetAddMediaSourceElements()
        {
            optionsMedia.ResetAddMediaSourceElements();
            SelectedMediaCategoryType = null;
            Notify(nameof(SourceMediaCategorySources));
            IsMediaTypeSourcesOptionVisible = true;
        }

        /// <summary>
        /// Validates the required information for adding a new media source
        /// </summary>
        /// <returns>True if required information is fine, False otherwise</returns>
        private bool ValidateAddMediaTypeSource()
        {
            bool isValid = !string.IsNullOrEmpty(MediaName) && SourceMediaCategorySources.Count > 0 && SelectedMediaCategoryType != null && !string.IsNullOrEmpty(SelectedMediaCategoryType.Text);
            if (!isValid && IsMediaTypesOptionVisible)
            {
                ShowHelpButton();
                WindowHelp = "\n";
                if (string.IsNullOrEmpty(MediaName))
                    WindowHelp += "Media name cannot be empty!\n";
                if (SourceMediaCategorySources.Count == 0)
                    WindowHelp += "No media source added!\n";
                if (SelectedMediaCategoryType != null && string.IsNullOrEmpty(SelectedMediaCategoryType.Text))
                    WindowHelp += "No media type chosen!\n";
            }
            else
                HideHelpButton();
            ValidationChanged?.Invoke(isValid);
            return isValid;
        }

        /// <summary>
        /// Adds a new media source to the selected media
        /// </summary>
        /// <param name="selectedDirectories">The paths of the selected directories representing the media sources to be added</param>
        private async Task AddMediaTypeSourceAsync(string selectedDirectories)
        {
            // ask whether the selected directories contain a single media or not (ie: selecting a single folder which contains more than one tv show inside)
            NotificationResult response = await notificationService.ShowAsync("Selected folder contains a single media?", "LEYA", NotificationButton.YesNo, NotificationImage.Question);
            try
            {
                foreach (string mediaSource in optionsMedia.AddMediaTypeSource(selectedDirectories, response == NotificationResult.Yes))
                {
                    // do not add same source path twice
                    if (SourceMediaCategorySources.Count(ms => ms.MediaSourcePath == mediaSource.ToUpper()) == 0)
                    {
                        optionsMedia.SourceMediaCategorySources.Add(new MediaTypeSourceEntity()
                        {
                            MediaSourcePath = mediaSource.ToUpper()
                        });
                    }
                }
                Notify(nameof(SourceMediaCategorySources));
                RefreshMediaSourcesAsync_Command.RaiseCanExecuteChanged();
            }
            catch (Exception ex) when (ex is IOException)
            {
                await notificationService.ShowAsync(ex.Message, "LEYA - Error", NotificationButton.OK, NotificationImage.Error);
            }
        }

        /// <summary>
        /// Removes <paramref name="item"/> from the list of media type sources
        /// </summary>
        /// <param name="item">The item to be removed from the media sources</param>
        private void DeleteMediaTypeSource(MediaTypeSourceEntity item)
        {
            optionsMedia.SourceMediaCategorySources.Remove(item);
            Notify(nameof(SourceMediaCategorySources));
            RefreshMediaSourcesAsync_Command.RaiseCanExecuteChanged();
        }

        /// <summary>
        /// Gets the media type sources for <paramref name="item"/>
        /// </summary>
        /// <param name="item">The media type for which to get the media type sources</param>
        private void GetMediaTypeSources(MediaTypeEntity item)
        {
            ShowProgressBar();
            IsMediaTypeSourcesOptionVisible = true; 
            // the user is updating the sources of an existing media type, not creating a new one
            optionsMedia.IsMediaTypeSourceUpdate = true;
            optionsMedia.Id = item.Id;
            // get the media type sources of the provided media type
            SourceMediaCategorySources = new ObservableCollection<MediaTypeSourceEntity>(mediaLibrary.Library.MediaTypes.SelectMany(mt => mt.MediaTypeSources)
                                                                                                                        .Where(mts => mts.MediaTypeId == item.Id));
            // re-assign the current media name and selected media type
            MediaName = item.MediaName;
            SelectedMediaCategoryType = SourceMediaCategoryTypes.First(mt => mt.Text == item.MediaType);
            HideProgressBar();
        }

        /// <summary>
        /// Deletes a media type and all its associated media type sources
        /// </summary>
        /// <param name="media">The media type to be deleted</param>
        private async Task DeleteMediaTypeAsync(MediaTypeEntity media)
        {
            try
            {
                await optionsMedia.DeleteMediaTypeAsync(media.Id);
                await optionsMedia.RefreshMediaTypes(mediaLibrary);
                GetMediaTypes();
            }
            catch (Exception ex) when (ex is InvalidOperationException || ex is ArgumentException)
            {
                await notificationService .ShowAsync(ex.Message, "LEYA - Error", NotificationButton.OK, NotificationImage.Error);
            }
        }

        /// <summary>
        /// Gets the user defined media types
        /// </summary>
        public void GetMediaTypes() 
        {
            ShowProgressBar();
            SourceMediaCategories = new ObservableCollection<MediaTypeEntity>(optionsMedia.GetMediaTypesAsync(mediaLibrary));
            HideProgressBar();
        }

        /// <summary>
        /// Displays the options related to the media library
        /// </summary>
        private void ShowMediaOptions()
        {
            IsMediaTypesOptionVisible = true;
            GetMediaTypes();
        }
        #endregion

        #region Navigation
        /// <summary>
        /// Triggers the event that signals the beginning of media library navigation
        /// </summary>
        private void InitiateNavigation()
        {
            UpdateMainSectionsVisibility();
            Navigated?.Invoke();
        }

        /// <summary>
        /// Initiates the navigation when clicking on a main menu item
        /// </summary>
        /// <param name="menu">The mainmenu item that was clicked</param>
        public void BeginMainMenuNavigation(MediaTypeEntity menu)
        {
            mediaLibraryNavigation.InitiateMainMenuNavigation(menu);
        }

        /// <summary>
        /// When media view is displayed, hides the options view (if visible)
        /// </summary>
        /// <remarks>Media type is not identified only by media type id, since multiple media types can share 
        /// the same media type id, ex: two libraries of type tv show, with different names: TV SHOWS and CARTOONS</remarks>
        public void CompleteNavigation()
        {
            if (CurrentNavigationLevel != NavigationLevel.System && CurrentNavigationLevel != NavigationLevel.Search && CurrentNavigationLevel != NavigationLevel.Favorite)
            {
                IEnumerable<IMediaEntity> temp = new List<IMediaEntity>();
                if (CurrentNavigationLevel == NavigationLevel.TvShow)
                    temp = mediaLibraryNavigation.GetTvShowsNavigationList(mediaLibrary, () => new MediaEntity());
                else if (CurrentNavigationLevel == NavigationLevel.Movie)
                    temp = mediaLibraryNavigation.GetMoviesNavigationList(mediaLibrary, () => new MediaEntity());
                else if (CurrentNavigationLevel == NavigationLevel.Artist)
                    temp = mediaLibraryNavigation.GetArtistsNavigationList(mediaLibrary, () => new MediaEntity());
                // the list is populated only when coming from the main menu, not when navigating the media library list
                if (temp.Count() > 0)
                {
                    SourceMedia = new ObservableCollection<IMediaEntity>(temp);
                    SourceFilter = new ObservableCollection<SearchEntity>(temp.Select(e => new SearchEntity() { Text = e.MediaName }));
                }
            }
        }

        /// <summary>
        /// Navigates one level up from the current media view level
        /// </summary>
        public async Task NavigateMediaLibraryUpAsync()
        {
            ShowProgressBar();
            if (sourceMedia != null && sourceMedia.Count > 0)
            {
                IMediaEntity media = sourceMedia[0];
                IEnumerable<IMediaEntity> temp = new List<IMediaEntity>();
                await Task.Run(() =>
                {
                    // get the parent items of the currently displayed items
                    if (CurrentNavigationLevel == NavigationLevel.Season)
                        temp = mediaLibraryNavigation.GetTvShowsNavigationList(mediaLibrary, () => new MediaEntity());
                    else if (CurrentNavigationLevel == NavigationLevel.Episode)
                        temp = mediaLibraryNavigation.GetSeasonsNavigationListFromEpisode(mediaLibrary, media, () => new MediaEntity());
                    else if (CurrentNavigationLevel == NavigationLevel.Album)
                        temp = mediaLibraryNavigation.GetArtistsNavigationList(mediaLibrary, () => new MediaEntity());
                    else if (CurrentNavigationLevel == NavigationLevel.Song)
                        temp = mediaLibraryNavigation.GetAlbumsNavigationListFromSong(mediaLibrary, media, () => new MediaEntity());
                });
                // update the displayed list with the parent items list
                SourceMedia = new ObservableCollection<IMediaEntity>(temp);
                SourceFilter = new ObservableCollection<SearchEntity>(temp.Select(e => new SearchEntity() { Text = e.MediaName }));
                // advance current level of navigation one level higher and notify the user interface that the navigation took place
                mediaLibraryNavigation.NavigateUp(mediaLibrary, media.Id);
            }
            else
            {
                // no media in the selected category, return to main menu
                mediaLibraryNavigation.CurrentNavigationLevel = NavigationLevel.None;
                mediaLibraryNavigation.IsBackNavigationPossible = false;
                UpdateMainSectionsVisibility();
                Navigated?.Invoke();
            }
            HideProgressBar();
        }

        /// <summary>
        /// Updates the visibility of the main section elements in the main view
        /// </summary>
        private void UpdateMainSectionsVisibility()
        {
            // update the visibility of elements based on the current navigation level
            switch (CurrentNavigationLevel)
            {
                case NavigationLevel.None:
                    IsMainMenuVisible = true;
                    IsMediaContainerVisible = false;
                    IsMaskBackgroundVisible = false;
                    IsOptionsContainerVisible = false;
                    IsMediaTypesOptionVisible = false;
                    IsMediaTypeSourcesOptionVisible = false;
                    break;
                case NavigationLevel.TvShow:
                case NavigationLevel.Movie:
                case NavigationLevel.Artist:
                    IsMainMenuVisible = false;
                    IsMediaContainerVisible = true;
                    break;
                case NavigationLevel.System:
                case NavigationLevel.Search:
                case NavigationLevel.Favorite:
                    IsMainMenuVisible = false;
                    IsMaskBackgroundVisible = true;
                    IsMediaContainerVisible = false;
                    IsOptionsContainerVisible = true;
                    break;
                default:
                    break;
            }
            // display the appropriate media library list, if applicable
            CompleteNavigation();
        }

        /// <summary>
        /// Navigates one level down from the current media view level, or if the media view is at lowest possible level, plays a Media
        /// </summary>
        /// <param name="media">The MediaDisplayEntity object containing the Id of the Media to open</param>
        public async Task NavigateMediaLibraryDownAsync(IMediaEntity media)
        {
            ShowProgressBar();
            if (CurrentNavigationLevel != NavigationLevel.Episode && CurrentNavigationLevel != NavigationLevel.Movie && CurrentNavigationLevel != NavigationLevel.Song)
            {
                IEnumerable<IMediaEntity> temp = new List<IMediaEntity>();
                await Task.Run(() =>
                {
                    if (CurrentNavigationLevel == NavigationLevel.TvShow)
                        temp = mediaLibraryNavigation.GetSeasonsNavigationListFromEpisode(mediaLibrary, media, () => new MediaEntity());
                    else if (CurrentNavigationLevel == NavigationLevel.Season)
                        temp = mediaLibraryNavigation.GetEpisodesNavigationList(mediaLibrary, media, () => new MediaEntity());
                    else if (CurrentNavigationLevel == NavigationLevel.Artist)
                        temp = mediaLibraryNavigation.GetAlbumsNavigationListFromSong(mediaLibrary, media, () => new MediaEntity());
                    else if (CurrentNavigationLevel == NavigationLevel.Album)
                        temp = mediaLibraryNavigation.GetSongsNavigationList(mediaLibrary, media, () => new MediaEntity());
                });
                SourceMedia = new ObservableCollection<IMediaEntity>(temp);
                SourceFilter = new ObservableCollection<SearchEntity>(temp.Select(e => new SearchEntity() { Text = e.MediaName }));
            }
            // advance current level of navigation one level higher and notify the user interface that the navigation took place
            try
            {
                await mediaLibraryNavigation.NavigateDown(mediaLibrary, media);
            }
            catch (Exception ex) when (ex is InvalidOperationException || ex is ArgumentException)
            {
                await notificationService.ShowAsync(ex.Message, "LEYA - Error", NotificationButton.OK, NotificationImage.Error);
            }
            HideProgressBar();
        }

        /// <summary>
        /// Exits the media library list and displays the main menu
        /// </summary>
        public async Task ExitMediaLibrary()
        {
            mediaLibraryNavigation.CurrentNavigationLevel = NavigationLevel.TvShow;
            await NavigateMediaLibraryUpAsync();
        }

        /// <summary>
        /// Exits the media type sources option list and returns to the media types option list
        /// </summary>
        private void ExitMediaTypeSourcesOptions()
        {
            IsMediaTypeSourcesOptionVisible = false;
            ResetAddMediaSourceElements_Command?.RaiseCanExecuteChanged();
            IsHelpButtonVisible = false;
        }

        /// <summary>
        /// Exits the media types options list and returns to the options list
        /// </summary>
        private void ExitMediaTypesOptions()
        {
            IsMediaTypesOptionVisible = false;
            IsHelpButtonVisible = false;
        }
        #endregion
        #endregion

        #region ============================================================= EVENT HANDLERS ================================================================================
        /// <summary>
        /// Handles the ViewOpenedAsync event of the Window
        /// </summary>
        private async Task ViewOpenedAsync()
        {
            // get the current version of the software
            Version = "v. " + FileVersionInfo.GetVersionInfo(Assembly.GetEntryAssembly().Location).FileVersion;
            ShowProgressBar();
            ScannerOutput = "Loading media library...";
            WindowTitle = "LEYA";
            IsScannerTextVisible = true;
            await mediaLibrary.GetMediaLibraryAsync();
            HideProgressBar();
        }

        /// <summary>
        /// Handles the event risen when the media types are loaded
        /// </summary>
        private void MediaLibrary_MediaTypesLoaded()
        {
            SourceMediaTypes = new ObservableCollection<MediaTypeEntity>(mediaLibrary.Library.MediaTypes);
        }

        /// <summary>
        /// Handles the event risen when the media library is loaded
        /// </summary>
        private void MediaLibrary_LibraryLoaded()
        {
            ScannerOutput = string.Empty;
            IsScannerTextVisible = false;
        }

        /// <summary>
        /// Handles Media SelectedItem event
        /// </summary>
        private void SelectedMediaChanged()
        {
            if (SourceMediaSelectedItem != null)
            {
                // if the UI displays a list of tv shows
                if (CurrentNavigationLevel == NavigationLevel.TvShow)
                    mediaLibraryNavigation.GetTvShowMediaInfo(mediaLibrary, SourceMediaSelectedItem.Id);
                // if the UI displays a list of tv show seasons
                else if (CurrentNavigationLevel == NavigationLevel.Season)
                    mediaLibraryNavigation.GetSeasonMediaInfo(mediaLibrary, SourceMediaSelectedItem.Id, SourceMediaSelectedItem.SeasonOrAlbumId);
                // if the UI displays a list of tv show episodes
                else if (CurrentNavigationLevel == NavigationLevel.Episode)
                    mediaLibraryNavigation.GetEpisodeMediaInfo(mediaLibrary, SourceMediaSelectedItem.Id, SourceMediaSelectedItem.SeasonOrAlbumId, SourceMediaSelectedItem.MediaName);
                // if the UI displays a list of movies
                else if (CurrentNavigationLevel == NavigationLevel.Movie)
                    mediaLibraryNavigation.GetMovieMediaInfo(mediaLibrary, SourceMediaSelectedItem.Id);
                // if the UI displays a list of artists
                else if (CurrentNavigationLevel == NavigationLevel.Artist)
                    mediaLibraryNavigation.GetArtistMediaInfo(mediaLibrary, SourceMediaSelectedItem.Id);
                // if the UI displays a list of albums
                else if (CurrentNavigationLevel == NavigationLevel.Album)
                    mediaLibraryNavigation.GetAlbumMediaInfo(mediaLibrary, SourceMediaSelectedItem.Id, SourceMediaSelectedItem.SeasonOrAlbumId);
                // if the UI displays a list of songs
                else if (CurrentNavigationLevel == NavigationLevel.Song)
                    mediaLibraryNavigation.GetSongMediaInfo(mediaLibrary, SourceMediaSelectedItem.Id, SourceMediaSelectedItem.SeasonOrAlbumId, SourceMediaSelectedItem.MediaName);
            }
        }
        #endregion
    }
}
