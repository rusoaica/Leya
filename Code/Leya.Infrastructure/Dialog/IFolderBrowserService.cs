/// Written by: Yulia Danilova
/// Creation Date: 30th of June, 2021
/// Purpose: Interface for custom folder browser dialogs
#region ========================================================================= USING =====================================================================================
using Leya.Infrastructure.Enums;
#endregion

namespace Leya.Infrastructure.Dialog
{
    public interface IFolderBrowserService
    {
        #region =============================================================== PROPERTIES ==================================================================================
        bool ShowNewFolderButton { get; set; }
        string InitialFolder { get; set; }
        string SelectedDirectories { get; set; }
        #endregion

        #region ================================================================= METHODS ===================================================================================
        /// <summary>
        /// Shows a new folder browser dialog
        /// </summary>
        /// <returns>A <see cref="NotificationResult"/> representing the result of displaying the custom folder browser dialog</returns>
        NotificationResult Show();
        #endregion
    }
}
