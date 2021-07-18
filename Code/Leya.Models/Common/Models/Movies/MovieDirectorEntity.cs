/// Written by: Yulia Danilova
/// Creation Date: 07th of July, 2021
/// Purpose: Data transfer object for the movie directors

namespace Leya.Models.Common.Models.Movies
{
    public class MovieDirectorEntity
    {
        #region ================================================================ PROPERTIES =================================================================================
        public int Id { get; set; }
        public int MovieId { get; set; }
        public string Director { get; set; }
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
