/// Written by: Yulia Danilova
/// Creation Date: 28th of October, 2019
/// Purpose: Handles Close command of windows
#region ========================================================================= USING =====================================================================================
using System;
using System.Windows;
using System.Windows.Input;
#endregion

namespace Leya.Views.Common.Styles.StyleableWindow
{
    public class WindowCloseCommand : ICommand
    {
        #region ============================================================== FIELD MEMBERS ================================================================================
        public event EventHandler CanExecuteChanged;
        #endregion

        #region ================================================================= METHODS ===================================================================================
        /// <summary>
        /// Indicates if <paramref name="wnd"/> can be closed or not
        /// </summary>
        /// <param name="wnd">The window</param>
        /// <returns>True</returns>
        public bool CanExecute(object wnd)
        {
            return true;
        }

        /// <summary>
        /// Closes a window
        /// </summary>
        /// <param name="wnd">The window to close</param>
        public void Execute(object wnd)
        {
            if (wnd is Window window)
                window.Close();
        }
        #endregion
    }
}
