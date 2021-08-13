/// Written by: Yulia Danilova
/// Creation Date: 16th of November, 2020
/// Purpose: Deserialization model for the MediaTypes storage container
#region ========================================================================= USING =====================================================================================
using Leya.DataAccess.StorageAccess;
using Leya.DataAccess.Common.Attributes;
#endregion

namespace Leya.DataAccess.Common.Models.Media
{
    public sealed class MediaTypeEntity : IStorageEntity
    {
        #region ================================================================ PROPERTIES =================================================================================
        [IgnoreOnInsert]
        public int Id { get; set; }
        public string MediaName { get; set; }
        public string MediaType { get; set; }
        public bool IsMedia { get; set; }
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
        #endregion
    }
}
