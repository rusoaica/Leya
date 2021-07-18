/// Written by: Yulia Danilova
/// Creation Date: 09th of December, 2020
/// Purpose: Data transfer object for the music artist types

namespace Leya.Models.Common.Models.Artists
{
    public class ArtistTypeEntity
    {
        #region ================================================================ PROPERTIES =================================================================================
        public int Id { get; set; }
        public int ArtistId { get; set; }
        public string Type { get; set; }
        #endregion

        #region ================================================================= METHODS ===================================================================================
        /// <summary>
        /// Customized ToString() method
        /// </summary>
        /// <returns>Custom string value showing relevant data for current class</returns>
        public override string ToString()
        {
            return Id + " :: " + Type;
        }
        #endregion
    }
}
