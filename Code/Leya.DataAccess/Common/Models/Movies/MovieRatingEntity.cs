/// Written by: Yulia Danilova
/// Creation Date: 24th of November, 2020
/// Purpose: Deserialization model for the MovieRatings storage container
#region ========================================================================= USING =====================================================================================
using Leya.DataAccess.StorageAccess;
using Leya.DataAccess.Common.Attributes;
#endregion

namespace Leya.DataAccess.Common.Models.Movies
{
    public sealed class MovieRatingEntity : IStorageEntity
    {
        #region ================================================================ PROPERTIES =================================================================================
        [IgnoreOnInsert]
        public int Id { get; set; }
        public int Max { get; set; }
        public int Votes { get; set; }
        public int MovieId { get; set; }
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
