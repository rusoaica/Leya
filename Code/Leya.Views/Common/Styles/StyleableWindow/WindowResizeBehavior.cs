/// Written by: Yulia Danilova
/// Creation Date: 20th of October, 2019
/// Purpose: Handles the resizing of Windows
#region ========================================================================= USING =====================================================================================
using System.Windows;
using System.Windows.Controls.Primitives;
#endregion

namespace Leya.Views.Common.Styles.StyleableWindow
{
    public static class WindowResizeBehavior
    {
        #region ========================================================== DEPENDENCY PROPERTIES ============================================================================
        public static readonly DependencyProperty TopResize = DependencyProperty.RegisterAttached("TopResize",
            typeof(Window), typeof(WindowResizeBehavior),
            new UIPropertyMetadata(null, OnTopResizeChanged));

        public static readonly DependencyProperty LeftResize = DependencyProperty.RegisterAttached("LeftResize",
            typeof(Window), typeof(WindowResizeBehavior),
            new UIPropertyMetadata(null, OnLeftResizeChanged));

        public static readonly DependencyProperty RightResize = DependencyProperty.RegisterAttached("RightResize",
            typeof(Window), typeof(WindowResizeBehavior),
            new UIPropertyMetadata(null, OnRightResizeChanged));

        public static readonly DependencyProperty BottomResize = DependencyProperty.RegisterAttached("BottomResize",
            typeof(Window), typeof(WindowResizeBehavior),
            new UIPropertyMetadata(null, OnBottomResizeChanged));

        public static readonly DependencyProperty TopLeftResize = DependencyProperty.RegisterAttached("TopLeftResize",
            typeof(Window), typeof(WindowResizeBehavior),
            new UIPropertyMetadata(null, OnTopLeftResizeChanged));

        public static readonly DependencyProperty TopRightResize = DependencyProperty.RegisterAttached("TopRightResize",
            typeof(Window), typeof(WindowResizeBehavior),
            new UIPropertyMetadata(null, OnTopRightResizeChanged));

        public static readonly DependencyProperty BottomLeftResize = DependencyProperty.RegisterAttached("BottomLeftResize",
            typeof(Window), typeof(WindowResizeBehavior),
            new UIPropertyMetadata(null, OnBottomLeftResizeChanged));

        public static readonly DependencyProperty BottomRightResize = DependencyProperty.RegisterAttached("BottomRightResize",
            typeof(Window), typeof(WindowResizeBehavior),
            new UIPropertyMetadata(null, OnBottomRightResizeChanged));
        #endregion

        #region ================================================================= METHODS ===================================================================================
        public static Window GetTopLeftResize(DependencyObject obj)
        {
            return (Window)obj.GetValue(TopLeftResize);
        }

        public static void SetTopLeftResize(DependencyObject obj, Window wnd)
        {
            obj.SetValue(TopLeftResize, wnd);
        }

