/// Written by: Yulia Danilova
/// Creation Date: 23rd of November, 2020
/// Purpose: Data transfer object for the subtitles information

namespace Leya.Models.Common.Models.Movies
{
    public class SubtitleEntity
    {
        #region ================================================================ PROPERTIES =================================================================================
        public int Id { get; set; }
        public int MovieId { get; set; }
        public string Language { get; set; }
        #endregion

        #region ================================================================= METHODS ===================================================================================
        /// <summary>
        /// Customized ToString() method
        /// </summary>
        /// <returns>Custom string value showing relevant data for current class</returns>
        public override string ToString()
        {
            return Id + " :: " + MovieId;
        }
        #endregion
    }
}
