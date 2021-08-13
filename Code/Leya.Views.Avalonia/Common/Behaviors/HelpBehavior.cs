﻿/// Written by: Yulia Danilova
/// Creation Date: 31st of July, 2021
/// Purpose: Behavior for windows help button 
#region ========================================================================= USING =====================================================================================
using System;
using Avalonia;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Leya.ViewModels.Common.MVVM;
#endregion

namespace Leya.Views.Common.Behaviors
{
    public class HelpBehavior : AvaloniaObject
    {
        #region ============================================================ ATTACHED PROPERTIES ============================================================================ 
        /// <summary>
        /// Identifies the <seealso cref="CommandProperty"/> avalonia attached property.
        /// </summary>
        /// <value>Provide an <see cref="string"/> derived object or binding.</value>
        public static readonly AttachedProperty<string> CommandProperty = AvaloniaProperty.RegisterAttached<HelpBehavior, Interactive, string>(
            "Command", default!, false, BindingMode.OneTime);

        /// <summary>
        /// Identifies the <seealso cref="CommandParameterProperty"/> avalonia attached property.
        /// Use this as the parameter for the <see cref="CommandProperty"/>.
        /// </summary>
        /// <value>Any value of type <see cref="object"/>.</value>
        public static readonly AttachedProperty<object> CommandParameterProperty = AvaloniaProperty.RegisterAttached<HelpBehavior, Interactive, object>(
            "CommandParameter", default!, false, BindingMode.OneWay, null);
        #endregion

        #region ================================================================== CTOR =====================================================================================
        /// <summary>
        /// Static C-tor
        /// </summary>
        static HelpBehavior()
        {
            CommandProperty.Changed.Subscribe(x => HandleCommandChanged(x.Sender));
        }
        #endregion

        #region ================================================================= METHODS ===================================================================================
        /// <summary>
        /// <see cref="CommandProperty"/> changed event handler.
        /// </summary>
        /// <param name="element">The UI element that initiated the help command</param>
        private static void HandleCommandChanged(IAvaloniaObject element)
        {
            if (element is Interactive interactElem)
                interactElem.AddHandler(InputElement.TappedEvent, Handler);
            // local handler function
            async void Handler(object? s, RoutedEventArgs e)
            {
                // get the parameter off of the gui element
                Window window = interactElem.GetValue(CommandParameterProperty) as Window;
                await (window.DataContext as BaseModel).ShowHelpAsync();
            }
        }

        /// <summary>
        /// Accessor for Attached property <see cref="CommandProperty"/>.
        /// </summary>
        public static void SetCommand(AvaloniaObject element, string commandValue)
        {
            element.SetValue(CommandProperty, commandValue);
        }

        /// <summary>
        /// Accessor for Attached property <see cref="CommandProperty"/>.
        /// </summary>
        public static string GetCommand(AvaloniaObject element)
        {
            return element.GetValue(CommandProperty);
        }

        /// <summary>
        /// Accessor for Attached property <see cref="CommandParameterProperty"/>.
        /// </summary>
        public static void SetCommandParameter(AvaloniaObject element, object parameter)
        {
            element.SetValue(CommandParameterProperty, parameter);
        }

        /// <summary>
        /// Accessor for Attached property <see cref="CommandParameterProperty"/>.
        /// </summary>
        public static object GetCommandParameter(AvaloniaObject element)
        {
            return element.GetValue(CommandParameterProperty);
        }
        #endregion
    }
}
