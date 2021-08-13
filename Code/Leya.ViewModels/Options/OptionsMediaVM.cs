/// Written by: Yulia Danilova
/// Creation Date: 19th of November, 2020
/// Purpose: View Model for the OptionsMedia page
#region ========================================================================= USING =====================================================================================
using System;
using System.IO;
using System.Linq;
using System.Diagnostics;
using System.Threading.Tasks;
using Leya.Infrastructure.Enums;
using System.Collections.Generic;
using Leya.ViewModels.Common.MVVM;
using System.Collections.ObjectModel;
using Leya.Models.Common.Models.Media;
using Leya.Models.Common.Models.Common;
using Leya.Infrastructure.Notification;
#endregion

namespace Leya.ViewModels.Options
{
    public class OptionsMediaVM : BaseModel, IOptionsMediaVM
    {
        #region ============================================================== FIELD MEMBERS ================================================================================
        public event Action<bool> ValidationChanged;
        private bool isMediaTypeSourceUpdate;
        #endregion

        #region ============================================================= BINDING COMMANDS ==============================================================================
        public IAsyncCommand<string> AddMediaSourceAsync_Command { get; private set; }
        public ISyncCommand<MediaTypeSourceEntity> DeleteMediaSource_Command { get; private set; }
        public IAsyncCommand<MediaTypeSourceEntity> OpenMediaSourceLocationAsync_Command { get; private set; }
        public IAsyncCommand Page_LoadedAsync_Command { get; private set; }
        public IAsyncCommand<MediaTypeEntity> DeleteMediaTypeAsync_Command { get; private set; }
        public IAsyncCommand<MediaTypeEntity> GetMediaTypeSourcesAsync_Command { get; private set; }
        public SyncCommand ResetAddMediaSourceElements_Command { get; private set; }
        public AsyncCommand RefreshMediaSourcesAsync_Command { get; private set; }
        #endregion

        #region ============================================================ BINDING PROPERTIES =============================================================================      
        private string mediaName;
        public string MediaName
        {
            get { return mediaName; }
            set { mediaName = value?.ToUpper(); Notify(); RefreshMediaSourcesAsync_Command?.RaiseCanExecuteChanged(); }
        }

        private ObservableCollection<MediaTypeEntity> sourceMedia = new ObservableCollection<MediaTypeEntity>();
        public ObservableCollection<MediaTypeEntity> SourceMedia
        {
            get { return sourceMedia; }
            set { sourceMedia = value; Notify(); }
        }

        private MediaTypeEntity sourceMediaSelectedItem;
        public MediaTypeEntity SourceMediaSelectedItem
        {
            get { return sourceMediaSelectedItem; }
            set { sourceMediaSelectedItem = value; Notify(); }
        }

        private ObservableCollection<MediaTypeSourceEntity> sourceMediaSources = new ObservableCollection<MediaTypeSourceEntity>();
        public ObservableCollection<MediaTypeSourceEntity> SourceMediaSources
        {
            get { return sourceMediaSources; }
            set { sourceMediaSources = value; Notify(); RefreshMediaSourcesAsync_Command?.RaiseCanExecuteChanged(); }
        }

        private ObservableCollection<SearchEntity> sourceMediaTypes = new ObservableCollection<SearchEntity>();
        public ObservableCollection<SearchEntity> SourceMediaTypes
        {
            get { return sourceMediaTypes; }
            set { sourceMediaTypes = value; Notify(); }
        }

        private SearchEntity selectedMediaType;
        public SearchEntity SelectedMediaType
        {
            get { return selectedMediaType; }
            set { selectedMediaType = value; Notify(); RefreshMediaSourcesAsync_Command?.RaiseCanExecuteChanged(); }
        }
        #endregion

        #region ================================================================ PROPERTIES =================================================================================
        public int Id { get; set; }
        #endregion

        #region ================================================================== CTOR =====================================================================================
        /// <summary>
        /// Default C-tor
        /// </summary>
        public OptionsMediaVM(INotificationService notificationService)
        {
            AddMediaSourceAsync_Command = new AsyncCommand<string>(AddMediaSourceAsync);
            ResetAddMediaSourceElements_Command = new SyncCommand(ResetAddMediaSourceElements);
            DeleteMediaSource_Command = new SyncCommand<MediaTypeSourceEntity>(DeleteMediaTypeSource);
            OpenMediaSourceLocationAsync_Command = new AsyncCommand<MediaTypeSourceEntity>(OpenMediaSourceLocationAsync);
            Page_LoadedAsync_Command = new AsyncCommand(Page_ContentRenderedAsync);
            DeleteMediaTypeAsync_Command = new AsyncCommand<MediaTypeEntity>(DeleteMediaTypeAsync);
            GetMediaTypeSourcesAsync_Command = new AsyncCommand<MediaTypeEntity>(GetMediaTypeSourcesAsync);
            RefreshMediaSourcesAsync_Command = new AsyncCommand(UpdateMediaLibraryAsync, ValidateAddMediaSource);
            SourceMediaTypes = new ObservableCollection<SearchEntity>()
            {
                new SearchEntity() { Text = "TV SHOW" },
                new SearchEntity() { Text = "MOVIE" },
                new SearchEntity() { Text = "MUSIC" },
            };
            this.notificationService = notificationService;
        }
        #endregion

