/// Written by: Yulia Danilova
/// Creation Date: 25th of June, 2021
/// Purpose: Code behind for the RecoverPasswordV view
#region ========================================================================= USING =====================================================================================
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using Leya.ViewModels.Register;
#endregion

namespace Leya.Views.Register
{
    /// <summary>
    /// Interaction logic for RecoverPasswordV.xaml
    /// </summary>
    public partial class RecoverPasswordV : Window, IRecoverPasswordView
    {
        #region ================================================================== CTOR =====================================================================================
        /// <summary>
        /// Default C-tor
        /// </summary>
        public RecoverPasswordV()
        {
            InitializeComponent();
        }
        #endregion

        #region ============================================================= EVENT HANDLERS ================================================================================
        // <summary>
        /// Handles PasswordChanged event for security answer
        /// </summary>
        private void SecurityAnswerChanged(object sender, RoutedEventArgs e)
        {
            // since there is no data binding for PasswordBox, update property value here
            // this doesn't break MVVM patterns, since it's still View binding related code
            if (DataContext != null && (sender as PasswordBox).IsFocused)
            {
                (DataContext as RecoverPasswordVM).SecurityAnswer = ((PasswordBox)sender).Password;
                (DataContext as RecoverPasswordVM).RecoverAccountAsync_Command.RaiseCanExecuteChanged();
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
            (DataContext as RecoverPasswordVM).ClosingView += (sender, e) => Close();
            txtSecurityAnswer.Focus();
        }
        #endregion
    }
}
