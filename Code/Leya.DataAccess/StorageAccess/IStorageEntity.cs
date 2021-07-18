/// Written by: Sameh Salem
/// Creation Date: 12th of June, 2021
/// Purpose: Interface for storage entities, enforces Id

namespace Leya.DataAccess.StorageAccess
{
    public interface IStorageEntity
    {
        #region ================================================================ PROPERTIES =================================================================================
        int Id { get; set; }
        #endregion
    }
}
