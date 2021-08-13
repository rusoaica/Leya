/// Written by: Yulia Danilova
/// Creation Date: 04th of July, 2021
/// Purpose: Explicit implementation of abstract custom file save service
#region ========================================================================= USING =====================================================================================
using System;
using System.Threading.Tasks;
using Leya.Infrastructure.Enums;
using Leya.Infrastructure.Dialog;
using System.Collections.Generic;
using Leya.ViewModels.Common.Dispatcher;
#endregion

namespace Leya.ViewModels.Common.Dialogs.FileSave
{
    public class FileSaveService : IFileSaveService
    {
        #region ============================================================== FIELD MEMBERS ================================================================================
        private readonly IDispatcher dispatcher;
        private readonly Func<IFileSaveDialogVM> fileSaveVM;
        #endregion

        #region =============================================================== PROPERTIES ==================================================================================
        public List<string> Filter { get; set; }
        public string Filename { get; set; }
        public string InitialFolder { get; set; }
        public bool ShowNewFolderButton { get; set; }
        public bool OverwriteExisting { get; private set; }
        #endregion

        #region ================================================================== CTOR =====================================================================================
        /// <summary>
        /// Overload C-tor
        /// </summary>
        /// <param name="dispatcher">The dispatcher to use</param>
        /// <param name="fileSaveVM">A Func that will force the creation of a new file save dialog viewmodel instance on each call</param>
        /// <remarks>When using simple constructor injection, the file save dialog instance is created only once, and can no longer trigger new views after the 
        /// first one is disposed, therefore the need of a new viewmodel on each call. Method injection could have been an approach too, but that would 
        /// require changing the signatures of the Show() methods in the interface, which is unacceptable. Service Locator pattern is also unacceptable, 
        /// and the viewmodel is not aware of a DI container anyway. Sending the DI container as injected parameter is definitely unacceptable.</remarks>
        public FileSaveService(IDispatcher dispatcher, Func<IFileSaveDialogVM> fileSaveVM)
        {
            this.dispatcher = dispatcher;
            this.fileSaveVM = fileSaveVM;
        }
        #endregion

        #region ================================================================= METHODS ===================================================================================
        /// <summary>
        /// Shows a new custom folder save dialog
        /// </summary>
        /// <returns>A <see cref="NotificationResult"/> value representing the result of the custom folder save dialog</returns>
        public async Task<NotificationResult> Show()
        {
            return await await dispatcher?.Dispatch(new Func<Task<NotificationResult>>(async () =>
            {
                IFileSaveDialogVM fileSaveDialogVM = fileSaveVM.Invoke();
                fileSaveDialogVM.Filter = Filter;
                fileSaveDialogVM.InitialFolder = InitialFolder;
                fileSaveDialogVM.Filename = Filename;
                fileSaveDialogVM.ShowNewFolderButton = ShowNewFolderButton;
                // display the file save dialog as modal, and get its result
                NotificationResult result = await fileSaveDialogVM.Show();
                // after file save dialog is closed, relay the provided filename and whether to override existing files or not
                Filename = fileSaveDialogVM.Filename;
                OverwriteExisting = fileSaveDialogVM.OverwriteExisting;
                return result;
            }));
        }
        #endregion
    }
}
