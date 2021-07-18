/// Written by: Yulia Danilova
/// Creation Date: 20th of October, 2019
/// Purpose: Handles Title Bar double click behavior for Windows
#region ========================================================================= USING =====================================================================================
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
#endregion

namespace Leya.Views.Common.Styles.StyleableWindow
{
    public static class ControlDoubleClickBehavior
    {
        #region ========================================================== DEPENDENCY PROPERTIES ============================================================================
        public static readonly DependencyProperty ExecuteCommand = DependencyProperty.RegisterAttached("ExecuteCommand", typeof(ICommand), typeof(ControlDoubleClickBehavior),
            new UIPropertyMetadata(null, OnExecuteCommandChanged));

        public static readonly DependencyProperty ExecuteCommandParameter = DependencyProperty.RegisterAttached("ExecuteCommandParameter", typeof(Window), typeof(ControlDoubleClickBehavior));
        #endregion

        #region ================================================================= METHODS ===================================================================================
        /// <summary>
        /// Gets the command of <paramref name="obj"/>
        /// </summary>
        public static ICommand GetExecuteCommand(DependencyObject obj)
        {
            return (ICommand)obj.GetValue(ExecuteCommand);
        }

        /// <summary>
        /// Sets the command of <paramref name="command"/>
        /// </summary>
        public static void SetExecuteCommand(DependencyObject obj, ICommand command)
        {
            obj.SetValue(ExecuteCommand, command);
        }

        /// <summary>
        /// Gets the parameter of the <paramref name="obj"/>
        /// </summary>
        public static Window GetExecuteCommandParameter(DependencyObject obj)
        {
            return (Window) obj.GetValue(ExecuteCommandParameter);
        }

        /// <summary>
        /// Sets the parameter of the <paramref name="command"/>
        /// </summary>
        public static void SetExecuteCommandParameter(DependencyObject obj, ICommand command)
        {
            obj.SetValue(ExecuteCommandParameter, command);
        }

        /// <summary>
        /// If the sender is a Control, subscribes the sender to the MouseDoubleClick event 
        /// </summary>
        private static void OnExecuteCommandChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is Control control)
                control.MouseDoubleClick += Control_MouseDoubleClick;
        }

        /// <summary>
        /// Handles Control MouseDoubleClick event
        /// </summary>
        static void Control_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is Control control)
            {
                ICommand command = control.GetValue(ExecuteCommand) as ICommand;
                object commandParameter = control.GetValue(ExecuteCommandParameter);
                if (command.CanExecute(e))
                    command.Execute(commandParameter);
            }
        }
        #endregion
    }
}