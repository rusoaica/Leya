/// Written by: Yulia Danilova
/// Creation Date: 09th of December, 2020
/// Purpose: Deserialization model for the Artists storage container
#region ========================================================================= USING =====================================================================================
using System;
using Leya.DataAccess.StorageAccess;
using Leya.DataAccess.Common.Attributes;
using Leya.DataAccess.Common.Models.Albums;
#endregion

namespace Leya.DataAccess.Common.Models.Artists
{
    public sealed class ArtistEntity : IStorageEntity
    {
        #region ================================================================ PROPERTIES =================================================================================
        [IgnoreOnInsert]
        public int Id { get; set; }
        public int MediaTypeId { get; set; }
        public int MediaTypeSourceId { get; set; }
        public string FormedIn { get; set; }
        public string Biography { get; set; }
        public string Title { get; set; }
        public string NamedTitle { get; set; }
        public string VideoClipLink { get; set; }
        public string MusicBrainzArtistID { get; set; }
        [IgnoreOnInsert]
        public ArtistTypeEntity[] Type { get; set; }
        [IgnoreOnInsert]
        public ArtistGenreEntity[] Genres { get; set; }
        public DateTime Formed { get; set; }
        public string IsListened { get; set; }
        public bool IsFavorite { get; set; }
        public bool IsDisbanded { get; set; }
        [IgnoreOnInsert]
        public BandMemberEntity[] Members { get; set; }
        [IgnoreOnInsert]
        public AlbumEntity[] Albums { get; set; }
        [IgnoreOnInsert]
        public DateTime Created { get; set; }
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
