/// Written by: Yulia Danilova
/// Creation Date: 13th of July, 2021
/// Purpose: Interface business model for media library navigation
#region ========================================================================= USING =====================================================================================
using System;
using System.Threading.Tasks;
using Leya.Infrastructure.Enums;
using System.Collections.Generic;
using Leya.Models.Core.MediaLibrary;
using Leya.Models.Common.Models.Media;
using Leya.Models.Common.Models.Common;
#endregion

namespace Leya.Models.Core.Navigation
{
    public interface IMediaLibraryNavigation
    {
        #region ============================================================== FIELD MEMBERS ================================================================================
        event Action Navigated;
        event Action<string> PropertyChanged;
        #endregion

        #region ================================================================ PROPERTIES =================================================================================
        int NumberOfSeasons { get; set; }
        int NumberOfEpisodes { get; set; }
        int NumberOfUnwatchedEpisodes { get; set; }
        string MPAA { get; set; }
        string Genre { get; set; }
        string Fanart { get; set; }
        string Poster { get; set; }
        string Banner { get; set; }
        string TagLine { get; set; }
        string Writers { get; set; }
        string Synopsis { get; set; }
        string Director { get; set; }
        string LocalPath { get; set; }
        string CurrentStatus { get; set; }
        bool IsTvShow { get; set; }
        bool IsFanartVisible { get; set; }
        bool AreWritersVisible { get; set; }
        bool IsDirectorVisible { get; set; }
        bool ShowsOnlyFavorites { get; set; }
        bool ShowsOnlyUnwatched { get; set; }
        bool IsMediaCoverVisible { get; set; }
        bool IsHelpButtonVisible { get; set; }
        bool IsSystemOptionVisible { get; set; }
        bool IsPlayerOptionVisible { get; set; }
        bool IsMediaContainerVisible { get; set; }
        bool IsInterfaceOptionVisible { get; set; }
        bool IsBackNavigationPossible { get; set; }
        bool IsSearchContainerVisible { get; set; }
        bool IsMediaTypesOptionVisible { get; set; }
        bool AreNumberOfSeasonsVisible { get; set; }
        bool IsOptionsContainerVisible { get; set; }
        bool IsMediaCategorySelectionVisible { get; set; }
        bool AreMediaTypeSourcesOptionVisible { get; set; }
        TimeSpan Runtime { get; set; }
        DateTime Added { get; set; }
        DateTime Premiered { get; set; }
        NavigationLevel CurrentNavigationLevel { get; set; }
        #endregion

        #region ================================================================= METHODS ===================================================================================
        /// <summary>
        /// Enters the media view and displays the category identified by <paramref name="menu"/> item
        /// </summary>
        /// <param name="menu">The menu item for which to display the media view</param>
        void InitiateMainMenuNavigation(MediaTypeEntity menu);

        /// <summary>
        /// Exits the interface options view and returns to the options list
        /// </summary>
        void ExitInterfaceOptions();

        /// <summary>
        /// Exits the system options view and returns to the options list
        /// </summary>
        void ExitSystemOptions();

        /// <summary>
        /// Exits the player options view and returns to the options list
        /// </summary>
        void ExitPlayerOptions();

        /// <summary>
        /// Exits the media types options list and returns to the options list
        /// </summary>
        void ExitMediaTypesOptions();
        
        /// <summary>
        /// Gets the additional info for the tv show identified by <paramref name="tvShowId"/>
        /// </summary>
        /// <param name="mediaLibrary">The library containing the tv show whose details are get</param>
        /// <param name="tvShowId">The id of the tv show whose details are get</param>
        void GetTvShowMediaInfo(IMediaLibrary mediaLibrary, int tvShowId);

        /// <summary>
        /// Gets the additional info for the tv show season identified by <paramref name="seasonId"/>
        /// </summary>
        /// <param name="mediaLibrary">The library containing the tv show's season whose details are get</param>
        /// <param name="tvShowId">The id of the tv show containing the season whose details are get</param>
        /// <param name="seasonId">The id of the season whose details are get</param>
        void GetSeasonMediaInfo(IMediaLibrary mediaLibrary, int tvShowId, int seasonId);

