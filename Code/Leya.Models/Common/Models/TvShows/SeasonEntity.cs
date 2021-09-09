/// Written by: Yulia Danilova
/// Creation Date: 23rd of November, 2020
/// Purpose: Data transfer object for the tv show seasons 
#region ========================================================================= USING =====================================================================================
using System;
using Leya.Models.Common.Models.Media;
#endregion

namespace Leya.Models.Common.Models.TvShows
{
    public class SeasonEntity : IMedia
    {
        #region ================================================================ PROPERTIES =================================================================================
        public int Id { get; set; }
        public int Year { get; set; }
        public int TvShowId { get; set; }
        public int Number { get; set; }
        public string Title { get; set; }
        public string Synopsis { get; set; }
        public bool? IsWatched { get; set; } = false;
        public bool IsFavorite { get; set; }
        public DateTime Premiered { get; set; }
        public EpisodeEntity[] Episodes { get; set; }
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
