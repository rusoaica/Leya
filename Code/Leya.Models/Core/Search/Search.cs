/// Written by: Yulia Danilova
/// Creation Date: 03rd of August, 2021
/// Purpose: Business model for media library searching
#region ========================================================================= USING =====================================================================================
using System.Linq;
using System.Threading.Tasks;
using Leya.Infrastructure.Enums;
using System.Collections.Generic;
using Leya.Models.Core.MediaLibrary;
using Leya.Models.Common.Broadcasting;
using Leya.Models.Common.Models.Media;
using Leya.Models.Common.Models.Common;
using Leya.Models.Common.Models.Movies;
using Leya.Models.Common.Models.TvShows;
using Leya.Models.Common.Models.Artists;
#endregion

namespace Leya.Models.Core.Search
{
    public class Search : NotifyPropertyChanged, ISearch
    {
        #region ============================================================== FIELD MEMBERS ================================================================================
        private readonly IMediaLibrary mediaLibrary;
        #endregion

        #region ================================================================ PROPERTIES =================================================================================
        private string searchMediaName;
        public string SearchMediaName
        {
            get { return searchMediaName; }
            set { searchMediaName = value; Notify(); }
        }

        private string searchMediaTag;
        public string SearchMediaTag
        {
            get { return searchMediaTag; }
            set { searchMediaTag = value; Notify(); }
        }

        private string searchMediaGenre;
        public string SearchMediaGenre
        {
            get { return searchMediaGenre; }
            set { searchMediaGenre = value; Notify(); }
        }

        private string searchMediaMember;
        public string SearchMediaMember
        {
            get { return searchMediaMember; }
            set { searchMediaMember = value; Notify(); }
        }

        private string searchMediaRole;
        public string SearchMediaRole
        {
            get { return searchMediaRole; }
            set { searchMediaRole = value; Notify(); }
        }

        private MediaTypeEntity selectedSearchMediaType;
        public MediaTypeEntity SelectedSearchMediaType
        {
            get { return selectedSearchMediaType; }
            set { selectedSearchMediaType = value; Notify(); }
        }
        #endregion

        #region ================================================================== CTOR =====================================================================================
        /// <summary>
        /// Overload C-tor
        /// </summary>
        /// <param name="mediaLibrary">The injected media library containing the elements to be searched</param>
        public Search(IMediaLibrary mediaLibrary)
        {
            this.mediaLibrary = mediaLibrary;
        }
        #endregion

        #region ================================================================= METHODS ===================================================================================
        /// <summary>
        /// Clears the advanced search terms
        /// </summary>
        public void ClearAdvancedSearchTerms()
        {
            SearchMediaName = string.Empty;
            SearchMediaTag = string.Empty;
            SearchMediaGenre = string.Empty;
            SearchMediaMember = string.Empty;
            SearchMediaRole = string.Empty;
        }

        /// <summary>
        /// Searches the media library
        /// </summary>
        /// <returns>An IEnumerable representing the result of searching the media library</returns>
        public async Task<IEnumerable<AdvancedSearchResultEntity>> SearchLibraryAsync()
        {
            IEnumerable<IMedia> temp = FilterByMediaType();
            await Task.Run(() =>
            {
                if (!string.IsNullOrEmpty(SearchMediaName))
                    temp = FilterByMediaName(temp);
                if (!string.IsNullOrEmpty(SearchMediaTag))
                    temp = FilterByTag(temp);
                if (!string.IsNullOrEmpty(SearchMediaGenre))
                    temp = FilterByGenre(temp);
                if (!string.IsNullOrEmpty(SearchMediaMember))
                    temp = FilterByMember(temp);
                if (!string.IsNullOrEmpty(SearchMediaRole))
                    temp = FilterByRole(temp);
            });
            return temp.Select(e => new AdvancedSearchResultEntity()
            {
                Id = e.Id,
                MediaTitle = e.Title,
                MediaType = e is EpisodeEntity ? NavigationLevel.Episode : e is SeasonEntity ? NavigationLevel.Season : 
                e is TvShowEntity ? NavigationLevel.TvShow : e is SongEntity ? NavigationLevel.Song : e is AlbumEntity ? NavigationLevel.Album : 
                e is ArtistEntity ? NavigationLevel.Artist : e is MovieEntity ? NavigationLevel.Movie : NavigationLevel.None,
            });
        }

