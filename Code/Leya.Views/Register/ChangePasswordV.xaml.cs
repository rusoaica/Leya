/// Written by: Yulia Danilova
/// Creation Date: 05th of July, 2021
/// Purpose: Code behind for the ChangePasswordV view
#region ========================================================================= USING =====================================================================================
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using Leya.ViewModels.Register;
using System.Threading.Tasks;
using Leya.ViewModels.Common.ViewFactory;
#endregion

namespace Leya.Views.Register
{
    /// <summary>
    /// Interaction logic for ChangePasswordV.xaml
    /// </summary>
    public partial class ChangePasswordV : Window, IChangePasswordView
    {
        #region ================================================================== CTOR =====================================================================================
        /// <summary>
        /// Default C-tor
        /// </summary>
        public ChangePasswordV()
        {
            InitializeComponent();
        }
        #endregion

        #region ============================================================= EVENT HANDLERS ================================================================================
        /// <summary>
        /// Handles PasswordChanged event for old password
        /// </summary>
        private void OldPasswordChanged(object sender, RoutedEventArgs e)
        {
            // since there is no data binding for PasswordBox, update property value here
            // this doesn't break MVVM patterns, since it's still View binding related code
            if (DataContext != null && (sender as PasswordBox).IsFocused)
            {
                (DataContext as ChangePasswordVM).OldPassword = ((PasswordBox)sender).Password;
                (DataContext as ChangePasswordVM).ChangePasswordAsync_Command.RaiseCanExecuteChanged();
            }
        }

        /// <summary>
        /// Handles PasswordChanged event for new password
        /// </summary>
        private void NewPasswordChanged(object sender, RoutedEventArgs e)
        {
            // since there is no data binding for PasswordBox, update property value here
            // this doesn't break MVVM patterns, since it's still View binding related code
            if (DataContext != null && (sender as PasswordBox).IsFocused)
            {
                (DataContext as ChangePasswordVM).NewPassword = ((PasswordBox)sender).Password;
                (DataContext as ChangePasswordVM).ChangePasswordAsync_Command.RaiseCanExecuteChanged();
            }
        }

        /// <summary>
        /// Handles PasswordChanged event for new password confirm
        /// </summary>
        private void NewPasswordConfirmChanged(object sender, RoutedEventArgs e)
        {
            // since there is no data binding for PasswordBox, update property value here
            // this doesn't break MVVM patterns, since it's still View binding related code
            if (DataContext != null && (sender as PasswordBox).IsFocused)
            {
                (DataContext as ChangePasswordVM).NewPasswordConfirm = ((PasswordBox)sender).Password;
                (DataContext as ChangePasswordVM).ChangePasswordAsync_Command.RaiseCanExecuteChanged();
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
            (DataContext as ChangePasswordVM).ClosingView += (sender, e) => Close();
            pwdOldPassword.Focus();
        }

        Task<bool?> IView.ShowDialog()
        {
            throw new System.NotImplementedException();
        }
        #endregion
    }
}
