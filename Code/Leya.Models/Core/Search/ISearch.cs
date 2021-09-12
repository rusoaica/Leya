/// Written by: Yulia Danilova
/// Creation Date: 03rd of August, 2021
/// Purpose: Interface business model for media library searching
#region ========================================================================= USING =====================================================================================
using System.Threading.Tasks;
using System.Collections.Generic;
using Leya.Models.Common.Models.Media;
using Leya.Models.Common.Models.Common;
#endregion

namespace Leya.Models.Core.Search
{
    public interface ISearch
    {
        #region ================================================================ PROPERTIES =================================================================================
        string SearchMediaTag { get; set; }
        string SearchMediaName { get; set; }
        string SearchMediaRole { get; set; }
        string SearchMediaGenre { get; set; }
        string SearchMediaMember { get; set; }
        MediaTypeEntity SelectedSearchMediaType { get; set; }
        #endregion

        #region ================================================================= METHODS ===================================================================================
        /// <summary>
        /// Clears the advanced search terms
        /// </summary>
        void ClearAdvancedSearchTerms();

        /// <summary>
        /// Searches the media library
        /// </summary>
        Task<IEnumerable<AdvancedSearchResultEntity>> SearchLibraryAsync();
        #endregion
    }
}
