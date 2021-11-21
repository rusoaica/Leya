/// Written by: Yulia Danilova
/// Creation Date: 18th of November, 2020
/// Purpose: Business model for media library
#region ========================================================================= USING =====================================================================================
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Leya.Models.Core.Navigation;
using Leya.Models.Common.Broadcasting;
using Leya.Models.Common.Models.Media;
using Leya.Models.Common.Models.Common;
#endregion

namespace Leya.Models.Core.MediaLibrary
{
    public class MediaLibrary : NotifyPropertyChanged, IMediaLibrary
    {
        #region ============================================================== FIELD MEMBERS ================================================================================
        public event Action MediaTypesLoaded;
        public event Action LibraryLoaded;

        private readonly ILibrary library;
        #endregion

        #region ================================================================ PROPERTIES =================================================================================
        public IMediaCast MediaCast { get; }
        public IMediaState MediaState { get; }
        public IMediaStatistics MediaStatistics { get; }
        public IMediaLibraryNavigation Navigation { get; }
        public MediaLibraryEntity Library { get; set; } = new MediaLibraryEntity();
        public List<FilterEntity> SourceQuickSearch { get; set; } = new List<FilterEntity>();
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
            MediaCast = mediaCast;
            MediaState = mediaState;
            MediaStatistics = mediaStatistics;
            Navigation = mediaLibraryNavigation;
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
            SourceQuickSearch = new List<FilterEntity>();
            await Task.Run(() =>
            {
                SourceQuickSearch.AddRange(from tv in Library.TvShows
                              from season in tv.Seasons
                              from episode in season.Episodes
                              select new FilterEntity()
                              {
                                  ChildTitle = episode.Title,
                                  ParentTitle = tv.Title,
                                  Roles = episode.Actors?.Select(e => e.Role).ToArray(),
                                  Members = episode.Actors?.Select(a => a.Name).ToArray(),
                                  Genres = episode.Genres?.Select(g => g.Genre).ToArray(),
                                  MediaItemPath = Library.MediaTypes.Where(mt => mt.Id == tv.MediaTypeId)
                                                                    .Select(mt => mt.MediaTypeSources.First(mts => mts.Id == tv.MediaTypeSourceId))
                                                                    .First().MediaSourcePath +
                                                                     Path.DirectorySeparatorChar + season.Title +
                                                                     Path.DirectorySeparatorChar + episode.NamedTitle
                              });
                SourceQuickSearch.AddRange(from movie in Library.Movies
                              select new FilterEntity()
                              {
                                  ChildTitle = movie.Title,
                                  Roles = movie.Actors?.Select(e => e.Role).ToArray(),
                                  Members = movie.Actors?.Select(a => a.Name).ToArray(),
                                  Genres = movie.Genres?.Select(g => g.Genre).ToArray(),
                                  MediaItemPath = Library.MediaTypes.Where(mt => mt.Id == movie.MediaTypeId)
                                                                    .Select(mt => mt.MediaTypeSources.First(mts => mts.Id == movie.MediaTypeSourceId))
                                                                    .First().MediaSourcePath +
                                                                     Path.DirectorySeparatorChar + movie.NamedTitle
                              });
                SourceQuickSearch.AddRange(from artist in Library.Artists
                              from album in artist.Albums
                              from song in album.Songs
                              select new FilterEntity()
                              {
                                  ChildTitle = song.Title,
                                  ParentTitle = artist.Title,
                                  Roles = artist.Members?.Select(e => e.Role).ToArray(),
                                  Members = artist.Members?.Select(a => a.Name).ToArray(),
                                  Genres = artist.Genres?.Select(g => g.Genre).ToArray(),
                                  MediaItemPath = Library.MediaTypes.Where(mt => mt.Id == artist.MediaTypeId)
                                                                    .Select(mt => mt.MediaTypeSources.First(mts => mts.Id == artist.MediaTypeSourceId))
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
