/// Written by: Yulia Danilova
/// Creation Date: 18th of November, 2020
/// Purpose: View code behind for the Register window
#region ========================================================================= USING =====================================================================================
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using Leya.ViewModels.Register;
#endregion

namespace Leya.Views.Register
{
    /// <summary>
    /// Interaction logic for RegisterV.xaml
    /// </summary>
    public partial class RegisterV : Window, IRegisterView
    {
        #region ================================================================== CTOR =====================================================================================
        /// <summary>
        /// Default C-tor
        /// </summary>
        public RegisterV()
        {
            InitializeComponent();
        }
        #endregion

        #region ============================================================= EVENT HANDLERS ================================================================================
        /// <summary>
        /// Handles PasswordChanged event for password
        /// </summary>
        private void PasswordChanged(object sender, RoutedEventArgs e)
        {
            // since there is no data binding for PasswordBox, update property value here;
            // this doesn't break MVVM patterns, since it's still View binding related code
            if (DataContext != null && (sender as PasswordBox).IsFocused)
            {
                (DataContext as RegisterVM).Password = ((PasswordBox)sender).SecurePassword;
                (DataContext as RegisterVM).RegisterAccount_Command.RaiseCanExecuteChanged();
            }
        }

        /// <summary>
        /// Handles PasswordChanged event for password confirmation
        /// </summary>
        private void ConfirmPasswordChanged(object sender, RoutedEventArgs e)
        {
            // since there is no data binding for PasswordBox, update property value here
            // this doesn't break MVVM patterns, since it's still View binding related code
            if (DataContext != null && (sender as PasswordBox).IsFocused)
            {
                (DataContext as RegisterVM).ConfirmPassword = ((PasswordBox)sender).SecurePassword;
                (DataContext as RegisterVM).RegisterAccount_Command.RaiseCanExecuteChanged();
            }
        }

        /// <summary>
        /// Handles PasswordChanged event for security answer
        /// </summary>
        private void SecurityAnswerChanged(object sender, RoutedEventArgs e)
        {
            // since there is no data binding for PasswordBox, update property value here
            // this doesn't break MVVM patterns, since it's still View binding related code
            if (DataContext != null && (sender as PasswordBox).IsFocused)
            {
                (DataContext as RegisterVM).SecurityAnswer = ((PasswordBox)sender).SecurePassword;
                (DataContext as RegisterVM).RegisterAccount_Command.RaiseCanExecuteChanged();
            }
        }

        /// <summary>
        /// Handles PasswordChanged event for security answer confirmation
        /// </summary>
        private void ConfirmSecurityAnswerChanged(object sender, RoutedEventArgs e)
        {
            // since there is no data binding for PasswordBox, update property value here
            // this doesn't break MVVM patterns, since it's still View binding related code
            if (DataContext != null && (sender as PasswordBox).IsFocused)
            {
                (DataContext as RegisterVM).ConfirmSecurityAnswer = ((PasswordBox)sender).SecurePassword;
                (DataContext as RegisterVM).RegisterAccount_Command.RaiseCanExecuteChanged();
            }
        }

        /// <summary>
        /// Handles Window KeyUp event
        /// </summary>
        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                Close();
        }

        /// <summary>
        /// Closes the window
        /// </summary>
        private void CloseWindow_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Handles Window ContentRendered
        /// </summary>
        private void Window_ContentRendered(object sender, System.EventArgs e)
        {
            // allow closing View from ViewModel without breaking MVVM
            (DataContext as RegisterVM).ClosingView += (sender, e) => Close();
            txtUsername.Focus();
        }
        #endregion
    }
}
