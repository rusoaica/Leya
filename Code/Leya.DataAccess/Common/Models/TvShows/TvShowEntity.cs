/// Written by: Yulia Danilova
/// Creation Date: 20th of November, 2020
/// Purpose: Deserialization model for the TvShows storage container
#region ========================================================================= USING =====================================================================================
using System;
using Leya.DataAccess.StorageAccess;
using Leya.DataAccess.Common.Attributes;
using Leya.DataAccess.Common.Models.Seasons;
#endregion

namespace Leya.DataAccess.Common.Models.TvShows
{
    public sealed class TvShowEntity : IStorageEntity
    {
        #region ================================================================ PROPERTIES =================================================================================
        [IgnoreOnInsert]
        public int Id { get; set; }
        public int TvDbId { get; set; }
        public int TmDbId { get; set; }
        public int Runtime { get; set; }
        public int MediaTypeId { get; set; }
        public int NumberOfSeasons { get; set; }
        public int NumberOfEpisodes { get; set; }
        public int MediaTypeSourceId { get; set; }
        public string MPAA { get; set; }
        public string ImDbId { get; set; }
        public string Studio { get; set; }
        public string Trailer { get; set; }
        public string TagLine { get; set; }
        public string Synopsis { get; set; }
        public string TvShowTitle { get; set; }
        public string TvShowNamedTitle { get; set; }
        [IgnoreOnInsert]
        public TvShowRatingEntity[] Ratings { get; set; }
        [IgnoreOnInsert]
        public TvShowGenreEntity[] Genre { get; set; }
        public bool IsEnded { get; set; }
        public bool IsWatched { get; set; }
        public bool IsFavorite { get; set; }
        [IgnoreOnInsert]
        public TvShowActorEntity[] Actors { get; set; }
        [IgnoreOnInsert]
        public SeasonEntity[] Seasons { get; set; }
        [IgnoreOnInsert]
        public TvShowResumeEntity Resume { get; set; }
        public DateTime Aired { get; set; }
        [IgnoreOnInsert]
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
            return Id + " :: " + TvShowTitle;
        }
        #endregion  
    }
}
