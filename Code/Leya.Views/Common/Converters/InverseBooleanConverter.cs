/// Written by: Yulia Danilova
/// Creation Date: 5th of November, 2019
/// Purpose: Converter for inverse boolean bindings
#region ========================================================================= USING =====================================================================================
using System;
using System.Windows.Data;
using System.Globalization;
#endregion

namespace Leya.Views.Common.Converters
{
    public class InverseBooleanConverter : IValueConverter
    {
        #region ================================================================= METHODS ===================================================================================
        /// <summary>
        /// Converts a string to a boolean
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
            try
            {
                bool test_value = (bool)value;
                return !test_value;
            }
            catch { return false; }
        }

        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="_value">The value that is produced by the binding target.</param>
        /// <param name="_targetType">The type to convert to.</param>
        /// <param name="_parameter">The converter parameter to use.</param>
        /// <param name="_culture">The culture to use in the converter.</param>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Convert(value, targetType, parameter, culture);
        }
        #endregion
    }
}
