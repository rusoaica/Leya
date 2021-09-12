/// Written by: Yulia Danilova
/// Creation Date: 13th of July, 2021
/// Purpose: Business model for media library navigation
#region ========================================================================= USING =====================================================================================
using System;
using System.Linq;
using System.Threading.Tasks;
using Leya.Models.Core.Player;
using Leya.Infrastructure.Enums;
using System.Collections.Generic;
using Leya.Models.Core.MediaLibrary;
using Leya.Models.Common.Broadcasting;
using Leya.Models.Common.Models.Media;
using Leya.Models.Common.Models.Movies;
using Leya.Models.Common.Models.Common;
using Leya.Models.Common.Models.TvShows;
using Leya.Models.Common.Models.Artists;
using Leya.Infrastructure.Miscellaneous;
#endregion

namespace Leya.Models.Core.Navigation
{
    public class MediaLibraryNavigation : NotifyPropertyChanged, IMediaLibraryNavigation
    {
        #region ============================================================== FIELD MEMBERS ================================================================================
        public event Action Navigated;

        private string currentMediaName;
        private readonly IMediaPlayer mediaPlayer;
        #endregion

        #region ================================================================ PROPERTIES =================================================================================
        private int numberOfSeasons;
        public int NumberOfSeasons
        {
            get { return numberOfSeasons; }
            set { numberOfSeasons = value; Notify(); }
        }

        private int numberOfEpisodes;
        public int NumberOfEpisodes
        {
            get { return numberOfEpisodes; }
            set { numberOfEpisodes = value; Notify(); }
        }

        private int numberOfUnwatchedEpisodes;
        public int NumberOfUnwatchedEpisodes
        {
            get { return numberOfUnwatchedEpisodes; }
            set { numberOfUnwatchedEpisodes = value; Notify(); }
        }

        private string poster;
        public string Poster
        {
            get { return poster; }
            set { poster = value; Notify(); IsMediaCoverVisible = value != null; }
        }

        private string fanart;
        public string Fanart
        {
            get { return fanart; }
            set { fanart = value; Notify(); }
        }

        private string banner;
        public string Banner
        {
            get { return banner; }
            set { banner = value; Notify(); }
        }

        private string mpaa;
        public string MPAA
        {
            get { return mpaa; }
            set { mpaa = value; Notify(); }
        }

        private string tagLine;
        public string TagLine
        {
            get { return tagLine; }
            set { tagLine = value; Notify(); }
        }

        private string synopsis;
        public string Synopsis
        {
            get { return synopsis; }
            set { synopsis = value; Notify(); }
        }

        private string localPath;
        public string LocalPath
        {
            get { return localPath; }
            set { localPath = value; Notify(); }
        }

        private string currentStatus = "Ended";
        public string CurrentStatus
        {
            get { return currentStatus; }
            set { currentStatus = value; Notify(); }
        }

        private string genre;
        public string Genre
        {
            get { return genre; }
            set { genre = value; Notify(); }
        }

        private string writers;
        public string Writers
        {
            get { return writers; }
            set { writers = value; Notify(); }
        }

        private string director;
        public string Director
        {
            get { return director; }
            set { director = value; Notify(); }
        }

        private bool isTvShow;
        public bool IsTvShow
        {
            get { return isTvShow; }
            set { isTvShow = value; Notify(); }
        }

        private bool isFanartVisible;
        public bool IsFanartVisible
        {
            get { return isFanartVisible; }
            set { isFanartVisible = value; Notify(); }
        }

        private bool areWritersVisible;
        public bool AreWritersVisible
        {
            get { return areWritersVisible; }
            set { areWritersVisible = value; Notify(); }
        }

        private bool isDirectorVisible;
        public bool IsDirectorVisible
        {
            get { return isDirectorVisible; }
            set { isDirectorVisible = value; Notify(); }
        }

        private bool areNumberOfSeasonsVisible;
        public bool AreNumberOfSeasonsVisible
        {
            get { return areNumberOfSeasonsVisible; }
            set { areNumberOfSeasonsVisible = value; Notify(); }
        }

        private bool isMediaCoverVisible = false;
        public bool IsMediaCoverVisible
        {
            get { return isMediaCoverVisible; }
            set { isMediaCoverVisible = value; Notify(); }
        }

        private bool isBackNavigationPossible = false;
        public bool IsBackNavigationPossible
        {
            get { return isBackNavigationPossible; }
            set { isBackNavigationPossible = value; Notify(); }
        }

        private bool isInterfaceOptionVisible = false;
        public bool IsInterfaceOptionVisible
        {
            get { return isInterfaceOptionVisible; }
            set { isInterfaceOptionVisible = value; Notify(); }
        }

        private bool isHelpButtonVisible;
        public bool IsHelpButtonVisible
        {
            get { return isHelpButtonVisible; }
            set { isHelpButtonVisible = value; Notify(); }
        }

        private bool isPlayerOptionVisible = false;
        public bool IsPlayerOptionVisible
        {
            get { return isPlayerOptionVisible; }
            set { isPlayerOptionVisible = value; Notify(); }
        }

        private bool isSystemOptionVisible = false;
        public bool IsSystemOptionVisible
        {
            get { return isSystemOptionVisible; }
            set { isSystemOptionVisible = value; Notify(); }
        }

        private bool isMediaTypesOptionVisible;
        public bool IsMediaTypesOptionVisible
        {
            get { return isMediaTypesOptionVisible; }
            set { isMediaTypesOptionVisible = value; Notify(); }
        }

        private bool isSearchContainerVisible;
        public bool IsSearchContainerVisible
        {
            get { return isSearchContainerVisible; }
            set { isSearchContainerVisible = value; Notify(); }
        }

        private bool isMediaContainerVisible;
        public bool IsMediaContainerVisible
        {
            get { return isMediaContainerVisible; }
            set { isMediaContainerVisible = value; Notify(); }
        }

        private bool areMediaTypeSourcesOptionVisible;
        public bool AreMediaTypeSourcesOptionVisible
        {
            get { return areMediaTypeSourcesOptionVisible; }
            set { areMediaTypeSourcesOptionVisible = value; Notify(); }
        }

        private bool isOptionsContainerVisible;
        public bool IsOptionsContainerVisible
        {
            get { return isOptionsContainerVisible; }
            set { isOptionsContainerVisible = value; Notify(); }
        }

        private TimeSpan runtime = TimeSpan.FromSeconds(0);
        public TimeSpan Runtime
        {
            get { return runtime; }
            set { runtime = value; Notify(); }
        }

        private DateTime added = DateTime.Now;
        public DateTime Added
        {
            get { return added; }
            set { added = value; Notify(); }
        }

        private DateTime premiered = DateTime.Now;

        public DateTime Premiered
        {
            get { return premiered; }
            set { premiered = value; Notify(); }
        }

        private NavigationLevel currentNavigationLevel;
        public NavigationLevel CurrentNavigationLevel
        {
            get { return currentNavigationLevel; }
            set { currentNavigationLevel = value; Notify(); }
        }
        #endregion