        /// <summary>
        /// Filters the media library by the selected media type
        /// </summary>
        /// <returns>An IEnumerable of filtered media library items</returns>
        private IEnumerable<IMedia> FilterByMediaType()
        {
            if (SelectedSearchMediaType.MediaName == "MOVIE")
                foreach (IMedia media in mediaLibrary.Library.Movies)
                    yield return media;
            else if (SelectedSearchMediaType.MediaName == "TV SHOW")
                foreach (IMedia media in mediaLibrary.Library.TvShows)
                    yield return media;
            else if (SelectedSearchMediaType.MediaName == "SEASON")
                foreach (IMedia media in mediaLibrary.Library.TvShows.SelectMany(t => t.Seasons))
                    yield return media;
            else if (SelectedSearchMediaType.MediaName == "EPISODE")
                foreach (IMedia media in mediaLibrary.Library.TvShows.SelectMany(t => t.Seasons)
                                                                     .SelectMany(s => s.Episodes))
                    yield return media;
            else if (SelectedSearchMediaType.MediaName == "ARTIST")
                foreach (IMedia media in mediaLibrary.Library.Artists)
                    yield return media;
            else if (SelectedSearchMediaType.MediaName == "ALBUM")
                foreach (IMedia media in mediaLibrary.Library.Artists.SelectMany(a => a.Albums))
                    yield return media;
            else if (SelectedSearchMediaType.MediaName == "SONG")
                foreach (IMedia media in mediaLibrary.Library.Artists.SelectMany(a => a.Albums)
                                                                     .SelectMany(a => a.Songs))
                    yield return media;
            else
            {
                foreach (IMedia media in mediaLibrary.Library.Movies?.Cast<IMedia>()
                    .Concat(mediaLibrary.Library.TvShows?.Cast<IMedia>() ?? Enumerable.Empty<IMedia>())
                    .Concat(mediaLibrary.Library.TvShows?.SelectMany(t => t.Seasons)
                                                        .Cast<IMedia>() ?? Enumerable.Empty<IMedia>())
                    .Concat(mediaLibrary.Library.TvShows?.SelectMany(t => t.Seasons)
                                                        .SelectMany(s => s.Episodes)
                                                        .Cast<IMedia>() ?? Enumerable.Empty<IMedia>())
                    .Concat(mediaLibrary.Library.Artists?.Cast<IMedia>() ?? Enumerable.Empty<IMedia>())
                    .Concat(mediaLibrary.Library.Artists?.SelectMany(a => a.Albums)
                                                        .Cast<IMedia>() ?? Enumerable.Empty<IMedia>())
                    .Concat(mediaLibrary.Library.Artists?.SelectMany(a => a.Albums)
                                                        .SelectMany(a => a.Songs)
                                                        .Cast<IMedia>() ?? Enumerable.Empty<IMedia>()) ?? Enumerable.Empty<IMedia>())
                    yield return media;
            }
        }

        /// <summary>
        /// Filters a media library by the name of the media elements
        /// </summary>
        /// <param name="library">The library to filter</param>
        /// <returns>An IEnumerable of filtered media library items</returns>
        private IEnumerable<IMedia> FilterByMediaName(IEnumerable<IMedia> library)
        {
            string searchTerm = SearchMediaName.ToLower();
            foreach (IMedia media in library)
            {
                if (media is MovieEntity movie && (movie.Title.ToLower().Contains(searchTerm) || movie.NamedTitle.ToLower().Contains(searchTerm)))
                    yield return media;
                else if (media is TvShowEntity tvShow && (tvShow.Title.ToLower().Contains(searchTerm) || tvShow.NamedTitle.ToLower().Contains(searchTerm)))
                    yield return media;
                else if (media is SeasonEntity season && season.Title.ToLower().Contains(searchTerm))
                    yield return media;
                else if (media is EpisodeEntity episode && (episode.Title.ToLower().Contains(searchTerm) || episode.NamedTitle.ToLower().Contains(searchTerm)))
                    yield return media;
                else if (media is ArtistEntity artist && (artist.Title.ToLower().Contains(searchTerm) || artist.NamedTitle.ToLower().Contains(searchTerm)))
                    yield return media;
                else if (media is AlbumEntity album && (album.Title.ToLower().Contains(searchTerm) || album.NamedTitle.ToLower().Contains(searchTerm)))
                    yield return media;
                else if (media is SongEntity song && (song.Title.ToLower().Contains(searchTerm) || song.NamedTitle.ToLower().Contains(searchTerm)))
                    yield return media;
            }
        }

