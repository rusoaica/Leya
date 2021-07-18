/// Written by: Yulia Danilova
/// Creation Date: 23rd of November, 2020
/// Purpose: Data transfer object for the stream details information

namespace Leya.Models.Common.Models.Movies
{
    public class StreamDetailEntity
    {
        #region ================================================================ PROPERTIES =================================================================================
        public int Id { get; set; }
        public int MovieId { get; set; }
        public VideoEntity Video { get; set; }
        public AudioEntity Audio { get; set; }
        public SubtitleEntity Subtitle { get; set; }
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
