/// Written by: Yulia Danilova
/// Creation Date: 5th of November, 2019
/// Purpose: Converter for two way percentage formatting bindings
#region ========================================================================= USING =====================================================================================
using System;
using System.Windows.Data;
using System.Globalization;
#endregion

namespace Leya.Views.Common.Converters
{
    public class PercentConverter : IValueConverter
    {
        #region ================================================================= METHODS ===================================================================================
        /// <summary>
        /// Converts an object to a decimal.
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
            if (string.IsNullOrEmpty(value.ToString()))
                return 0;
            if (value.GetType() == typeof(decimal))
                return (decimal)value;
            return value;
        }

        /// <summary>
        /// Converts a string number formatted as percent to a decimal
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
            if (string.IsNullOrEmpty(value.ToString()))
                return 0;
            string trimmedValue = value.ToString().Length > 2 ? value.ToString().Substring(0, value.ToString().Length - 2) : value.ToString();
            if (targetType == typeof(decimal))
                return decimal.TryParse(trimmedValue, out decimal result) ? result : value;
            return value;
        }
        #endregion
    }
}
