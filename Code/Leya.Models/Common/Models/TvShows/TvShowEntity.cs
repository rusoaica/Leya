/// Written by: Yulia Danilova
/// Creation Date: 20th of November, 2020
/// Purpose: Data transfer object for the tv shows
#region ========================================================================= USING =====================================================================================
using System;
using Leya.Models.Common.Models.Media;
using Leya.Models.Common.Infrastructure;
#endregion

namespace Leya.Models.Common.Models.TvShows
{
    public class TvShowEntity : IMedia
    {
        #region ================================================================ PROPERTIES =================================================================================
        public int Id { get; set; }
        public int TvDbId { get; set; }
        public int TmDbId { get; set; }
        public int Runtime { get; set; }
        public int MediaTypeId { get; set; }
        public int NumberOfSeasons { get; set; }
        public int NumberOfEpisodes { get; set; }
        public int MediaTypeSourceId { get; set; }
        public string MPAA { get; set; }
        public string Title { get; set; }
        public string ImDbId { get; set; }
        public string Studio { get; set; }
        public string TagLine { get; set; }
        public string Trailer { get; set; }
        public string Synopsis { get; set; }
        public string NamedTitle { get; set; }
        public bool IsEnded { get; set; }
        public bool? IsWatched { get; set; } = false;
        public bool IsFavorite { get; set; }
        public SeasonEntity[] Seasons { get; set; }
        public TvShowResumeEntity Resume { get; set; }
        public TvShowGenreEntity[] Genres { get; set; }
        public TvShowActorEntity[] Actors { get; set; }
        public TvShowRatingEntity[] Ratings { get; set; }
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

        /// <summary>
        /// Maps between this entity and the coresponding persistance entity
        /// </summary>
        /// <returns>A data storage entity representation of this entity</returns>
        public DataAccess.Common.Models.TvShows.TvShowEntity ToStorageEntity()
        {
            return Services.AutoMapper.Map<DataAccess.Common.Models.TvShows.TvShowEntity>(this);
        }
        #endregion
    }
}
