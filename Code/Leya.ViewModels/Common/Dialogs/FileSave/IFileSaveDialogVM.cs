/// Written by: Yulia Danilova
/// Creation Date: 04th of July, 2021
/// Purpose: Interface for the view model for the FileSaveDialog view
#region ========================================================================= USING =====================================================================================
using System.Threading.Tasks;
using Leya.Infrastructure.Enums;
using System.Collections.Generic;
using Leya.ViewModels.Common.MVVM;
#endregion

namespace Leya.ViewModels.Common.Dialogs.FileSave
{
    public interface IFileSaveDialogVM : IBaseModel
    {
        #region =============================================================== PROPERTIES ==================================================================================
        List<string> Filter { get; set; }
        string Filename { get; set; }
        string InitialFolder { get; set; }
        bool OverwriteExisting { get; }
        bool ShowNewFolderButton { get; set; }
        #endregion

        #region ================================================================= METHODS ===================================================================================
        /// <summary>
        /// Shows a new instance of the folder browser dialog
        /// </summary>
        /// <returns>A <see cref="NotificationResult"/> representing the DialogResult of the folder browser dialog</returns>
        Task<NotificationResult> ShowAsync();
        #endregion
    }
}
