/// Written by: Yulia Danilova
/// Creation Date: 11th of November, 2020
/// Purpose: Interface business model for media library
#region ========================================================================= USING =====================================================================================
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Leya.Models.Core.Navigation;
using Leya.Models.Common.Models.Media;
using Leya.Models.Common.Models.Common;
#endregion

namespace Leya.Models.Core.MediaLibrary
{
    public interface IMediaLibrary
    {
        #region ============================================================== FIELD MEMBERS ================================================================================
        event Action LibraryLoaded;
        event Action MediaTypesLoaded;
        #endregion

        #region ================================================================ PROPERTIES =================================================================================
        IMediaCast MediaCast { get; }
        IMediaState MediaState { get; }
        IMediaStatistics MediaStatistics { get; }
        IMediaLibraryNavigation Navigation { get; }
        MediaLibraryEntity Library { get; set; }
        List<SearchEntity> SourceSearch { get; set; }
        #endregion

        #region ================================================================= METHODS ===================================================================================
        /// <summary>
        /// Gets all the media library from the storage medium
        /// </summary>
        Task GetMediaLibraryAsync();

        /// <summary>
        /// Deletes all media library from the storage medium
        /// </summary>
        Task UpdateMediaLibraryAsync();
        #endregion
    }
}
