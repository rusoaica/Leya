/// Written by: Yulia Danilova
/// Creation Date: 23rd of November, 2020
/// Purpose: Data transfer object for the movie resume information

namespace Leya.Models.Common.Models.Movies
{
    public class MovieResumeEntity
    {
        #region ================================================================ PROPERTIES =================================================================================
        public int Id { get; set; }
        public int Total { get; set; }
        public int MovieId { get; set; }
        public int Position { get; set; }
        public int EpisodeId { get; set; }
        public int NamedSeasonId { get; set; }
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
