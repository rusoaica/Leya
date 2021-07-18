/// Written by: Yulia Danilova
/// Creation Date: 17th of November, 2020
/// Purpose: Data transfer object for the list of media type sources

namespace Leya.Models.Common.Models.Media
{
    public class MediaTypeSourceEntity
    {
        #region ================================================================ PROPERTIES =================================================================================
        public int Id { get; set; }
        public int MediaTypeId { get; set; }
        public string MediaSourcePath { get; set; }
        #endregion

        #region ================================================================= METHODS ===================================================================================
        /// <summary>
        /// Customized ToString() method
        /// </summary>
        /// <returns>Custom string value showing relevant data for current class</returns>
        public override string ToString()
        {
            return Id + " :: " + MediaTypeId;
        }
        #endregion  
    }
}
