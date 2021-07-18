/// Written by: Yulia Danilova
/// Creation Date: 5th of November, 2019
/// Purpose: Converter for changing the casing of a string value
#region ========================================================================= USING =====================================================================================
using System;
using System.Windows.Data;
using System.Globalization;
using System.Windows.Controls;
#endregion

namespace Leya.Views.Common.Converters
{
    public class CaseConverter : IValueConverter
    {
        #region ================================================================ PROPERTIES =================================================================================
        public CharacterCasing Case { get; set; } = CharacterCasing.Upper;
        #endregion

        #region ================================================================= METHODS ===================================================================================
        /// <summary>
        /// Converts a string casing to the provided character casing
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
            if (value as string != null)
            {
                switch (Case)
                {
                    case CharacterCasing.Lower:
                        return (value as string).ToLower();
                    case CharacterCasing.Normal:
                        return value as string;
                    case CharacterCasing.Upper:
                        return (value as string).ToUpper();
                    default:
                        return value as string;
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// Converts back a value
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
            return Binding.DoNothing;
        }
        #endregion
    }
}
