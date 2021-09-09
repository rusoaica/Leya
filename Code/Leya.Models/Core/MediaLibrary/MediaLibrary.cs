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
using System.Linq;
using Leya.Models.Common.Models.Common;
using System.Collections.Generic;
using Leya.Models.Common.Models.TvShows;
using System.IO;
using Leya.Models.Core.Navigation;
#endregion

namespace Leya.Models.Core.MediaLibrary
{
    public class MediaLibrary : NotifyPropertyChanged, IMediaLibrary
    {
        #region ============================================================== FIELD MEMBERS ================================================================================
        public event Action MediaTypesLoaded;
        public event Action LibraryLoaded;

        private readonly ILibrary library;
        private readonly IMediaCast mediaCast;
        private readonly IMediaState mediaState;
        private readonly IMediaStatistics mediaStatistics;
        private readonly IMediaLibraryNavigation mediaLibraryNavigation;
        #endregion

        #region ================================================================ PROPERTIES =================================================================================
        public IMediaCast MediaCast => mediaCast;
        public IMediaState MediaState => mediaState;
        public IMediaStatistics MediaStatistics => mediaStatistics;
        public IMediaLibraryNavigation Navigation => mediaLibraryNavigation;
        public MediaLibraryEntity Library { get; set; } = new MediaLibraryEntity();
        public List<SearchEntity> SourceSearch { get; set; } = new List<SearchEntity>();
        #endregion

        #region ================================================================== CTOR =====================================================================================
        /// <summary>
        /// Overload C-tor
        /// <param name="library">Injected library business model</param>
        /// <param name="mediaState">Injected media state business model</param>
        /// <param name="mediaCast">Injected media cast business model</param>
        /// <param name="mediaLibraryNavigation">Injected media library navigation business model</param>
        /// <param name="mediaStatistics">Injected media library statistics</param>
        /// </summary>
        public MediaLibrary(ILibrary library, IMediaState mediaState, IMediaCast mediaCast, IMediaLibraryNavigation mediaLibraryNavigation, IMediaStatistics mediaStatistics)
        {
            this.library = library;
            this.mediaCast = mediaCast;
            this.mediaState = mediaState;
            this.mediaStatistics = mediaStatistics;
            this.mediaLibraryNavigation = mediaLibraryNavigation;
        }
        #endregion

        #region ================================================================= METHODS ===================================================================================
        /// <summary>
        /// Gets all the media library from the storage medium
        /// </summary>
        public async Task GetMediaLibraryAsync()
        {
            await library.GetMediaLibraryAsync();
            Library.MediaTypes = library.MediaTypes;
            MediaTypesLoaded?.Invoke();
            Library.TvShows = library.TvShows;
            Library.Movies = library.Movies;
            Library.Artists = library.Artists;
            await GetMediaLibrarySearchListAsync();
            LibraryLoaded?.Invoke();
        }

        /// <summary>
        /// Gets a list of searchable items from the media library
        /// </summary>
        private async Task GetMediaLibrarySearchListAsync()
        {
            SourceSearch = new List<SearchEntity>();
            await Task.Run(() =>
            {
                SourceSearch.AddRange(from tv in Library.TvShows
                              from season in tv.Seasons
                              from episode in season.Episodes
                              select new SearchEntity()
                              {
                                  
                                  Text = episode.Title,
                                  Value = tv.Title,
                                  Hover = episode.Actors?.Select(e => e.Role).ToArray(),
                                  Actors = episode.Actors?.Select(a => a.Name).ToArray(),
                                  Genres = episode.Genres?.Select(g => g.Genre).ToArray(),
                                  MediaItemPath = Library.MediaTypes.Where(mt => mt.Id == tv.MediaTypeId)
                                                                        .Select(mt => mt.MediaTypeSources.Where(mts => mts.Id == tv.MediaTypeSourceId)
                                                                                                         .First())
                                                                        .First().MediaSourcePath +
                                                       Path.DirectorySeparatorChar + season.Title +
                                                       Path.DirectorySeparatorChar + episode.NamedTitle
                              });
                SourceSearch.AddRange(from movie in Library.Movies
                              select new SearchEntity()
                              {
                                  Text = movie.Title,
                                  Hover = movie.Actors?.Select(e => e.Role).ToArray(),
                                  Actors = movie.Actors?.Select(a => a.Name).ToArray(),
                                  Genres = movie.Genres?.Select(g => g.Genre).ToArray(),
                                  MediaItemPath = Library.MediaTypes.Where(mt => mt.Id == movie.MediaTypeId)
                                                                        .Select(mt => mt.MediaTypeSources.Where(mts => mts.Id == movie.MediaTypeSourceId)
                                                                                                         .First())
                                                                        .First().MediaSourcePath +
                                                       Path.DirectorySeparatorChar + movie.NamedTitle
                              });
                SourceSearch.AddRange(from artist in Library.Artists
                              from album in artist.Albums
                              from song in album.Songs
                              select new SearchEntity()
                              {
                                  Text = song.Title,
                                  Value = artist.Title,
                                  Hover = artist.Members?.Select(e => e.Role).ToArray(),
                                  Actors = artist.Members?.Select(a => a.Name).ToArray(),
                                  Genres = artist.Genres?.Select(g => g.Genre).ToArray(),
                                  MediaItemPath = Library.MediaTypes.Where(mt => mt.Id == artist.MediaTypeId)
                                                                        .Select(mt => mt.MediaTypeSources.Where(mts => mts.Id == artist.MediaTypeSourceId)
                                                                                                         .First())
                                                                        .First().MediaSourcePath +
                                                       Path.DirectorySeparatorChar + album.NamedTitle +
                                                       Path.DirectorySeparatorChar + song.NamedTitle
                              });
            });
        }

        /// <summary>
        /// Updates all media library in the storage medium
        /// </summary>
        public async Task UpdateMediaLibraryAsync()
        {
            await library.UpdateMediaLibraryAsync();
        }
        #endregion
    }
}
