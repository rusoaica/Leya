/// Written by: Yulia Danilova
/// Creation Date: 19th of May, 2021
/// Purpose: Deserialization model for the ArtistGenres storage container
#region ========================================================================= USING =====================================================================================
using Leya.DataAccess.StorageAccess;
using Leya.DataAccess.Common.Attributes;
#endregion

namespace Leya.DataAccess.Common.Models.Artists
{
    public sealed class ArtistGenreEntity : IStorageEntity
    {
        #region ================================================================ PROPERTIES =================================================================================
        [IgnoreOnInsert]
        public int Id { get; set; }
        public int ArtistId { get; set; }
        public string Genre { get; set; }
        #endregion
    }
}
