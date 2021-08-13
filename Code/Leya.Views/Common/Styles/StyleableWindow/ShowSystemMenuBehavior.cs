/// Written by: Yulia Danilova
/// Creation Date: 20th of October, 2019
/// Purpose: Handles the behavior of the system menu shown when clicking on the icon on the Title Bars of Windows
#region ========================================================================= USING =====================================================================================
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
#endregion

namespace Leya.Views.Common.Styles.StyleableWindow
{
    public static class ShowSystemMenuBehavior
    {
        #region ============================================================== FIELD MEMBERS ================================================================================
        static bool leftButtonToggle = true;
        #endregion

        #region ========================================================== DEPENDENCY PROPERTIES ============================================================================
        public static readonly DependencyProperty TargetWindow = DependencyProperty.RegisterAttached("TargetWindow", typeof(Window), typeof(ShowSystemMenuBehavior));        
        
        public static readonly DependencyProperty LeftButtonShowAt = DependencyProperty.RegisterAttached("LeftButtonShowAt", typeof(UIElement), typeof(ShowSystemMenuBehavior),
            new UIPropertyMetadata(null, LeftButtonShowAtChanged));

        public static readonly DependencyProperty RightButtonShow = DependencyProperty.RegisterAttached("RightButtonShow", typeof(bool), typeof(ShowSystemMenuBehavior),
            new UIPropertyMetadata(false, RightButtonShowChanged));
        #endregion

        #region ================================================================= METHODS ===================================================================================
        #region TargetWindow
        public static Window GetTargetWindow(DependencyObject obj)
        {
            return (Window)obj.GetValue(TargetWindow);
        }

        public static void SetTargetWindow(DependencyObject obj, Window wnd)
        {
            obj.SetValue(TargetWindow, wnd);
        }
        #endregion

        #region LeftButtonShowAt
        public static UIElement GetLeftButtonShowAt(DependencyObject obj)
        {
            return (UIElement)obj.GetValue(LeftButtonShowAt);
        }

        public static void SetLeftButtonShowAt(DependencyObject obj, UIElement element)
        {
            obj.SetValue(LeftButtonShowAt, element);
        }
        #endregion

        #region RightButtonShow
        public static bool GetRightButtonShow(DependencyObject obj)
        {
            return (bool)obj.GetValue(RightButtonShow);
        }

        public static void SetRightButtonShow(DependencyObject obj, bool argument)
        {
            obj.SetValue(RightButtonShow, argument);
        }
        #endregion

        #region LeftButtonShowAt        
        static void LeftButtonShowAtChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is UIElement element)
                element.MouseLeftButtonDown += LeftButtonDownShow;
        }

        static void LeftButtonDownShow(object sender, MouseButtonEventArgs e)
        {
            if (leftButtonToggle)
            {
                object element = ((UIElement)sender).GetValue(LeftButtonShowAt);
                Point showMenuAt = ((Visual)element).PointToScreen(new Point(0, 0));
                Window targetWindow = ((UIElement)sender).GetValue(TargetWindow) as Window;
                SystemMenuManager.ShowMenu(targetWindow, showMenuAt);
                leftButtonToggle = !leftButtonToggle;
            }
            else
                leftButtonToggle = !leftButtonToggle;
        }
        #endregion

        #region RightButtonShow handlers
        private static void RightButtonShowChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is UIElement element)
                element.MouseRightButtonDown += RightButtonDownShow;
        }

        static void RightButtonDownShow(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            UIElement element = (UIElement)sender;
            Window targetWindow = element.GetValue(TargetWindow) as Window;
            Point showMenuAt = targetWindow.PointToScreen(Mouse.GetPosition((targetWindow)));
            SystemMenuManager.ShowMenu(targetWindow, showMenuAt);
        }
        #endregion
        #endregion
    }
}