        #region ================================================================== CTOR =====================================================================================
        /// <summary>
        /// Overload C-tor
        /// </summary>
        /// <param name="mediaPlayer">The injected media player</param>
        public MediaLibraryNavigation(IMediaPlayer mediaPlayer)
        {
            this.mediaPlayer = mediaPlayer;
        }
        #endregion

        #region ================================================================= METHODS ===================================================================================
        /// <summary>
        /// Navigates one level up in media library
        /// </summary>
        /// <param name="mediaLibrary">The media library to be navigated</param>
        /// <param name="mediaId">The id of the element from which to navigate one level down</param>
        public void NavigateUp(IMediaLibrary mediaLibrary, int mediaId)
        {
            switch (CurrentNavigationLevel)
            {
                case NavigationLevel.TvShow:
                case NavigationLevel.Movie:
                case NavigationLevel.Artist:
                {
                    // for tv shows, movies and artists, there is no higher level to navigate to
                    CurrentNavigationLevel = NavigationLevel.None;
                    ExitMediaDisplay();
                    break;
                }
                case NavigationLevel.Season:
                {
                    // from a season, navigation is returned to the list of tv shows, display them
                    CurrentNavigationLevel = NavigationLevel.TvShow;
                    SetTvShowOrArtistDisplayedElements();
                    // runtime is the total runtime length of all episodes of all tv shows
                    Runtime = TimeSpan.FromSeconds(mediaLibrary.Library.TvShows.SelectMany(t => t.Seasons)
                                                                               .SelectMany(s => s.Episodes)
                                                                               .Sum(e => e.Runtime));
                    break;
                }
                case NavigationLevel.Episode:
                {
                    // from an episode, navigation is returned to the list of seasons of the tv show the episode belongs to, display them
                    CurrentNavigationLevel = NavigationLevel.Season;
                    SetSeasonOrAlbumDisplayedElements();
                    // runtime is the total runtime length of all episodes of the current tv show
                    Runtime = TimeSpan.FromSeconds(mediaLibrary.Library.TvShows.SelectMany(t => t.Seasons)
                                                                               .SelectMany(s => s.Episodes)
                                                                               .Where(e => e.TvShowId == mediaId)
                                                                               .Sum(e => e.Runtime));
                    break;
                }
                case NavigationLevel.Album:
                {
                    // from an album, navigation is returned to the list of artists, display them
                    CurrentNavigationLevel = NavigationLevel.Artist;
                    SetTvShowOrArtistDisplayedElements();
                    // runtime is the total runtime length of all songs of all artists
                    Runtime = TimeSpan.FromSeconds(mediaLibrary.Library.Artists.SelectMany(a => a.Albums)
                                                                               .SelectMany(a => a.Songs)
                                                                               .Sum(s => s.Length));
                    break;
                }
                case NavigationLevel.Song:
                {
                    // from a song, navigation is returned to the list of albums of the artist the song belongs to, display them
                    CurrentNavigationLevel = NavigationLevel.Album;
                    SetSeasonOrAlbumDisplayedElements();
                    // runtime is the total runtime length of all songs of the current artist
                    Runtime = TimeSpan.FromSeconds(mediaLibrary.Library.Artists.SelectMany(a => a.Albums)
                                                                               .SelectMany(a => a.Songs)
                                                                               .Where(s => s.ArtistId == mediaId)
                                                                               .Sum(s => s.Length));
                    break;
                }
            }
            Navigated?.Invoke();
        }

        /// <summary>
        /// Navigates one level down in media library
        /// </summary>
        /// <param name="mediaLibrary">The media library to be navigated</param>
        /// <param name="media">The element from which to navigate one level down</param>
        public async Task NavigateDownAsync(IMediaLibrary mediaLibrary, IMediaEntity media)
        {
            // when current navigation level is episode or movie or song, this is not further navigation, so play the media item
            if (CurrentNavigationLevel == NavigationLevel.Episode)
                await mediaPlayer.PlayEpisodeAsync(mediaLibrary, media);
            else if (CurrentNavigationLevel == NavigationLevel.Movie)
                await mediaPlayer.PlayMovieAsync(mediaLibrary, media);
            else if (CurrentNavigationLevel == NavigationLevel.Song)
                await mediaPlayer.PlaySongAsync(mediaLibrary, media);
            else
            {
                // advance current level of navigation one level deeper
                IsBackNavigationPossible = true;
                switch (CurrentNavigationLevel)
                {
                    case NavigationLevel.TvShow:
                    {
                        // from a tv show, navigation is advanced to the list of its seasons
                        CurrentNavigationLevel = NavigationLevel.Season;
                        SetEpisodeDisplayedElements();
                        break;
                    }
                    case NavigationLevel.Season:
                    {
                        // from a season, navigation is advanced to the list of its episodes
                        CurrentNavigationLevel = NavigationLevel.Episode;
                        SetEpisodeDisplayedElements();
                        break;
                    }
                    case NavigationLevel.Artist:
                    {
                        // from an artist, navigation is advanced to the list of its albums
                        CurrentNavigationLevel = NavigationLevel.Album;
                        SetSeasonOrAlbumDisplayedElements();
                        break;
                    }
                    case NavigationLevel.Album:
                    {
                        // from an album, navigation is advanced to the list of its songs
                        CurrentNavigationLevel = NavigationLevel.Song;
                        SetSongDisplayedElements();
                        break;
                    }
                }
            }
            Navigated?.Invoke();
        }

        /// <summary>
        /// Initiates the media library navigation and displays the category identified by <paramref name="menu"/> item
        /// </summary>
        /// <param name="menu">The menu item for which to display the media view</param>
        public void InitiateMainMenuNavigation(MediaTypeEntity menu)
        {
            currentMediaName = menu.MediaName;
            if (menu.MediaName == "SYSTEM")
                CurrentNavigationLevel = NavigationLevel.System;
            else if (menu.MediaName == "SEARCH")
                CurrentNavigationLevel = NavigationLevel.Search;
            else if (menu.MediaName == "FAVORITES")
                CurrentNavigationLevel = NavigationLevel.Favorite;
            else
            {
                if (menu.MediaType == "TV SHOW")
                    CurrentNavigationLevel = NavigationLevel.TvShow;
                else if (menu.MediaType == "MOVIE")
                    CurrentNavigationLevel = NavigationLevel.Movie;
                else if (menu.MediaType == "MUSIC")
                    CurrentNavigationLevel = NavigationLevel.Artist;
            }
            Navigated?.Invoke();
        }

        /// <summary>
        /// Exits the interface options view and returns to the options list
        /// </summary>
        public void ExitInterfaceOptions()
        {
            IsInterfaceOptionVisible = false;
            IsHelpButtonVisible = false;
        }

        /// <summary>
        /// Exits the system options view and returns to the options list
        /// </summary>
        public void ExitSystemOptions()
        {
            IsSystemOptionVisible = false;
            IsHelpButtonVisible = false;
        }

        /// <summary>
        /// Exits the player options view and returns to the options list
        /// </summary>
        public void ExitPlayerOptions()
        {
            IsPlayerOptionVisible = false;
            IsHelpButtonVisible = false;
        }

