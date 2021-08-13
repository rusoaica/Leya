/// Written by: Yulia Danilova
/// Creation Date: 22nd of November, 2020
/// Purpose: Deserialization model for the Seasons storage container
#region ========================================================================= USING =====================================================================================
using System;
using Leya.DataAccess.StorageAccess;
using Leya.DataAccess.Common.Attributes;
#endregion

namespace Leya.DataAccess.Common.Models.Seasons
{
    public sealed class SeasonEntity : IStorageEntity
    {
        #region ================================================================ PROPERTIES =================================================================================
        [IgnoreOnInsert]
        public int Id { get; set; }
        public int Year { get; set; }
        public int TvShowId { get; set; }
        public int SeasonNumber { get; set; }
        public string Synopsis { get; set; }
        public string SeasonName { get; set; }
        public bool IsWatched { get; set; }
        public bool IsFavorite { get; set; }
        public DateTime Premiered { get; set; }
        #endregion

        #region ================================================================= METHODS ===================================================================================
        /// <summary>
        /// Customized ToString() method
        /// </summary>
        /// <returns>Custom string value showing relevant data for current class</returns>
        public override string ToString()
        {
            return Id + " :: " + SeasonName;
        }
        #endregion
    }
}
