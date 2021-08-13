/// Written by: Yulia Danilova
/// Creation Date: 09th of December, 2020
/// Purpose: Data transfer object for the songs 

namespace Leya.Models.Common.Models.Artists
{
    public class SongEntity
    {
        #region ================================================================ PROPERTIES =================================================================================
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
