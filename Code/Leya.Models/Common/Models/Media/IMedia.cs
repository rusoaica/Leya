/// Written by: Yulia Danilova
/// Creation Date: 0tth of August, 2021
/// Purpose: Interface for common media library entities

namespace Leya.Models.Common.Models.Media
{
    public interface IMedia
    {
        #region ================================================================ PROPERTIES =================================================================================
        int Id { get; set; }
        string Title { get; set; }
        #endregion
    }
}
