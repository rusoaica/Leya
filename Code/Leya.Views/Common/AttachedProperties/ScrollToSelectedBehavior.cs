/// Written by: Yulia Danilova
/// Creation Date: 16th of January, 2020
/// Purpose: Attached behavior for automatic scroll into view of ListViewItems
#region ========================================================================= USING =====================================================================================
using System.Windows;
using System.Windows.Controls;
#endregion

namespace Leya.Views.Common.AttachedProperties
{
    public static class ScrollToSelectedBehavior
    {
        #region ========================================================== DEPENDENCY PROPERTIES ============================================================================
        public static readonly DependencyProperty SelectedValueProperty = DependencyProperty.RegisterAttached("SelectedValue", typeof(object), typeof(ScrollToSelectedBehavior),
            new PropertyMetadata(null, OnSelectedValueChange));
        #endregion

        #region ================================================================= METHODS ===================================================================================
        public static void SetSelectedValue(DependencyObject source, object value)
        {
            source.SetValue(SelectedValueProperty, value);
        }

        public static object GetSelectedValue(DependencyObject source)
        {
            return source.GetValue(SelectedValueProperty);
        }

        /// <summary>
        /// Handles SelectedValueChanged event
        /// </summary>
        private static void OnSelectedValueChange(DependencyObject _d, DependencyPropertyChangedEventArgs e)
        {
            var listview = _d as ListView;
            listview.ScrollIntoView(e.NewValue);
        }
        #endregion
    }
}