        /// <summary>
        /// Gets the additional info for the tv show episode identified by <paramref name="episodeName"/>
        /// </summary>
        /// <param name="mediaLibrary">The library containing the tv show containing the episode whose details are get</param>
        /// <param name="tvShowId">The id of the tv show containing the episode whose details are get</param>
        /// <param name="seasonId">The id of the season containing the episode whose details are get</param>
        /// <param name="episodeName">The name of the episode whose details are get</param>
        void GetEpisodeMediaInfo(IMediaLibrary mediaLibrary, int tvShowId, int seasonId, string episodeName);

        /// <summary>
        /// Gets the additional info for the movie identified by <paramref name="movieId"/>
        /// </summary>
        /// <param name="mediaLibrary">The library containing the tv show whose details are get</param>
        /// <param name="movieId">The id of the movie whose details are get</param>
        void GetMovieMediaInfo(IMediaLibrary mediaLibrary, int movieId);

        /// <summary>
        /// Gets the additional info for the artist identified by <paramref name="artistId"/>
        /// </summary>
        /// <param name="mediaLibrary">The library containing the artist whose details are get</param>
        /// <param name="artistId">The id of the artist whose details are get</param>
        void GetArtistMediaInfo(IMediaLibrary mediaLibrary, int artistId);

        /// <summary>
        /// Gets the additional info for the artist album identified by <paramref name="albumId"/>
        /// </summary>
        /// <param name="mediaLibrary">The library containing the tv show's season whose details are get</param>
        /// <param name="artistId">The id of the artist containing the album whose details are get</param>
        /// <param name="albumId">The id of the album whose details are get</param>
        void GetAlbumMediaInfo(IMediaLibrary mediaLibrary, int artistId, int albumId);

        /// <summary>
        /// Gets the additional info for the artist song identified by <paramref name="songName"/>
        /// </summary>
        /// <param name="mediaLibrary">The library containing the artist containing the song whose details are get</param>
        /// <param name="artistId">The id of the artist containing the song whose details are get</param>
        /// <param name="albumId">The id of the album containing the song whose details are get</param>
        /// <param name="songName">The name of the song whose details are get</param>
        void GetSongMediaInfo(IMediaLibrary mediaLibrary, int artistId, int albumId, string songName);

        /// <summary>
        /// Navigates one level up in media library
        /// </summary>
        /// <param name="mediaLibrary">The media library to be navigated</param>
        /// <param name="mediaId">The id of the element from which to navigate one level down</param>
        void NavigateUp(IMediaLibrary mediaLibrary, int mediaId);

        /// <summary>
        /// Navigates one level down in media library
        /// </summary>
        /// <param name="mediaLibrary">The media library to be navigated</param>
        /// <param name="media">The element from which to navigate one level down</param>
        Task NavigateDownAsync(IMediaLibrary mediaLibrary, IMediaEntity media);

        /// <summary>
        /// Gets the list of tv shows to be displayed in the navigation list
        /// </summary>
        /// <param name="mediaLibrary">The library from which to take the tv shows to be displayed</param>
        IEnumerable<IMediaEntity> GetTvShowsNavigationList(IMediaLibrary mediaLibrary, Func<IMediaEntity> mediaFactory, string mediaName = null);

        /// <summary>
        /// Gets the list of seasons to be displayed in the navigation list
        /// </summary>
        /// <param name="mediaLibrary">The library from which to take the seasons to be displayed</param>
        /// <param name="fromEpisode">An episode in the current navigation list from which to take the parent seasons</param>
        IEnumerable<IMediaEntity> GetSeasonsNavigationListFromEpisode(IMediaLibrary mediaLibrary, IMediaEntity fromEpisode, Func<IMediaEntity> mediaFactory);

