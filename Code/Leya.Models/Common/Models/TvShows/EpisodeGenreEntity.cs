﻿/// Written by: Yulia Danilova
/// Creation Date: 06th of July, 2021
/// Purpose: Data transfer object for the episode genres

namespace Leya.Models.Common.Models.TvShows
{
    public class EpisodeGenreEntity
    {
        #region ================================================================ PROPERTIES =================================================================================
        public int Id { get; set; }
        public int EpisodeId { get; set; }
        public string Genre { get; set; }
        #endregion

        #region ================================================================= METHODS ===================================================================================
        /// <summary>
        /// Customized ToString() method
        /// </summary>
        /// <returns>Custom string value showing relevant data for current class</returns>
        public override string ToString()
        {
            return Id + " :: " + Genre;
        }
        #endregion
    }
}
