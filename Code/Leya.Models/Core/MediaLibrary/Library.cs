/// Written by: Yulia Danilova
/// Creation Date: 03rd of August, 2021
/// Purpose: Business model for libraries data repositores
#region ========================================================================= USING =====================================================================================
using System.Linq;
using System.Threading.Tasks;
using Leya.Models.Core.Movies;
using Leya.Models.Core.Artists;
using Leya.Models.Core.TvShows;
using Leya.Models.Common.Models.Media;
using Leya.Models.Common.Models.Movies;
using Leya.Models.Common.Models.Artists;
using Leya.Models.Common.Models.TvShows;
#endregion

namespace Leya.Models.Core.MediaLibrary
{
    public class Library : ILibrary
    {
        #region ============================================================== FIELD MEMBERS ================================================================================
        private readonly IMovie movies;
        private readonly IArtist artists;
        private readonly ITvShow tvShows;
        private readonly IMediaType mediaTypes;
        #endregion

        #region ================================================================ PROPERTIES =================================================================================
        public MovieEntity[] Movies => movies.Movies;
        public ArtistEntity[] Artists => artists.Artists;
        public TvShowEntity[] TvShows => tvShows.TvShows;
        public MediaTypeEntity[] MediaTypes => mediaTypes.MediaTypes;
        #endregion

        #region ================================================================== CTOR =====================================================================================
        /// <summary>
        /// Overload C-tor
        /// <param name="mediaTypes">Injected media types business model</param>
        /// <param name="tvShows">Injected tv show business model</param>
        /// <param name="movies">Injected movie business model</param>
        /// <param name="artists">Injected artist business model</param>
        /// </summary>
        public Library(IMediaType mediaTypes, ITvShow tvShows, IMovie movies, IArtist artists)
        {
            this.movies = movies;
            this.tvShows = tvShows;
            this.artists = artists;
            this.mediaTypes = mediaTypes;
        }
        #endregion

        #region ================================================================= METHODS ===================================================================================
        /// <summary>
        /// Gets all the media library from the storage medium
        /// </summary>
        public async Task GetMediaLibraryAsync()
        {
            // get the media types and their sources from the storage medium
            await mediaTypes.GetMediaTypesAsync();
            // load the rest of the media library 
            await tvShows.GetAllAsync();
            await movies.GetAllAsync();
            await artists.GetAllAsync();
        }

        /// <summary>
        /// Updates all media library in the storage medium
        /// </summary>
        public async Task UpdateMediaLibraryAsync()
        {
            // delete all the media library before re-inserting it
            await tvShows.DeleteAllAsync();
            await movies.DeleteAllAsync();
            await artists.DeleteAllAsync();
            // insert all media items that are actual media
            foreach (MediaTypeEntity mediaType in mediaTypes.MediaTypes.Where(mt => mt.IsMedia))
            {
                foreach (MediaTypeSourceEntity mediaTypeSource in mediaType.MediaTypeSources)
                {
                    if (mediaType.MediaType == "TV SHOW")
                        await tvShows.SaveAsync(mediaTypeSource, mediaType.Id);
                    else if (mediaType.MediaType == "MOVIE")
                        await movies.SaveAsync(mediaTypeSource, mediaType.Id);
                    else if (mediaType.MediaType == "MUSIC")
                        await artists.SaveAsync(mediaTypeSource, mediaType.Id);
                }
            }
        }
        #endregion
    }
}
