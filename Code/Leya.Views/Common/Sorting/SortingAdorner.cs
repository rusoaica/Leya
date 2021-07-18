/// Written by: Yulia Danilova
/// Creation Date: 29th of October, 2019
/// Purpose: Draws a small triagle that indicates the sorting order (ascending/descending)
#region ========================================================================= USING =====================================================================================
using System.Windows.Media;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Documents;
#endregion

namespace Leya.Views.Common.Sorting
{
    public class SortingAdorner : Adorner
    {
        #region ============================================================== FIELD MEMBERS ================================================================================
        private Geometry sortDirection;
        private static Geometry arrowUp = Geometry.Parse("M 5,5 15,5 10,0 5,5");
        private static Geometry arrowDown = Geometry.Parse("M 5,0 10,5 15,0 5,0");
        #endregion

        #region ================================================================== CTOR =====================================================================================
        /// <summary>
        /// Overload C-tor
        /// </summary>
        /// <param name="adornedElement">The element to be adorned</param>
        /// <param name="sortDirection">The direction of sorting</param>
        public SortingAdorner(GridViewColumnHeader adornedElement, ListSortDirection sortDirection) : base(adornedElement)
        {
            this.sortDirection = sortDirection == ListSortDirection.Ascending ? arrowUp : arrowDown;
        }
        #endregion

        #region ================================================================= METHODS ===================================================================================
        /// <summary>
        /// Override method for rendering
        /// </summary>
        /// <param name="drawingContext">The drawing context used in rendering</param>
        protected override void OnRender(DrawingContext drawingContext)
        {
            double x = AdornedElement.RenderSize.Width - 20;
            double y = (AdornedElement.RenderSize.Height - 5) / 2;
            if (x >= 20)
            {
                // right order of the statements is important
                drawingContext.PushTransform(new TranslateTransform(x, y));
                drawingContext.DrawGeometry(Brushes.Black, null, sortDirection);
                drawingContext.Pop();
            }
        }
        #endregion
    }
}