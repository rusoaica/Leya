/// Written by: Yulia Danilova
/// Creation Date: 23rd of November, 2020
/// Purpose: Data transfer object for the movies
#region ========================================================================= USING =====================================================================================
using Leya.Models.Common.Infrastructure;
using Leya.Models.Common.Models.Media;
using System;
#endregion

namespace Leya.Models.Common.Models.Movies
{
    public class MovieEntity : IMedia
    {
        #region ================================================================ PROPERTIES =================================================================================
        public int Id { get; set; }
        public int TmDbId { get; set; }
        public int Runtime { get; set; }
        public int MediaTypeId { get; set; }
        public int MediaTypeSourceId { get; set; }
        public string Set { get; set; }
        public string MPAA { get; set; }
        public string Title { get; set; }
        public string ImDbId { get; set; }
        public string Studio { get; set; }
        public string Tagline { get; set; }
        public string Trailer { get; set; }
        public string Synopsis { get; set; }
        public string NamedTitle { get; set; }
        public string TvShowLink { get; set; }
        public string OriginalTitle { get; set; }
        public MovieTagEntity[] Tags { get; set; }
        public FileInfoEntity FileInfo { get; set; }
        public MovieGenreEntity[] Genres { get; set; }
        public MovieResumeEntity Resume { get; set; }
        public MovieActorEntity[] Actors { get; set; }
        public MovieCreditEntity[] Credits { get; set; }
        public MovieRatingEntity[] Ratings { get; set; }
        public MovieCountryEntity[] Countries { get; set; }
        public MovieDirectorEntity[] Directors { get; set; }
        public bool IsEnded { get; set; }
        public bool? IsWatched { get; set; } = false;
        public bool IsFavorite { get; set; }
        public DateTime Created { get; set; }
        public DateTime Premiered { get; set; }
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
        public DataAccess.Common.Models.Movies.MovieEntity ToStorageEntity()
        {
            return Services.AutoMapper.Map<DataAccess.Common.Models.Movies.MovieEntity>(this);
        }
        #endregion
    }
}
