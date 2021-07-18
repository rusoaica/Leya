/// Written by: Yulia Danilova
/// Creation Date: 13th of May, 2020
/// Purpose: Handles Help command of Windows
#region ========================================================================= USING =====================================================================================
using System;
using System.Windows;
using System.Windows.Input;
using Leya.ViewModels.Common.MVVM;
#endregion

namespace Leya.Views.Common.Styles.StyleableWindow
{
    public class WindowHelpCommand : ICommand
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
                (window.DataContext as BaseModel).ShowHelp();
        }
        #endregion
    }
}
