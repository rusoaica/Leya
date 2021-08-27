/// Written by: Yulia Danilova
/// Creation Date: 09th of December, 2020
/// Purpose: Data transfer object for the music artists
#region ========================================================================= USING =====================================================================================
using Leya.Models.Common.Infrastructure;
using System;
#endregion

namespace Leya.Models.Common.Models.Artists
{
    public class ArtistEntity
    {
        #region ================================================================ PROPERTIES =================================================================================
        public int Id { get; set; }
        public int MediaTypeId { get; set; }
        public int MediaTypeSourceId { get; set; }
        public string FormedIn { get; set; }
        public string Biography { get; set; }
        public string ArtistName { get; set; }
        public string VideoClipLink { get; set; }
        public string ArtistNamedName { get; set; }
        public string MusicBrainzArtistID { get; set; }
        public bool? IsListened { get; set; }
        public bool IsFavorite { get; set; }
        public bool IsDisbanded { get; set; }
        public AlbumEntity[] Albums { get; set; }
        public ArtistTypeEntity[] Type { get; set; }
        public ArtistGenreEntity[] Genres { get; set; }
        public BandMemberEntity[] Members { get; set; }
        public DateTime Formed { get; set; }
        public DateTime Created { get; set; }
        #endregion

        #region ================================================================= METHODS ===================================================================================
        /// <summary>
        /// Customized ToString() method
        /// </summary>
        /// <returns>Custom string value showing relevant data for current class</returns>
        public override string ToString()
        {
            return Id + " :: " + ArtistName;
        }

        /// <summary>
        /// Maps between this entity and the coresponding persistance entity
        /// </summary>
        /// <returns>A data storage entity representation of this entity</returns>
        public DataAccess.Common.Models.Artists.ArtistEntity ToStorageEntity()
        {
            return Services.AutoMapper.Map<DataAccess.Common.Models.Artists.ArtistEntity>(this);
        }
        #endregion
    }
}
