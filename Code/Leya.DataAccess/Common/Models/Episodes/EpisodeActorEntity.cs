/// Written by: Yulia Danilova
/// Creation Date: 22nd of November, 2020
/// Purpose: Deserialization model for the EpisodeActors storage container
#region ========================================================================= USING =====================================================================================
using Leya.DataAccess.StorageAccess;
using Leya.DataAccess.Common.Attributes;
#endregion

namespace Leya.DataAccess.Common.Models.Episodes
{
    public sealed class EpisodeActorEntity : IStorageEntity
    {
        #region ================================================================ PROPERTIES =================================================================================
        [IgnoreOnInsert]
        public int Id { get; set; }
        public int Order { get; set; }
        public int EpisodeId { get; set; }
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