        /// <summary>
        /// Filters a media library by tags
        /// </summary>
        /// <param name="library">The library to filter</param>
        /// <returns>An IEnumerable of filtered media library items</returns>
        private IEnumerable<IMedia> FilterByTag(IEnumerable<IMedia> library)
        {
            foreach (IMedia media in library)
                if (media is MovieEntity movie && movie.Tags.Where(t => t.Tag.ToLower().Contains(SearchMediaTag.ToLower())).Count() > 0)
                    yield return media;
        }

        /// <summary>
        /// Filters a media library by genre
        /// </summary>
        /// <param name="library">The library to filter</param>
        /// <returns>An IEnumerable of filtered media library items</returns>
        private IEnumerable<IMedia> FilterByGenre(IEnumerable<IMedia> library)
        {
            string searchTerm = SearchMediaName.ToLower();
            foreach (IMedia media in library)
            {
                if (media is MovieEntity movie && movie.Genres.Where(g => g.Genre.ToLower().Contains(searchTerm)).Count() > 0)
                    yield return media;
                else if (media is TvShowEntity tvShow && tvShow.Genres.Where(g => g.Genre.ToLower().Contains(searchTerm)).Count() > 0)
                    yield return media;
                else if (media is EpisodeEntity episode && episode.Genres.Where(g => g.Genre.ToLower().Contains(searchTerm)).Count() > 0)
                    yield return media;
                else if (media is ArtistEntity artist && artist.Genres.Where(g => g.Genre.ToLower().Contains(searchTerm)).Count() > 0)
                    yield return media;
            }
        }

        /// <summary>
        /// Filters a media library by the name of members or actors
        /// </summary>
        /// <param name="library">The library to filter</param>
        /// <returns>An IEnumerable of filtered media library items</returns>
        private IEnumerable<IMedia> FilterByMember(IEnumerable<IMedia> library)
        {
            string searchTerm = SearchMediaMember.ToLower();
            foreach (IMedia media in library)
            {
                if (media is MovieEntity movie && movie.Actors.Where(a => a.Name.ToLower().Contains(searchTerm)).Count() > 0)
                    yield return media;
                else if (media is TvShowEntity tvShow && tvShow.Actors.Where(a => a.Name.ToLower().Contains(searchTerm)).Count() > 0)
                    yield return media;
                else if (media is EpisodeEntity episode && episode.Actors.Where(a => a.Name.ToLower().Contains(searchTerm)).Count() > 0)
                    yield return media;
                else if (media is ArtistEntity artist && artist.Members.Where(a => a.Name.ToLower().Contains(searchTerm)).Count() > 0)
                    yield return media;
            }
        }

        /// <summary>
        /// Filters a media library by the role of members or actors
        /// </summary>
        /// <param name="library">The library to filter</param>
        /// <returns>An IEnumerable of filtered media library items</returns>
        private IEnumerable<IMedia> FilterByRole(IEnumerable<IMedia> library)
        {
            string searchTerm = SearchMediaRole.ToLower();
            foreach (IMedia media in library)
            {
                if (media is MovieEntity movie && movie.Actors.Select(a => a.Role.ToLower().Contains(searchTerm)).Count() > 0)
                    yield return media;
                else if (media is TvShowEntity tvShow && tvShow.Actors.Select(a => a.Role.ToLower().Contains(searchTerm)).Count() > 0)
                    yield return media;
                else if (media is EpisodeEntity episode && episode.Actors.Select(a => a.Role.ToLower().Contains(searchTerm)).Count() > 0)
                    yield return media;
                else if (media is ArtistEntity artist && artist.Members.Select(a => a.Role.ToLower().Contains(searchTerm)).Count() > 0)
                    yield return media;
            }
        }
        #endregion
    }
}
