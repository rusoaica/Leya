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

        private string currentMediaName = string.Empty;
        private readonly IMediaPlayer mediaPlayer;
        #endregion

        #region ================================================================ PROPERTIES =================================================================================
        private int numberOfSeasons = 0;
        public int NumberOfSeasons
        {
            get { return numberOfSeasons; }
            set { numberOfSeasons = value; Notify(); }
        }

        private int numberOfEpisodes = 0;
        public int NumberOfEpisodes
        {
            get { return numberOfEpisodes; }
            set { numberOfEpisodes = value; Notify(); }
        }

        private int numberOfUnwatchedEpisodes = 0;
        public int NumberOfUnwatchedEpisodes
        {
            get { return numberOfUnwatchedEpisodes; }
            set { numberOfUnwatchedEpisodes = value; Notify(); }
        }

        private string poster;
        public string Poster
        {
            get { return poster; }
            set { poster = value; Notify(); }
        }

        private string fanart;
        public string Fanart
        {
            get { return fanart; }
            set { fanart = value; Notify(); }
        }

        private string banner = string.Empty;
        public string Banner
        {
            get { return banner; }
            set { banner = value; Notify(); }
        }

        private string mpaa = string.Empty;
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

        private string synopsis = string.Empty;
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

        private string genre = string.Empty;
        public string Genre
        {
            get { return genre; }
            set { genre = value; Notify(); }
        }

        private string writers = string.Empty;
        public string Writers
        {
            get { return writers; }
            set { writers = value; Notify(); }
        }

        private string director = string.Empty;
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

        private bool isFanartVisible = false;
        public bool IsFanartVisible
        {
            get { return isFanartVisible; }
            set { isFanartVisible = value; Notify(); }
        }

        private bool isWritersVisible;
        public bool IsWritersVisible
        {
            get { return isWritersVisible; }
            set { isWritersVisible = value; Notify(); }
        }

        private bool isDirectorVisible;
        public bool IsDirectorVisible
        {
            get { return isDirectorVisible; }
            set { isDirectorVisible = value; Notify(); }
        }

        private bool isNumberOfSeasonsVisible;
        public bool IsNumberOfSeasonsVisible
        {
            get { return isNumberOfSeasonsVisible; }
            set { isNumberOfSeasonsVisible = value; Notify(); }
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
        public async Task NavigateDown(IMediaLibrary mediaLibrary, IMediaEntity media)
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
            Genre = string.Join(" / ", tvShow.Genre.Select(g => g.Genre) ?? Enumerable.Empty<string>());
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
            Poster = LogoHelpers.GetPossibleImagePath(source.MediaSourcePath + @"\" + season.SeasonName);
            // assign the rest of the tv show season info
            MPAA = string.Empty;
            Synopsis = season.Synopsis;
            Premiered = season.Premiered;
            LocalPath = source.MediaSourcePath + @"\" + season.SeasonName;
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
        /// Gets the additional info for the tv show episode identified by <paramref name="episodeName"/>
        /// </summary>
        /// <param name="mediaLibrary">The library containing the tv show containing the episode whose details are get</param>
        /// <param name="tvShowId">The id of the tv show containing the episode whose details are get</param>
        /// <param name="seasonId">The id of the season containing the episode whose details are get</param>
        /// <param name="episodeName">The name of the episode whose details are get</param>
        public void GetEpisodeMediaInfo(IMediaLibrary mediaLibrary, int tvShowId, int seasonId, string episodeName)
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
                                                                .Where(e => e.TvShowId == tvShowId && e.SeasonId == seasonId && e.Title == episodeName)
                                                                .FirstOrDefault();
            // get the poster art for the episode
            string fanart = source.MediaSourcePath + @"\" + mediaLibrary.Library.TvShows.SelectMany(t => t.Seasons)
                                                                                        .Where(s => s.Id == episode.SeasonId)
                                                                                        .First().SeasonName + @"\" + episode.NamedTitle.Substring(0, episode.NamedTitle.LastIndexOf("."));
            Fanart = LogoHelpers.GetPossibleImagePath(fanart);
            // assign the rest of the tv show episode info
            MPAA = episode.MPAA;
            Premiered = episode.Aired;
            Synopsis = episode.Synopsis;
            Director = episode.Director;
            Genre = string.Join(" / ", episode.Genre.Select(g => g.Genre) ?? Enumerable.Empty<string>());
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
            Genre = string.Join(" / ", movie.Genre.Select(g => g.Genre) ?? Enumerable.Empty<string>());
            Writers = string.Join(", ", movie.Credits.Select(c => c.Credit) ?? Enumerable.Empty<string>());
            Director = string.Join(", ", movie.Director.Select(d => d.Director) ?? Enumerable.Empty<string>());
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
        /// Gets the additional info for the artist song identified by <paramref name="songName"/>
        /// </summary>
        /// <param name="mediaLibrary">The library containing the artist containing the song whose details are get</param>
        /// <param name="artistId">The id of the artist containing the song whose details are get</param>
        /// <param name="albumId">The id of the album containing the song whose details are get</param>
        /// <param name="songName">The name of the song whose details are get</param>
        public void GetSongMediaInfo(IMediaLibrary mediaLibrary, int artistId, int albumId, string songName)
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
                                                          .Where(s => s.ArtistId == artistId && s.AlbumId == albumId && s.Title == songName)
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
        public IEnumerable<IMediaEntity> GetTvShowsNavigationList(IMediaLibrary mediaLibrary, Func<IMediaEntity> mediaFactory)
        {
            int mediaTypeId = mediaLibrary.Library.MediaTypes.Where(mt => mt.MediaName == currentMediaName).First().Id;
            List<IMediaEntity> temp = new List<IMediaEntity>();
            foreach (TvShowEntity tvShow in mediaLibrary.Library.TvShows.Where(t => t.MediaTypeId == mediaTypeId))
            {
                IMediaEntity media = mediaFactory.Invoke();
                media.Id = tvShow.Id;
                media.MediaName = tvShow.TvShowTitle;
                media.IsWatched = tvShow.IsWatched;
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
            foreach (SeasonEntity season in mediaLibrary.Library.TvShows.SelectMany(t => t.Seasons).Where(s => s.TvShowId == fromEpisode.Id))
            {
                IMediaEntity media = mediaFactory.Invoke();
                media.Id = season.TvShowId;
                media.MediaName = season.SeasonName;
                media.IsWatched = season.IsWatched;
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
                media.MediaName = season.SeasonName;
                media.IsWatched = season.IsWatched;
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
            foreach (EpisodeEntity episode in mediaLibrary.Library.TvShows.SelectMany(t => t.Seasons)
                                                                          .SelectMany(s => s.Episodes)
                                                                          .Where(e => e.SeasonId == fromSeason.SeasonOrAlbumId && e.TvShowId == fromSeason.Id))
            {
                IMediaEntity media = mediaFactory.Invoke();
                media.Id = episode.TvShowId;
                media.SeasonOrAlbumId = fromSeason.SeasonOrAlbumId;
                media.EpisodeOrSongId = episode.Id;
                media.MediaName = episode.Title;
                media.IsWatched = episode.IsWatched;
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
        public IEnumerable<IMediaEntity> GetMoviesNavigationList(IMediaLibrary mediaLibrary, Func<IMediaEntity> mediaFactory)
        {
            int mediaTypeId = mediaLibrary.Library.MediaTypes.Where(mt => mt.MediaName == currentMediaName).First().Id;
            List<IMediaEntity> temp = new List<IMediaEntity>();
            foreach (MovieEntity movie in mediaLibrary.Library.Movies.Where(t => t.MediaTypeId == mediaTypeId))
            {
                IMediaEntity media = mediaFactory.Invoke();
                media.Id = movie.Id;
                media.MediaName = movie.MovieTitle;
                media.IsWatched = movie.IsWatched;
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
        public IEnumerable<IMediaEntity> GetArtistsNavigationList(IMediaLibrary mediaLibrary, Func<IMediaEntity> mediaFactory)
        {
            int mediaTypeId = mediaLibrary.Library.MediaTypes.Where(mt => mt.MediaName == currentMediaName).First().Id;
            foreach (ArtistEntity artist in mediaLibrary.Library.Artists.Where(a => a.MediaTypeId == mediaTypeId))
            {
                IMediaEntity media = mediaFactory.Invoke();
                media.Id = artist.Id;
                media.MediaName = artist.ArtistName;
                media.IsWatched = artist.IsListened;
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
            foreach (AlbumEntity album in mediaLibrary.Library.Artists.SelectMany(a => a.Albums).Where(a => a.ArtistId == fromSong.Id))
            {
                IMediaEntity media = mediaFactory.Invoke();
                media.Id = album.ArtistId;
                media.MediaName = album.Title;
                media.IsWatched = album.IsListened;
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
            foreach (SongEntity song in mediaLibrary.Library.Artists.SelectMany(a => a.Albums)
                                                                    .SelectMany(a => a.Songs)
                                                                    .Where(s => s.AlbumId == fromAlbum.SeasonOrAlbumId && s.ArtistId == fromAlbum.Id))
            {
                IMediaEntity media = mediaFactory.Invoke();
                media.Id = song.ArtistId;
                media.SeasonOrAlbumId = fromAlbum.SeasonOrAlbumId;
                media.EpisodeOrSongId = song.Id;
                media.MediaName = song.Title;
                media.IsWatched = song.IsListened;
                media.Year = fromAlbum.Year;
                media.Rating = song.Rating;
                media.CommonRating = song.Rating.ToString();
                yield return media;
            }
        }

        /// <summary>
        /// Updates the displayed elements for tv shows or artists
        /// </summary>
        private void SetTvShowOrArtistDisplayedElements()
        {
            IsTvShow = true;
            IsFanartVisible = true;
            IsWritersVisible = false;
            IsDirectorVisible = false;
            IsNumberOfSeasonsVisible = true;
        }

        /// <summary>
        /// Updates the displayed elements for seasons or albums
        /// </summary>
        private void SetSeasonOrAlbumDisplayedElements()
        {
            IsFanartVisible = false;
            IsWritersVisible = false;
            IsDirectorVisible = false;
            IsNumberOfSeasonsVisible = true;
        }

        /// <summary>
        /// Updates the displayed elements for episodes
        /// </summary>
        private void SetEpisodeDisplayedElements()
        {
            IsFanartVisible = true;
            IsWritersVisible = true;
            IsDirectorVisible = true;
            IsNumberOfSeasonsVisible = false;
        }

        /// <summary>
        /// Updates the displayed elements for movies
        /// </summary>
        private void SetMovieDisplayedElements()
        {
            IsTvShow = false;
            IsFanartVisible = true;
            IsWritersVisible = true;
            IsDirectorVisible = true;
            IsNumberOfSeasonsVisible = false;
        }

        /// <summary>
        /// Updates the displayed elements for songs
        /// </summary>
        private void SetSongDisplayedElements()
        {
            IsFanartVisible = true;
            IsWritersVisible = false;
            IsDirectorVisible = false;
            IsNumberOfSeasonsVisible = false;
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
