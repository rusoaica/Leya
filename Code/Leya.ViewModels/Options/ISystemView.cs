/// Written by: Yulia Danilova
/// Creation Date: 10th of June, 2021
/// Purpose: Interface for the options view
#region ========================================================================= USING =====================================================================================
using Leya.ViewModels.Common.ViewFactory;
#endregion

namespace Leya.ViewModels.Options
{
    public interface ISystemView : IView
    {
        #region ================================================================ PROPERTIES =================================================================================
        dynamic Owner { get; set; }
        #endregion
    }
}
