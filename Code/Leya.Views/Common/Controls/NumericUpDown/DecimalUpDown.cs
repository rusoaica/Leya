/*************************************************************************************
   
   Toolkit for WPF

   Copyright (C) 2007-2018 Xceed Software Inc.

   This program is provided to you under the terms of the Microsoft Public
   License (Ms-PL) as published at http://wpftoolkit.codeplex.com/license 

   For more features, controls, and fast professional support,
   pick up the Plus Edition at https://xceed.com/xceed-toolkit-plus-for-wpf/

   Stay informed: follow @datagrid on Twitter or Like http://facebook.com/datagrids

  ***********************************************************************************/

using System;
using System.Collections;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using Xceed.Wpf.Toolkit.Core.Input;

namespace Xceed.Wpf.Toolkit
{
    public class DecimalUpDown : CommonNumericUpDown<decimal>
    {
        public bool IsChildControlFocused
        {
            get { return (bool)GetValue(IsChildControlFocusedProperty); }
            set { SetValue(IsChildControlFocusedProperty, value); }
        }

       
        public static readonly DependencyProperty IsChildControlFocusedProperty =
            DependencyProperty.Register("IsChildControlFocused", typeof(bool), typeof(DecimalUpDown), new UIPropertyMetadata(false));

      
        #region Constructors

        static DecimalUpDown()
        {
            UpdateMetadata(typeof(DecimalUpDown), 1m, decimal.MinValue, decimal.MaxValue);
        }

        public DecimalUpDown()
          : base(Decimal.TryParse, (d) => d, (v1, v2) => v1 < v2, (v1, v2) => v1 > v2)
        {
            GotFocus += DecimalUpDown_GotFocus;
            LostFocus += DecimalUpDown_LostFocus;
        }

        private void DecimalUpDown_LostFocus(object sender, RoutedEventArgs e)
        {
            IsChildControlFocused = false;
        }

        private void DecimalUpDown_GotFocus(object sender, RoutedEventArgs e)
        {
            IsChildControlFocused = true;
        }

        #endregion //Constructors

        #region Base Class Overrides

        protected override decimal IncrementValue(decimal value, decimal increment)
        {
            return value + increment;
        }

        protected override decimal DecrementValue(decimal value, decimal increment)
        {
            return value - increment;
        }
        #endregion //Base Class Overrides
    }
}
