/// Written by: Yulia Danilova
/// Creation Date: 25th of July, 2021
/// Purpose:  Code behind for the HorizontalList user control
#region ========================================================================= USING =====================================================================================
using System;
using Avalonia;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.Layout;
using Avalonia.Controls;
using System.Collections;
using Avalonia.Collections;
using Avalonia.Markup.Xaml;
using Leya.Models.Common.Models.Media;
#endregion

namespace Leya.Views.Common.Controls
{
    public partial class HorizontalList : UserControl
    {
        #region ============================================================== FIELD MEMBERS ================================================================================
        public event Action<MediaTypeEntity> Click;

        private readonly Grid container;
        #endregion

        #region ================================================================ PROPERTIES =================================================================================
        public int Count { get; set; }
        #endregion

        #region ========================================================== DEPENDENCY PROPERTIES ============================================================================
        private IEnumerable items = new AvaloniaList<object>();  
        public IEnumerable Items
        {
            get { return items; }
            set { SetAndRaise(ItemsProperty, ref items, value); SetVisualItems(); }
        }

        /// <summary>
        /// Defines the <see cref="Items"/> property.
        /// </summary>
        public static readonly DirectProperty<HorizontalList, IEnumerable> ItemsProperty =
            AvaloniaProperty.RegisterDirect<HorizontalList, IEnumerable>(nameof(Items), e => e.Items, (e, v) => e.Items = v);

        private int itemsOffset;
        public int ItemsOffset
        {
            get { return itemsOffset; }
            set { SetAndRaise(ItemsOffsetProperty, ref itemsOffset, value); SetVisualItems(); }
        }

        /// <summary>
        /// Defines the <see cref="ItemsOffset"/> property.
        /// </summary>
        public static readonly DirectProperty<HorizontalList, int> ItemsOffsetProperty =
            AvaloniaProperty.RegisterDirect<HorizontalList, int>(nameof(ItemsOffset), e => e.ItemsOffset, (e, v) => e.ItemsOffset = v);

        private int scrollSpeed;
        public int ScrollSpeed
        {
            get { return scrollSpeed; }
            set { SetAndRaise(ScrollSpeedProperty, ref scrollSpeed, value); SetVisualItems(); }
        }

        /// <summary>
        /// Defines the <see cref="ScrollSpeed"/> property.
        /// </summary>
        public static readonly DirectProperty<HorizontalList, int> ScrollSpeedProperty =
            AvaloniaProperty.RegisterDirect<HorizontalList, int>(nameof(ScrollSpeed), e => e.ScrollSpeed, (e, v) => e.ScrollSpeed = v);
        #endregion

        #region ================================================================== CTOR =====================================================================================
        /// <summary>
        /// Default C-tor
        /// </summary>
        public HorizontalList()
        {
            AvaloniaXamlLoader.Load(this);
            container = this.Find<Grid>("container");
            PointerWheelChanged += Control_PointerWheelChanged;
        }
        #endregion

        #region ================================================================= METHODS ===================================================================================
        /// <summary>
        /// Sets the child elements of the control
        /// </summary>
        private void SetVisualItems()
        {
            if (items != null)
            {
                Count = 0;
                int offset = 0;
                container.Children.Clear();
                foreach (var item in items)
                {
                    if (item is MediaTypeEntity mediaTypeEntity)
                    {
                        Label childItem = new Label();
                        childItem.Tag = item; 
                        childItem.FontSize = FontSize;
                        childItem.Width = ItemsOffset;
                        childItem.Foreground = Foreground;
                        childItem.Padding = new Thickness(0);
                        childItem.FontWeight = FontWeight.Bold;
                        childItem.Background = Brushes.Transparent;
                        childItem.PointerEnter += ChildItem_PointerEnter;
                        childItem.PointerLeave += ChildItem_PointerLeave;
                        childItem.Margin = new Thickness(offset, 0, 0, 0);
                        childItem.Content = mediaTypeEntity.MediaName.ToUpper();
                        childItem.Cursor = new Cursor(StandardCursorType.Hand);
                        childItem.HorizontalAlignment = HorizontalAlignment.Left;
                        childItem.VerticalContentAlignment = VerticalAlignment.Center;
                        childItem.HorizontalContentAlignment = HorizontalAlignment.Center;
                        childItem.PointerReleased += ChildItem_PointerReleased;
                        container.Children.Add(childItem);
                        offset += ItemsOffset;
                    }
                    Count++;
                }
            }
        }
        #endregion

        #region ============================================================= EVENT HANDLERS ================================================================================
        /// <summary>
        /// Handles the control's PointerWheelChanged event
        /// </summary>
        private void Control_PointerWheelChanged(object? sender, PointerWheelEventArgs e)
        {
            if (e.Delta.Y > 0) // scroll up
            {
                // move container to the right while the leftmost item's left margin is less than half the menu's width
                if (container.Margin.Left < Bounds.Width / 2 - ItemsOffset / 2)
                    container.Margin = new Thickness(container.Margin.Left + ScrollSpeed, container.Margin.Top, container.Margin.Right - ScrollSpeed, container.Margin.Bottom);
            }
            else
            {
                // move container to the left while the rightmost item's right margin is less than half the menu's width
                if (container.Margin.Right < (Count * ItemsOffset) - Bounds.Width / 2 - ItemsOffset / 2)
                    container.Margin = new Thickness(container.Margin.Left - ScrollSpeed, container.Margin.Top, container.Margin.Right + ScrollSpeed, container.Margin.Bottom);
            }
        }

        /// <summary>
        /// Handles the horizontal list element's PointerReleased event
        /// </summary>
        private void ChildItem_PointerReleased(object? sender, PointerReleasedEventArgs e)
        {
            Click?.Invoke((sender as Label).Tag as MediaTypeEntity);
        }

        /// <summary>
        /// Handles the horizontal list element's PointerLeave event
        /// </summary>
        private void ChildItem_PointerLeave(object? sender, PointerEventArgs e)
        {
            (sender as Label).Foreground = Foreground;
        }

        /// <summary>
        /// Handles the horizontal list element's PointerEnter event
        /// </summary>
        private void ChildItem_PointerEnter(object? sender, PointerEventArgs e)
        {
            (sender as Label).Foreground = Brushes.White;
        }
        #endregion
    }
}
