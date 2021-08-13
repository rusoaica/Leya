/// Written by: Yulia Danilova
/// Creation Date: 17th of November, 2020
/// Purpose: Deserialization model for the MediaTypeSources storage container
#region ========================================================================= USING =====================================================================================
using Leya.DataAccess.StorageAccess;
using Leya.DataAccess.Common.Attributes;
#endregion

namespace Leya.DataAccess.Common.Models.Media
{
    public sealed class MediaTypeSourceEntity : IStorageEntity
    {
        #region ================================================================ PROPERTIES =================================================================================
        [IgnoreOnInsert]
        public int Id { get; set; }
        public int MediaTypeId { get; set; }
        public string MediaSourcePath { get; set; }
        #endregion

        #region ================================================================= METHODS ===================================================================================
        /// <summary>
        /// Customized ToString() method
        /// </summary>
        /// <returns>Custom string value showing relevant data for current class</returns>
        public override string ToString()
        {
            return Id + " :: " + MediaTypeId;
        }
        #endregion  
    }
}
