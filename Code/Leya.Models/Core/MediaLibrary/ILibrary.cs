/// Written by: Yulia Danilova
/// Creation Date: 03rd of August, 2021
/// Purpose: Interface business model for libraries data repositores
#region ========================================================================= USING =====================================================================================
using System.Threading.Tasks;
using Leya.Models.Common.Models.Media;
using Leya.Models.Common.Models.Movies;
using Leya.Models.Common.Models.Artists;
using Leya.Models.Common.Models.TvShows;
#endregion

namespace Leya.Models.Core.MediaLibrary
{
    public interface ILibrary
    {
        #region ================================================================ PROPERTIES =================================================================================
        MovieEntity[] Movies { get; }
        ArtistEntity[] Artists { get; }
        TvShowEntity[] TvShows { get; }
        MediaTypeEntity[] MediaTypes { get; }
        #endregion

        #region ================================================================= METHODS ===================================================================================
        /// <summary>
        /// Gets all the media library from the storage medium
        /// </summary>
        Task GetMediaLibraryAsync();

        /// <summary>
        /// Updates all media library in the storage medium
        /// </summary>
        Task UpdateMediaLibraryAsync();
        #endregion
    }
}
