/// Written by: Yulia Danilova
/// Creation Date: 29th of October, 2019
/// Purpose: Sorts a CollectionView and updates the adorner accordingly
#region ========================================================================= USING =====================================================================================
using System.Windows;
using Xceed.Wpf.Toolkit;
using System.Windows.Data;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Documents;
#endregion

namespace Leya.Views.Common.Sorting
{
    public class Sorting
    {
        #region ============================================================== FIELD MEMBERS ================================================================================
        private ListSortDirection sortDirection;
        private GridViewColumnHeader sortColumn;
        #endregion

        #region ================================================================= METHODS ===================================================================================
        /// <summary>
        /// Sets an adorner to a column header
        /// </summary>
        /// <param name="columnHeader">The column header for which to set the adorner</param>
        /// <returns>The path to the binding source property</returns>
        private string SetAdorner(object columnHeader)
        {
            if (!(columnHeader is GridViewColumnHeader _column))
                return null;
            // remove arrow from previously sorted header
            if (sortColumn != null)
            {
                AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(sortColumn);
                try
                {
                    adornerLayer.Remove((adornerLayer.GetAdorners(sortColumn))[0]);
                }
                catch { }
            }
            // toggle sorting direction
            if (sortColumn == _column)
                sortDirection = sortDirection == ListSortDirection.Ascending ? ListSortDirection.Descending : ListSortDirection.Ascending;
            else
            {
                sortColumn = _column;
                sortDirection = ListSortDirection.Ascending;
            }
            SortingAdorner sortingAdorner = new SortingAdorner(_column, sortDirection);
            AdornerLayer.GetAdornerLayer(_column).Add(sortingAdorner);
            string header = string.Empty;
            // if binding is used and property name doesn't match header content
            if (sortColumn.Column.DisplayMemberBinding is Binding b)
                header = b.Path.Path;
            DataTemplate cellTemplate = sortColumn.Column.CellTemplate;
            // handle various types of cell content
            if (cellTemplate != null)
            {
                if (cellTemplate.LoadContent() is TextBox tb)
                    header = tb.GetBindingExpression(TextBox.TextProperty).ParentBinding.Path.Path;
                else
                {
                    if (cellTemplate.LoadContent() is ComboBox cb)
                        header = cb.GetBindingExpression(ComboBox.SelectedItemProperty).ParentBinding.Path.Path;
                    else
                    {
                        if (cellTemplate.LoadContent() is Image im)
                            header = im.GetBindingExpression(Image.VisibilityProperty).ParentBinding.Path.Path;
                        else
                        {
                            if (cellTemplate.LoadContent() is ContentPresenter cp)
                                header = cp.GetBindingExpression(ContentPresenter.ContentProperty).ParentBinding.Path.Path;
                            else
                            {
                                if (cellTemplate.LoadContent() is Label lb)
                                    header = lb.GetBindingExpression(Label.ContentProperty).ParentBinding.Path.Path;
                                else
                                {
                                    if (cellTemplate.LoadContent() is CheckBox chk)
                                        header = chk.GetBindingExpression(CheckBox.IsCheckedProperty).ParentBinding.Path.Path;
                                    else
                                    {
                                        if (cellTemplate.LoadContent() is DecimalUpDown _numeric)
                                            header = _numeric.GetBindingExpression(DecimalUpDown.ValueProperty).ParentBinding.Path.Path;
                                        else
                                        {
                                            if (cellTemplate.LoadContent() is Button btn)
                                                header = btn.GetBindingExpression(Button.ContentProperty).ParentBinding.Path.Path;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return header;
        }

        /// <summary>
        /// Sorts <paramref name="list"/> by <paramref name="columnHeader"/>
        /// </summary>
        /// <param name="columnHeader">The column header used for sorting</param>
        /// <param name="list">The list to be sorted</param>
        public void Sort(object columnHeader, CollectionView list)
        {
            try
            {
                string column = SetAdorner(columnHeader);
                if (column != null && column != string.Empty)
                {
                    list.SortDescriptions.Clear();
                    list.SortDescriptions.Add(new SortDescription(column, sortDirection));
                }
            }
            catch {}
        }
        #endregion
    }
}