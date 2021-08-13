/// Written by: Yulia Danilova
/// Creation Date: 18th of November, 2020
/// Purpose: Business model for media library
#region ========================================================================= USING =====================================================================================
using System;
using System.Threading.Tasks;
using Leya.Models.Core.Movies;
using Leya.Models.Core.Artists;
using Leya.Models.Core.TvShows;
using Leya.Models.Common.Broadcasting;
using Leya.Models.Common.Models.Media;
#endregion

namespace Leya.Models.Core.MediaLibrary
{
    public class MediaLibrary : NotifyPropertyChanged, IMediaLibrary
    {
        #region ============================================================== FIELD MEMBERS ================================================================================
        public event Action MediaTypesLoaded;
        public event Action LibraryLoaded;

        private readonly IMovie movies;
        private readonly IArtist artists;
        private readonly ITvShow tvShows;
        private readonly IMediaType mediaTypes;
        private readonly IMediaState mediaState;
        #endregion

        #region ================================================================ PROPERTIES =================================================================================
        public IMediaState MediaState => mediaState;
        public MediaLibraryEntity Library { get; set; } = new MediaLibraryEntity();
        #endregion

        #region ================================================================== CTOR =====================================================================================
        /// <summary>
        /// Overload C-tor
        /// <param name="mediaTypes">Injected media types business model</param>
        /// <param name="tvShows">Injected tv show business model</param>
        /// <param name="movies">Injected movie business model</param>
        /// <param name="artists">Injected artist business model</param>
        /// <param name="mediaState">Injected media state business model</param>
        /// </summary>
        public MediaLibrary(IMediaType mediaTypes, ITvShow tvShows, IMovie movies, IArtist artists, IMediaState mediaState)
        {
            this.movies = movies;
            this.tvShows = tvShows;
            this.artists = artists;
            this.mediaState = mediaState;
            this.mediaTypes = mediaTypes;
        }
        #endregion

        #region ================================================================= METHODS ===================================================================================
        /// <summary>
        /// Gets all the media library from the storage medium
        /// </summary>
        public async Task GetMediaLibraryAsync()
        {
            // get the media types and raise an event when finished, so that an eventual UI can be 
            // able to display the main menu items before waiting for the whole library to load
            await mediaTypes.GetMediaTypesAsync();
            Library.MediaTypes = mediaTypes.MediaTypes;
            MediaTypesLoaded?.Invoke();
            // load the rest of the media library and notify an eventual UI when done
            await tvShows.GetTvShowsAsync();
            await movies.GetMoviesAsync();
            await artists.GetArtistsAsync();
            Library.TvShows = tvShows.TvShows;
            Library.Movies = movies.Movies;
            Library.Artists = artists.Artists;
            LibraryLoaded?.Invoke();
        }
        #endregion
    }
}