        /// <summary>
        /// Exits the media types options list and returns to the options list
        /// </summary>
        public void ExitMediaTypesOptions()
        {
            IsMediaTypesOptionVisible = false;
            IsHelpButtonVisible = false;
        }

        /// <summary>
        /// Gets the additional info for the tv show identified by <paramref name="tvShowId"/>
        /// </summary>
        /// <param name="mediaLibrary">The library containing the tv show whose details are get</param>
        /// <param name="tvShowId">The id of the tv show whose details are get</param>
        public void GetTvShowMediaInfo(IMediaLibrary mediaLibrary, int tvShowId)
        {
            // get the tv show from the selected media
            TvShowEntity tvShow = mediaLibrary.Library.TvShows.Where(t => t.Id == tvShowId)
                                                              .FirstOrDefault();
            // get the media type source of the tv show
            MediaTypeSourceEntity source = mediaLibrary.Library.MediaTypes.SelectMany(mt => mt.MediaTypeSources)
                                                                          .Where(mts => mts.Id == tvShow.MediaTypeSourceId)
                                                                          .FirstOrDefault();
            // get the poster art for the tv show
            Poster = LogoHelpers.GetPossibleImagePath(source.MediaSourcePath + @"\poster");
            Fanart = LogoHelpers.GetPossibleImagePath(source.MediaSourcePath + @"\fanart");
            Banner = LogoHelpers.GetPossibleImagePath(source.MediaSourcePath + @"\banner");
            // assign the rest of the tv show info
            MPAA = tvShow.MPAA;
            Added = tvShow.Created;
            TagLine = tvShow.TagLine;
            Premiered = tvShow.Aired;
            Synopsis = tvShow.Synopsis;
            LocalPath = source.MediaSourcePath;
            NumberOfSeasons = tvShow.NumberOfSeasons;
            NumberOfEpisodes = tvShow.NumberOfEpisodes;
            CurrentStatus = tvShow.IsEnded ? "Ended" : "Ongoing";
            Genre = string.Join(" / ", tvShow.Genres.Select(g => g.Genre) ?? Enumerable.Empty<string>());
            Runtime = TimeSpan.FromSeconds(mediaLibrary.Library.TvShows.SelectMany(t => t.Seasons)
                                                                       .SelectMany(s => s.Episodes)
                                                                       .Where(e => e.TvShowId == tvShowId)
                                                                       .Sum(e => e.Runtime));
            NumberOfUnwatchedEpisodes = mediaLibrary.Library.TvShows.SelectMany(t => t.Seasons)
                                                                    .SelectMany(s => s.Episodes)
                                                                    .Where(e => e.IsWatched == false && e.TvShowId == tvShow.Id)
                                                                    .Count();
            SetTvShowOrArtistDisplayedElements();
        }

