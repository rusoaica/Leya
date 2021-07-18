/// Written by: Yulia Danilova
/// Creation Date: 09th of December, 2020
/// Purpose: Deserialization model for the BandMembers storage container
#region ========================================================================= USING =====================================================================================
using Leya.DataAccess.StorageAccess;
using Leya.DataAccess.Common.Attributes;
#endregion

namespace Leya.DataAccess.Common.Models.Artists
{
    public sealed class BandMemberEntity : IStorageEntity
    {
        #region ================================================================ PROPERTIES =================================================================================
        [IgnoreOnInsert]
        public int Id { get; set; }
        public int Order { get; set; }
        public int ArtistId { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
        public string Thumb { get; set; }
        #endregion

        #region ================================================================= METHODS ===================================================================================
        /// <summary>
        /// Customized ToString() method
        /// </summary>
        /// <returns>Custom string value showing relevant data for current class</returns>
        public override string ToString()
        {
            return Id + " :: " + Name;
        }
        #endregion
    }
}
