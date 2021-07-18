/// Written by: Yulia Danilova
/// Creation Date: 5th of November, 2019
/// Purpose: Converter for visibilities provided by boolean values
#region ========================================================================= USING =====================================================================================
using System;
using System.Windows;
using System.Windows.Data;
using System.Globalization;
#endregion

namespace Leya.Views.Common.Converters
{
    public class BooleanToVisibilityConverter : IValueConverter
    {
        #region ================================================================= METHODS ===================================================================================
        /// <summary>
        /// Converts a boolean to visibility
        /// </summary>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
                return (bool)value ? Visibility.Visible : Visibility.Collapsed;
            return DependencyProperty.UnsetValue;
        }

        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value that is produced by the binding target.</param>
        /// <param name="targetType">The type to convert to.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // According to https://msdn.microsoft.com/en-us/library/system.windows.data.ivalueconverter.convertback(v=vs.110).aspx#Anchor_1
            // (kudos Scott Chamberlain), if you do not support a conversion back you should return a Binding.DoNothing or a DependencyProperty.UnsetValue
            return Binding.DoNothing;
        }
        #endregion
    }
}