        /// <summary>
        /// Gets the list of seasons to be displayed in the navigation list
        /// </summary>
        /// <param name="mediaLibrary">The library from which to take the seasons to be displayed</param>
        /// <param name="fromTvShow">A tv show in the current navigation list from which to take the child seasons</param>
        IEnumerable<IMediaEntity> GetSeasonsNavigationListFromTvShow(IMediaLibrary mediaLibrary, IMediaEntity fromTvShow, Func<IMediaEntity> mediaFactory);

        /// <summary>
        /// Gets the list of episodes to be displayed in the navigation list
        /// </summary>
        /// <param name="mediaLibrary">The library from which to take the episodes to be displayed</param>
        /// <param name="fromSeason">A season in the current navigation list from which to take the child episodes</param>
        IEnumerable<IMediaEntity> GetEpisodesNavigationList(IMediaLibrary mediaLibrary, IMediaEntity fromSeason, Func<IMediaEntity> mediaFactory);
        
        /// <summary>
        /// Gets the list of movies to be displayed in the navigation list
        /// </summary>
        /// <param name="mediaLibrary">The library from which to take the movies to be displayed</param>
        IEnumerable<IMediaEntity> GetMoviesNavigationList(IMediaLibrary mediaLibrary, Func<IMediaEntity> mediaFactory, string mediaName = null);

        /// <summary>
        /// Gets the list of artists to be displayed in the navigation list
        /// </summary>
        /// <param name="mediaLibrary">The library from which to take the artists to be displayed</param>
        IEnumerable<IMediaEntity> GetArtistsNavigationList(IMediaLibrary mediaLibrary, Func<IMediaEntity> mediaFactory, string mediaName = null);

        /// <summary>
        /// Gets the list of albums to be displayed in the navigation list
        /// </summary>
        /// <param name="mediaLibrary">The library from which to take the albums to be displayed</param>
        /// <param name="fromSong">A song in the current navigation list from which to take the parent albums</param>
        IEnumerable<IMediaEntity> GetAlbumsNavigationListFromSong(IMediaLibrary mediaLibrary, IMediaEntity fromSong, Func<IMediaEntity> mediaFactory);

        /// <summary>
        /// Gets the list of albums to be displayed in the navigation list
        /// </summary>
        /// <param name="mediaLibrary">The library from which to take the albums to be displayed</param>
        /// <param name="fromArtist">An artist in the current navigation list from which to take the child albums</param>
        IEnumerable<IMediaEntity> GetAlbumsNavigationListFromArtist(IMediaLibrary mediaLibrary, IMediaEntity fromArtist, Func<IMediaEntity> mediaFactory);

        /// <summary>
        /// Gets the list of songs to be displayed in the navigation list
        /// </summary>
        /// <param name="mediaLibrary">The library from which to take the songs to be displayed</param>
        /// <param name="fromAlbum">An album in the current navigation list from which to take the child songs</param>
        IEnumerable<IMediaEntity> GetSongsNavigationList(IMediaLibrary mediaLibrary, IMediaEntity fromAlbum, Func<IMediaEntity> mediaFactory);

        /// <summary>
        /// Navigates to the episode media list from an episode advanced search result
        /// </summary>
        /// <param name="mediaLibrary">The library from which to take the episodes to be displayed</param>
        /// <param name="mediaFactory">The factory for creating new instances of type <see cref="IMediaEntity"/></param>
        /// <param name="searchEntity">The advanced search entity that initiated the navigation</param>
        /// <returns>An IEnumerable of media items representing the episodes containing the <paramref name="searchEntity"/></returns>
        IEnumerable<IMediaEntity> NavigateToEpisodeSearchResult(IMediaLibrary mediaLibrary, Func<IMediaEntity> mediaFactory, AdvancedSearchResultEntity searchEntity);

