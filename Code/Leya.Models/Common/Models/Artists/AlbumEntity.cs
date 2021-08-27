/// Written by: Yulia Danilova
/// Creation Date: 09th of December, 2020
/// Purpose: Data transfer object for the music artist's albums 

namespace Leya.Models.Common.Models.Artists
{
    public class AlbumEntity
    {
        #region ================================================================ PROPERTIES =================================================================================
        public int Id { get; set; }
        public int Year { get; set; }
        public int ArtistId { get; set; }
        public string Title { get; set; }
        public string NamedTitle { get; set; }
        public string Description { get; set; }
        public float Rating { get; set; }
        public bool? IsListened { get; set; }
        public bool IsFavorite { get; set; }
        public SongEntity[] Songs { get; set; }
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
