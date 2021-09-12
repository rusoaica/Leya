/// Written by: Yulia Danilova
/// Creation Date: 09th of September, 2021
/// Purpose: Model for the list of quick search results 

namespace Leya.Models.Common.Models.Common
{
    public class FilterEntity
    {
        #region ================================================================ PROPERTIES =================================================================================
        public string ChildTitle { get; set; }
        public string ParentTitle { get; set; } 
        public string MediaItemPath { get; set; }
        public string[] Tags { get; set; }
        public string[] Roles { get; set; } 
        public string[] Genres { get; set; }
        public string[] Members { get; set; }
        #endregion
    }
}
