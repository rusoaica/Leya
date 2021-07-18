/// Written by: Charlie
/// Creation Date: 29th of May, 2014
/// Purpose: Behavior for horiontal scrolling in listviews
/// Remarks: https://stackoverflow.com/questions/12069407/activate-horizontal-scrolling-with-mouse-on-listview
#region ========================================================================= USING =====================================================================================
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using Microsoft.Xaml.Behaviors;
using Leya.Views.Common.Miscellaneous;
#endregion

namespace Leya.Views.Common.AttachedProperties
{
    public class HorizontalScrollBehavior : Behavior<ItemsControl>
    {
        #region ================================================================ PROPERTIES =================================================================================
        /// <summary>
        /// A reference to the internal ScrollViewer.
        /// </summary>
        private ScrollViewer ScrollViewer { get; set; }

        /// <summary>
        /// By default, scrolling down on the wheel translates to right, and up to left.
        /// Set this to true to invert that translation.
        /// </summary>
        public bool IsInverted { get; set; }
        #endregion

        #region ================================================================= METHODS ===================================================================================
        /// <summary>
        /// The ScrollViewer is not available in the visual tree until the control is loaded.
        /// </summary>
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            AssociatedObject.Loaded -= OnLoaded;
            ScrollViewer = VisualTreeHelpers.FindVisualChild<ScrollViewer>(AssociatedObject);
            if (ScrollViewer != null)
                ScrollViewer.PreviewMouseWheel += OnPreviewMouseWheel;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            if (ScrollViewer != null)
                ScrollViewer.PreviewMouseWheel -= OnPreviewMouseWheel;
        }

        private void OnPreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            double newOffset = IsInverted ? ScrollViewer.HorizontalOffset + e.Delta : ScrollViewer.HorizontalOffset - e.Delta;
            ScrollViewer.ScrollToHorizontalOffset(newOffset);
        }
        #endregion
    }
}
