/// Written by: Ron
/// Creation Date: 19th of March, 2014
/// Purpose: Helper generic method for finding a visual child of a dependency object
/// Remarks: https://codereview.stackexchange.com/questions/44760/is-there-a-better-way-to-get-a-child
#region ========================================================================= USING =====================================================================================
using System.Windows;
using System.Windows.Media;
#endregion

namespace Leya.Views.Common.Miscellaneous
{
    public class VisualTreeHelpers
    {
        #region ================================================================= METHODS ===================================================================================
        /// <summary>
        /// Return the first visual child of element by type.
        /// </summary>
        /// <typeparam name="T">The type of the Child</typeparam>
        /// <param name="obj">The parent Element</param>
        public static T FindVisualChild<T>(DependencyObject obj) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                if (child != null && child is T)
                    return (T)child;
                else
                {
                    T childOfChild = FindVisualChild<T>(child);
                    if (childOfChild != null)
                        return childOfChild;
                }
            }
            return null;
        }
        #endregion
    }
}
