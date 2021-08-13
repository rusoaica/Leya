/// Written by: Yulia Danilova
/// Creation Date: 23rd of November, 2020
/// Purpose: Data transfer object for the tv show ratings 

namespace Leya.Models.Common.Models.TvShows
{
    public class TvShowRatingEntity
    {
        #region ================================================================ PROPERTIES =================================================================================
        public int Id { get; set; }
        public int Max { get; set; }
        public int Votes { get; set; }
        public int TvShowId { get; set; }
        public string Name { get; set; }
        public decimal Value { get; set; }
        #endregion

        #region ================================================================= METHODS ===================================================================================
        /// <summary>
        /// Customized ToString() method
        /// </summary>
        /// <returns>Custom string value showing relevant data for current class</returns>
        public override string ToString()
        {
            return Id + " :: " + Name;
        }
        #endregion
    }
}
