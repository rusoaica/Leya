/// Written by: Yulia Danilova
/// Creation Date: 24th of November, 2020
/// Purpose: Deserialization model for the MovieVideoInfo storage container
#region ========================================================================= USING =====================================================================================
using Leya.DataAccess.StorageAccess;
using Leya.DataAccess.Common.Attributes;
#endregion

namespace Leya.DataAccess.Common.Models.Movies
{
    public sealed class VideoEntity : IStorageEntity
    {
        #region ================================================================ PROPERTIES =================================================================================
        [IgnoreOnInsert]
        public int Id { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int MovieId { get; set; }
        public string Codec { get; set; }
        public float Aspect { get; set; }
        public bool Is3D { get; set; }
        #endregion

        #region ================================================================= METHODS ===================================================================================
        /// <summary>
        /// Customized ToString() method
        /// </summary>
        /// <returns>Custom string value showing relevant data for current class</returns>
        public override string ToString()
        {
            return Id + " :: " + MovieId;
        }
        #endregion
    }
}
