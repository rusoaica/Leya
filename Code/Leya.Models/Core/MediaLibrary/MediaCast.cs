/// Written by: Yulia Danilova
/// Creation Date: 29th of August, 2021
/// Purpose: Business model for media library cast
#region ========================================================================= USING =====================================================================================
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Leya.Models.Common.Models.Media;
using Leya.Models.Common.Broadcasting;
#endregion

namespace Leya.Models.Core.MediaLibrary
{
    public class MediaCast : NotifyPropertyChanged, IMediaCast
    {
        #region ================================================================ PROPERTIES =================================================================================
        private bool areActorsVisible = false;
        public bool AreActorsVisible
        {
            get { return areActorsVisible; }
            set { areActorsVisible = value; Notify(); }
        }
        #endregion

        #region ================================================================= METHODS ===================================================================================
        /// <summary>
        /// Gets the list of actors of an episode
        /// </summary>
        /// <param name="mediaLibrary">The library from which to take the episode for which to get the list of actors</param>
        /// <param name="media">The navigation media entity containing the id of the episode for which to get the list of actors</param>
        /// <returns>An enumeration of actors of an episode</returns>
        public IEnumerable<CastEntity> ShowEpisodeMediaCastAsync(IMediaLibrary mediaLibrary, IMediaEntity media)
        {
            var tvShow = mediaLibrary.Library.TvShows.Where(t => t.Id == media.Id)
                                                     .First();
            var episode = mediaLibrary.Library.TvShows.SelectMany(t => t.Seasons)
                                                      .SelectMany(s => s.Episodes)
                                                      .Where(e => e.Id == media.EpisodeOrSongId)
                                                      .First();
            var mediaTypeSourceId = mediaLibrary.Library.MediaTypes.SelectMany(mt => mt.MediaTypeSources)
                                                                   .Where(mts => mts.Id == tvShow.MediaTypeSourceId)
                                                                   .FirstOrDefault();
            foreach (var actor in episode.Actors.GroupBy(a => a.Name)
                                                .Select(a => a.First())
                                                .OrderBy(a => a.Order))
            {
                yield return new CastEntity()
                {
                    Name = actor.Name,
                    Role = actor.Role,
                    Order = actor.Order,
                    Thumb = string.IsNullOrEmpty(actor.Thumb) ?
                                @"avares://Leya/Assets/no_actor.jpg" :
                                mediaTypeSourceId.MediaSourcePath + Path.DirectorySeparatorChar + ".actors" + Path.DirectorySeparatorChar + actor.Thumb
                };
            }
        }

        /// <summary>
        /// Gets the list of actors of a tv show episodes
        /// </summary>
        /// <param name="mediaLibrary">The library from which to take the tv show episodes for which to get the list of actors</param>
        /// <param name="media">The navigation media entity containing the id of the tv show for which to get the list of actors</param>
        /// <returns>An enumeration of actors of a tv show</returns>
        public IEnumerable<CastEntity> ShowTvShowMediaCastAsync(IMediaLibrary mediaLibrary, IMediaEntity media)
        {
            var tvShow = mediaLibrary.Library.TvShows.Where(t => t.Id == media.Id)
                                                     .First();
            var episodes = mediaLibrary.Library.TvShows.SelectMany(t => t.Seasons)
                                                       .SelectMany(s => s.Episodes)
                                                       .Where(e => e.TvShowId == tvShow.Id);
            var mediaTypeSourceId = mediaLibrary.Library.MediaTypes.SelectMany(mt => mt.MediaTypeSources)
                                                                   .Where(mts => mts.Id == tvShow.MediaTypeSourceId)
                                                                   .FirstOrDefault();
            // create a list of all actors of all episodes of the tv show, without duplicates
            foreach (var actor in episodes.SelectMany(e => e.Actors)
                                          .GroupBy(a => a.Name)
                                          .Select(a => a.First())
                                          .OrderBy(a => a.Order))
            {
                yield return new CastEntity()
                {
                    Name = actor.Name,
                    Role = actor.Role,
                    Order = actor.Order,
                    Thumb = string.IsNullOrEmpty(actor.Thumb) ?
                                @"avares://Leya/Assets/no_actor.jpg" :
                                mediaTypeSourceId.MediaSourcePath + Path.DirectorySeparatorChar + ".actors" + Path.DirectorySeparatorChar + actor.Thumb
                };
            }
        }

        /// <summary>
        /// Gets the list of actors of a movie
        /// </summary>
        /// <param name="mediaLibrary">The library from which to take the movie for which to get the list of actors</param>
        /// <param name="media">The navigation media entity containing the id of the movie for which to get the list of actors</param>
        /// <returns>An enumeration of actors of a movie</returns>
        public IEnumerable<CastEntity> ShowMovieMediaCastAsync(IMediaLibrary mediaLibrary, IMediaEntity media)
        {
            var movie = mediaLibrary.Library.Movies.Where(m => m.Id == media.Id)
                                                   .First();
            var mediaTypeSourceId = mediaLibrary.Library.MediaTypes.SelectMany(mt => mt.MediaTypeSources)
                                                                   .Where(mts => mts.Id == movie.MediaTypeSourceId)
                                                                   .FirstOrDefault();
            foreach (var actor in movie.Actors.GroupBy(a => a.Name)
                                              .Select(a => a.First())
                                              .OrderBy(a => a.Order))
            {
                yield return new CastEntity()
                {
                    Name = actor.Name,
                    Role = actor.Role,
                    Order = actor.Order,
                    Thumb = string.IsNullOrEmpty(actor.Thumb) ?
                                @"avares://Leya/Assets/no_actor.jpg" : 
                                mediaTypeSourceId.MediaSourcePath + Path.DirectorySeparatorChar + ".actors" + Path.DirectorySeparatorChar + actor.Thumb
                };
            }
        }
        #endregion
    }
}
