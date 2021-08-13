/// Written by: Yulia Danilova
/// Creation Date: 19th of November, 2020
/// Purpose: Business model for media options
#region ========================================================================= USING =====================================================================================
using System.IO;
using System.Linq;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.Generic;
using Leya.Models.Core.MediaLibrary;
using System.Text.RegularExpressions;
using Leya.Models.Common.Broadcasting;
using Leya.Models.Common.Models.Media;
using Leya.Models.Common.Models.Common;
#endregion

namespace Leya.Models.Core.Options
{
    public class OptionsMedia : NotifyPropertyChanged, IOptionsMedia
    {
        #region ============================================================== FIELD MEMBERS ================================================================================
        private readonly IMediaType mediaType;
        private readonly IMediaTypeSource mediaTypeSource;
        public bool IsMediaTypeSourceUpdate { get; set; }
        #endregion

        public int Id { get; set; }

        private string mediaName;
        public string MediaName
        {
            get { return mediaName; }
            set { mediaName = value?.ToUpper(); Notify(); }
        }


        private List<MediaTypeSourceEntity> sourceMediaCategorySources = new List<MediaTypeSourceEntity>();
        public List<MediaTypeSourceEntity> SourceMediaCategorySources
        {
            get { return sourceMediaCategorySources; }
            set { sourceMediaCategorySources = value; Notify(); }
        }


        private SearchEntity selectedMediaCategoryType;
        public SearchEntity SelectedMediaCategoryType
        {
            get { return selectedMediaCategoryType; }
            set { selectedMediaCategoryType = value; Notify(); }
        }

        #region ================================================================== CTOR =====================================================================================
        public OptionsMedia(IMediaType mediaType, IMediaTypeSource mediaTypeSource)
        {
            this.mediaType = mediaType;
            this.mediaTypeSource = mediaTypeSource;
        }
        #endregion

        #region ================================================================= METHODS ===================================================================================
        /// <summary>
        /// Deletes an entity identified by <paramref name="id"/> from the storage medium
        /// </summary>
        /// <param name="id">The id of the entity to be deleted</param>
        public async Task DeleteMediaTypeAsync(int id)
        {
            await mediaType.DeleteMediaTypeAsync(id);
        }

        public async Task<int> SaveMediaTypeAsync(MediaTypeEntity media)
        {
            return await mediaType.AddMediaTypeAsync(media);
        }

        public async Task SaveMediaTypeSourceAsync(MediaTypeSourceEntity media)
        {            
            await mediaTypeSource.InsertMediaTypeSource(media);
        }

        public async Task RefreshMediaTypes(IMediaLibrary mediaLibrary)
        {
            await mediaType.GetMediaTypesAsync();
            mediaLibrary.Library.MediaTypes = mediaType.MediaTypes;
        }

        /// <summary>
        /// Adds a new media source to the selected media
        /// </summary>
        /// <param name="selectedDirectories">The paths of the selected directories representing the media sources to be added</param>
        public IEnumerable<string> AddMediaTypeSource(string selectedDirectories, bool containsSingleMedia)
        {
            string[] directories = Regex.Split(selectedDirectories, "\""); // better way!
            if (directories.Where(d => !string.IsNullOrWhiteSpace(d)).All(d => Directory.Exists(d)))
            {
                List<string> allPaths = new List<string>();
                // ask user if the folder is a single media container, or if it contains other subfolders (ex: folder containing multiple tv shows)
                if (containsSingleMedia)
                    allPaths.AddRange(directories.Where(d => !string.IsNullOrWhiteSpace(d)));
                else
                    foreach (string path in directories.Where(d => !string.IsNullOrWhiteSpace(d)))
                        foreach (string directory in Directory.EnumerateDirectories(path, "*.*", SearchOption.TopDirectoryOnly))
                            allPaths.Add(directory);
                foreach (string mediaSource in allPaths)
                    yield return mediaSource.ToUpper();
            }
            else
                throw new IOException("The directory does not exist!");
        }



        /// <summary>
        /// Resets the fields required to create a new media type to their default values
        /// </summary>
        public void ResetAddMediaSourceElements()
        {
            Id = 0;
            MediaName = null;
            SelectedMediaCategoryType = null;
            SourceMediaCategorySources.Clear();
            IsMediaTypeSourceUpdate = false;
        }

        /// <summary>
        /// Inserts media type paths for the specified media type
        /// <param name="mediaTypeId">The Id of the media type for which to add the media type paths</param>
        /// </summary>
        public async Task SaveMediaLibrarySourcesAsync(int mediaTypeId)
        {
            // iterate all media type sources and send them to the API
            foreach (MediaTypeSourceEntity source in SourceMediaCategorySources)
            {
                source.MediaTypeId = mediaTypeId;
                await SaveMediaTypeSourceAsync(source);
            }
        }


        /// <summary>
        /// Opens the location of the <paramref name="item"/> path
        /// </summary>
        /// <param name="item">The item containing the path to the location to be opened</param>
        public void OpenMediaSourceLocation(MediaTypeSourceEntity item)
        {
            if (!string.IsNullOrEmpty(item.MediaSourcePath) && Directory.Exists(item.MediaSourcePath))
                Process.Start(item.MediaSourcePath); // TODO: cross-platform explorer process!
            else
                throw new IOException("The directory does not exist!");
        }











        public IEnumerable<MediaTypeEntity> GetMediaTypesAsync(IMediaLibrary mediaLibrary) // ```0
        {
            foreach (MediaTypeEntity mediaType in mediaLibrary.Library.MediaTypes.Where(mt => mt.IsMedia))
                yield return mediaType;
        }
        #endregion
    }
}
