/// Written by: Yulia Danilova
/// Creation Date: 20th of October, 2019
/// Purpose: Handles maximize command of Windows
#region ========================================================================= USING =====================================================================================
using System;
using System.Windows;
using System.Windows.Input;
#endregion

namespace Leya.Views.Common.Styles.StyleableWindow
{
    public class WindowMaximizeCommand : ICommand
    {
        #region ============================================================== FIELD MEMBERS ================================================================================
        public event EventHandler CanExecuteChanged;
        #endregion

        #region ================================================================= METHODS ===================================================================================
        /// <summary>
        /// Indicates if <paramref name="wnd"/> can be maximized or not
        /// </summary>
        /// <param name="wnd">The window to be maximized</param>
        /// <returns>True</returns>
        public bool CanExecute(object wnd)
        {
            return true;
        }

        /// <summary>
        /// MMaximizes <paramref name="wnd"/>
        /// </summary>
        /// <param name="wnd">The window to maximize</param>
        public void Execute(object wnd)
        {
            if (wnd is Window window)
            {
                if (window.WindowState == WindowState.Maximized)
                    window.WindowState = WindowState.Normal;
                else
                    window.WindowState = WindowState.Maximized;
            }
        }
        #endregion
    }
}