        #region ================================================================= METHODS ===================================================================================
        /// <summary>
        /// Resets the fields required to create a new media type to their default values
        /// </summary>
        private void ResetAddMediaSourceElements()
        {
            Id = 0;
            MediaName = null;
            SelectedMediaType = null;
            SourceMediaSources.Clear();
            isMediaTypeSourceUpdate = false;
        }

        /// <summary>
        /// Opens the location of the <paramref name="_item"/> path
        /// </summary>
        /// <param name="_item">The item containing the path to the location to be opened</param>
        private async Task OpenMediaSourceLocationAsync(MediaTypeSourceEntity _item)
        {
            if (!string.IsNullOrEmpty(_item.MediaSourcePath) && Directory.Exists(_item.MediaSourcePath))
                Process.Start(_item.MediaSourcePath);
            else
                await notificationService.ShowAsync("The directory does not exist!", "LEYA - Error", NotificationButton.OK, NotificationImage.Error);
        }

        /// <summary>
        /// Removes <paramref name="_item"/> from the list of media type sources
        /// </summary>
        /// <param name="_item">The item to be removed from the media sources</param>
        private void DeleteMediaTypeSource(MediaTypeSourceEntity _item)
        {
            SourceMediaSources.Remove(_item);
            RefreshMediaSourcesAsync_Command.RaiseCanExecuteChanged();
        }

        /// <summary>
        /// Deletes a media type and all its associated media type sources
        /// </summary>
        /// <param name="_item"></param>
        private async Task DeleteMediaTypeAsync(MediaTypeEntity _item)
        {
            //if (!string.IsNullOrEmpty((await GetService<IMediaTypeData>().DeleteAsync(_item.Id))?.Error))
            //    notificationService.Show("Error deleting the media type in the database!", "LEYA - Error", NotificationButton.OK, NotificationImage.Error);
            await GetMediaTypesAsync();
        }

        /// <summary>
        /// Adds a new media source to the selected media
        /// </summary>
        /// <param name="_path">The path of the media source to be added</param>
        private async Task AddMediaSourceAsync(string _path)
        {
            if (Directory.Exists(_path))
            {
                List<string> _paths = new List<string>();
                // ask user if the folder is a single media container, or if it contains other subfolders (ex: folder containing multiple tv shows)
                if (await notificationService.ShowAsync("Selected folder contains a single media?", "LEYA", NotificationButton.YesNo, NotificationImage.Question) == NotificationResult.Yes)
                    _paths.Add(_path);
                else
                    foreach (string _folder in Directory.EnumerateDirectories(_path, "*.*", SearchOption.TopDirectoryOnly))
                        _paths.Add(_folder);
                foreach (string _media_source in _paths)
                {
                    // do not add same source path twice
                    if (SourceMediaSources.Where(_e => _e.MediaSourcePath == _media_source.ToUpper()).Count() == 0)
                    {
                        SourceMediaSources.Add(new MediaTypeSourceEntity()
                        {
                            MediaSourcePath = _media_source.ToUpper()
                        });
                    }
                }
                RefreshMediaSourcesAsync_Command.RaiseCanExecuteChanged();
            }
            else
                await notificationService.ShowAsync("The directory does not exist!", "LEYA - Error", NotificationButton.OK, NotificationImage.Error);
        }

        /// <summary>
        /// Validates the required information for adding a new media source
        /// </summary>
        /// <returns>True if required information is fine, False otherwise</returns>
        private bool ValidateAddMediaSource()
        {
            bool _is_valid = !string.IsNullOrEmpty(MediaName) && SourceMediaSources.Count > 0 && SelectedMediaType != null && !string.IsNullOrEmpty(SelectedMediaType.Text);
            if (!_is_valid)
            {
                ShowHelpButton();
                WindowHelp = "\n";
                if (string.IsNullOrEmpty(MediaName))
                    WindowHelp += "Media name cannot be empty!\n";
                if (SourceMediaSources.Count == 0)
                    WindowHelp += "No media source added!\n";
                if (SelectedMediaType != null && string.IsNullOrEmpty(SelectedMediaType.Text))
                    WindowHelp += "No media type chosen!\n";
            }
            else
                HideHelpButton();
            ValidationChanged?.Invoke(_is_valid);
            return _is_valid;
        }

