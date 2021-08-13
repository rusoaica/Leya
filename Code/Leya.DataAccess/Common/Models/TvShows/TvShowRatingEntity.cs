/// Written by: Yulia Danilova
/// Creation Date: 20th of November, 2020
/// Purpose: Deserialization model for the TvShowRatings storage container
#region ========================================================================= USING =====================================================================================
using Leya.DataAccess.StorageAccess;
using Leya.DataAccess.Common.Attributes;
#endregion

namespace Leya.DataAccess.Common.Models.TvShows
{
    public sealed class TvShowRatingEntity : IStorageEntity
    {
        #region ================================================================ PROPERTIES =================================================================================
        [IgnoreOnInsert]
        public int Id { get; set; }
        public int Max { get; set; }
        public int Votes { get; set; }
        public int TvShowId { get; set; }
        public string Name { get; set; }
        public float Value { get; set; }
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
