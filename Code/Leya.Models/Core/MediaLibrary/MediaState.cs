/// Written by: Yulia Danilova
/// Creation Date: 18th of July, 2021
/// Purpose: Business model for media library common states
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
    public class MediaState : IMediaState
    {
        #region ============================================================== FIELD MEMBERS ================================================================================
        private readonly ISong song;
        private readonly IMovie movie;
        private readonly IAlbum album;
        private readonly ISeason season;
        private readonly ITvShow tvShow;
        private readonly IArtist artist;
        private readonly IEpisode episode;
        #endregion

        #region ================================================================== CTOR =====================================================================================
        /// <summary>
        /// Overload C-tor
        /// </summary>
        /// <param name="episode">Injected episode business model</param>
        /// <param name="season">Injected season business model</param>
        /// <param name="tvShow">Injected tv show business model</param>
        /// <param name="movie">Injected movie business model</param>
        /// <param name="song">Injected song business model</param>
        /// <param name="album">Injected album business model</param>
        /// <param name="artist">Injected artist business model</param>
        public MediaState(IEpisode episode, ISeason season, ITvShow tvShow, IMovie movie, ISong song, IAlbum album, IArtist artist)
        {
            this.episode = episode;
            this.season = season;
            this.tvShow = tvShow;
            this.movie = movie;
            this.song = song;
            this.album = album;
            this.artist = artist;
        }
        #endregion

        #region ================================================================= METHODS ===================================================================================
        /// <summary>
        /// Updates the IsWatched status for <paramref name="media"/>
        /// </summary>
        /// <param name="mediaLibrary">The media library containing the episode whose IsWatched status is updated</param>
        /// <param name="media">The media item for which to update the IsWatched status</param>
        public async Task SetEpisodeIsWatchedStatusAsync(IMediaLibrary mediaLibrary, IMediaEntity media)
        {
            // get the info of the episode
            EpisodeEntity episode = mediaLibrary.Library.TvShows.SelectMany(t => t.Seasons)
                                                                .SelectMany(s => s.Episodes)
                                                                .Where(e => e.Id == media.EpisodeOrSongId)
                                                                .First();
            // mark its watched status and update it in the repository
            episode.IsWatched = media.IsWatched;
            await this.episode.UpdateIsWatchedStatusAsync(media.EpisodeOrSongId, media.IsWatched);
            // check if all the episodes in the season of the current episode are watched, and if so, mark the season itself as watched
            SeasonEntity season = mediaLibrary.Library.TvShows.SelectMany(t => t.Seasons)
                                                              .Where(s => s.Id == media.SeasonOrAlbumId)
                                                              .First();
            if (season.Episodes.All(e => e.IsWatched))
            {
                await this.season.UpdateIsWatchedStatusAsync(media.SeasonOrAlbumId, true);
                season.IsWatched = true;
            }
            else
            {
                // not all episodes in the season are watched, the season itself cannot be marked as watched
                await this.season.UpdateIsWatchedStatusAsync(media.SeasonOrAlbumId, false);
                season.IsWatched = false;
            }
            // check if all the seasons in the tv show of the current episode are watched, and if so, mark the tv show itself as watched
            TvShowEntity tvShow = mediaLibrary.Library.TvShows.Where(t => t.Id == media.Id)
                                                              .First();
            if (tvShow.Seasons.All(t => t.IsWatched))
            {
                await this.tvShow.UpdateIsWatchedStatusAsync(media.Id, media.IsWatched);
                tvShow.IsWatched = true;
            }
            else
            {
                // not all seasons in the tv show are watched, the tv show itself cannot be marked as watched
                await this.tvShow.UpdateIsWatchedStatusAsync(media.Id, media.IsWatched);
                tvShow.IsWatched = false;
            }

            //// update number of unwatched episodes
            //NumberOfUnwatchedEpisodes = mediaLibrary.Episodes.Where(e => e.IsWatched == false && e.NamedSeasonId == media.SeasonOrAlbumId && e.TvShowId == media.Id).Count();
        }

        /// <summary>
        /// Updates the IsWatched status for <paramref name="media"/>
        /// </summary>
        /// <param name="mediaLibrary">The media library containing the season whose IsWatched status is updated</param>
        /// <param name="media">The media item for which to update the IsWatched status</param>
        public async Task SetSeasonIsWatchedStatusAsync(IMediaLibrary mediaLibrary, IMediaEntity media)
        {
            // update the IsWatched status of all the episodes in the season the same as the season's IsWatched status
            foreach (EpisodeEntity episode in mediaLibrary.Library.TvShows.SelectMany(t => t.Seasons)
                                                                          .SelectMany(s => s.Episodes)
                                                                          .Where(e => e.SeasonId == media.SeasonOrAlbumId && e.TvShowId == media.Id))
            {
                // mark its watched status according to the UI watch status
                episode.IsWatched = media.IsWatched;
                // update the watched status of the episodes of the season
                await this.episode.UpdateIsWatchedStatusAsync(episode.Id, episode.IsWatched);
            }
            // update the watched status of the season
            SeasonEntity season = mediaLibrary.Library.TvShows.SelectMany(t => t.Seasons)
                                                              .Where(s => s.Id == media.SeasonOrAlbumId)
                                                              .First();
            season.IsWatched = media.IsWatched;
            await this.season.UpdateIsWatchedStatusAsync(season.Id, season.IsWatched);
            // check if all the seasons in the tv show of the current season are watched, and if so, mark the tv show itself as watched
            TvShowEntity tvShow = mediaLibrary.Library.TvShows.Where(t => t.Id == media.Id)
                                                              .First();
            if (tvShow.Seasons.All(t => t.IsWatched))
            {
                await this.tvShow.UpdateIsWatchedStatusAsync(media.Id, media.IsWatched);
                tvShow.IsWatched = true;
            }
            else
            {
                // not all seasons in the tv show are watched, the tv show itself cannot be marked as watched
                await this.tvShow.UpdateIsWatchedStatusAsync(media.Id, media.IsWatched);
                tvShow.IsWatched = false;
            }
            //// update number of unwatched episodes
            //NumberOfUnwatchedEpisodes = mediaLibrary.Episodes.Where(e => e.IsWatched == false && e.NamedSeasonId == media.SeasonOrAlbumId && e.TvShowId == media.Id).Count();
        }

        /// <summary>
        /// Updates the IsWatched status for <paramref name="media"/>
        /// </summary>
        /// <param name="mediaLibrary">The media library containing the tv show whose IsWatched status is updated</param>
        /// <param name="media">The media item for which to update the IsWatched status</param>
        public async Task SetTvShowIsWatchedStatusAsync(IMediaLibrary mediaLibrary, IMediaEntity media)
        {
            // update the watched status of the seasons of the tv show
            foreach (SeasonEntity season in mediaLibrary.Library.TvShows.Where(t => t.Id == media.Id)
                                                                        .First().Seasons)
            {
                season.IsWatched = media.IsWatched;
                // update the watched status of the seasons of the tv show
                await this.season.UpdateIsWatchedStatusAsync(season.Id, season.IsWatched);
                // update the watched status of the episodes of the season
                foreach (EpisodeEntity episode in mediaLibrary.Library.TvShows.SelectMany(t => t.Seasons)
                                                                              .SelectMany(s => s.Episodes)
                                                                              .Where(e => e.SeasonId == season.Id && e.TvShowId == media.Id))
                {
                    episode.IsWatched = media.IsWatched;
                    await this.episode.UpdateIsWatchedStatusAsync(episode.Id, episode.IsWatched);
                }
            }
            // update the watched status of the tv show
            TvShowEntity tvShow = mediaLibrary.Library.TvShows.Where(t => t.Id == media.Id)
                                                              .First();
            tvShow.IsWatched = media.IsWatched;
            await this.tvShow.UpdateIsWatchedStatusAsync(tvShow.Id, tvShow.IsWatched);
            // update number of unwatched episodes
            //NumberOfUnwatchedEpisodes = mediaLibrary.Episodes.Where(e => e.IsWatched == false && e.TvShowId == tvShow.Id).Count();
        }

        /// <summary>
        /// Updates the IsWatched status for <paramref name="media"/>
        /// </summary>
        /// <param name="mediaLibrary">The media library containing the movie whose IsWatched status is updated</param>
        /// <param name="media">The media item for which to update the IsWatched status</param>
        public async Task SetMovieIsWatchedStatusAsync(IMediaLibrary mediaLibrary, IMediaEntity media)
        {
            MovieEntity movie = mediaLibrary.Library.Movies.Where(m => m.Id == media.Id)
                                                           .First();
            movie.IsWatched = media.IsWatched;
            // update the watched status of the movie
            await this.movie.UpdateIsWatchedStatusAsync(movie.Id, movie.IsWatched);
        }

        /// <summary>
        /// Updates the IsListened status for <paramref name="media"/>
        /// </summary>
        /// <param name="mediaLibrary">The media library containing the song whose IsListened status is updated</param>
        /// <param name="media">The media item for which to update the IsListened status</param>
        public async Task SetSongIsListenedStatusAsync(IMediaLibrary mediaLibrary, IMediaEntity media)
        {
            // get the info of the song
            SongEntity song = mediaLibrary.Library.Artists.SelectMany(a => a.Albums)
                                                          .SelectMany(a => a.Songs)
                                                          .Where(s => s.Id == media.EpisodeOrSongId)
                                                          .First();
            // mark its listened status and update it in the repository
            song.IsListened = media.IsWatched;
            await this.song.UpdateIsListenedStatusAsync(media.EpisodeOrSongId, media.IsWatched);
            // check if all the songs in the album of the current song are listened, and if so, mark the album itself as listened
            AlbumEntity album = mediaLibrary.Library.Artists.SelectMany(a => a.Albums)
                                                            .Where(a => a.Id == media.SeasonOrAlbumId)
                                                            .First();
            if (album.Songs.All(s => s.IsListened))
            {
                await this.album.UpdateIsListenedStatusAsync(media.SeasonOrAlbumId, true);
                album.IsListened = true;
            }
            else
            {
                // not all songs in the album are listened, the album itself cannot be marked as listened
                await this.album.UpdateIsListenedStatusAsync(media.SeasonOrAlbumId, false);
                album.IsListened = false;
            }
            // check if all the albums of the artist of the current song are listened, and if so, mark the artist itself as listened
            ArtistEntity artist = mediaLibrary.Library.Artists.Where(a => a.Id == media.Id)
                                                              .First();
            if (artist.Albums.All(a => a.IsListened))
            {
                await this.artist.UpdateIsListenedStatusAsync(media.Id, media.IsWatched);
                artist.IsListened = true;
            }
            else
            {
                // not all albums of the artist are listened, the artist itself cannot be marked as listened
                await this.artist.UpdateIsListenedStatusAsync(media.Id, media.IsWatched);
                artist.IsListened = false;
            }

            //// update number of unwatched episodes
            //NumberOfUnwatchedEpisodes = mediaLibrary.Episodes.Where(e => e.IsWatched == false && e.NamedSeasonId == media.SeasonOrAlbumId && e.TvShowId == media.Id).Count();
        }

        /// <summary>
        /// Updates the IsListened status for <paramref name="media"/>
        /// </summary>
        /// <param name="mediaLibrary">The media library containing the album whose IsListened status is updated</param>
        /// <param name="media">The media item for which to update the IsListened status</param>
        public async Task SetAlbumIsListenedStatusAsync(IMediaLibrary mediaLibrary, IMediaEntity media)
        {
            // update the IsListened status of all the songs in the album the same as the album's IsListened status
            foreach (SongEntity song in mediaLibrary.Library.Artists.SelectMany(a => a.Albums)
                                                                    .SelectMany(a => a.Songs)
                                                                    .Where(s => s.AlbumId == media.SeasonOrAlbumId && s.ArtistId == media.Id))
            {
                // mark its listened status according to the UI watch status
                song.IsListened = media.IsWatched;
                // update the listened status of the songs of the album
                await this.song.UpdateIsListenedStatusAsync(song.Id, song.IsListened);
            }
            // update the listened status of the album
            AlbumEntity album = mediaLibrary.Library.Artists.SelectMany(a => a.Albums)
                                                            .Where(a => a.Id == media.SeasonOrAlbumId)
                                                            .First();
            album.IsListened = media.IsWatched;
            await this.album.UpdateIsListenedStatusAsync(album.Id, album.IsListened);
            // check if all the albums of the artist of the current album are listened, and if so, mark the artist itself as listened
            ArtistEntity artist = mediaLibrary.Library.Artists.Where(a => a.Id == media.Id)
                                                              .First();
            if (artist.Albums.All(a => a.IsListened))
            {
                await this.artist.UpdateIsListenedStatusAsync(media.Id, media.IsWatched);
                artist.IsListened = true;
            }
            else
            {
                // not all albums of the artist are listened, the artist itself cannot be marked as listened
                await this.artist.UpdateIsListenedStatusAsync(media.Id, media.IsWatched);
                artist.IsListened = false;
            }
            //// update number of unwatched episodes
            //NumberOfUnwatchedEpisodes = mediaLibrary.Episodes.Where(e => e.IsWatched == false && e.NamedSeasonId == media.SeasonOrAlbumId && e.TvShowId == media.Id).Count();
        }

        /// <summary>
        /// Updates the IsListened status for <paramref name="media"/>
        /// </summary>
        /// <param name="mediaLibrary">The media library containing the artist whose IsListened status is updated</param>
        /// <param name="media">The media item for which to update the IsListened status</param>
        public async Task SetArtistIsListenedStatusAsync(IMediaLibrary mediaLibrary, IMediaEntity media)
        {
            // update the listened status of the albums of the artist
            foreach (AlbumEntity album in mediaLibrary.Library.Artists.Where(a => a.Id == media.Id)
                                                                      .First().Albums)
            {
                album.IsListened = media.IsWatched;
                // update the listened status of the albums of the artist
                await this.album.UpdateIsListenedStatusAsync(album.Id, album.IsListened);
                // update the listened status of the songs of the album
                foreach (SongEntity song in mediaLibrary.Library.Artists.SelectMany(a => a.Albums)
                                                                        .SelectMany(a => a.Songs)
                                                                        .Where(s => s.AlbumId == album.Id && s.ArtistId == media.Id))
                {
                    song.IsListened = media.IsWatched;
                    await this.song.UpdateIsListenedStatusAsync(song.Id, song.IsListened);
                }
            }
            // update the listened status of the artist
            ArtistEntity artist = mediaLibrary.Library.Artists.Where(a => a.Id == media.Id)
                                                              .First();
            artist.IsListened = media.IsWatched;
            await this.artist.UpdateIsListenedStatusAsync(artist.Id, artist.IsListened);
            // update number of unwatched episodes
            //NumberOfUnwatchedEpisodes = mediaLibrary.Episodes.Where(e => e.IsWatched == false && e.TvShowId == tvShow.Id).Count();
        }

        /// <summary>
        /// Updates the IsFavorite status for <paramref name="media"/>
        /// </summary>
        /// <param name="mediaLibrary">The media library containing the episode whose IsFavorite status is updated</param>
        /// <param name="media">The media item for which to update the IsFavorite status</param>
        public async Task SetEpisodeIsFavoriteStatusAsync(IMediaLibrary mediaLibrary, IMediaEntity media)
        {
            // get the info of the episode
            EpisodeEntity episode = mediaLibrary.Library.TvShows.SelectMany(t => t.Seasons)
                                                                .SelectMany(s => s.Episodes)
                                                                .Where(e => e.Id == media.EpisodeOrSongId)
                                                                .First();
            // mark its favorite status and update it in the repository
            episode.IsFavorite = media.IsFavorite;
            await this.episode.UpdateIsFavoriteStatusAsync(media.EpisodeOrSongId, media.IsFavorite);
            // check if all the episodes in the season of the current episode are favorite, and if so, mark the season itself as favorite
            SeasonEntity season = mediaLibrary.Library.TvShows.SelectMany(t => t.Seasons)
                                                              .Where(s => s.Id == media.SeasonOrAlbumId)
                                                              .First();
            if (season.Episodes.All(e => e.IsFavorite))
            {
                await this.season.UpdateIsFavoriteStatusAsync(media.SeasonOrAlbumId, true);
                season.IsFavorite = true;
            }
            else
            {
                // not all episodes in the season are favorite, the season itself cannot be marked as favorite
                await this.season.UpdateIsFavoriteStatusAsync(media.SeasonOrAlbumId, false);
                season.IsFavorite = false;
            }
            // check if all the seasons in the tv show of the current episode are favorite, and if so, mark the tv show itself as favorite
            TvShowEntity tvShow = mediaLibrary.Library.TvShows.Where(t => t.Id == media.Id)
                                                              .First();
            if (tvShow.Seasons.All(t => t.IsFavorite))
            {
                await this.tvShow.UpdateIsFavoriteStatusAsync(media.Id, media.IsFavorite);
                tvShow.IsFavorite = true;
            }
            else
            {
                // not all seasons in the tv show are favorite, the tv show itself cannot be marked as favorite
                await this.tvShow.UpdateIsFavoriteStatusAsync(media.Id, media.IsFavorite);
                tvShow.IsFavorite = false;
            }

            //// update number of unfavorite episodes
            //NumberOfUnfavoriteEpisodes = mediaLibrary.Episodes.Where(e => e.IsFavorite == false && e.NamedSeasonId == media.SeasonOrAlbumId && e.TvShowId == media.Id).Count();
        }

        /// <summary>
        /// Updates the IsFavorite status for <paramref name="media"/>
        /// </summary>
        /// <param name="mediaLibrary">The media library containing the season whose IsFavorite status is updated</param>
        /// <param name="media">The media item for which to update the IsFavorite status</param>
        public async Task SetSeasonIsFavoriteStatusAsync(IMediaLibrary mediaLibrary, IMediaEntity media)
        {
            // update the IsFavorite status of all the episodes in the season the same as the season's IsFavorite status
            foreach (EpisodeEntity episode in mediaLibrary.Library.TvShows.SelectMany(t => t.Seasons)
                                                                          .SelectMany(s => s.Episodes)
                                                                          .Where(e => e.SeasonId == media.SeasonOrAlbumId && e.TvShowId == media.Id))
            {
                // mark its favorite status according to the UI watch status
                episode.IsFavorite = media.IsFavorite;
                // update the favorite status of the episodes of the season
                await this.episode.UpdateIsFavoriteStatusAsync(episode.Id, episode.IsFavorite);
            }
            // update the favorite status of the season
            SeasonEntity season = mediaLibrary.Library.TvShows.SelectMany(t => t.Seasons)
                                                              .Where(s => s.Id == media.SeasonOrAlbumId)
                                                              .First();
            season.IsFavorite = media.IsFavorite;
            await this.season.UpdateIsFavoriteStatusAsync(season.Id, season.IsFavorite);
            // check if all the seasons in the tv show of the current season are favorite, and if so, mark the tv show itself as favorite
            TvShowEntity tvShow = mediaLibrary.Library.TvShows.Where(t => t.Id == media.Id)
                                                              .First();
            if (tvShow.Seasons.All(t => t.IsFavorite))
            {
                await this.tvShow.UpdateIsFavoriteStatusAsync(media.Id, media.IsFavorite);
                tvShow.IsFavorite = true;
            }
            else
            {
                // not all seasons in the tv show are favorite, the tv show itself cannot be marked as favorite
                await this.tvShow.UpdateIsFavoriteStatusAsync(media.Id, media.IsFavorite);
                tvShow.IsFavorite = false;
            }
            //// update number of unfavorite episodes
            //NumberOfUnfavoriteEpisodes = mediaLibrary.Episodes.Where(e => e.IsFavorite == false && e.NamedSeasonId == media.SeasonOrAlbumId && e.TvShowId == media.Id).Count();
        }

        /// <summary>
        /// Updates the IsFavorite status for <paramref name="media"/>
        /// </summary>
        /// <param name="mediaLibrary">The media library containing the tv show whose IsFavorite status is updated</param>
        /// <param name="media">The media item for which to update the IsFavorite status</param>
        public async Task SetTvShowIsFavoriteStatusAsync(IMediaLibrary mediaLibrary, IMediaEntity media)
        {
            // update the favorite status of the seasons of the tv show
            foreach (SeasonEntity season in mediaLibrary.Library.TvShows.Where(t => t.Id == media.Id)
                                                                        .First().Seasons)
            {
                season.IsFavorite = media.IsFavorite;
                // update the favorite status of the seasons of the tv show
                await this.season.UpdateIsFavoriteStatusAsync(season.Id, season.IsFavorite);
                // update the favorite status of the episodes of the season
                foreach (EpisodeEntity episode in mediaLibrary.Library.TvShows.SelectMany(t => t.Seasons)
                                                                              .SelectMany(s => s.Episodes)
                                                                              .Where(e => e.SeasonId == season.Id && e.TvShowId == media.Id))
                {
                    episode.IsFavorite = media.IsFavorite;
                    await this.episode.UpdateIsFavoriteStatusAsync(episode.Id, episode.IsFavorite);
                }
            }
            // update the favorite status of the tv show
            TvShowEntity tvShow = mediaLibrary.Library.TvShows.Where(t => t.Id == media.Id)
                                                              .First();
            tvShow.IsFavorite = media.IsFavorite;
            await this.tvShow.UpdateIsFavoriteStatusAsync(tvShow.Id, tvShow.IsFavorite);
            // update number of unfavorite episodes
            //NumberOfUnfavoriteEpisodes = mediaLibrary.Episodes.Where(e => e.IsFavorite == false && e.TvShowId == tvShow.Id).Count();
        }

        /// <summary>
        /// Updates the IsFavorite status for <paramref name="media"/>
        /// </summary>
        /// <param name="mediaLibrary">The media library containing the movie whose IsFavorite status is updated</param>
        /// <param name="media">The media item for which to update the IsFavorite status</param>
        public async Task SetMovieIsFavoriteStatusAsync(IMediaLibrary mediaLibrary, IMediaEntity media)
        {
            MovieEntity movie = mediaLibrary.Library.Movies.Where(m => m.Id == media.Id)
                                                           .First();
            movie.IsFavorite = media.IsFavorite;
            // update the favorite status of the movie
            await this.movie.UpdateIsFavoriteStatusAsync(movie.Id, movie.IsFavorite);
        }

        /// <summary>
        /// Updates the IsFavorite status for <paramref name="media"/>
        /// </summary>
        /// <param name="mediaLibrary">The media library containing the song whose IsFavorite status is updated</param>
        /// <param name="media">The media item for which to update the IsFavorite status</param>
        public async Task SetSongIsFavoriteStatusAsync(IMediaLibrary mediaLibrary, IMediaEntity media)
        {
            // get the info of the song
            SongEntity song = mediaLibrary.Library.Artists.SelectMany(a => a.Albums)
                                                          .SelectMany(a => a.Songs)
                                                          .Where(s => s.Id == media.EpisodeOrSongId)
                                                          .First();
            // mark its favorite status and update it in the repository
            song.IsFavorite = media.IsFavorite;
            await this.song.UpdateIsFavoriteStatusAsync(media.EpisodeOrSongId, media.IsFavorite);
            // check if all the songs in the album of the current song are favorite, and if so, mark the album itself as favorite
            AlbumEntity album = mediaLibrary.Library.Artists.SelectMany(a => a.Albums)
                                                            .Where(a => a.Id == media.SeasonOrAlbumId)
                                                            .First();
            if (album.Songs.All(s => s.IsFavorite))
            {
                await this.album.UpdateIsFavoriteStatusAsync(media.SeasonOrAlbumId, true);
                album.IsFavorite = true;
            }
            else
            {
                // not all songs in the album are favorite, the album itself cannot be marked as favorite
                await this.album.UpdateIsFavoriteStatusAsync(media.SeasonOrAlbumId, false);
                album.IsFavorite = false;
            }
            // check if all the albums of the artist of the current song are favorite, and if so, mark the artist itself as favorite
            ArtistEntity artist = mediaLibrary.Library.Artists.Where(a => a.Id == media.Id)
                                                              .First();
            if (artist.Albums.All(a => a.IsFavorite))
            {
                await this.artist.UpdateIsFavoriteStatusAsync(media.Id, media.IsFavorite);
                artist.IsFavorite = true;
            }
            else
            {
                // not all albums of the artist are favorite, the artist itself cannot be marked as favorite
                await this.artist.UpdateIsFavoriteStatusAsync(media.Id, media.IsFavorite);
                artist.IsFavorite = false;
            }

            //// update number of unfavorite episodes
            //NumberOfUnfavoriteEpisodes = mediaLibrary.Episodes.Where(e => e.IsFavorite == false && e.NamedSeasonId == media.SeasonOrAlbumId && e.TvShowId == media.Id).Count();
        }

        /// <summary>
        /// Updates the IsFavorite status for <paramref name="media"/>
        /// </summary>
        /// <param name="mediaLibrary">The media library containing the album whose IsFavorite status is updated</param>
        /// <param name="media">The media item for which to update the IsFavorite status</param>
        public async Task SetAlbumIsFavoriteStatusAsync(IMediaLibrary mediaLibrary, IMediaEntity media)
        {
            // update the IsFavorite status of all the songs in the album the same as the album's IsFavorite status
            foreach (SongEntity song in mediaLibrary.Library.Artists.SelectMany(a => a.Albums)
                                                                    .SelectMany(a => a.Songs)
                                                                    .Where(s => s.AlbumId == media.SeasonOrAlbumId && s.ArtistId == media.Id))
            {
                // mark its favorite status according to the UI watch status
                song.IsFavorite = media.IsFavorite;
                // update the favorite status of the songs of the album
                await this.song.UpdateIsFavoriteStatusAsync(song.Id, song.IsFavorite);
            }
            // update the favorite status of the album
            AlbumEntity album = mediaLibrary.Library.Artists.SelectMany(a => a.Albums)
                                                            .Where(a => a.Id == media.SeasonOrAlbumId)
                                                            .First();
            album.IsFavorite = media.IsFavorite;
            await this.album.UpdateIsFavoriteStatusAsync(album.Id, album.IsFavorite);
            // check if all the albums of the artist of the current album are favorite, and if so, mark the artist itself as favorite
            ArtistEntity artist = mediaLibrary.Library.Artists.Where(a => a.Id == media.Id)
                                                              .First();
            if (artist.Albums.All(a => a.IsFavorite))
            {
                await this.artist.UpdateIsFavoriteStatusAsync(media.Id, media.IsFavorite);
                artist.IsFavorite = true;
            }
            else
            {
                // not all albums of the artist are favorite, the artist itself cannot be marked as favorite
                await this.artist.UpdateIsFavoriteStatusAsync(media.Id, media.IsFavorite);
                artist.IsFavorite = false;
            }
            //// update number of unfavorite episodes
            //NumberOfUnfavoriteEpisodes = mediaLibrary.Episodes.Where(e => e.IsFavorite == false && e.NamedSeasonId == media.SeasonOrAlbumId && e.TvShowId == media.Id).Count();
        }

        /// <summary>
        /// Updates the IsFavorite status for <paramref name="media"/>
        /// </summary>
        /// <param name="mediaLibrary">The media library containing the artist whose IsFavorite status is updated</param>
        /// <param name="media">The media item for which to update the IsFavorite status</param>
        public async Task SetArtistIsFavoriteStatusAsync(IMediaLibrary mediaLibrary, IMediaEntity media)
        {
            // update the favorite status of the albums of the artist
            foreach (AlbumEntity album in mediaLibrary.Library.Artists.Where(a => a.Id == media.Id)
                                                                      .First().Albums)
            {
                album.IsFavorite = media.IsFavorite;
                // update the favorite status of the albums of the artist
                await this.album.UpdateIsFavoriteStatusAsync(album.Id, album.IsFavorite);
                // update the favorite status of the songs of the album
                foreach (SongEntity song in mediaLibrary.Library.Artists.SelectMany(a => a.Albums)
                                                                        .SelectMany(a => a.Songs)
                                                                        .Where(s => s.AlbumId == album.Id && s.ArtistId == media.Id))
                {
                    song.IsFavorite = media.IsFavorite;
                    await this.song.UpdateIsFavoriteStatusAsync(song.Id, song.IsFavorite);
                }
            }
            // update the favorite status of the artist
            ArtistEntity artist = mediaLibrary.Library.Artists.Where(a => a.Id == media.Id)
                                                              .First();
            artist.IsFavorite = media.IsFavorite;
            await this.artist.UpdateIsFavoriteStatusAsync(artist.Id, artist.IsFavorite);
            // update number of unfavorite episodes
            //NumberOfUnfavoriteEpisodes = mediaLibrary.Episodes.Where(e => e.IsFavorite == false && e.TvShowId == tvShow.Id).Count();
        }
        #endregion
    }
}
