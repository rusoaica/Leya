/// Written by: Yulia Danilova
/// Creation Date: 23rd of November, 2020
/// Purpose: Data transfer object for the tv show resume 

namespace Leya.Models.Common.Models.TvShows
{
    public class TvShowResumeEntity
    {
        #region ================================================================ PROPERTIES =================================================================================
        public int Id { get; set; }
        public int Total { get; set; }
        public int TvShowId { get; set; }
        public int SeasonId { get; set; }
        public int Position { get; set; }
        public int EpisodeId { get; set; }
        #endregion

        #region ================================================================= METHODS ===================================================================================
        /// <summary>
        /// Customized ToString() method
        /// </summary>
        /// <returns>Custom string value showing relevant data for current class</returns>
        public override string ToString()
        {
            return Id + " :: " + Position;
        }
        #endregion
    }
}
