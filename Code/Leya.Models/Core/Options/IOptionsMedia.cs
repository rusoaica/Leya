/// Written by: Yulia Danilova
/// Creation Date: 19th of November, 2020
/// Purpose: Interface business model for media options
#region ========================================================================= USING =====================================================================================
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Leya.Models.Common.Models.Media;
using Leya.Models.Core.MediaLibrary;
#endregion

namespace Leya.Models.Core.Options
{
    public interface IOptionsMedia
    {
        int Id { get; set; }
        bool IsMediaTypeSourceUpdate { get; set; }

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
