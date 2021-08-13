/// Written by: Yulia Danilova
/// Creation Date: 19th of November, 2020
/// Purpose: Interface for the view model for the media options 
#region ========================================================================= USING =====================================================================================
using System;
using Leya.ViewModels.Common.MVVM;
#endregion

namespace Leya.ViewModels.Options
{
    public interface IOptionsMediaVM : IBaseModel
    {
        #region ============================================================== FIELD MEMBERS ================================================================================
        event Action<bool> ValidationChanged;
        #endregion
    }
}
