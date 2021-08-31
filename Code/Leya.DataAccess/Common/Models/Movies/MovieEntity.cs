/// Written by: Yulia Danilova
/// Creation Date: 24th of November, 2020
/// Purpose: Deserialization model for the Movies storage container
#region ========================================================================= USING =====================================================================================
using System;
using Leya.DataAccess.StorageAccess;
using Leya.DataAccess.Common.Attributes;
#endregion

namespace Leya.DataAccess.Common.Models.Movies
{
    public sealed class MovieEntity : IStorageEntity
    {
        #region ================================================================ PROPERTIES =================================================================================
        [IgnoreOnInsert]
        public int Id { get; set; }
        public int TmDbId { get; set; }
        public int Runtime { get; set; }
        public int MediaTypeId { get; set; }
        public int MediaTypeSourceId { get; set; }
        public string Set { get; set; }
        public string MPAA { get; set; }
        public string ImDbId { get; set; }
        public string Studio { get; set; }
        public string Trailer { get; set; }
        public string Tagline { get; set; }
        public string Synopsis { get; set; }
        public string MovieTitle { get; set; }
        public string NamedTitle { get; set; }
        public string TvShowLink { get; set; }
        public string OriginalTitle { get; set; }
        [IgnoreOnInsert]
        public MovieRatingEntity[] Ratings { get; set; }
        [IgnoreOnInsert]
        public MovieGenreEntity[] Genre { get; set; }
        [IgnoreOnInsert]
        public MovieCountryEntity[] Country { get; set; }
        [IgnoreOnInsert]
        public MovieCreditEntity[] Credits { get; set; }
        [IgnoreOnInsert]
        public MovieDirectorEntity[] Director { get; set; }
        [IgnoreOnInsert]
        public MovieTagEntity[] Tags { get; set; }
        public bool IsEnded { get; set; }
        [IgnoreOnInsert]
        public FileInfoEntity FileInfo { get; set; }
        [IgnoreOnInsert]
        public MovieActorEntity[] Actors { get; set; }
        [IgnoreOnInsert]
        public MovieResumeEntity Resume { get; set; }
        public string IsWatched { get; set; } = "False";
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
            return Id + " :: " + MovieTitle;
        }
        #endregion  
    }
}
