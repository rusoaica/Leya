﻿/// Written by: Yulia Danilova
/// Creation Date: 18th of November, 2020
/// Purpose: View Model for the Register view
#region ========================================================================= USING =====================================================================================
using System;
using System.Security;
using System.Threading.Tasks;
using Leya.Models.Core.Security;
using Leya.Infrastructure.Enums;
using Leya.ViewModels.Common.MVVM;
using Leya.Models.Common.Extensions;
using Leya.Infrastructure.Notification;
#endregion

namespace Leya.ViewModels.Register
{
    public class RegisterVM : BaseModel, IRegisterVM
    {
        #region ============================================================== FIELD MEMBERS ================================================================================
        private readonly IAuthentication authentication;
        #endregion

        #region ============================================================= BINDING COMMANDS ==============================================================================
        public SyncCommand ContentRendered_Command { get; private set; }
        public AsyncCommand RegisterAccount_Command { get; private set; }
        #endregion

        #region ============================================================ BINDING PROPERTIES =============================================================================
        private SecureString password = new SecureString();
        public SecureString Password
        {
            private get { return password; }
            set { password = value; RegisterAccount_Command?.RaiseCanExecuteChanged(); authentication.User.Password = value; }
        }

        private SecureString securityAnswer = new SecureString();
        public SecureString SecurityAnswer
        {
            private get { return securityAnswer; }
            set { securityAnswer = value; RegisterAccount_Command?.RaiseCanExecuteChanged(); authentication.User.SecurityAnswer = value; }
        }
        
        private string username = string.Empty;
        public string Username
        {
            get { return username; }
            set { username = value; Notify(); RegisterAccount_Command.RaiseCanExecuteChanged(); authentication.User.Username = value; }
        }

        private string securityQuestion = string.Empty;
        public string SecurityQuestion
        {
            get { return securityQuestion; }
            set { securityQuestion = value; Notify(); RegisterAccount_Command.RaiseCanExecuteChanged(); authentication.User.SecurityQuestion = value; }
        }
        #endregion

        #region ================================================================ PROPERTIES =================================================================================
        public SecureString ConfirmPassword { get; set; }
        public SecureString ConfirmSecurityAnswer { get; set; }
        #endregion

        #region ================================================================== CTOR =====================================================================================
        /// <summary>
        /// Overload C-tor
        /// </summary>
        /// <param name="authentication">The injected authentication domain model</param>
        /// <param name="notificationService">Injected notification service</param>
        public RegisterVM(IAuthentication authentication, INotificationService notificationService)
        {
            ContentRendered_Command = new SyncCommand(Window_ContentRendered);
            RegisterAccount_Command = new AsyncCommand(RegisterUsername, ValidateRegister);
            this.authentication = authentication;
            this.notificationService = notificationService;
        }
        #endregion

        #region ================================================================= METHODS ===================================================================================
        /// <summary>
        /// Registers a new user account
        /// </summary>
        public async Task RegisterUsername()
        {
            ShowProgressBar();
            try
            {
                await authentication.RegisterUsernameAsync();
                notificationService.Show("Account created!", "LEYA - Success");
                CloseView();
            }
            catch (Exception ex) when (ex is InvalidOperationException || ex is ArgumentException)
            {
                notificationService.Show(ex.Message, "LEYA - Error", NotificationButton.OK, NotificationImage.Error);
            }
            HideProgressBar();
        }

        /// <summary>
        /// Validates the required information for registering a username
        /// </summary>
        /// <returns>True if required information is fine, False otherwise</returns>
        public bool ValidateRegister()
        {
            bool isValid = !string.IsNullOrEmpty(Username) && Password != null && Password.Length > 0 && ConfirmPassword != null && ConfirmPassword.Length > 0 && SecurityQuestion != null && 
                SecurityQuestion.Length > 0 && SecurityAnswer != null && SecurityAnswer.Length > 0 && Password.IsSecureStringEqual(ConfirmPassword) && ConfirmSecurityAnswer != null && 
                ConfirmSecurityAnswer.Length > 0 && SecurityAnswer.IsSecureStringEqual(ConfirmSecurityAnswer);
            if (!isValid)
            {
                ShowHelpButton();
                WindowHelp = "\n";
                if (string.IsNullOrWhiteSpace(Username))
                    WindowHelp += "Username cannot be empty!\n";
                if (Password == null || Password.Length == 0)
                    WindowHelp += "Password cannot be empty!\n";
                if (ConfirmPassword == null || ConfirmPassword.Length == 0)
                    WindowHelp += "Password Confirm cannot be empty!\n";
                if (Password != null && ConfirmPassword != null && !Password.IsSecureStringEqual(ConfirmPassword))
                    WindowHelp += "Password and Password Confirm do not match!\n";
                if (string.IsNullOrWhiteSpace(SecurityQuestion))
                    WindowHelp += "Security Question cannot be empty!\n";
                if (SecurityAnswer == null || SecurityAnswer.Length == 0)
                    WindowHelp += "Security Answer cannot be empty!\n";
                if (ConfirmSecurityAnswer == null || ConfirmSecurityAnswer.Length == 0)
                    WindowHelp += "Security Answer Confirm cannot be empty!\n";
                if (SecurityAnswer != null && ConfirmSecurityAnswer != null && !SecurityAnswer.IsSecureStringEqual(ConfirmSecurityAnswer))
                    WindowHelp += "Security Answer and Security Answer Confirm do not match!\n";
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
            WindowTitle = "LEYA - Register new account";
        }
        #endregion
    }
}