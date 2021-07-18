/// Written by: Yulia Danilova
/// Creation Date: 11th of November, 2020
/// Purpose: Model for the list of media in the main library list
#region ========================================================================= USING =====================================================================================
using Leya.ViewModels.Common.MVVM;
using Leya.Models.Common.Models.Media;
#endregion

namespace Leya.ViewModels.Common.Models.Media
{
    public class MediaEntity : LightBaseModel, IMediaEntity
    {
        #region ================================================================ PROPERTIES =================================================================================
        public int Id { get; set; }
        /// <summary>
        /// Some media elements require storing multiple id's: 
        /// Id - first level id (tv show, artist); 
        /// SeasonOrAlbumId: second level id (season, album); 
        /// EpisodeOrSongIdId: third level id (episode, song); 
        /// </summary>
        public int SeasonOrAlbumId { get; set; }
        public int EpisodeOrSongId { get; set; }
        public int Year { get; set; }
        public decimal Rating { get; set; }
        public string CommonRating { get; set; }
        public string MediaName { get; set; }
        #endregion

        #region ============================================================ BINDING PROPERTIES ============================================================================= 
        private bool isWatched;
        public bool IsWatched
        {
            get { return isWatched; }
            set { isWatched = value; Notify(); }
        }

        private bool isFavorite;
        public bool IsFavorite 
        {
            get { return isFavorite; }
            set { isFavorite = value; Notify(); }
        }
        #endregion

        #region ================================================================= METHODS ===================================================================================
        /// <summary>
        /// Customized ToString() method
        /// </summary>
        /// <returns>Custom string value showing relevant data for current class</returns>
        public override string ToString()
        {
            return Id + " :: " + MediaName;
        }
        #endregion
    }
}
