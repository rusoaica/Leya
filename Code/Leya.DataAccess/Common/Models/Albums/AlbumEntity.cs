/// Written by: Yulia Danilova
/// Creation Date: 09th of December, 2020
/// Purpose: Deserialization model for the Albums storage container
#region ========================================================================= USING =====================================================================================
using Leya.DataAccess.StorageAccess;
using Leya.DataAccess.Common.Attributes;
#endregion

namespace Leya.DataAccess.Common.Models.Albums
{
    public sealed class AlbumEntity : IStorageEntity
    {
        #region ================================================================ PROPERTIES =================================================================================
        [IgnoreOnInsert]
        public int Id { get; set; }
        public int Year { get; set; }
        public int ArtistId { get; set; }
        public float Rating { get; set; }
        public string Title { get; set; }
        public string NamedTitle { get; set; }
        public string Description { get; set; }
        public string IsListened { get; set; }
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
