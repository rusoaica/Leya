/// Written by: Yulia Danilova
/// Creation Date: 09th of November, 2020
/// Purpose: Data transfer object for the list of media types 
#region ========================================================================= USING =====================================================================================
using Leya.Models.Common.Infrastructure;
#endregion

namespace Leya.Models.Common.Models.Media
{
    public class MediaTypeEntity
    {
        #region ================================================================ PROPERTIES =================================================================================
        public int Id { get; set; }

        public string MediaName { get; set; }

        public string MediaType { get; set; }

        public bool IsMedia { get; set; }

        public MediaTypeSourceEntity[] MediaTypeSources { get; set; }
        #endregion

        #region ================================================================= METHODS ===================================================================================
        /// <summary>
        /// Customized ToString() method
        /// </summary>
        /// <returns>Custom string value showing relevant data for current class</returns>
        public override string ToString()
        {
            return Id + " :: " + MediaName;
        }


        /// <summary>
        /// Maps between this entity and the coresponding persistance entity
        /// </summary>
        /// <returns>A data storage entity representation of this entity</returns>
        public DataAccess.Common.Models.Media.MediaTypeEntity ToStorageEntity()
        {
            return Services.AutoMapper.Map<DataAccess.Common.Models.Media.MediaTypeEntity>(this);
        }
        #endregion
    }
}
