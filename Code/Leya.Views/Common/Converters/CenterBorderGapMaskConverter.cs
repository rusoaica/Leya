/// Written by: Thomas Levesque
/// Creation Date: 20th of November, 2010
/// Purpose: Converter for centering the header gap of a group box.
/// Remarks: https://stackoverflow.com/questions/4235252/wpf-groupbox-header-customization
#region ========================================================================= USING =====================================================================================
using System;
using System.Windows;
using System.Windows.Data;
using System.Globalization;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Controls;
#endregion

namespace Leya.Views.Common.Converters
{
    internal partial class CenterBorderGapMaskConverter : IMultiValueConverter
    {
        #region ================================================================= METHODS ===================================================================================
        /// <summary>
        /// Converter for centering the header gap of a group box
        /// </summary>
        /// <param name="values">The value produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            Type type = typeof(double);
            if (values == null
                || values.Length != 3
                || values[0] == null
                || values[1] == null
                || values[2] == null
                || !type.IsAssignableFrom(values[0].GetType())
                || !type.IsAssignableFrom(values[1].GetType())
                || !type.IsAssignableFrom(values[2].GetType()))
            {
                return DependencyProperty.UnsetValue;
            }
            double pixels = (double)values[0];
            double width = (double)values[1];
            double height = (double)values[2];
            if ((width == 0.0) || (height == 0.0))
            {
                return null;
            }
            Grid visual = new Grid();
            visual.Width = width;
            visual.Height = height;
            ColumnDefinition colDefinition1 = new ColumnDefinition();
            ColumnDefinition colDefinition2 = new ColumnDefinition();
            ColumnDefinition colDefinition3 = new ColumnDefinition();
            colDefinition1.Width = new GridLength(1.0, GridUnitType.Star);
            colDefinition2.Width = new GridLength(pixels);
            colDefinition3.Width = new GridLength(1.0, GridUnitType.Star);
            visual.ColumnDefinitions.Add(colDefinition1);
            visual.ColumnDefinitions.Add(colDefinition2);
            visual.ColumnDefinitions.Add(colDefinition3);
            RowDefinition rowDefinition1 = new RowDefinition();
            RowDefinition rowDefinition2 = new RowDefinition();
            rowDefinition1.Height = new GridLength(height / 2.0);
            rowDefinition2.Height = new GridLength(1.0, GridUnitType.Star);
            visual.RowDefinitions.Add(rowDefinition1);
            visual.RowDefinitions.Add(rowDefinition2);
            Rectangle rectangle1 = new Rectangle();
            Rectangle rectangle2 = new Rectangle();
            Rectangle rectangle3 = new Rectangle();
            rectangle1.Fill = Brushes.Black;
            rectangle2.Fill = Brushes.Black;
            rectangle3.Fill = Brushes.Black;
            Grid.SetRowSpan(rectangle1, 2);
            Grid.SetRow(rectangle1, 0);
            Grid.SetColumn(rectangle1, 0);
            Grid.SetRow(rectangle2, 1);
            Grid.SetColumn(rectangle2, 1);
            Grid.SetRowSpan(rectangle3, 2);
            Grid.SetRow(rectangle3, 0);
            Grid.SetColumn(rectangle3, 2);
            visual.Children.Add(rectangle1);
            visual.Children.Add(rectangle2);
            visual.Children.Add(rectangle3);
            return new VisualBrush(visual);
        }

        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value that is produced by the binding target.</param>
        /// <param name="targetTypes">The type to convert to.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return new object[] { Binding.DoNothing };
        }
        #endregion
    }
}
