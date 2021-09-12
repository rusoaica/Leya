/// Written by: Yulia Danilova
/// Creation Date: 19th of November, 2020
/// Purpose: Interface business model for media options
#region ========================================================================= USING =====================================================================================
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Leya.Models.Core.MediaLibrary;
using Leya.Models.Common.Models.Media;
using Leya.Models.Common.Models.Common;
#endregion

namespace Leya.Models.Core.Options
{
    public interface IOptionsMedia
    {
        #region ============================================================== FIELD MEMBERS ================================================================================
        event Action<string> PropertyChanged;
        #endregion

        #region ================================================================ PROPERTIES =================================================================================
        int Id { get; set; }
        bool IsMediaTypeSourceUpdate { get; set; }
        string MediaName { get; set; }
        SearchEntity SelectedMediaCategoryType { get; set; }
        List<MediaTypeSourceEntity> SourceMediaCategorySources { get; set; }
        #endregion

        #region ================================================================= METHODS ===================================================================================
        /// <summary>
        /// Deletes a media type entity identified by <paramref name="id"/> from the storage medium
        /// </summary>
        /// <param name="id">The id of the media type entity to be deleted</param>
        Task DeleteMediaTypeAsync(int id);

        /// <summary>
        /// Saves <paramref name="media"/> in the storage medium
        /// </summary>
        /// <param name="media">The media type entity to be stored</param>
        Task<int> SaveMediaTypeAsync(MediaTypeEntity media);

        /// <summary>
        /// Saves <paramref name="media"/> in the storage medium
        /// </summary>
        /// <param name="media">The media type source to be stored</param>
        Task SaveMediaTypeSourceAsync(MediaTypeSourceEntity media);

        /// <summary>
        /// Gets the list of media types from the storage medium
        /// </summary>
        /// <param name="mediaLibrary">The media library whose list of media types is refreshed</param>
        Task RefreshMediaTypes(IMediaLibrary mediaLibrary);

        /// <summary>
        /// Gets a list of directory paths that are used as media type sources
        /// </summary>
        /// <param name="selectedDirectories">The paths of the selected directories representing the media sources to be added</param>
        /// <param name="selectedDirectories">Indicates whether <paramref name="selectedDirectories"/> should be used, or the directories contained inside them</param>
        /// <returns>A list of directories representing the media type sources in <paramref name="selectedDirectories"/></returns>
        IEnumerable<string> GetMediaTypeSource(string selectedDirectories, bool containsSingleMedia);

        /// <summary>
        /// Resets the fields required to create a new media type to their default values
        /// </summary>
        void ResetAddMediaTypeSourceElements();

        /// <summary>
        /// Inserts media type source paths for the specified media type in the storage medium
        /// <param name="mediaTypeId">The Id of the media type for which to add the media type sources</param>
        /// </summary>
        Task SaveMediaTypeSourcesAsync(int mediaTypeId);

        /// <summary>
        /// Opens the location of the <paramref name="item"/>'s path
        /// </summary>
        /// <param name="item">The item containing the path to the location to be opened</param>
        void OpenMediaTypeSourceLocation(MediaTypeSourceEntity item);

        /// <summary>
        /// Gets the media types that are media items
        /// </summary>
        /// <param name="mediaLibrary">The media library from which to take the media types</param>
        /// <returns>A list of media type entities</returns>
        IEnumerable<MediaTypeEntity> GetMediaTypesAsync(IMediaLibrary mediaLibrary);
        #endregion
    }
}
