/// Written by: Yulia Danilova
/// Creation Date: 08th of September, 2021
/// Purpose: Model for the list of advanced search results 
#region ========================================================================= USING =====================================================================================
using Leya.Infrastructure.Enums;
#endregion

namespace Leya.Models.Common.Models.Common
{
    public class AdvancedSearchResultEntity
    {
        #region ================================================================ PROPERTIES =================================================================================
        public int Id { get; set; }
        public string MediaTitle { get; set; }
        public NavigationLevel MediaType { get; set; }
        #endregion

        #region ================================================================= METHODS ===================================================================================
        /// <summary>
        /// Customized ToString() method
        /// </summary>
        /// <returns>Custom string value showing relevant data for current class</returns>
        public override string ToString()
        {
            return MediaTitle;
        }
        #endregion
    }
}
