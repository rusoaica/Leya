﻿/// Written by: Yulia Danilova
/// Creation Date: 03rd of July, 2021
/// Purpose: Interface for custom file browser dialogs
#region ========================================================================= USING =====================================================================================
using Leya.Infrastructure.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;
#endregion

namespace Leya.Infrastructure.Dialog
{
    public interface IFileBrowserService
    {
        #region =============================================================== PROPERTIES ==================================================================================
        List<string> Filter { get; set; }
        string SelectedFiles { get; set; }
        string InitialFolder { get; set; }
        bool ShowNewFolderButton { get; set; }
        bool AllowMultiselection { get; set; }
        #endregion

        #region ================================================================= METHODS ===================================================================================
        /// <summary>
        /// Shows a new folder browser dialog
        /// </summary>
        /// <returns>A <see cref="NotificationResult"/> representing the result of displaying the custom folder browser dialog</returns>
        Task<NotificationResult> Show();
        #endregion
    }
}
