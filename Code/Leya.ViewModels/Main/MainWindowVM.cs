﻿/// Written by: Yulia Danilova
/// Creation Date: 11th of November, 2020
/// Purpose: View Model for the main application Window
#region ========================================================================= USING =====================================================================================
using System;
using System.Linq;
using System.Reflection;
using System.Diagnostics;
using System.Threading.Tasks;
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

        private readonly IMediaLibrary mediaLibrary;
        private readonly IMediaLibraryNavigation mediaLibraryNavigation;
        private readonly IAppOptions appOptions;
        private readonly IMediaStatistics mediaStatistics;
        #endregion

        #region ============================================================= BINDING COMMANDS ==============================================================================
        public IAsyncCommand PlayAllMediaAsync_Command { get; private set; }
        public IAsyncCommand SaveMediaLibraryAsync_Command { get; private set; }
        public IAsyncCommand Search_EnterKeyUpAsync_Command { get; private set; }
        public IAsyncCommand Search_DropDownClosingAsync_Command { get; private set; }
        public IAsyncCommand ViewOpenedAsync_Command { get; private set; }
        public IAsyncCommand<IMediaEntity> ShowMediaCastAsync_Command { get; private set; }
        public IAsyncCommand<IMediaEntity> SetIsWatchedStatusAsync_Command { get; private set; }
        public IAsyncCommand<IMediaEntity> SetIsFavoriteStatusAsync_Command { get; private set; }
        public IAsyncCommand<MediaTypeEntity> DeleteMediaTypeAsync_Command { get; private set; }
        public ISyncCommand<MediaTypeEntity> GetMediaTypeSources_Command { get; private set; }
        public ISyncCommand<MediaTypeEntity> DisplayMediaTypeInfo_Command { get; private set; }
        public ISyncCommand<MediaTypeSourceEntity> DeleteMediaTypeSource_Command { get; private set; }
        public ISyncCommand<MediaTypeSourceEntity> OpenMediaSourceLocation_Command { get; private set; }
        public IAsyncCommand<string> AddMediaSourceAsync_Command { get; private set; }
        public ISyncCommand<decimal> DisplayOffset_ValueChanged_Command { get; private set; }
        public ISyncCommand ShowMediaOptions_Command { get; private set; }
        public ISyncCommand ShowPlayerOptions_Command { get; private set; }
        public ISyncCommand ShowInterfaceOptions_Command { get; private set; }
        public ISyncCommand OpenMediaFolder_Command { get; private set; }
        public ISyncCommand Filter_EnterKeyUp_Command { get; private set; }
        public ISyncCommand SelectedMediaChanged_Command { get; private set; }
        public ISyncCommand Filter_DropDownClosing_Command { get; private set; }
        public ISyncCommand OpenLoggingDirectory_Command { get; private set; }
        public ISyncCommand ResetAddMediaSourceElements_Command { get; private set; }
        public IAsyncCommand UpdatePlayerOptionsAsync_Command { get; private set; }
        public IAsyncCommand UpdateInterfaceOptionsAsync_Command { get; private set; }
        public IAsyncCommand RefreshMediaSourcesAsync_Command { get; private set; }
        public ISyncCommand ExitMediaTypeSourcesOptions_Command { get; private set; }
        public ISyncCommand ExitMediaTypesOptions_Command { get; private set; }
        public ISyncCommand ExitPlayerOptions_Command { get; private set; }
        public ISyncCommand ExitInterfaceOptions_Command { get; set; }
        public ISyncCommand ExitMediaCast_Command { get; private set; }
        //public AutoCompleteFilterPredicate<object> SearchMediaLibrary_Command { get; private set; }
        public ISyncCommand<bool> ChangeRepeatArgument_Command { get; private set; }
        public ISyncCommand<bool> ChangeShuffleArgument_Command { get; private set; }
        public ISyncCommand<bool> ChangeAutoscaleArgument_Command { get; private set; }
        public ISyncCommand<bool> ChangeFullScreenArgument_Command { get; private set; }
        public ISyncCommand<bool> ChangeAlwaysOnTopArgument_Command { get; private set; }
        public ISyncCommand<bool> ChangePlayAndStopArgument_Command { get; private set; }
        public ISyncCommand<bool> ChangeSingleInstanceArgument_Command { get; private set; }
        public ISyncCommand<bool> ChangeEnqueueFilesInSingleInstanceModeArgument_Command { get; private set; }
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

        public string Fanart
        {
            get { return mediaLibraryNavigation.Fanart; }
            set { mediaLibraryNavigation.Fanart = value; Notify(); }
        }

        public string Banner
        {
            get { return mediaLibraryNavigation.Banner; }
            set { mediaLibraryNavigation.Banner = value; Notify(); }
        }

        public string Poster
        {
            get { return mediaLibraryNavigation.Poster; }
            set { mediaLibraryNavigation.Poster = value; Notify(); IsMediaCoverVisible = value != null; }
        }

        public string TagLine
        {
            get { return mediaLibraryNavigation.TagLine; }
            set { mediaLibraryNavigation.TagLine = value; Notify(); }
        }

        public string MPAA
        {
            get { return mediaLibraryNavigation.MPAA; }
            set { mediaLibraryNavigation.MPAA = value; Notify(); }
        }

        private string mediaCase = "bg-bluray.png";
        public string MediaCase
        {
            get { return mediaCase; }
            set { mediaCase = value; Notify(); }
        }

        public string Writers
        {
            get { return mediaLibraryNavigation.Writers; }
            set { mediaLibraryNavigation.Writers = value; Notify(); }
        }

        public string Director
        {
            get { return mediaLibraryNavigation.Director; }
            set { mediaLibraryNavigation.Director = value; Notify(); }
        }

        private string filterText;
        public string FilterText
        {
            get { return filterText; }
            set { filterText = value; Notify(); }
        }

        public string CurrentStatus
        {
            get { return mediaLibraryNavigation.CurrentStatus; }
            set { mediaLibraryNavigation.CurrentStatus = value; Notify(); }
        }

        public string LocalPath
        {
            get { return mediaLibraryNavigation.LocalPath; }
            set { mediaLibraryNavigation.LocalPath = value; Notify(); }
        }

        public string Synopsis
        {
            get { return mediaLibraryNavigation.Synopsis; }
            set { mediaLibraryNavigation.Synopsis = value; Notify(); }
        }

        public string Genre
        {
            get { return mediaLibraryNavigation.Genre; }
            set { mediaLibraryNavigation.Genre = value; Notify(); }
        }

        public string Forecast
        {
            get { return mediaStatistics.Forecast; }
            set { mediaStatistics.Forecast = value; Notify(); }
        }

        public string TotalMediaCountType
        {
            get { return mediaStatistics.TotalMediaCountType; }
            set { mediaStatistics.TotalMediaCountType = value; Notify(); }
        }

        public string MediaName
        {
            get { return appOptions.OptionsMedia.MediaName; }
            set { appOptions.OptionsMedia.MediaName = value?.ToUpper(); Notify(); RefreshMediaSourcesAsync_Command?.RaiseCanExecuteChanged(); }
        }

        public string PlayerPath
        {
            get { return appOptions.OptionsPlayer.PlayerPath; }
            set { appOptions.OptionsPlayer.PlayerPath = value; Notify(); UpdatePlayerOptionsAsync_Command.RaiseCanExecuteChanged(); }
        }

        public string PlayerArguments
        {
            get { return appOptions.OptionsPlayer.PlayerArguments; }
            set { appOptions.OptionsPlayer.PlayerArguments = value; Notify(); }
        }

        public string BackgroundImagePath
        {
            get { return appOptions.OptionsInterface.BackgroundImagePath; }
            set { appOptions.OptionsInterface.BackgroundImagePath = value; Notify(); }
        }

        public string WeatherUrl
        {
            get { return appOptions.OptionsInterface.WeatherUrl; }
            set { appOptions.OptionsInterface.WeatherUrl = value; Notify(); }
        }

        public int SelectedThemeIndex
        {
            get { return appOptions.OptionsInterface.SelectedThemeIndex; }
            set { appOptions.OptionsInterface.SelectedThemeIndex = value; Notify(); }
        }

        public int TotalMediaCount
        {
            get { return mediaStatistics.TotalMediaCount; }
            set { mediaStatistics.TotalMediaCount = value; Notify(); }
        }

        public int TotalWatchedCount
        {
            get { return mediaStatistics.TotalWatchedCount; }
            set { mediaStatistics.TotalWatchedCount = value; Notify(); }
        }

        public int TotalUnwatchedCount
        {
            get { return mediaStatistics.TotalUnwatchedCount; }
            set { mediaStatistics.TotalUnwatchedCount = value; Notify(); }
        }

        public int NumberOfSeasons
        {
            get { return mediaLibraryNavigation.NumberOfSeasons; }
            set { mediaLibraryNavigation.NumberOfSeasons = value; Notify(); }
        }

        public int NumberOfUnwatchedEpisodes
        {
            get { return mediaLibraryNavigation.NumberOfUnwatchedEpisodes; }
            set { mediaLibraryNavigation.NumberOfUnwatchedEpisodes = value; Notify(); }
        }

        public int NumberOfEpisodes
        {
            get { return mediaLibraryNavigation.NumberOfEpisodes; }
            set { mediaLibraryNavigation.NumberOfEpisodes = value; Notify(); }
        }

        public double Celsius
        {
            get { return mediaStatistics.Celsius; }
            set { mediaStatistics.Celsius = value; Notify(); }
        }

        public double Fahrenheit
        {
            get { return mediaStatistics.Fahrenheit; }
            set { mediaStatistics.Fahrenheit = value; Notify(); }
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

        public bool IsNumberOfSeasonsVisible
        {
            get { return mediaLibraryNavigation.IsNumberOfSeasonsVisible; }
            set { mediaLibraryNavigation.IsNumberOfSeasonsVisible = value; Notify(); }
        }

        private bool isScannerTextVisible = true;
        public bool IsScannerTextVisible
        {
            get { return isScannerTextVisible; }
            set { isScannerTextVisible = value; Notify(); }
        }

        public bool IsTvShow
        {
            get { return mediaLibraryNavigation.IsTvShow; }
            set { mediaLibraryNavigation.IsTvShow = value; Notify(); }
        }

        private bool isMediaBluray = true;
        public bool IsMediaBluray
        {
            get { return isMediaBluray; }
            set { isMediaBluray = value; Notify(); }
        }

        public bool IsFanartVisible
        {
            get { return mediaLibraryNavigation.IsFanartVisible; }
            set { mediaLibraryNavigation.IsFanartVisible = value; Notify(); }
        }

        public bool IsMediaCoverVisible
        {
            get { return mediaLibraryNavigation.IsMediaCoverVisible; }
            set { mediaLibraryNavigation.IsMediaCoverVisible = value; Notify(); }
        }

        public bool IsBackNavigationPossible
        {
            get { return mediaLibraryNavigation.IsBackNavigationPossible; }
            set { mediaLibraryNavigation.IsBackNavigationPossible = value; Notify(); }
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

        private bool isPlayerOptionVisible = false;
        public bool IsPlayerOptionVisible
        {
            get { return isPlayerOptionVisible; }
            set { isPlayerOptionVisible = value; Notify(); }
        }

        private bool isInterfaceOptionVisible = false;
        public bool IsInterfaceOptionVisible
        {
            get { return isInterfaceOptionVisible; }
            set { isInterfaceOptionVisible = value; Notify(); }
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

        public bool IsWritersVisible
        {
            get { return mediaLibraryNavigation.IsWritersVisible; }
            set { mediaLibraryNavigation.IsWritersVisible = value; Notify(); }
        }

        public bool IsDirectorVisible
        {
            get { return mediaLibraryNavigation.IsDirectorVisible; }
            set { mediaLibraryNavigation.IsDirectorVisible = value; Notify(); }
        }

        public bool UsesFullScreenArgument
        {
            get { return appOptions.OptionsPlayer.UsesFullScreenArgument; }
            set { appOptions.OptionsPlayer.UsesFullScreenArgument = value; Notify(); }
        }

        public bool IsSingleInstanceArgument
        {
            get { return appOptions.OptionsPlayer.IsSingleInstanceArgument; }
            set { appOptions.OptionsPlayer.IsSingleInstanceArgument = value; Notify(); }
        }

        public bool EnquesFilesInSingleInstanceModeArgument
        {
            get { return appOptions.OptionsPlayer.EnquesFilesInSingleInstanceModeArgument; }
            set { appOptions.OptionsPlayer.EnquesFilesInSingleInstanceModeArgument = value; Notify(); }
        }

        public bool IsAlwaysOnTopArgument
        {
            get { return appOptions.OptionsPlayer.IsAlwaysOnTopArgument; }
            set { appOptions.OptionsPlayer.IsAlwaysOnTopArgument = value; Notify(); }
        }

        public bool UsesAutoscaleArgument
        {
            get { return appOptions.OptionsPlayer.UsesAutoscaleArgument; }
            set { appOptions.OptionsPlayer.UsesAutoscaleArgument = value; Notify(); }
        }

        public bool ResumesFromLastTimeIndexArgument
        {
            get { return appOptions.OptionsPlayer.ResumesFromLastTimeIndexArgument; }
            set { appOptions.OptionsPlayer.ResumesFromLastTimeIndexArgument = value; Notify(); }
        }

        public bool RepeatsPlaylistArgument
        {
            get { return appOptions.OptionsPlayer.RepeatsPlaylistArgument; }
            set { appOptions.OptionsPlayer.RepeatsPlaylistArgument = value; Notify(); }
        }

        public bool ShufflesPlaylistArgument
        {
            get { return appOptions.OptionsPlayer.ShufflesPlaylistArgument; }
            set { appOptions.OptionsPlayer.ShufflesPlaylistArgument = value; Notify(); }
        }

        public bool PlayAndStopArgument
        {
            get { return appOptions.OptionsPlayer.PlayAndStopArgument; }
            set { appOptions.OptionsPlayer.PlayAndStopArgument = value; Notify(); }
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

        public DateTime Premiered
        {
            get { return mediaLibraryNavigation.Premiered; }
            set { mediaLibraryNavigation.Premiered = value; Notify(); }
        }

        public DateTime Added
        {
            get { return mediaLibraryNavigation.Added; }
            set { mediaLibraryNavigation.Added = value; Notify(); }
        }

        public TimeSpan Runtime
        {
            get { return mediaLibraryNavigation.Runtime; }
            set { mediaLibraryNavigation.Runtime = value; Notify(); }
        }

        private ObservableCollection<MediaTypeEntity> sourceMediaCategories = new ObservableCollection<MediaTypeEntity>();
        public ObservableCollection<MediaTypeEntity> SourceMediaCategories
        {
            get { return sourceMediaCategories; }
            set { sourceMediaCategories = value; Notify(); }
        }

        public ObservableCollection<MediaTypeSourceEntity> SourceMediaCategorySources
        {
            get { return new ObservableCollection<MediaTypeSourceEntity>(appOptions.OptionsMedia.SourceMediaCategorySources); }
            set
            {
                appOptions.OptionsMedia.SourceMediaCategorySources = value.ToList();
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

        private ObservableCollection<CastEntity> sourceActors;
        public ObservableCollection<CastEntity> SourceActors
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
        /// <param name="mediaLibrary">Injected media library business model</param>
        /// <param name="mediaLibraryNavigation">Injected media library navigation business model</param>
        /// <param name="appOptions">Injected application options</param>
        /// <param name="mediaStatistics">Injected media library statistics</param>
        /// <param name="notificationService">Injected notification service</param>
        public MainWindowVM(IMediaLibrary mediaLibrary, IMediaLibraryNavigation mediaLibraryNavigation, IAppOptions appOptions, IMediaStatistics mediaStatistics, INotificationService notificationService)
        {
            this.mediaLibrary = mediaLibrary;
            this.notificationService = notificationService;
            this.mediaLibraryNavigation = mediaLibraryNavigation;
            this.appOptions = appOptions;
            this.mediaStatistics = mediaStatistics;
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
            ShowMediaCastAsync_Command = new AsyncCommand<IMediaEntity>(ShowMediaCastAsync);
            DisplayMediaTypeInfo_Command = new SyncCommand<MediaTypeEntity>(DisplayMediaTypeStatistics);
            SetIsWatchedStatusAsync_Command = new AsyncCommand<IMediaEntity>(SetIsWatchedStatusAsync);
            ViewOpenedAsync_Command = new AsyncCommand(ViewOpenedAsync);
            ExitMediaTypeSourcesOptions_Command = new SyncCommand(ExitMediaTypeSourcesOptions);
            ExitMediaTypesOptions_Command = new SyncCommand(ExitMediaTypesOptions);
            ExitPlayerOptions_Command = new SyncCommand(ExitPlayerOptions);
            ExitInterfaceOptions_Command = new SyncCommand(ExitInterfaceOptions);
            ExitMediaCast_Command = new SyncCommand(() => AreActorsVisible = false);
            //Search_DropDownClosingAsync_Command = new AsyncCommand(Search_DropDownClosingAsync);
            SetIsFavoriteStatusAsync_Command = new AsyncCommand<IMediaEntity>(SetIsFavoriteStatusAsync);
            //DisplayOffset_ValueChanged_Command = new SyncCommand<decimal>(DisplayOffset_ValueChanged);
            mediaLibraryNavigation.Navigated += InitiateNavigation;
            OpenLoggingDirectory_Command = new SyncCommand(OpenLoggingDirectory);
            // SearchMediaLibrary_Command = new AutoCompleteFilterPredicate<object>(SearchMediaLibrary);
            this.mediaStatistics.PropertyChanged += DomainModelPropertyChanged;
            this.mediaLibraryNavigation.PropertyChanged += DomainModelPropertyChanged;
            this.appOptions.OptionsMedia.PropertyChanged += DomainModelPropertyChanged;
            this.appOptions.OptionsPlayer.PropertyChanged += DomainModelPropertyChanged;
            this.appOptions.OptionsInterface.PropertyChanged += DomainModelPropertyChanged;
            AddMediaSourceAsync_Command = new AsyncCommand<string>(AddMediaTypeSourceAsync);
            RefreshMediaSourcesAsync_Command = new AsyncCommand(UpdateMediaLibraryAsync, ValidateAddMediaTypeSource);
            ResetAddMediaSourceElements_Command = new SyncCommand(ResetAddMediaSourceElements);
            UpdatePlayerOptionsAsync_Command = new AsyncCommand(this.appOptions.OptionsPlayer.UpdatePlayerOptionsAsync, ValidateUpdatePlayerOptions);
            UpdateInterfaceOptionsAsync_Command = new AsyncCommand(this.appOptions.OptionsInterface.UpdateUserInterfaceOptionsAsync, ValidateUpdateInterfaceOptions);
            DeleteMediaTypeAsync_Command = new AsyncCommand<MediaTypeEntity>(DeleteMediaTypeAsync);
            GetMediaTypeSources_Command = new SyncCommand<MediaTypeEntity>(GetMediaTypeSources);
            DeleteMediaTypeSource_Command = new SyncCommand<MediaTypeSourceEntity>(DeleteMediaTypeSource);
            OpenMediaSourceLocation_Command = new SyncCommand<MediaTypeSourceEntity>(this.appOptions.OptionsMedia.OpenMediaTypeSourceLocation);

            ChangeRepeatArgument_Command = new SyncCommand<bool>(this.appOptions.OptionsPlayer.ChangeRepeatArgument);
            ChangeShuffleArgument_Command = new SyncCommand<bool>(this.appOptions.OptionsPlayer.ChangeShuffleArgument);
            ChangeAutoscaleArgument_Command = new SyncCommand<bool>(this.appOptions.OptionsPlayer.ChangeAutoscaleArgument);
            ChangeFullScreenArgument_Command = new SyncCommand<bool>(this.appOptions.OptionsPlayer.ChangeFullScreenArgument);
            ChangePlayAndStopArgument_Command = new SyncCommand<bool>(this.appOptions.OptionsPlayer.ChangePlayAndStopArgument);
            ChangeAlwaysOnTopArgument_Command = new SyncCommand<bool>(this.appOptions.OptionsPlayer.ChangeAlwaysOnTopArgument);
            ChangeSingleInstanceArgument_Command = new SyncCommand<bool>(this.appOptions.OptionsPlayer.ChangeSingleInstanceArgument);
            ChangeEnqueueFilesInSingleInstanceModeArgument_Command = new SyncCommand<bool>(this.appOptions.OptionsPlayer.ChangeEnqueueFilesInSingleInstanceModeArgument);

            ShowMediaOptions_Command = new SyncCommand(ShowMediaOptions);
            ShowPlayerOptions_Command = new SyncCommand(ShowPlayerOptions);
            ShowInterfaceOptions_Command = new SyncCommand(ShowInterfaceOptions);
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
        private void DomainModelPropertyChanged(string propertyName)
        {
            Notify(propertyName);
        }

        /// <summary>
        /// Gets the weather information from the webpage located at <paramref name="url"/>
        /// </summary>
        /// <param name="url">The weather.com link of the chosen area</param>
        public async Task GetWeatherInfoAsync(string url)
        {
            await mediaStatistics.GetWeatherInfoAsync(url);
        }

        /// <summary>
        /// Updates statistics related to the media library
        /// </summary>
        /// <param name="mediaType">The media type item for which to update the statistics</param>
        private void DisplayMediaTypeStatistics(MediaTypeEntity mediaType)
        {
            mediaStatistics.GetMediaTypeStatistics(mediaLibrary, mediaType);
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
        /// Shows the actors of <paramref name="media"/>
        /// </summary>
        /// <param name="media">The media for which to show the cast</param>
        private async Task ShowMediaCastAsync(IMediaEntity media)
        {
            if (media != null)
            {
                IEnumerable<CastEntity> temp = new List<CastEntity>();
                await Task.Run(() =>
                {
                    if (CurrentNavigationLevel == NavigationLevel.Episode)
                        temp = mediaLibrary.MediaCast.ShowEpisodeMediaCastAsync(mediaLibrary, media);
                    else if (CurrentNavigationLevel == NavigationLevel.TvShow)
                        temp = mediaLibrary.MediaCast.ShowTvShowMediaCastAsync(mediaLibrary, media);
                    else if (CurrentNavigationLevel == NavigationLevel.Movie)
                        temp = mediaLibrary.MediaCast.ShowMovieMediaCastAsync(mediaLibrary, media);
                });
                SourceActors = new ObservableCollection<CastEntity>(temp);
                if (CurrentNavigationLevel == NavigationLevel.TvShow || CurrentNavigationLevel == NavigationLevel.Episode || CurrentNavigationLevel == NavigationLevel.Movie)
                    AreActorsVisible = true;
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
            if (SourceMediaCategories.Count(mc => mc.MediaName == MediaName) > 0 && !appOptions.OptionsMedia.IsMediaTypeSourceUpdate)
                await notificationService.ShowAsync("The specified media name already exists!", "LEYA - Error", NotificationButton.OK, NotificationImage.Error);
            else
            {
                MediaTypeEntity item = new MediaTypeEntity() { MediaName = MediaName, IsMedia = true, MediaType = SelectedMediaCategoryType.Text, Id = appOptions.OptionsMedia.IsMediaTypeSourceUpdate ? appOptions.OptionsMedia.Id : 0 };
                // if we are updating an already existing media type, delete it and its sources before re-inserting them (might contain modifications)
                if (appOptions.OptionsMedia.IsMediaTypeSourceUpdate)
                    await DeleteMediaTypeAsync(item);
                // add the new media type and get the returned row id
                int newMediaId = await appOptions.OptionsMedia.SaveMediaTypeAsync(item);
                if (newMediaId != -1)
                    await appOptions.OptionsMedia.SaveMediaTypeSourcesAsync(newMediaId);
                // refresh the list of media types
                await appOptions.OptionsMedia.RefreshMediaTypes(mediaLibrary);
                GetMediaTypes();
                appOptions.OptionsMedia.IsMediaTypeSourceUpdate = false;
                ExitMediaTypeSourcesOptions();
                await notificationService.ShowAsync("Media library updated!", "LEYA - Success");
            }
        }

        private async Task UpdateInterfaceOptionsAsync()
        {

        }

        /// <summary>
        /// Resets the fields required to create a new media type to their default values
        /// </summary>
        private void ResetAddMediaSourceElements()
        {
            appOptions.OptionsMedia.ResetAddMediaTypeSourceElements();
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
        /// Validates the required information for updating player options
        /// </summary>
        /// <returns>True if required information is fine, False otherwise</returns>
        private bool ValidateUpdatePlayerOptions()
        {
            bool isValid = !string.IsNullOrEmpty(PlayerPath) && File.Exists(PlayerPath);
            if (!isValid && IsPlayerOptionVisible)
            {
                ShowHelpButton();
                WindowHelp = "\n";
                if (string.IsNullOrEmpty(PlayerPath))
                    WindowHelp += "Player path cannot be empty!\n";
                else if (!File.Exists(PlayerPath))
                    WindowHelp += "Player path must point to a valid file!\n";
            }
            else
                HideHelpButton();
            ValidationChanged?.Invoke(isValid);
            return isValid;
        }

        /// <summary>
        /// Validates the required information for updating user interface options
        /// </summary>
        /// <returns>True if required information is fine, False otherwise</returns>
        private bool ValidateUpdateInterfaceOptions()
        {
            return true;
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
                foreach (string mediaSource in appOptions.OptionsMedia.AddMediaTypeSource(selectedDirectories, response == NotificationResult.Yes))
                {
                    // do not add same source path twice
                    if (SourceMediaCategorySources.Count(ms => ms.MediaSourcePath == mediaSource.ToUpper()) == 0)
                    {
                        appOptions.OptionsMedia.SourceMediaCategorySources.Add(new MediaTypeSourceEntity()
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
            appOptions.OptionsMedia.SourceMediaCategorySources.Remove(item);
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
            appOptions.OptionsMedia.IsMediaTypeSourceUpdate = true;
            appOptions.OptionsMedia.Id = item.Id;
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
                await appOptions.OptionsMedia.DeleteMediaTypeAsync(media.Id);
                await appOptions.OptionsMedia.RefreshMediaTypes(mediaLibrary);
                GetMediaTypes();
            }
            catch (Exception ex) when (ex is InvalidOperationException || ex is ArgumentException)
            {
                await notificationService.ShowAsync(ex.Message, "LEYA - Error", NotificationButton.OK, NotificationImage.Error);
            }
        }

        /// <summary>
        /// Gets the user defined media types
        /// </summary>
        public void GetMediaTypes()
        {
            ShowProgressBar();
            SourceMediaCategories = new ObservableCollection<MediaTypeEntity>(appOptions.OptionsMedia.GetMediaTypesAsync(mediaLibrary));
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

        /// <summary>
        /// Displays the options related to the media player
        /// </summary>
        private void ShowPlayerOptions()
        {
            IsPlayerOptionVisible = true;
            UpdatePlayerOptionsAsync_Command.RaiseCanExecuteChanged();
            appOptions.OptionsPlayer.GetPlayerOptions();
        }

        /// <summary>
        /// Displays the options related to the user interface
        /// </summary>
        private void ShowInterfaceOptions()
        {
            IsInterfaceOptionVisible = true;
            UpdatePlayerOptionsAsync_Command.RaiseCanExecuteChanged(); // ?
            appOptions.OptionsInterface.GetUserInterfaceOptions();
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
                    IsPlayerOptionVisible = false;
                    IsMediaContainerVisible = false;
                    IsMaskBackgroundVisible = false;
                    IsInterfaceOptionVisible = false;
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

        /// <summary>
        /// Exits the player options view and returns to the options list
        /// </summary>
        private void ExitPlayerOptions()
        {
            IsPlayerOptionVisible = false;
            IsHelpButtonVisible = false;
        }

        /// <summary>
        /// Exits the interface options view and returns to the options list
        /// </summary>
        private void ExitInterfaceOptions()
        {
            IsInterfaceOptionVisible = false;
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
            IsMediaPlayingIndicatorSocketVisible = true;
            try
            {
                await mediaLibrary.GetMediaLibraryAsync();
            }
            catch (Exception ex)
            {
                ScannerOutput = string.Empty;
                IsScannerTextVisible = true;
            }
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
            // update the displayed details of the selected media item, depending on its type
            if (SourceMediaSelectedItem != null)
            {
                if (CurrentNavigationLevel == NavigationLevel.TvShow) // if the UI displays a list of tv shows
                    mediaLibraryNavigation.GetTvShowMediaInfo(mediaLibrary, SourceMediaSelectedItem.Id);
                else if (CurrentNavigationLevel == NavigationLevel.Season) // if the UI displays a list of tv show seasons
                    mediaLibraryNavigation.GetSeasonMediaInfo(mediaLibrary, SourceMediaSelectedItem.Id, SourceMediaSelectedItem.SeasonOrAlbumId);
                else if (CurrentNavigationLevel == NavigationLevel.Episode) // if the UI displays a list of tv show episodes
                    mediaLibraryNavigation.GetEpisodeMediaInfo(mediaLibrary, SourceMediaSelectedItem.Id, SourceMediaSelectedItem.SeasonOrAlbumId, SourceMediaSelectedItem.MediaName);
                else if (CurrentNavigationLevel == NavigationLevel.Movie) // if the UI displays a list of movies
                    mediaLibraryNavigation.GetMovieMediaInfo(mediaLibrary, SourceMediaSelectedItem.Id);
                else if (CurrentNavigationLevel == NavigationLevel.Artist) // if the UI displays a list of artists
                    mediaLibraryNavigation.GetArtistMediaInfo(mediaLibrary, SourceMediaSelectedItem.Id);
                else if (CurrentNavigationLevel == NavigationLevel.Album) // if the UI displays a list of albums
                    mediaLibraryNavigation.GetAlbumMediaInfo(mediaLibrary, SourceMediaSelectedItem.Id, SourceMediaSelectedItem.SeasonOrAlbumId);
                else if (CurrentNavigationLevel == NavigationLevel.Song) // if the UI displays a list of songs
                    mediaLibraryNavigation.GetSongMediaInfo(mediaLibrary, SourceMediaSelectedItem.Id, SourceMediaSelectedItem.SeasonOrAlbumId, SourceMediaSelectedItem.MediaName);
            }
        }
        #endregion
    }
}
