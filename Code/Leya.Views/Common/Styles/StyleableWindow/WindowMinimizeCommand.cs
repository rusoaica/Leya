/// Written by: Yulia Danilova
/// Creation Date: 20th of October, 2019
/// Purpose: Handles Minimize command of Windows
#region ========================================================================= USING =====================================================================================
using System;
using System.Windows;
using System.Windows.Input;
#endregion

namespace Leya.Views.Common.Styles.StyleableWindow
{
    public class WindowMinimizeCommand : ICommand 
    {
        #region ============================================================== FIELD MEMBERS ================================================================================
        public event EventHandler CanExecuteChanged;
        #endregion

        #region ================================================================= METHODS ===================================================================================
        /// <summary>
        /// Indicates if <paramref name="wnd"/> can be minimized
        /// </summary>
        /// <param name="wnd">The window</param>
        /// <returns>True</returns>
        public bool CanExecute(object wnd)
        {
            return true;
        }

        /// <summary>
        /// Minimizes <paramref name="wnd"/>
        /// </summary>
        /// <param name="wnd">The window to be minimized</param>
        public void Execute(object wnd)
        {
            if (wnd is Window window)
                window.WindowState = WindowState.Minimized;
        }
        #endregion
    }
}
