/// Written by: Matt Hamilton
/// Creation Date: 5th of September, 2011
/// Purpose: Converter for lengths provided by grid lenghts
/// Remarks: https://stackoverflow.com/questions/7304203/binding-to-the-width-of-a-columndefinition
#region ========================================================================= USING =====================================================================================
using System;
using System.Windows;
using System.Windows.Data;
using System.Globalization;
#endregion

namespace Leya.Views.Common.Converters
{
    public class GridLengthValueConverter : IValueConverter
    {
        #region ============================================================== FIELD MEMBERS ================================================================================
        GridLengthConverter converter = new GridLengthConverter();
        #endregion

        #region ================================================================= METHODS ===================================================================================
        /// <summary>
        /// Converts a string to a decimal
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
            return converter.ConvertFrom(value);
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
            return converter.ConvertTo(value, targetType);
        }
        #endregion
    }
}
