/// Written by: Yulia Danilova
/// Creation Date: 19th of November, 2020
/// Purpose: Interface business model for media options
#region ========================================================================= USING =====================================================================================
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Leya.Models.Common.Models.Media;
using Leya.Models.Core.MediaLibrary;
using Leya.Models.Common.Models.Common;
#endregion

namespace Leya.Models.Core.Options
{
    public interface IOptionsMedia
    {
        #region ============================================================== FIELD MEMBERS ================================================================================
        event Action<string, string> PropertyChanged;
        #endregion

        #region ================================================================ PROPERTIES =================================================================================
        int Id { get; set; }
        bool IsMediaTypeSourceUpdate { get; set; }
        string MediaName { get; set; }
        SearchEntity SelectedMediaCategoryType { get; set; }
        List<MediaTypeSourceEntity> SourceMediaCategorySources { get; set; }
        #endregion

        #region ================================================================= METHODS ===================================================================================
        Task DeleteMediaTypeAsync(int id);
        Task<int> SaveMediaTypeAsync(MediaTypeEntity media);
        Task SaveMediaTypeSourceAsync(MediaTypeSourceEntity media);
        Task RefreshMediaTypes(IMediaLibrary mediaLibrary);
        IEnumerable<string> AddMediaTypeSource(string selectedDirectories, bool containsSingleMedia);

        void ResetAddMediaSourceElements();
        Task SaveMediaLibrarySourcesAsync(int mediaTypeId);
        void OpenMediaSourceLocation(MediaTypeSourceEntity _item);

        IEnumerable<MediaTypeEntity> GetMediaTypesAsync(IMediaLibrary mediaLibrary);
        #endregion
    }
}
