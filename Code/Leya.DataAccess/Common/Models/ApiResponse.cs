/// Written by: Yulia Danilova
/// Creation Date: 13th of January, 2020
/// Purpose: Container for deserialization models and other data supplied by the storage medium
#region ========================================================================= USING =====================================================================================
using Leya.DataAccess.StorageAccess;
#endregion

namespace Leya.DataAccess.Common.Models
{
    public sealed class ApiResponse<T> where T : IStorageEntity
    {
        #region ================================================================ PROPERTIES =================================================================================
        public T[] Data { get; set; }
        public int Count { get; set; }
        public string Error { get; set; }
        #endregion
    }

    public sealed class ApiResponse
    {
        #region ================================================================ PROPERTIES =================================================================================
        public int Count { get; set; }
        public string Error { get; set; }
        #endregion
    }
}
