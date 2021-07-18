/// Written by: Josh Smith, Yulia Danilova
/// Creation Date: 30th of August, 2008
/// Purpose: Behavior for scrolling to the selected item inside a treeview
/// Remarks: https://www.codeproject.com/Articles/28959/Introduction-to-Attached-Behaviors-in-WPF
#region ========================================================================= USING =====================================================================================
using System;
using System.Windows;
using System.Windows.Controls;
#endregion

namespace Leya.Views.Common.AttachedProperties
{
    public static class TreeViewItemBehavior
    {
        #region ========================================================== DEPENDENCY PROPERTIES ============================================================================
        public static readonly DependencyProperty IsBroughtIntoViewWhenSelectedProperty = DependencyProperty.RegisterAttached("IsBroughtIntoViewWhenSelected", typeof(bool), 
            typeof(TreeViewItemBehavior), new UIPropertyMetadata(false, OnIsBroughtIntoViewWhenSelectedChanged));
        #endregion

        #region ================================================================= METHODS ===================================================================================
        public static bool GetIsBroughtIntoViewWhenSelected(TreeViewItem treeViewItem)
        {
            return (bool)treeViewItem.GetValue(IsBroughtIntoViewWhenSelectedProperty);
        }

        public static void SetIsBroughtIntoViewWhenSelected(TreeViewItem treeViewItem, bool value)
        {
            treeViewItem.SetValue(IsBroughtIntoViewWhenSelectedProperty, value);
        }

        static void OnIsBroughtIntoViewWhenSelectedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is TreeViewItem item))
                return;
            if (e.NewValue is bool == false)
                return;
            if ((bool)e.NewValue)
            {
                item.Selected += OnTreeViewItemSelected;
                // the setter for the dependency property is not applied to the items in the treeview until after they are made visible by expanding them, 
                // therefore, TreeViewItem.Selected does not have an event attached initially; the solution is to check if the dependency property is enabled
                // and manually rise the event that triggers the scrolling
                if (item.IsSelected)  
                    item.RaiseEvent(new RoutedEventArgs(TreeViewItem.SelectedEvent)); 
            }
            else
                item.Selected -= OnTreeViewItemSelected;
        }

        static void OnTreeViewItemSelected(object sender, RoutedEventArgs e)
        {
            // only react to the Selected event raised by the TreeViewItem whose IsSelected property was modified
            // ignore all ancestors who are merely reporting that a descendant's Selected event fired.
            if (!ReferenceEquals(sender, e.OriginalSource))
                return;
            if (e.OriginalSource is TreeViewItem item)
                item.BringIntoView();
        }
        #endregion
    }
}
