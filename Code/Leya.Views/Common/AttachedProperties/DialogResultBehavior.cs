/// Written by: Yulia Danilova
/// Creation Date: 11th of November, 2019
/// Purpose: Attached behavior that allows returning the DialogResult of a window in MVVM
#region ========================================================================= USING =====================================================================================
using System.Windows;
#endregion

namespace Leya.Views.Common.AttachedProperties
{
    public static class DialogResultBehavior
    {
        #region ============================================================ ATTACHED PROPERTIES ============================================================================ 
        public static readonly DependencyProperty DialogResultProperty = DependencyProperty.RegisterAttached("DialogResult", typeof(bool?), typeof(DialogResultBehavior), new PropertyMetadata(DialogResultChanged));
        #endregion

        #region ================================================================= METHODS ===================================================================================
        private static void DialogResultChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Window window)
                window.DialogResult = e.NewValue as bool?;
        }

        public static void SetDialogResult(Window target, bool? value)
        {
            target.SetValue(DialogResultProperty, value);
        }
        #endregion
    }
}