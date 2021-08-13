/// Written by: Yulia Danilova
/// Creation Date: 20th of November, 2020
/// Purpose: Deserialization model for the TvShowResume storage container
#region ========================================================================= USING =====================================================================================
using Leya.DataAccess.StorageAccess;
using Leya.DataAccess.Common.Attributes;
#endregion

namespace Leya.DataAccess.Common.Models.TvShows
{
    public sealed class TvShowResumeEntity : IStorageEntity
    {
        #region ================================================================ PROPERTIES =================================================================================
        [IgnoreOnInsert]
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
            return Id.ToString();
        }
        #endregion
    }
}