        /// <summary>
        /// Refreshes the media library
        /// </summary>
        private async Task UpdateMediaLibraryAsync()
        {
            // do not allow two medias with same name
            if (SourceMedia.Where(_e => _e.MediaName == MediaName).Count() > 0 && !isMediaTypeSourceUpdate)
                await notificationService.ShowAsync("The specified media name already exists!", "LEYA - Error", NotificationButton.OK, NotificationImage.Error);
            else
            {
                MediaTypeEntity _item = new MediaTypeEntity() { MediaName = MediaName, IsMedia = true, MediaType = SelectedMediaType.Text, Id = isMediaTypeSourceUpdate ? Id : 0 };
                // if we are updating an already existing media type, delete it and its sources before re-inserting them (might contain modifications)
                if (isMediaTypeSourceUpdate)
                    await DeleteMediaTypeAsync(_item);
                // add the new media type and get the returned row id
                int _new_media_id = await SaveMediaTypeAsync(_item);
                if (_new_media_id != -1)
                    await SaveMediaLibrarySourcesAsync(_new_media_id);
                // refresh the list of media types
                await GetMediaTypesAsync();
                isMediaTypeSourceUpdate = false;
            }
        }

        /// <summary>
        /// Inserts media type paths for the specified media type
        /// <param name="_mediaTypeId">The Id of the media type for which to add the media type paths</param>
        /// </summary>
        private async Task SaveMediaLibrarySourcesAsync(int _mediaTypeId)
        {
            // iterate all media type sources and send them to the API
            foreach (MediaTypeSourceEntity _source in SourceMediaSources)
            {
                // assign the media type id to which these media type sources belong to
                _source.MediaTypeId = _mediaTypeId;
                //if (!string.IsNullOrEmpty((await GetService<IMediaTypeSourceData>().InsertAsync(AutoMapper.Map<MediaTypeSourceDB>(_source))).Error))
                //    notificationService.Show("Error inserting the media type source in the database!", "LEYA - Error", NotificationButton.OK, NotificationImage.Error);
            }
        }

        /// <summary>
        /// Sends <paramref name="_media"/> to the API in order to save it in the database
        /// </summary>
        /// <param name="_media">The media to be saved</param>
        public async Task<int> SaveMediaTypeAsync(MediaTypeEntity _media)
        {
            //ApiResponseM<int> _response = (await GetService<IMediaTypeData>().InsertAsync(AutoMapper.Map<MediaTypeDB>(_media)));
            //if (!string.IsNullOrEmpty(_response.Error))
            //    notificationService.Show("Error inserting the media type in the database!", "LEYA - Error", NotificationButton.OK, NotificationImage.Error);
            //return _response.Data?[0] ?? -1;
            return 1;
        }

        /// <summary>
        /// Gets the media type sources for <paramref name="_item"/>
        /// </summary>
        /// <param name="_item">The media type for which to get the media type sources</param>
        private async Task GetMediaTypeSourcesAsync(MediaTypeEntity _item)
        {
            ShowProgressBar();
            SourceMediaSources.Clear();
            isMediaTypeSourceUpdate = true;
            Id = _item.Id;
            // get the media type sources from the API
            //foreach (MediaTypeSourceDB _media_type_source in (await GetService<IMediaTypeSourceData>().GetByIdAsync(_item.Id))?.Data ?? Enumerable.Empty<object>())
                //SourceMediaSources.Add(AutoMapper.Map<MediaTypeSourceEntity>(_media_type_source));
            // re-assign the current media name and selected media type
            MediaName = _item.MediaName;
            SelectedMediaType = SourceMediaTypes.Where(_e => _e.Text == _item.MediaType).First();
            HideProgressBar();
        }

        /// <summary>
        /// Gets the user defined media items from the API
        /// </summary>
        private async Task GetMediaTypesAsync()
        {
            ShowProgressBar();
            SourceMedia.Clear();
            // get the media types from the API
            //foreach (MediaTypeDB _media_type in (await GetService<IMediaTypeData>().GetAllAsync())?.Data ?? Enumerable.Empty<object>())
            //    if (_media_type.IsMedia)
            //        SourceMedia.Add(AutoMapper.Map<MediaTypeEntity>(_media_type));
            HideProgressBar();
        }
        #endregion

        #region ============================================================= EVENT HANDLERS ================================================================================
        /// <summary>
        /// Handles the ContentRendered event of the Page
        /// </summary>
        private async Task Page_ContentRenderedAsync()
        {
            await GetMediaTypesAsync();
        }
        #endregion
    }
}
