/// Written by: the Tranxition team
/// Creation Date: 12th of October, 2008
/// Purpose: Attached behavior for checking if a TextBlock's text is trimmed or not
/// Remark: http://web.archive.org/web/20130316081653/http://tranxcoder.wordpress.com/2008/10/12/customizing-lookful-wpf-controls-take-2/
#region ========================================================================= USING =====================================================================================
using System;
using System.Windows;
using System.Globalization;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Threading;
#endregion

namespace Leya.Views.Common.AttachedProperties
{
    public static class TrimmedTextBehavior
    {
        #region ========================================================== DEPENDENCY PROPERTIES ============================================================================
        private static readonly DependencyPropertyKey IsTextTrimmedKey = DependencyProperty.RegisterAttachedReadOnly("IsTextTrimmed", typeof(bool), typeof(TrimmedTextBehavior),
            new PropertyMetadata(false));

        public static readonly DependencyProperty AutomaticToolTipEnabledProperty = DependencyProperty.RegisterAttached("AutomaticToolTipEnabled", typeof(bool), typeof(TrimmedTextBehavior),
            new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.Inherits));
        
        public static readonly DependencyProperty IsTextTrimmedProperty = IsTextTrimmedKey.DependencyProperty;
        #endregion

        #region ================================================================== CTOR =====================================================================================
        /// <summary>
        /// Static C-tor
        /// </summary>
        static TrimmedTextBehavior()
        {
            // register for the SizeChanged event on all TextBlocks, even if the event was handled.
            EventManager.RegisterClassHandler(typeof(TextBlock), FrameworkElement.SizeChangedEvent, new SizeChangedEventHandler(OnTextBlockSizeChanged), true);
        }
        #endregion

        #region ================================================================= METHODS ===================================================================================
        /// <summary>
        /// Determines whether <paramref name="target"/>'s text is trimmed or not
        /// </summary>
        /// <param name="target">The TextBlock whose text is checked if it is trimmed or not</param>
        /// <returns>True if the text of <paramref name="target"/> is trimmed, False otherwise</returns>
        [AttachedPropertyBrowsableForType(typeof(TextBlock))]
        public static bool GetIsTextTrimmed(TextBlock target)
        {
            return (bool)target.GetValue(IsTextTrimmedProperty);
        }

        /// <summary>
        /// Getter for the AutomaticToolTipEnabled attached property
        /// </summary>
        [AttachedPropertyBrowsableForType(typeof(DependencyObject))]
        public static bool GetAutomaticToolTipEnabled(DependencyObject element)
        {
            if (element == null)
                throw new ArgumentNullException("element");
            return (bool)element.GetValue(AutomaticToolTipEnabledProperty);
        }

        /// <summary>
        /// Setter for the AutomaticToolTipEnabled attached property
        /// </summary>
        public static void SetAutomaticToolTipEnabled(DependencyObject element, bool value)
        {
            if (element == null)
                throw new ArgumentNullException("element");
            element.SetValue(AutomaticToolTipEnabledProperty, value);
        }

        /// <summary>
        /// Handles SizeChangedEvent event
        /// </summary>
        private static void OnTextBlockSizeChanged(object sender, SizeChangedEventArgs e)
        {
            TriggerTextRecalculation(sender);
        }

        /// <summary>
        /// Updates the status of the <paramref name="sender"/>'s text being trimmed or not
        /// </summary>
        /// <param name="sender">The TextBlock for which the status is updated</param>
        private static void TriggerTextRecalculation(object sender)
        {
            if (!(sender is TextBlock textBlock))
                return;
            if (TextTrimming.None == textBlock.TextTrimming)
                textBlock.SetValue(IsTextTrimmedKey, false);
            else
            {
                // if this function is called before databinding has finished, the tooltip will never show;
                // this invoke defers the calculation of the text trimming untill after all current pending databinding has completed.
                bool isTextTrimmed = textBlock.Dispatcher.Invoke(() => CalculateIsTextTrimmed(textBlock), DispatcherPriority.DataBind);
                textBlock.SetValue(IsTextTrimmedKey, isTextTrimmed);
            }
        }

        /// <summary>
        /// Checks if the size of the text of <paramref name="textBlock"/> is greater than its visible height
        /// </summary>
        /// <param name="textBlock">The TextBlock whose text height is checked</param>
        /// <returns>True if the height of the text of <paramref name="textBlock"/> is greater than its visible height, False otherwise</returns>
        private static bool CalculateIsTextTrimmed(TextBlock textBlock)
        {
            if (!textBlock.IsArrangeValid)
                return GetIsTextTrimmed(textBlock);
            Size size = MeasureString(textBlock);
            // when the maximum text width of the FormattedText instance is set to the actual width of the textBlock, if the textBlock is being
            // trimmed to fit then the formatted text will report a larger height than the textBlock. Should work whether the textBlock is single or multi-line.
            return size.Height > textBlock.ActualHeight;
        }

        /// <summary>
        /// Measures the size of the text of <paramref name="textBlock"/>
        /// </summary>
        /// <param name="textBlock">The TextBlock whose text size is measured</param>
        /// <returns>The size of the text of <paramref name="textBlock"/></returns>
        private static Size MeasureString(TextBlock textBlock)
        {
            FormattedText formattedText = new FormattedText(textBlock.Text, CultureInfo.CurrentCulture, FlowDirection.LeftToRight,
                new Typeface(textBlock.FontFamily, textBlock.FontStyle, textBlock.FontWeight, textBlock.FontStretch),
                textBlock.FontSize, Brushes.Black, new NumberSubstitution(), 1);
            return new Size(formattedText.Width, formattedText.Height);
        }
        #endregion
    }
}
