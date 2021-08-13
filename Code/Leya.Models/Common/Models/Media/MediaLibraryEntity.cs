/// Written by: Yulia Danilova
/// Creation Date: 22nd of November, 2020
/// Purpose: Data transfer object for the whole media library
#region ========================================================================= USING =====================================================================================
using Leya.Models.Common.Models.Movies;
using Leya.Models.Common.Models.Artists;
using Leya.Models.Common.Models.TvShows;
#endregion

namespace Leya.Models.Common.Models.Media
{
    public class MediaLibraryEntity
    {
        #region ================================================================ PROPERTIES =================================================================================
        public TvShowEntity[] TvShows { get; set; }
        public MovieEntity[] Movies { get; set; }
        public ArtistEntity[] Artists { get; set; }
        public MediaTypeEntity[] MediaTypes { get; set; }
        #endregion
    }
}
