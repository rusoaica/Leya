/// Written by: Yulia Danilova
/// Creation Date: 23rd of May, 2021
/// Purpose: Deserialization model for the EpisodeGenre storage container
#region ========================================================================= USING =====================================================================================
using Leya.DataAccess.StorageAccess;
using Leya.DataAccess.Common.Attributes;
#endregion

namespace Leya.DataAccess.Common.Models.Episodes
{
    public sealed class EpisodeGenreEntity : IStorageEntity
    {
        #region ================================================================ PROPERTIES =================================================================================
        [IgnoreOnInsert]
        public int Id { get; set; }
        public int EpisodeId { get; set; }
        public string Genre { get; set; }
        #endregion

        #region ================================================================= METHODS ===================================================================================
        /// <summary>
        /// Customized ToString() method
        /// </summary>
        /// <returns>Custom string value showing relevant data for current class</returns>
        public override string ToString()
        {
            return Id + " :: " + Genre;
        }
        #endregion
    }
}
