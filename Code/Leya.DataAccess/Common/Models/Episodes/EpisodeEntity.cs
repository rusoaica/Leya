/// Written by: Yulia Danilova
/// Creation Date: 22nd of November, 2020
/// Purpose: Deserialization model for the Episode storage container
#region ========================================================================= USING =====================================================================================
using System;
using Leya.DataAccess.StorageAccess;
using Leya.DataAccess.Common.Attributes;
#endregion

namespace Leya.DataAccess.Common.Models.Episodes
{
    public sealed class EpisodeEntity : IStorageEntity
    {
        #region ================================================================ PROPERTIES =================================================================================
        [IgnoreOnInsert]
        public int Id { get; set; }
        public int TvDbId { get; set; }
        public int TmDbId { get; set; }
        public int Episode { get; set; }
        public int Runtime { get; set; }
        public int TvShowId { get; set; }
        public int SeasonId { get; set; }
        public string MPAA { get; set; }
        public string Year { get; set; }
        public string Title { get; set; }
        public string ImDbId { get; set; }
        public string Synopsis { get; set; }
        public string Director { get; set; }
        public string NamedTitle { get; set; }
        [IgnoreOnInsert]
        public EpisodeRatingEntity[] Ratings { get; set; }
        [IgnoreOnInsert]
        public EpisodeGenreEntity[] Genre { get; set; }
        [IgnoreOnInsert]
        public EpisodeCreditEntity[] Credits { get; set; }
        public bool IsWatched { get; set; }
        public bool IsFavorite { get; set; }
        [IgnoreOnInsert]
        public EpisodeActorEntity[] Actors { get; set; }
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
            return Id + " :: " + Title;
        }
        #endregion
    }
}
