/// Written by: Yulia Danilova
/// Creation Date: 27th of June, 2021
/// Purpose: Interface for the view model for the FolderBrowseDialog view
#region ========================================================================= USING =====================================================================================
using Leya.Infrastructure.Enums;
using Leya.ViewModels.Common.MVVM;
#endregion

namespace Leya.ViewModels.Common.Dialogs.FolderBrowser
{
    public interface IFolderBrowserDialogVM : IBaseModel
    {
        #region =============================================================== PROPERTIES ==================================================================================
        bool ShowNewFolderButton { get; set; }
        string InitialFolder { get; set; }
        string SelectedDirectories { get; set; }
        #endregion

        #region ================================================================= METHODS ===================================================================================
        /// <summary>
        /// Shows a new instance of the folder browser dialog
        /// </summary>
        /// <returns>A <see cref="NotificationResult"/> representing the DialogResult of the folder browser dialog</returns>
        NotificationResult Show();
        #endregion
    }
}