        /// <summary>
        /// Gets the additional info for the tv show season identified by <paramref name="seasonId"/>
        /// </summary>
        /// <param name="mediaLibrary">The library containing the tv show's season whose details are get</param>
        /// <param name="tvShowId">The id of the tv show containing the season whose details are get</param>
        /// <param name="seasonId">The id of the season whose details are get</param>
        public void GetSeasonMediaInfo(IMediaLibrary mediaLibrary, int tvShowId, int seasonId)
        {
            // get the tv show from the selected media
            TvShowEntity tvShow = mediaLibrary.Library.TvShows.Where(t => t.Id == tvShowId)
                                                              .FirstOrDefault();
            // get the media type source of the tv show
            MediaTypeSourceEntity source = mediaLibrary.Library.MediaTypes.SelectMany(mt => mt.MediaTypeSources)
                                                                          .Where(mts => mts.Id == tvShow.MediaTypeSourceId)
                                                                          .FirstOrDefault();
            // get the season of the tv show from the selected media
            SeasonEntity season = tvShow.Seasons.Where(s => s.Id == seasonId && s.TvShowId == tvShowId)
                                                .First();
            // get the poster art for the season
            Poster = LogoHelpers.GetPossibleImagePath(source.MediaSourcePath + @"\" + season.Title);
            // assign the rest of the tv show season info
            MPAA = string.Empty;
            Synopsis = season.Synopsis;
            Premiered = season.Premiered;
            LocalPath = source.MediaSourcePath + @"\" + season.Title;
            NumberOfEpisodes = mediaLibrary.Library.TvShows.SelectMany(t => t.Seasons)
                                                           .SelectMany(s => s.Episodes)
                                                           .Where(e => e.SeasonId == seasonId && e.TvShowId == tvShowId)
                                                           .Count();
            Runtime = TimeSpan.FromSeconds(mediaLibrary.Library.TvShows.SelectMany(t => t.Seasons)
                                                                       .SelectMany(s => s.Episodes)
                                                                       .Where(e => e.TvShowId == tvShowId && e.SeasonId == season.Id)
                                                                       .Sum(e => e.Runtime));
            NumberOfUnwatchedEpisodes = mediaLibrary.Library.TvShows.SelectMany(t => t.Seasons)
                                                                    .SelectMany(s => s.Episodes)
                                                                    .Where(e => e.IsWatched == false && e.SeasonId == seasonId && e.TvShowId == tvShowId)
                                                                    .Count();
            SetSeasonOrAlbumDisplayedElements();
        }

        /// <summary>
        /// Gets the additional info for the tv show episode identified by <paramref name="episodeTitle"/>
        /// </summary>
        /// <param name="mediaLibrary">The library containing the tv show containing the episode whose details are get</param>
        /// <param name="tvShowId">The id of the tv show containing the episode whose details are get</param>
        /// <param name="seasonId">The id of the season containing the episode whose details are get</param>
        /// <param name="episodeTitle">The name of the episode whose details are get</param>
        public void GetEpisodeMediaInfo(IMediaLibrary mediaLibrary, int tvShowId, int seasonId, string episodeTitle)
        {
            // get the tv show from the selected media
            TvShowEntity tvShow = mediaLibrary.Library.TvShows.Where(t => t.Id == tvShowId)
                                                              .FirstOrDefault();
            // get the media type source of the tv show
            MediaTypeSourceEntity source = mediaLibrary.Library.MediaTypes.SelectMany(mt => mt.MediaTypeSources)
                                                                          .Where(mts => mts.Id == tvShow.MediaTypeSourceId)
                                                                          .FirstOrDefault();
            // get the episode of the tv show from the selected media
            EpisodeEntity episode = mediaLibrary.Library.TvShows.SelectMany(t => t.Seasons)
                                                                .SelectMany(s => s.Episodes)
                                                                .Where(e => e.TvShowId == tvShowId && e.SeasonId == seasonId && e.Title == episodeTitle)
                                                                .FirstOrDefault();
            // get the poster art for the episode
            string fanart = source.MediaSourcePath + @"\" + mediaLibrary.Library.TvShows.SelectMany(t => t.Seasons)
                                                                                        .Where(s => s.Id == episode.SeasonId)
                                                                                        .First().Title + @"\" + episode.NamedTitle.Substring(0, episode.NamedTitle.LastIndexOf("."));
            Fanart = LogoHelpers.GetPossibleImagePath(fanart);
            // assign the rest of the tv show episode info
            MPAA = episode.MPAA;
            Premiered = episode.Aired;
            Synopsis = episode.Synopsis;
            Director = episode.Director;
            Genre = string.Join(" / ", episode.Genres.Select(g => g.Genre) ?? Enumerable.Empty<string>());
            Writers = string.Join(", ", episode.Credits.Select(g => g.Credit) ?? Enumerable.Empty<string>());
            Runtime = TimeSpan.FromSeconds(episode.Runtime);
            NumberOfUnwatchedEpisodes = mediaLibrary.Library.TvShows.SelectMany(t => t.Seasons)
                                                                    .SelectMany(s => s.Episodes)
                                                                    .Where(e => e.IsWatched == false && e.SeasonId == seasonId && e.TvShowId == tvShowId)
                                                                    .Count();
            SetEpisodeDisplayedElements();
        }

        /// <summary>
        /// Gets the additional info for the movie identified by <paramref name="movieId"/>
        /// </summary>
        /// <param name="mediaLibrary">The library containing the tv show whose details are get</param>
        /// <param name="movieId">The id of the movie whose details are get</param>
        public void GetMovieMediaInfo(IMediaLibrary mediaLibrary, int movieId)
        {
            // get the movie from the selected media
            MovieEntity movie = mediaLibrary.Library.Movies.Where(m => m.Id == movieId)
                                                           .FirstOrDefault();
            // get the media type source of the movie
            MediaTypeSourceEntity source = mediaLibrary.Library.MediaTypes.SelectMany(mt => mt.MediaTypeSources)
                                                                          .Where(mts => mts.Id == movie.MediaTypeSourceId)
                                                                          .FirstOrDefault();
            // get the poster art for the movie
            Poster = LogoHelpers.GetPossibleImagePath(source.MediaSourcePath + @"\poster");
            Fanart = LogoHelpers.GetPossibleImagePath(source.MediaSourcePath + @"\fanart");
            Banner = LogoHelpers.GetPossibleImagePath(source.MediaSourcePath + @"\banner");
            // assign the rest of the movie info
            MPAA = movie.MPAA;
            Added = movie.Created;
            TagLine = movie.Tagline;
            Synopsis = movie.Synopsis;
            Premiered = movie.Premiered;
            LocalPath = source.MediaSourcePath;
            Runtime = TimeSpan.FromSeconds(movie.Runtime);
            CurrentStatus = movie.IsEnded ? "Ended" : "Ongoing";
            Genre = string.Join(" / ", movie.Genres.Select(g => g.Genre) ?? Enumerable.Empty<string>());
            Writers = string.Join(", ", movie.Credits.Select(c => c.Credit) ?? Enumerable.Empty<string>());
            Director = string.Join(", ", movie.Directors.Select(d => d.Director) ?? Enumerable.Empty<string>());
            Runtime = TimeSpan.FromSeconds(mediaLibrary.Library.Movies.Sum(m => m.Runtime));
            SetMovieDisplayedElements();
        }

        /// <summary>
        /// Gets the additional info for the artist identified by <paramref name="artistId"/>
        /// </summary>
        /// <param name="mediaLibrary">The library containing the artist whose details are get</param>
        /// <param name="artistId">The id of the artist whose details are get</param>
        public void GetArtistMediaInfo(IMediaLibrary mediaLibrary, int artistId)
        {
            // get the artist from the selected media
            ArtistEntity artist = mediaLibrary.Library.Artists.Where(a => a.Id == artistId)
                                                              .FirstOrDefault();
            // get the media type source of the artist
            MediaTypeSourceEntity source = mediaLibrary.Library.MediaTypes.SelectMany(mt => mt.MediaTypeSources)
                                                                          .Where(mts => mts.Id == artist.MediaTypeSourceId)
                                                                          .FirstOrDefault();
            // get the poster art for the artist
            Poster = LogoHelpers.GetPossibleImagePath(source.MediaSourcePath + @"\poster");
            Fanart = LogoHelpers.GetPossibleImagePath(source.MediaSourcePath + @"\fanart");
            Banner = LogoHelpers.GetPossibleImagePath(source.MediaSourcePath + @"\banner");
            // assign the rest of the artist info
            Added = artist.Created;
            Premiered = artist.Formed;
            Synopsis = artist.Biography;
            LocalPath = source.MediaSourcePath;
            NumberOfSeasons = artist.Albums.Count();
            CurrentStatus = artist.IsDisbanded ? "Disbanded" : "Ongoing";
            Genre = string.Join(" / ", artist.Genres.Select(g => g.Genre) ?? Enumerable.Empty<string>());
            NumberOfEpisodes = mediaLibrary.Library.Artists.SelectMany(a => a.Albums)
                                                           .SelectMany(a => a.Songs)
                                                           .Where(s => s.ArtistId == artist.Id)
                                                           .Count();
            Runtime = TimeSpan.FromSeconds(mediaLibrary.Library.Artists.SelectMany(a => a.Albums)
                                                                       .SelectMany(a => a.Songs)
                                                                       .Where(s => s.ArtistId == artistId)
                                                                       .Sum(s => s.Length));
            NumberOfUnwatchedEpisodes = mediaLibrary.Library.Artists.SelectMany(a => a.Albums)
                                                                    .SelectMany(a => a.Songs)
                                                                    .Where(s => s.IsListened == false && s.ArtistId == artist.Id)
                                                                    .Count();
            SetTvShowOrArtistDisplayedElements();
        }

        /// <summary>
        /// Gets the additional info for the artist album identified by <paramref name="albumId"/>
        /// </summary>
        /// <param name="mediaLibrary">The library containing the tv show's season whose details are get</param>
        /// <param name="artistId">The id of the artist containing the album whose details are get</param>
        /// <param name="albumId">The id of the album whose details are get</param>
        public void GetAlbumMediaInfo(IMediaLibrary mediaLibrary, int artistId, int albumId)
        {
            // get the artist from the selected media
            ArtistEntity artist = mediaLibrary.Library.Artists.Where(a => a.Id == artistId)
                                                              .FirstOrDefault();
            // get the media type source of the artist
            MediaTypeSourceEntity source = mediaLibrary.Library.MediaTypes.SelectMany(mt => mt.MediaTypeSources)
                                                                          .Where(mts => mts.Id == artist.MediaTypeSourceId)
                                                                          .FirstOrDefault();
            // get the album from the selected media
            AlbumEntity season = artist.Albums.Where(a => a.Id == albumId && a.ArtistId == artistId)
                                              .First();
            // get the poster art for the album
            Poster = LogoHelpers.GetPossibleImagePath(source.MediaSourcePath + @"\" + season.Title);
            // assign the rest of the album info
            MPAA = string.Empty;
            Synopsis = season.Description;
            Premiered = new DateTime(season.Year, 0, 0);
            LocalPath = source.MediaSourcePath + @"\" + season.Title;
            NumberOfEpisodes = mediaLibrary.Library.Artists.SelectMany(a => a.Albums)
                                                           .SelectMany(a => a.Songs)
                                                           .Where(s => s.AlbumId == albumId && s.ArtistId == artistId)
                                                           .Count();
            Runtime = TimeSpan.FromSeconds(mediaLibrary.Library.Artists.SelectMany(a => a.Albums)
                                                                       .SelectMany(a => a.Songs)
                                                                       .Where(s => s.ArtistId == artistId && s.AlbumId == season.Id)
                                                                       .Sum(s => s.Length));
            NumberOfUnwatchedEpisodes = mediaLibrary.Library.Artists.SelectMany(a => a.Albums)
                                                                    .SelectMany(a => a.Songs)
                                                                    .Where(s => s.IsListened == false && s.AlbumId == albumId && s.ArtistId == artistId)
                                                                    .Count();
            SetSeasonOrAlbumDisplayedElements();
        }

        /// <summary>
        /// Gets the additional info for the artist song identified by <paramref name="songTitle"/>
        /// </summary>
        /// <param name="mediaLibrary">The library containing the artist containing the song whose details are get</param>
        /// <param name="artistId">The id of the artist containing the song whose details are get</param>
        /// <param name="albumId">The id of the album containing the song whose details are get</param>
        /// <param name="songTitle">The name of the song whose details are get</param>
        public void GetSongMediaInfo(IMediaLibrary mediaLibrary, int artistId, int albumId, string songTitle)
        {
            // get the artist from the selected media
            ArtistEntity artist = mediaLibrary.Library.Artists.Where(a => a.Id == artistId)
                                                              .FirstOrDefault();
            // get the media type source of the artist
            MediaTypeSourceEntity source = mediaLibrary.Library.MediaTypes.SelectMany(mt => mt.MediaTypeSources)
                                                                          .Where(mts => mts.Id == artist.MediaTypeSourceId)
                                                                          .FirstOrDefault();
            // get the song from the selected media
            SongEntity song = mediaLibrary.Library.Artists.SelectMany(a => a.Albums)
                                                          .SelectMany(a => a.Songs)
                                                          .Where(s => s.ArtistId == artistId && s.AlbumId == albumId && s.Title == songTitle)
                                                          .FirstOrDefault();
            // get the poster art for the song
            string fanart = source.MediaSourcePath + @"\" + mediaLibrary.Library.Artists.SelectMany(a => a.Albums)
                                                                                        .Where(a => a.Id == song.AlbumId)
                                                                                        .First().NamedTitle + @"\" + song.NamedTitle.Substring(0, song.NamedTitle.LastIndexOf("."));
            Fanart = LogoHelpers.GetPossibleImagePath(fanart);
            // assign the rest of the song info
            Runtime = TimeSpan.FromSeconds(song.Length);
            NumberOfUnwatchedEpisodes = mediaLibrary.Library.Artists.SelectMany(a => a.Albums)
                                                                    .SelectMany(a => a.Songs)
                                                                    .Where(s => s.IsListened == false && s.AlbumId == albumId && s.ArtistId == artistId)
                                                                    .Count();
            SetSongDisplayedElements();
        }

        /// <summary>
        /// Gets the list of tv shows to be displayed in the navigation list
        /// </summary>
        /// <param name="mediaLibrary">The library from which to take the tv shows to be displayed</param>
        public IEnumerable<IMediaEntity> GetTvShowsNavigationList(IMediaLibrary mediaLibrary, Func<IMediaEntity> mediaFactory, string mediaName = null)
        {
            int mediaTypeId = mediaLibrary.Library.MediaTypes.Where(mt => mt.MediaName == (mediaName ?? currentMediaName)).First().Id;
            int count = 0;
            foreach (TvShowEntity tvShow in mediaLibrary.Library.TvShows.Where(t => t.MediaTypeId == mediaTypeId))
            {
                IMediaEntity media = mediaFactory.Invoke();
                media.Id = tvShow.Id;
                media.Index = ++count;
                media.MediaName = tvShow.Title;
                media.IsWatched = tvShow.IsWatched;
                media.IsFavorite = tvShow.IsFavorite;
                media.Year = tvShow.Aired.Year;
                media.Rating = tvShow.Ratings.Where(r => r.Name == "ImDb").First().Value;
                media.CommonRating = string.Join("\n", tvShow.Ratings.Select(r => r.Name + ": " + r.Value + "/" + r.Max + " (" + r.Votes + " votes)"));
                yield return media;
            }
        }

        /// <summary>
        /// Gets the list of seasons to be displayed in the navigation list
        /// </summary>
        /// <param name="mediaLibrary">The library from which to take the seasons to be displayed</param>
        /// <param name="fromEpisode">An episode in the current navigation list from which to take the parent seasons</param>
        public IEnumerable<IMediaEntity> GetSeasonsNavigationListFromEpisode(IMediaLibrary mediaLibrary, IMediaEntity fromEpisode, Func<IMediaEntity> mediaFactory)
        {
            int count = 0;
            foreach (SeasonEntity season in mediaLibrary.Library.TvShows.SelectMany(t => t.Seasons).Where(s => s.TvShowId == fromEpisode.Id))
            {
                IMediaEntity media = mediaFactory.Invoke();
                media.Id = season.TvShowId;
                media.Index = ++count;
                media.MediaName = season.Title;
                media.IsWatched = season.IsWatched;
                media.IsFavorite = season.IsFavorite;
                media.Year = season.Year;
                media.SeasonOrAlbumId = season.Id;
                yield return media;
            }
        }

        /// <summary>
        /// Gets the list of seasons to be displayed in the navigation list
        /// </summary>
        /// <param name="mediaLibrary">The library from which to take the seasons to be displayed</param>
        /// <param name="fromTvShow">A tv show in the current navigation list from which to take the child seasons</param>
        public IEnumerable<IMediaEntity> GetSeasonsNavigationListFromTvShow(IMediaLibrary mediaLibrary, IMediaEntity fromTvShow, Func<IMediaEntity> mediaFactory)
        {
            foreach (SeasonEntity season in mediaLibrary.Library.TvShows.Where(t => t.Id == fromTvShow.Id).SelectMany(t => t.Seasons))
            {
                IMediaEntity media = mediaFactory.Invoke();
                media.Id = season.TvShowId;
                media.MediaName = season.Title;
                media.IsWatched = season.IsWatched;
                media.IsFavorite = season.IsFavorite;
                media.Year = season.Year;
                media.SeasonOrAlbumId = season.Id;
                yield return media;
            }
        }

        /// <summary>
        /// Gets the list of episodes to be displayed in the navigation list
        /// </summary>
        /// <param name="mediaLibrary">The library from which to take the episodes to be displayed</param>
        /// <param name="fromSeason">A season in the current navigation list from which to take the child episodes</param>
        public IEnumerable<IMediaEntity> GetEpisodesNavigationList(IMediaLibrary mediaLibrary, IMediaEntity fromSeason, Func<IMediaEntity> mediaFactory)
        {
            int count = 0;
            foreach (EpisodeEntity episode in mediaLibrary.Library.TvShows.SelectMany(t => t.Seasons)
                                                                          .SelectMany(s => s.Episodes)
                                                                          .Where(e => e.SeasonId == fromSeason.SeasonOrAlbumId && e.TvShowId == fromSeason.Id))
            {
                IMediaEntity media = mediaFactory.Invoke();
                media.Id = episode.TvShowId;
                media.Index = ++count;
                media.SeasonOrAlbumId = fromSeason.SeasonOrAlbumId;
                media.EpisodeOrSongId = episode.Id;
                media.MediaName = episode.Title;
                media.IsWatched = episode.IsWatched;
                media.IsFavorite = episode.IsFavorite;
                media.Year = fromSeason.Year;
                media.Rating = episode.Ratings.Where(r => r.Name == "ImDb").First().Value;
                media.CommonRating = string.Join("\n", episode.Ratings.Select(r => r.Name + ": " + r.Value + "/" + r.Max + " (" + r.Votes + " votes)"));
                yield return media;
            }
        }

        /// <summary>
        /// Gets the list of movies to be displayed in the navigation list
        /// </summary>
        /// <param name="mediaLibrary">The library from which to take the movies to be displayed</param>
        public IEnumerable<IMediaEntity> GetMoviesNavigationList(IMediaLibrary mediaLibrary, Func<IMediaEntity> mediaFactory, string mediaName = null)
        {
            int mediaTypeId = mediaLibrary.Library.MediaTypes.Where(mt => mt.MediaName == (mediaName ?? currentMediaName)).First().Id;
            List<IMediaEntity> temp = new List<IMediaEntity>();
            int count = 0;
            foreach (MovieEntity movie in mediaLibrary.Library.Movies.Where(t => t.MediaTypeId == mediaTypeId))
            {
                IMediaEntity media = mediaFactory.Invoke();
                media.Id = movie.Id;
                media.Index = ++count;
                media.MediaName = movie.Title;
                media.IsWatched = movie.IsWatched;
                media.IsFavorite = movie.IsFavorite;
                media.Year = movie.Premiered.Year;
                media.Rating = movie.Ratings.Where(r => r.Name == "ImDb").First().Value;
                media.CommonRating = string.Join("\n", movie.Ratings.Select(r => r.Name + ": " + r.Value + "/" + r.Max + " (" + r.Votes + " votes)"));
                yield return media;
            }
        }

        /// <summary>
        /// Gets the list of artists to be displayed in the navigation list
        /// </summary>
        /// <param name="mediaLibrary">The library from which to take the artists to be displayed</param>
        public IEnumerable<IMediaEntity> GetArtistsNavigationList(IMediaLibrary mediaLibrary, Func<IMediaEntity> mediaFactory, string mediaName = null)
        {
            int mediaTypeId = mediaLibrary.Library.MediaTypes.Where(mt => mt.MediaName == (mediaName ?? currentMediaName)).First().Id;
            int count = 0;
            foreach (ArtistEntity artist in mediaLibrary.Library.Artists.Where(a => a.MediaTypeId == mediaTypeId))
            {
                IMediaEntity media = mediaFactory.Invoke();
                media.Id = artist.Id;
                media.Index = ++count;
                media.MediaName = artist.Title;
                media.IsWatched = artist.IsListened;
                media.IsFavorite = artist.IsFavorite;
                media.Year = artist.Formed.Year;
                yield return media;
            }
        }

        /// <summary>
        /// Gets the list of albums to be displayed in the navigation list
        /// </summary>
        /// <param name="mediaLibrary">The library from which to take the albums to be displayed</param>
        /// <param name="fromSong">A song in the current navigation list from which to take the parent albums</param>
        public IEnumerable<IMediaEntity> GetAlbumsNavigationListFromSong(IMediaLibrary mediaLibrary, IMediaEntity fromSong, Func<IMediaEntity> mediaFactory)
        {
            int count = 0;
            foreach (AlbumEntity album in mediaLibrary.Library.Artists.SelectMany(a => a.Albums).Where(a => a.ArtistId == fromSong.Id))
            {
                IMediaEntity media = mediaFactory.Invoke();
                media.Id = album.ArtistId;
                media.Index = ++count;
                media.MediaName = album.Title;
                media.IsWatched = album.IsListened;
                media.IsFavorite = album.IsFavorite;
                media.Year = album.Year;
                media.SeasonOrAlbumId = album.Id;
                yield return media;
            }
        }

        /// <summary>
        /// Gets the list of albums to be displayed in the navigation list
        /// </summary>
        /// <param name="mediaLibrary">The library from which to take the albums to be displayed</param>
        /// <param name="fromArtist">An artist in the current navigation list from which to take the child albums</param>
        public IEnumerable<IMediaEntity> GetAlbumsNavigationListFromArtist(IMediaLibrary mediaLibrary, IMediaEntity fromArtist, Func<IMediaEntity> mediaFactory)
        {
            foreach (AlbumEntity album in mediaLibrary.Library.Artists.Where(a => a.Id == fromArtist.Id).SelectMany(a => a.Albums))
            {
                IMediaEntity media = mediaFactory.Invoke();
                media.Id = album.ArtistId;
                media.MediaName = album.Title;
                media.IsWatched = album.IsListened;
                media.IsFavorite = album.IsFavorite;
                media.Year = album.Year;
                media.SeasonOrAlbumId = album.Id;
                yield return media;
            }
        }

        /// <summary>
        /// Gets the list of songs to be displayed in the navigation list
        /// </summary>
        /// <param name="mediaLibrary">The library from which to take the songs to be displayed</param>
        /// <param name="fromAlbum">An album in the current navigation list from which to take the child songs</param>
        public IEnumerable<IMediaEntity> GetSongsNavigationList(IMediaLibrary mediaLibrary, IMediaEntity fromAlbum, Func<IMediaEntity> mediaFactory)
        {
            int count = 0;
            foreach (SongEntity song in mediaLibrary.Library.Artists.SelectMany(a => a.Albums)
                                                                    .SelectMany(a => a.Songs)
                                                                    .Where(s => s.AlbumId == fromAlbum.SeasonOrAlbumId && s.ArtistId == fromAlbum.Id))
            {
                IMediaEntity media = mediaFactory.Invoke();
                media.Id = song.ArtistId;
                media.Index = ++count;
                media.SeasonOrAlbumId = fromAlbum.SeasonOrAlbumId;
                media.EpisodeOrSongId = song.Id;
                media.MediaName = song.Title;
                media.IsWatched = song.IsListened;
                media.IsFavorite = song.IsFavorite;
                media.Year = fromAlbum.Year;
                media.Rating = song.Rating;
                media.CommonRating = song.Rating.ToString();
                yield return media;
            }
        }

        /// <summary>
        /// Navigates to the episode media list from an episode advanced search result
        /// </summary>
        /// <param name="mediaLibrary">The library from which to take the episodes to be displayed</param>
        /// <param name="mediaFactory">The factory for creating new instances of type <see cref="IMediaEntity"/></param>
        /// <param name="searchEntity">The advanced search entity that initiated the navigation</param>
        /// <returns>An IEnumerable of media items representing the episodes containing the <paramref name="searchEntity"/></returns>
        public IEnumerable<IMediaEntity> NavigateToEpisodeSearchResult(IMediaLibrary mediaLibrary, Func<IMediaEntity> mediaFactory, AdvancedSearchResultEntity searchEntity)
        {
            CurrentNavigationLevel = NavigationLevel.Episode;
            // get the episode from the media library with an id and title similar to the searched entity
            var episode = mediaLibrary.Library.TvShows.SelectMany(t => t.Seasons)
                                                      .SelectMany(s => s.Episodes)
                                                      .Where(e => e.Id == searchEntity.Id && e.Title == searchEntity.MediaTitle)
                                                      .FirstOrDefault();
            // get the season of the above episode
            var season = mediaLibrary.Library.TvShows.SelectMany(t => t.Seasons)
                                                     .Where(s => s.Episodes.Where(e => e.Id == episode.Id)
                                                                           .Count() > 0)
                                                     .FirstOrDefault();
            // get the tv show of the above season
            var tvshow = mediaLibrary.Library.TvShows.Where(t => t.Seasons.Where(s => s.Id == season.Id)
                                                                          .Count() > 0)
                                                     .FirstOrDefault();
            // get the media type of the above tv show
            MediaTypeEntity mediaType = mediaLibrary.Library.MediaTypes.Where(mt => mt.Id == tvshow.MediaTypeId)
                                                                       .FirstOrDefault();
            currentMediaName = mediaType.MediaName;
            // update the displayed elements for tv show episodes
            GetTvShowMediaInfo(mediaLibrary, tvshow.Id);
            GetSeasonMediaInfo(mediaLibrary, tvshow.Id, season.Id);
            GetEpisodeMediaInfo(mediaLibrary, tvshow.Id, season.Id, episode.Title);
            // return the media navigation list containing the searched episode
            IMediaEntity mediaEntity = mediaFactory.Invoke();
            mediaEntity.SeasonOrAlbumId = season.Id;
            mediaEntity.Id = tvshow.Id;
            mediaEntity.Year = season.Year;
            return GetEpisodesNavigationList(mediaLibrary, mediaEntity, mediaFactory);
        }

        /// <summary>
        /// Navigates to the season media list from a season advanced search result
        /// </summary>
        /// <param name="mediaLibrary">The library from which to take the seasons to be displayed</param>
        /// <param name="mediaFactory">The factory for creating new instances of type <see cref="IMediaEntity"/></param>
        /// <param name="searchEntity">The advanced search entity that initiated the navigation</param>
        /// <returns>An IEnumerable of media items representing the seasons containing the <paramref name="searchEntity"/></returns>
        public IEnumerable<IMediaEntity> NavigateToSeasonSearchResult(IMediaLibrary mediaLibrary, Func<IMediaEntity> mediaFactory, AdvancedSearchResultEntity searchEntity)
        {
            CurrentNavigationLevel = NavigationLevel.Season;
            // get the season from the media library with an id and title similar to the searched entity
            var season = mediaLibrary.Library.TvShows.SelectMany(t => t.Seasons)
                                                     .Where(s => s.Id == searchEntity.Id && s.Title == searchEntity.MediaTitle)
                                                     .FirstOrDefault();
            // get the tv show of the above season
            var tvshow = mediaLibrary.Library.TvShows.Where(t => t.Seasons.Where(s => s.Id == season.Id)
                                                                          .Count() > 0)
                                                     .FirstOrDefault();
            // get the media type of the above tv show
            MediaTypeEntity mediaType = mediaLibrary.Library.MediaTypes.Where(mt => mt.Id == tvshow.MediaTypeId)
                                                                       .FirstOrDefault();
            currentMediaName = mediaType.MediaName;
            // update the displayed elements for tv show 
            GetTvShowMediaInfo(mediaLibrary, tvshow.Id);
            // return the media navigation list containing the searched season
            IMediaEntity mediaEntity = mediaFactory.Invoke();
            mediaEntity.Id = tvshow.Id;
            return GetSeasonsNavigationListFromEpisode(mediaLibrary, mediaEntity, mediaFactory);
        }

        /// <summary>
        /// Navigates to the tv show media list from a tv show advanced search result
        /// </summary>
        /// <param name="mediaLibrary">The library from which to take the tv shows to be displayed</param>
        /// <param name="mediaFactory">The factory for creating new instances of type <see cref="IMediaEntity"/></param>
        /// <param name="searchEntity">The advanced search entity that initiated the navigation</param>
        /// <returns>An IEnumerable of media items representing the tv shows containing the <paramref name="searchEntity"/></returns>
        public IEnumerable<IMediaEntity> NavigateToTvShowSearchResult(IMediaLibrary mediaLibrary, Func<IMediaEntity> mediaFactory, AdvancedSearchResultEntity searchEntity)
        {
            CurrentNavigationLevel = NavigationLevel.TvShow;
            // get the tv show from the media library with an id and title similar to the searched entity
            var tvshow = mediaLibrary.Library.TvShows.Where(t => t.Id == searchEntity.Id)
                                                     .FirstOrDefault();
            // get the media type of the above tv show
            MediaTypeEntity mediaType = mediaLibrary.Library.MediaTypes.Where(mt => mt.Id == tvshow.MediaTypeId)
                                                                       .FirstOrDefault();
            currentMediaName = mediaType.MediaName;
            // return the media navigation list containing the searched tv show
            return GetTvShowsNavigationList(mediaLibrary, mediaFactory, mediaType.MediaName);
        }

        /// <summary>
        /// Navigates to the song media list from a song advanced search result
        /// </summary>
        /// <param name="mediaLibrary">The library from which to take the songs to be displayed</param>
        /// <param name="mediaFactory">The factory for creating new instances of type <see cref="IMediaEntity"/></param>
        /// <param name="searchEntity">The advanced search entity that initiated the navigation</param>
        /// <returns>An IEnumerable of media items representing the songs containing the <paramref name="searchEntity"/></returns>
        public IEnumerable<IMediaEntity> NavigateToSongSearchResult(IMediaLibrary mediaLibrary, Func<IMediaEntity> mediaFactory, AdvancedSearchResultEntity searchEntity)
        {
            CurrentNavigationLevel = NavigationLevel.Song;
            // get the song from the media library with an id and title similar to the searched entity
            var song = mediaLibrary.Library.Artists.SelectMany(t => t.Albums)
                                                   .SelectMany(s => s.Songs)
                                                   .Where(e => e.Id == searchEntity.Id && e.Title == searchEntity.MediaTitle)
                                                   .FirstOrDefault();
            // get the album of the above song
            var album = mediaLibrary.Library.Artists.SelectMany(t => t.Albums)
                                                    .Where(s => s.Songs.Where(e => e.Id == song.Id)
                                                                       .Count() > 0)
                                                    .FirstOrDefault();
            // get the artist of the above album
            var artist = mediaLibrary.Library.Artists.Where(t => t.Albums.Where(s => s.Id == album.Id)
                                                                         .Count() > 0)
                                                     .FirstOrDefault();
            // get the media type of the above artist
            MediaTypeEntity mediaType = mediaLibrary.Library.MediaTypes.Where(mt => mt.Id == artist.MediaTypeId)
                                                                       .FirstOrDefault();
            currentMediaName = mediaType.MediaName;
            // update the displayed elements for artist songs
            GetArtistMediaInfo(mediaLibrary, artist.Id);
            GetAlbumMediaInfo(mediaLibrary, artist.Id, album.Id);
            GetSongMediaInfo(mediaLibrary, artist.Id, album.Id, song.Title);
            // return the media navigation list containing the searched song
            IMediaEntity mediaEntity = mediaFactory.Invoke();
            mediaEntity.SeasonOrAlbumId = album.Id;
            mediaEntity.Id = artist.Id;
            mediaEntity.Year = album.Year;
            return GetSongsNavigationList(mediaLibrary, mediaEntity, mediaFactory);
        }

        /// <summary>
        /// Navigates to the album media list from an album advanced search result
        /// </summary>
        /// <param name="mediaLibrary">The library from which to take the albums to be displayed</param>
        /// <param name="mediaFactory">The factory for creating new instances of type <see cref="IMediaEntity"/></param>
        /// <param name="searchEntity">The advanced search entity that initiated the navigation</param>
        /// <returns>An IEnumerable of media items representing the albums containing the <paramref name="searchEntity"/></returns>
        public IEnumerable<IMediaEntity> NavigateToAlbumSearchResult(IMediaLibrary mediaLibrary, Func<IMediaEntity> mediaFactory, AdvancedSearchResultEntity searchEntity)
        {
            CurrentNavigationLevel = NavigationLevel.Album;
            // get the album from the media library with an id and title similar to the searched entity
            var album = mediaLibrary.Library.Artists.SelectMany(t => t.Albums)
                                                    .Where(s => s.Id == searchEntity.Id && s.Title == searchEntity.MediaTitle)
                                                    .FirstOrDefault();
            // get the artist of the above album
            var artist = mediaLibrary.Library.Artists.Where(t => t.Albums.Where(s => s.Id == album.Id)
                                                                         .Count() > 0)
                                                     .FirstOrDefault();
            // get the media type of the above artist
            MediaTypeEntity mediaType = mediaLibrary.Library.MediaTypes.Where(mt => mt.Id == artist.MediaTypeId)
                                                                       .FirstOrDefault();
            currentMediaName = mediaType.MediaName;
            // update the displayed elements for artist
            GetArtistMediaInfo(mediaLibrary, artist.Id);
            // return the media navigation list containing the searched album
            IMediaEntity mediaEntity = mediaFactory.Invoke();
            mediaEntity.Id = artist.Id;
            return GetAlbumsNavigationListFromSong(mediaLibrary, mediaEntity, mediaFactory);
        }

        /// <summary>
        /// Navigates to the artist media list from an artist advanced search result
        /// </summary>
        /// <param name="mediaLibrary">The library from which to take the artists to be displayed</param>
        /// <param name="mediaFactory">The factory for creating new instances of type <see cref="IMediaEntity"/></param>
        /// <param name="searchEntity">The advanced search entity that initiated the navigation</param>
        /// <returns>An IEnumerable of media items representing the artists containing the <paramref name="searchEntity"/></returns>
        public IEnumerable<IMediaEntity> NavigateToArtistSearchResult(IMediaLibrary mediaLibrary, Func<IMediaEntity> mediaFactory, AdvancedSearchResultEntity searchEntity)
        {
            CurrentNavigationLevel = NavigationLevel.Artist;
            // get the artist from the media library with an id and title similar to the searched entity
            var artist = mediaLibrary.Library.Artists.Where(t => t.Id == searchEntity.Id)
                                                     .FirstOrDefault();
            // get the media type of the above artist
            MediaTypeEntity mediaType = mediaLibrary.Library.MediaTypes.Where(mt => mt.Id == artist.MediaTypeId)
                                                                       .FirstOrDefault();
            currentMediaName = mediaType.MediaName;
            // return the media navigation list containing the searched artist
            return GetArtistsNavigationList(mediaLibrary, mediaFactory, mediaType.MediaName);
        }

        /// <summary>
        /// Navigates to the movie media list from a movie advanced search result
        /// </summary>
        /// <param name="mediaLibrary">The library from which to take the movies to be displayed</param>
        /// <param name="mediaFactory">The factory for creating new instances of type <see cref="IMediaEntity"/></param>
        /// <param name="searchEntity">The advanced search entity that initiated the navigation</param>
        /// <returns>An IEnumerable of media items representing the movies containing the <paramref name="searchEntity"/></returns>
        public IEnumerable<IMediaEntity> NavigateToMovieSearchResult(IMediaLibrary mediaLibrary, Func<IMediaEntity> mediaFactory, AdvancedSearchResultEntity searchEntity)
        {
            CurrentNavigationLevel = NavigationLevel.Movie;
            // get the movie from the media library with an id and title similar to the searched entity
            var movie = mediaLibrary.Library.Movies.Where(t => t.Id == searchEntity.Id)
                                                   .FirstOrDefault();
            // get the media type of the above movie
            MediaTypeEntity mediaType = mediaLibrary.Library.MediaTypes.Where(mt => mt.Id == movie.MediaTypeId)
                                                                       .FirstOrDefault();
            currentMediaName = mediaType.MediaName;
            // return the media navigation list containing the searched movie
            return GetMoviesNavigationList(mediaLibrary, mediaFactory, mediaType.MediaName);
        }

        /// <summary>
        /// Updates the displayed elements for tv shows or artists
        /// </summary>
        private void SetTvShowOrArtistDisplayedElements()
        {
            IsTvShow = true;
            IsFanartVisible = true;
            AreWritersVisible = false;
            IsDirectorVisible = false;
            AreNumberOfSeasonsVisible = true;
        }

        /// <summary>
        /// Updates the displayed elements for seasons or albums
        /// </summary>
        private void SetSeasonOrAlbumDisplayedElements()
        {
            IsFanartVisible = false;
            AreWritersVisible = false;
            IsDirectorVisible = false;
            AreNumberOfSeasonsVisible = true;
        }

        /// <summary>
        /// Updates the displayed elements for episodes
        /// </summary>
        private void SetEpisodeDisplayedElements()
        {
            IsFanartVisible = true;
            AreWritersVisible = true;
            IsDirectorVisible = true;
            AreNumberOfSeasonsVisible = false;
        }

        /// <summary>
        /// Updates the displayed elements for movies
        /// </summary>
        private void SetMovieDisplayedElements()
        {
            IsTvShow = false;
            IsFanartVisible = true;
            AreWritersVisible = true;
            IsDirectorVisible = true;
            AreNumberOfSeasonsVisible = false;
        }

        /// <summary>
        /// Updates the displayed elements for songs
        /// </summary>
        private void SetSongDisplayedElements()
        {
            IsFanartVisible = true;
            AreWritersVisible = false;
            IsDirectorVisible = false;
            AreNumberOfSeasonsVisible = false;
        }

        /// <summary>
        /// Updates the displayed elements when exiting media library display
        /// </summary>
        private void ExitMediaDisplay()
        {
            IsBackNavigationPossible = false;
            IsMediaCoverVisible = false;
        }
        #endregion
    }
}