        private static void OnTopLeftResizeChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is Thumb thumb)
                thumb.DragDelta += DragTopLeft;
        }

        public static Window GetTopRightResize(DependencyObject obj)
        {
            return (Window)obj.GetValue(TopRightResize);
        }

        public static void SetTopRightResize(DependencyObject obj, Window wnd)
        {
            obj.SetValue(TopRightResize, wnd);
        }

        private static void OnTopRightResizeChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is Thumb thumb)
                thumb.DragDelta += DragTopRight;
        }

        public static Window GetBottomRightResize(DependencyObject obj)
        {
            return (Window)obj.GetValue(BottomRightResize);
        }

        public static void SetBottomRightResize(DependencyObject obj, Window wnd)
        {
            obj.SetValue(BottomRightResize, wnd);
        }

        private static void OnBottomRightResizeChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is Thumb thumb)
                thumb.DragDelta += DragBottomRight;
        }

        public static Window GetBottomLeftResize(DependencyObject obj)
        {
            return (Window)obj.GetValue(BottomLeftResize);
        }

        public static void SetBottomLeftResize(DependencyObject obj, Window wnd)
        {
            obj.SetValue(BottomLeftResize, wnd);
        }

        private static void OnBottomLeftResizeChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is Thumb thumb)
                thumb.DragDelta += DragBottomLeft;
        }

        public static Window GetLeftResize(DependencyObject obj)
        {
            return (Window)obj.GetValue(LeftResize);
        }

        public static void SetLeftResize(DependencyObject obj, Window wnd)
        {
            obj.SetValue(LeftResize, wnd);
        }

        private static void OnLeftResizeChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is Thumb thumb)
                thumb.DragDelta += DragLeft;
        }

        public static Window GetRightResize(DependencyObject obj)
        {
            return (Window)obj.GetValue(RightResize);
        }

        public static void SetRightResize(DependencyObject obj, Window wnd)
        {
            obj.SetValue(RightResize, wnd);
        }

        private static void OnRightResizeChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is Thumb thumb)
                thumb.DragDelta += DragRight;
        }

        public static Window GetTopResize(DependencyObject obj)
        {
            return (Window)obj.GetValue(TopResize);
        }

        public static void SetTopResize(DependencyObject obj, Window wnd)
        {
            obj.SetValue(TopResize, wnd);
        }

        private static void OnTopResizeChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is Thumb thumb)
                thumb.DragDelta += DragTop;
        }

        public static Window GetBottomResize(DependencyObject obj)
        {
            return (Window)obj.GetValue(BottomResize);
        }

        public static void SetBottomResize(DependencyObject obj, Window wnd)
        {
            obj.SetValue(BottomResize, wnd);
        }

        private static void OnBottomResizeChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is Thumb thumb)
                thumb.DragDelta += DragBottom;
        }

        private static void DragLeft(object sender, DragDeltaEventArgs e)
        {
            Thumb thumb = sender as Thumb;
            if (thumb.GetValue(LeftResize) is Window wnd)
            {
                double horizontalChange = wnd.SafeWidthChange(e.HorizontalChange, false);
                wnd.Width -= horizontalChange;
                wnd.Left += horizontalChange;
            }
        }

        private static void DragRight(object sender, DragDeltaEventArgs e)
        {
            Thumb thumb = sender as Thumb;
            if (thumb.GetValue(RightResize) is Window wnd)
                wnd.Width += wnd.SafeWidthChange(e.HorizontalChange);
        }

        private static void DragTop(object sender, DragDeltaEventArgs e)
        {
            Thumb thumb = sender as Thumb;
            if (thumb.GetValue(TopResize) is Window wnd)
            {
                double verticalChange = wnd.SafeHeightChange(e.VerticalChange, false);
                wnd.Height -= verticalChange;
                wnd.Top += verticalChange;
            }
        }

        private static void DragBottom(object sender, DragDeltaEventArgs e)
        {
            Thumb thumb = sender as Thumb;
            if (thumb.GetValue(BottomResize) is Window wnd)
                wnd.Height += wnd.SafeHeightChange(e.VerticalChange);
        }

        private static void DragTopLeft(object sender, DragDeltaEventArgs e)
        {
            Thumb thumb = sender as Thumb;
            if (thumb.GetValue(TopLeftResize) is Window wnd)
            {
                double verticalChange = wnd.SafeHeightChange(e.VerticalChange, false);
                double horizontalChange = wnd.SafeWidthChange(e.HorizontalChange, false);
                wnd.Width -= horizontalChange;
                wnd.Left += horizontalChange;
                wnd.Height -= verticalChange;
                wnd.Top += verticalChange;
            }
        }

        private static void DragTopRight(object sender, DragDeltaEventArgs e)
        {
            Thumb thumb = sender as Thumb;
            if (thumb.GetValue(TopRightResize) is Window wnd)
            {
                double verticalChange = wnd.SafeHeightChange(e.VerticalChange, false);
                double horizontalChange = wnd.SafeWidthChange(e.HorizontalChange);
                wnd.Width += horizontalChange;
                wnd.Height -= verticalChange;
                wnd.Top += verticalChange;
            }
        }

        private static void DragBottomRight(object sender, DragDeltaEventArgs e)
        {
            Thumb thumb = sender as Thumb;
            if (thumb.GetValue(BottomRightResize) is Window wnd)
            {
                wnd.Width += wnd.SafeWidthChange(e.HorizontalChange);
                wnd.Height += wnd.SafeHeightChange(e.VerticalChange);
            }
        }

        private static void DragBottomLeft(object sender, DragDeltaEventArgs e)
        {
            Thumb thumb = sender as Thumb;
            if (thumb.GetValue(BottomLeftResize) is Window wnd)
            {
                double verticalChange = wnd.SafeHeightChange(e.VerticalChange);
                double horizontalChange = wnd.SafeWidthChange(e.HorizontalChange, false);
                wnd.Width -= horizontalChange;
                wnd.Left += horizontalChange;
                wnd.Height += verticalChange;
            }
        }

        private static double SafeWidthChange(this Window wnd, double change, bool isPositive = true)
        {
            double result = isPositive ? wnd.Width + change : wnd.Width - change;
            if (result <= wnd.MinWidth)
                return 0;
            else if (result >= wnd.MaxWidth)
                return 0;
            else if(result < 0)
                return 0;
            else
                return change;
        }

        private static double SafeHeightChange(this Window wnd, double change, bool isPositive = true)
        {
            double result = isPositive ? wnd.Height + change : wnd.Height - change;
            if (result <= wnd.MinHeight)
                return 0;
            else if (result >= wnd.MaxHeight)
                return 0;
            else if (result < 0)
                return 0;
            else
                return change;
        }
        #endregion
    }
}
