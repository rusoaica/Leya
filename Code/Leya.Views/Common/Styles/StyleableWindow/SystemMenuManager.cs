/// Written by: Yulia Danilova
/// Creation Date: 20th of October, 2019
/// Purpose: Shows the system menu when clicking on the Title Bar icons of Windows
#region ========================================================================= USING =====================================================================================
using System;
using System.Windows;
using System.Windows.Interop;
using System.Runtime.InteropServices;
#endregion

namespace Leya.Views.Common.Styles.StyleableWindow
{
    public static class SystemMenuManager
    {
        #region ============================================================== FIELD MEMBERS ================================================================================
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [DllImport("user32.dll")]
        internal static extern IntPtr PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        internal static extern bool EnableMenuItem(IntPtr hMenu, uint uIDEnableItem, uint uEnable);

        [DllImport("user32.dll")]
        internal static extern int TrackPopupMenuEx(IntPtr hmenu, uint fuFlags, int x, int y, IntPtr hwnd, IntPtr lptpm);
        #endregion

        #region ================================================================= METHODS ===================================================================================
        /// <summary>
        /// Shows the system menu when clicking on the Title Bar icons of <paramref name="targetWindow"/>
        /// </summary>
        /// <param name="targetWindow">The window for which to show the titlebar icon context menu</param>
        /// <param name="menuLocation">The location of the titlebar icon context menu</param>
        public static void ShowMenu(Window targetWindow, Point menuLocation)
        {
            if (targetWindow == null)
                throw new ArgumentNullException("TargetWindow is null.");
            int x, y;
            try
            {
                x = Convert.ToInt32(menuLocation.X);
                y = Convert.ToInt32(menuLocation.Y);
            }
            catch (OverflowException)
            {
                x = 0;
                y = 0;
            }
            uint WM_SYSCOMMAND = 0x112, TPM_LEFTALIGN = 0x0000, TPM_RETURNCMD = 0x0100;
            IntPtr window = new WindowInteropHelper(targetWindow).Handle;
            IntPtr wMenu = GetSystemMenu(window, false);
            int command = TrackPopupMenuEx(wMenu, TPM_LEFTALIGN | TPM_RETURNCMD, x, y, window, IntPtr.Zero);
            if (command == 0)
                return;
            PostMessage(window, WM_SYSCOMMAND, new IntPtr(command), IntPtr.Zero);
        }
        #endregion
    }
}
