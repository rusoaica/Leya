/// Written by: Yulia Danilova
/// Creation Date: 29th of October, 2019
/// Purpose: Handles GridViews column header clicks
#region ========================================================================= USING =====================================================================================
using System.Windows;
using System.Windows.Controls;
using Microsoft.Xaml.Behaviors;
#endregion

namespace Leya.Views.Common.Sorting
{
    public class SortingBehavior : Behavior<ListView>
    {
        #region ============================================================== FIELD MEMBERS ================================================================================
        private Sorting sorting;
        #endregion

        #region ================================================================== CTOR =====================================================================================
        /// <summary>
        /// Default C-tor
        /// </summary>
        public SortingBehavior()
        {
            sorting = new Sorting();
        }
        #endregion

        #region ================================================================= METHODS ===================================================================================
        /// <summary>
        /// Override method for OnAttached
        /// </summary>
        protected override void OnAttached()
        {
            AssociatedObject.AddHandler(GridViewColumnHeader.ClickEvent, new RoutedEventHandler(OnColumnHeaderClicked));
        }

        /// <summary>
        /// Override method for OnDetaching
        /// </summary>
        protected override void OnDetaching()
        {
            AssociatedObject.RemoveHandler(GridViewColumnHeader.ClickEvent, new RoutedEventHandler(OnColumnHeaderClicked));
        }

        /// <summary>
        /// Handles ColumnHeaderClicked event
        /// </summary>
        private void OnColumnHeaderClicked(object sender, RoutedEventArgs e)
        {
            if (!(sender is ListView listView))
                return;
            sorting.Sort(e.OriginalSource, listView.Items);
        }
        #endregion
    }
}