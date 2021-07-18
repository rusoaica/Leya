/// Written by: Yulia Danilova
/// Creation Date: 05th of July, 2021
/// Purpose: View Model for the Change Password view
#region ========================================================================= USING =====================================================================================
using System;
using System.Security;
using System.Threading.Tasks;
using Leya.Models.Core.Security;
using Leya.Infrastructure.Enums;
using Leya.ViewModels.Common.MVVM;
using Leya.Models.Common.Extensions;
using Leya.Infrastructure.Notification;
using Leya.Infrastructure.Configuration;
#endregion

namespace Leya.ViewModels.Register
{
    public class ChangePasswordVM : BaseModel, IChangePasswordVM
    {
        #region ============================================================== FIELD MEMBERS ================================================================================
        private readonly IAuthentication authentication;
        #endregion

        #region ============================================================= BINDING COMMANDS ==============================================================================
        public SyncCommand ContentRendered_Command { get; private set; }
        public AsyncCommand ChangePasswordAsync_Command { get; private set; }
        #endregion

        #region ============================================================ BINDING PROPERTIES =============================================================================
        private SecureString oldPassword = new SecureString();
        public SecureString OldPassword
        {
            private get { return oldPassword; }
            set { oldPassword = value; authentication.User.Password = value; }
        }

        private SecureString newPassword = new SecureString();
        public SecureString NewPassword
        {
            private get { return newPassword; }
            set { newPassword = value; }
        }

        private SecureString newPasswordConfirm = new SecureString();
        public SecureString NewPasswordConfirm
        {
            private get { return newPasswordConfirm; }
            set { newPasswordConfirm = value; }
        }

        private string username = string.Empty;
        public string Username
        {
            get { return username; }
            set { username = value; Notify(); authentication.User.Username = value; }
        }
        #endregion

        #region ================================================================ PROPERTIES =================================================================================
        public string Id { set { Username = value; } }
        #endregion

        #region ================================================================== CTOR =====================================================================================
        /// <summary>
        /// Overload C-tor
        /// </summary>
        /// <param name="authentication">The injected authentication domain model</param>
        /// <param name="notificationService">Injected notification service</param>
        public ChangePasswordVM(IAuthentication authentication, INotificationService notificationService)
        {
            ContentRendered_Command = new SyncCommand(Window_ContentRendered);
            ChangePasswordAsync_Command = new AsyncCommand(ChangePasswordAsync, ValidateChangePassword);
            this.authentication = authentication;
            this.notificationService = notificationService;
        }
        #endregion

        #region ================================================================= METHODS ===================================================================================
        /// <summary>
        /// Changes the password of a user account
        /// </summary>
        public async Task ChangePasswordAsync()
        {
            ShowProgressBar();
            try
            {
                // test if authentication succeeds (password is correct)
                await authentication.LoginAsync();
                // assign the new password
                authentication.User.Password = NewPassword;
                await authentication.ChangePasswordAsync();
                notificationService.Show("The password has been changed!", "LEYA - Success");
                authentication.RememberLoginCredentials();
                CloseView();
            }
            catch (Exception ex) when (ex is InvalidOperationException || ex is ArgumentException)
            {
                notificationService.Show(ex.Message, "LEYA - Error", NotificationButton.OK, NotificationImage.Error);
            }
            HideProgressBar();
        }

        /// <summary>
        /// Validates the required information for changing a password
        /// </summary>
        /// <returns>True if required information is fine, False otherwise</returns>
        public bool ValidateChangePassword()
        {
            bool isValid = !string.IsNullOrEmpty(Username) && 
                OldPassword != null && OldPassword.Length > 0 && 
                NewPassword != null && NewPassword.Length > 0 && 
                NewPasswordConfirm != null && NewPasswordConfirm.Length > 0 &&
                NewPassword.IsSecureStringEqual(NewPasswordConfirm);
            if (!isValid)
            {
                ShowHelpButton();
                WindowHelp = "\n";
                if (string.IsNullOrWhiteSpace(Username))
                    WindowHelp += "Username cannot be empty!\n";
                if (OldPassword == null || OldPassword.Length == 0)
                    WindowHelp += "Current password cannot be empty!\n";
                if (NewPassword == null || NewPassword.Length == 0)
                    WindowHelp += "Password cannot be empty!\n";
                if (NewPasswordConfirm == null || NewPasswordConfirm.Length == 0)
                    WindowHelp += "Password Confirm cannot be empty!\n";
                if (NewPassword != null && NewPasswordConfirm != null && !NewPassword.IsSecureStringEqual(NewPasswordConfirm))
                    WindowHelp += "Password and Password Confirm do not match!\n";
            }
            else
                HideHelpButton();
            return isValid;
        }

        /// <summary>
        /// Displays the help for the current window
        /// </summary>
        public override void ShowHelp()
        {
            notificationService.Show(WindowHelp, "LEYA - Help");
        }
        #endregion

        #region ============================================================= EVENT HANDLERS ================================================================================
        /// <summary>
        /// Handles the ContentRendered event of the view
        /// </summary>
        private void Window_ContentRendered()
        {
            WindowTitle = "LEYA - Change password";
            try
            {
                ShowProgressBar();
                // get the account details based on the provided username
                authentication.GetUserAsync(username);
                HideProgressBar();
            }
            catch (Exception ex) when (ex is InvalidOperationException)
            {
                notificationService.Show(ex.Message, "LEYA - Error", NotificationButton.OK, NotificationImage.Error);
            }
        }
        #endregion
    }
}