        /// <summary>
        /// Navigates to the season media list from a season advanced search result
        /// </summary>
        /// <param name="mediaLibrary">The library from which to take the seasons to be displayed</param>
        /// <param name="mediaFactory">The factory for creating new instances of type <see cref="IMediaEntity"/></param>
        /// <param name="searchEntity">The advanced search entity that initiated the navigation</param>
        /// <returns>An IEnumerable of media items representing the seasons containing the <paramref name="searchEntity"/></returns>
        IEnumerable<IMediaEntity> NavigateToSeasonSearchResult(IMediaLibrary mediaLibrary, Func<IMediaEntity> mediaFactory, AdvancedSearchResultEntity searchEntity);

        /// <summary>
        /// Navigates to the tv show media list from a tv show advanced search result
        /// </summary>
        /// <param name="mediaLibrary">The library from which to take the tv shows to be displayed</param>
        /// <param name="mediaFactory">The factory for creating new instances of type <see cref="IMediaEntity"/></param>
        /// <param name="searchEntity">The advanced search entity that initiated the navigation</param>
        /// <returns>An IEnumerable of media items representing the tv shows containing the <paramref name="searchEntity"/></returns>
        IEnumerable<IMediaEntity> NavigateToTvShowSearchResult(IMediaLibrary mediaLibrary, Func<IMediaEntity> mediaFactory, AdvancedSearchResultEntity searchEntity);

        /// <summary>
        /// Navigates to the song media list from a song advanced search result
        /// </summary>
        /// <param name="mediaLibrary">The library from which to take the songs to be displayed</param>
        /// <param name="mediaFactory">The factory for creating new instances of type <see cref="IMediaEntity"/></param>
        /// <param name="searchEntity">The advanced search entity that initiated the navigation</param>
        /// <returns>An IEnumerable of media items representing the songs containing the <paramref name="searchEntity"/></returns>
        IEnumerable<IMediaEntity> NavigateToSongSearchResult(IMediaLibrary mediaLibrary, Func<IMediaEntity> mediaFactory, AdvancedSearchResultEntity searchEntity);

        /// <summary>
        /// Navigates to the album media list from an album advanced search result
        /// </summary>
        /// <param name="mediaLibrary">The library from which to take the albums to be displayed</param>
        /// <param name="mediaFactory">The factory for creating new instances of type <see cref="IMediaEntity"/></param>
        /// <param name="searchEntity">The advanced search entity that initiated the navigation</param>
        /// <returns>An IEnumerable of media items representing the albums containing the <paramref name="searchEntity"/></returns>
        IEnumerable<IMediaEntity> NavigateToAlbumSearchResult(IMediaLibrary mediaLibrary, Func<IMediaEntity> mediaFactory, AdvancedSearchResultEntity searchEntity);

        /// <summary>
        /// Navigates to the artist media list from an artist advanced search result
        /// </summary>
        /// <param name="mediaLibrary">The library from which to take the artists to be displayed</param>
        /// <param name="mediaFactory">The factory for creating new instances of type <see cref="IMediaEntity"/></param>
        /// <param name="searchEntity">The advanced search entity that initiated the navigation</param>
        /// <returns>An IEnumerable of media items representing the artists containing the <paramref name="searchEntity"/></returns>
        IEnumerable<IMediaEntity> NavigateToArtistSearchResult(IMediaLibrary mediaLibrary, Func<IMediaEntity> mediaFactory, AdvancedSearchResultEntity searchEntity);

        /// <summary>
        /// Navigates to the movie media list from a movie advanced search result
        /// </summary>
        /// <param name="mediaLibrary">The library from which to take the movies to be displayed</param>
        /// <param name="mediaFactory">The factory for creating new instances of type <see cref="IMediaEntity"/></param>
        /// <param name="searchEntity">The advanced search entity that initiated the navigation</param>
        /// <returns>An IEnumerable of media items representing the movies containing the <paramref name="searchEntity"/></returns>
        IEnumerable<IMediaEntity> NavigateToMovieSearchResult(IMediaLibrary mediaLibrary, Func<IMediaEntity> mediaFactory, AdvancedSearchResultEntity searchEntity);
        #endregion
    }
}
