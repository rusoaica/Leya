/// Written by: Yulia Danilova
/// Creation Date: 23rd of November, 2020
/// Purpose: Data trabsfer object for the tv show actors 

namespace Leya.Models.Common.Models.TvShows
{
    public class TvShowActorEntity
    {
        #region ================================================================ PROPERTIES =================================================================================
        public int Id { get; set; }
        public int Order { get; set; }
        public int TvShowId { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
        public string Thumb { get; set; }
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
