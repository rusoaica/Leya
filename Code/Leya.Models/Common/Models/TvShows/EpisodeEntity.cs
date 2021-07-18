/// Written by: Yulia Danilova
/// Creation Date: 22nd of November, 2020
/// Purpose: Data transfer object for the episodes 
#region ========================================================================= USING =====================================================================================
using System;
#endregion

namespace Leya.Models.Common.Models.TvShows
{
    public class EpisodeEntity
    {
        #region ================================================================ PROPERTIES =================================================================================
        public int Id { get; set; }
        public int TvDbId { get; set; }
        public int TmDbId { get; set; }
        public int Runtime { get; set; }
        public int Episode { get; set; }
        public int TvShowId { get; set; }
        public int SeasonId { get; set; }
        public string Year { get; set; }
        public string MPAA { get; set; }
        public string Title { get; set; }
        public string ImDbId { get; set; }
        public string Director { get; set; }
        public string Synopsis { get; set; }
        public string NamedTitle { get; set; }
        public EpisodeGenreEntity[] Genre { get; set; }
        public EpisodeActorEntity[] Actors { get; set; }
        public EpisodeCreditEntity[] Credits { get; set; }
        public EpisodeRatingEntity[] Ratings { get; set; }
        public bool IsWatched { get; set; }
        public bool IsFavorite { get; set; }
        public DateTime Aired { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastPlayed { get; set; }
        #endregion

        #region ================================================================= METHODS ===================================================================================
        /// <summary>
        /// Customized ToString() method
        /// </summary>
        /// <returns>Custom string value showing relevant data for current class</returns>
        public override string ToString()
        {
            return Id + " :: " + Title;
        }
        #endregion
    }
}
