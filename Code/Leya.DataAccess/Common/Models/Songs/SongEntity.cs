/// Written by: Yulia Danilova
/// Creation Date: 10th of December, 2020
/// Purpose: Deserialization model for the Songs storage container
#region ========================================================================= USING =====================================================================================
using Leya.DataAccess.StorageAccess;
using Leya.DataAccess.Common.Attributes;
#endregion

namespace Leya.DataAccess.Common.Models.Songs
{
    public sealed class SongEntity : IStorageEntity
    {
        #region ================================================================ PROPERTIES =================================================================================
        [IgnoreOnInsert]
        public int Id { get; set; }
        public int Length { get; set; }
        public int AlbumId { get; set; }
        public int ArtistId { get; set; }
        public int AlbumOrder { get; set; }
        public string Title { get; set; }
        public string NamedTitle { get; set; }
        public decimal Rating { get; set; }
        public bool IsListened { get; set; }
        public bool IsFavorite { get; set; }
        #endregion

        #region ================================================================= METHODS ===================================================================================
        /// <summary>
        /// Customized ToString() method
        /// </summary>
        /// <returns>Custom string value showing relevant data for current class</returns>
        public override string ToString()
        {
            return Id + " :: " + Title;
        }
        #endregion
    }
}
