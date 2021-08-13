/// Written by: Yulia Danilova
/// Creation Date: 19th of May, 2021
/// Purpose: Deserialization model for the TvShowGenre storage container
#region ========================================================================= USING =====================================================================================
using Leya.DataAccess.StorageAccess;
using Leya.DataAccess.Common.Attributes;
#endregion

namespace Leya.DataAccess.Common.Models.TvShows
{
    public sealed class TvShowGenreEntity : IStorageEntity
    {
        #region ================================================================ PROPERTIES =================================================================================
        [IgnoreOnInsert]
        public int Id { get; set; }
        public int TvShowId { get; set; }
        public string Genre { get; set; }
        #endregion

        #region ================================================================= METHODS ===================================================================================
        /// <summary>
        /// Customized ToString() method
        /// </summary>
        /// <returns>Custom string value showing relevant data for current class</returns>
        public override string ToString()
        {
            return Id + " :: " + TvShowId;
        }
        #endregion
    }
}
