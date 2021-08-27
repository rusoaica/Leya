/// Written by: Yulia Danilova
/// Creation Date: 11th of November, 2020
/// Purpose: Interface for the model used for the list of media in the main library list
#region ========================================================================= USING =====================================================================================
using System.Runtime.CompilerServices;
#endregion

namespace Leya.Models.Common.Models.Media
{
    public interface IMediaEntity
    {
        #region ================================================================ PROPERTIES =================================================================================
        int Id { get; set; }
        int Year { get; set; }
        int SeasonOrAlbumId { get; set; }
        int EpisodeOrSongId { get; set; }
        decimal Rating { get; set; }
        string MediaName { get; set; }
        string CommonRating { get; set; }
        bool? IsWatched { get; set; }
        bool IsFavorite { get; set; }
        #endregion

        #region ================================================================= METHODS ===================================================================================
        void Notify([CallerMemberName] string propertyName = null);
        #endregion
    }
}
