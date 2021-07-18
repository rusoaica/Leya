/// Written by: Yulia Danilova
/// Creation Date: 12th of November, 2020
/// Purpose: View Model for the startup login view
#region ========================================================================= USING =====================================================================================
using System;
using System.Security;
using Leya.ViewModels.Main;
using System.Threading.Tasks;
using Leya.ViewModels.Register;
using Leya.Infrastructure.Enums;
using Leya.Models.Core.Security;
using Leya.ViewModels.Common.MVVM;
using Leya.Infrastructure.Security;
using Leya.Infrastructure.Notification;
using Leya.Infrastructure.Configuration;
using Leya.ViewModels.Common.ViewFactory;
#endregion

namespace Leya.ViewModels.Startup
{
    public class StartupVM : BaseModel, IStartupVM
    {
        #region ============================================================== FIELD MEMBERS ================================================================================
        private readonly IAppConfig config;
        private readonly IViewFactory viewFactory;
        private readonly IAuthentication authentication;
        #endregion

        #region ============================================================= BINDING COMMANDS ==============================================================================
        public AsyncCommand LoginAsync_Command { get; private set; }
        public SyncCommand ChangePassword_Command { get; private set; }
        public SyncCommand ContentRendered_Command { get; private set; }
        public SyncCommand RecoverPassword_Command { get; private set; }
        public SyncCommand RegisterUsername_Command { get; private set; }
        public SyncCommand RememberCredentials_Command { get; private set; }
        public IAsyncCommand AutoLoginAsync_Command { get; private set; }
        #endregion

        #region ============================================================ BINDING PROPERTIES =============================================================================
        public SecureString Password
        {
            private get { return authentication.User.Password; }
            set 
            {
                authentication.User.Password = value;
                if (value.Length == 0)
                {
                    RememberCredentials = false;
                    AutoLogin = false;
                }
                LoginAsync_Command?.RaiseCanExecuteChanged(); 
            }
        }

        public string Username
        {
            get { return authentication.User.Username; }
            set 
            {
                authentication.User.Username = value;
                Notify();
                if (string.IsNullOrEmpty(value))
                {
                    RememberCredentials = false;
                    AutoLogin = false;
                }
                LoginAsync_Command.RaiseCanExecuteChanged();
                RecoverPassword_Command.RaiseCanExecuteChanged();
                ChangePassword_Command.RaiseCanExecuteChanged();
            }
        }

        private bool isWindowVisible = true;
        public bool IsWindowVisible
        {
            get { return isWindowVisible; }
            set { isWindowVisible = value; Notify(); }
        }

        public bool AutoLogin
        {
            get { return authentication.AutoLogin; }
            set { authentication.AutoLogin = value;  Notify(); }
        }

        public bool RememberCredentials
        {
            get { return authentication.RememberCredentials; }
            set { authentication.RememberCredentials = value; Notify(); }
        }
        #endregion

        #region ================================================================== CTOR =====================================================================================
        /// <summary>
        /// Overload C-tor
        /// <param name="authentication">The injected authentication domain model</param>
        /// <param name="notificationService">Injected notification service</param>
        /// <param name="viewFactory">The injected abstract factory for creating views</param>
        /// <param name="config">The injected application's configuration</param>
        /// </summary>
        public StartupVM(IAuthentication authentication, INotificationService notificationService, IViewFactory viewFactory, IAppConfig config)
        {
            this.config = config;
            this.viewFactory = viewFactory;
            this.authentication = authentication;
            this.notificationService = notificationService;
            AutoLoginAsync_Command = new AsyncCommand(AutologinAsync);
            ContentRendered_Command = new SyncCommand(ContentRendered);
            RegisterUsername_Command = new SyncCommand(RegisterUsername);
            LoginAsync_Command = new AsyncCommand(LoginAsync, ValidateLogin);
            RememberCredentials_Command = new SyncCommand(UpdateRememberCredentials);
            ChangePassword_Command = new SyncCommand(ChangePassword, ValidateRecoverPassword);
            RecoverPassword_Command = new SyncCommand(RecoverPassword, ValidateRecoverPassword);
        }
        #endregion

        #region ================================================================= METHODS ===================================================================================
        /// <summary>
        /// Stores the credentials in the application's configuration file, for later retrival
        /// </summary>
        private void UpdateRememberCredentials()
        {
            try
            {
                authentication.RememberLoginCredentials();
            }
            catch (Exception ex) when (ex is InvalidOperationException || ex is ArgumentException)
            {
                notificationService.Show(ex.Message, "LEYA - Error", NotificationButton.OK, NotificationImage.Error);
            }
            if (string.IsNullOrEmpty(Username) || Password.Length == 0)
            {
                RememberCredentials = false;
                AutoLogin = false;
            }
        }

        /// <summary>
        /// Logs in a user automatically, if <see cref="RememberCredentials"/> is True
        /// </summary>
        private async Task AutologinAsync()
        {
            await authentication.AutoLoginAsync();
            if (string.IsNullOrEmpty(Username) || Password.Length == 0)
            {
                RememberCredentials = false;
                AutoLogin = false;
            }
        }

        /// <summary>
        /// Performs the login
        /// </summary>
        public async Task LoginAsync()
        {
            ShowProgressBar();
            try
            {
                // authenticate the user
                await authentication.LoginAsync();
                // if login was successful, hide this view and display the main view as modal
                IsWindowVisible = false;
                viewFactory.CreateView<IMainWindowView>().ShowDialog();
                IsWindowVisible = true;
            }
            catch (Exception ex) when (ex is InvalidOperationException || ex is ArgumentException)
            {
                notificationService.Show(ex.Message, "LEYA - Error", NotificationButton.OK, NotificationImage.Error);
            }
            HideProgressBar();
        }

        /// <summary>
        /// Validates the required information for Login
        /// </summary>
        /// <returns>True if required information is fine, False otherwise</returns>
        private bool ValidateLogin()
        {
            bool isValid = !string.IsNullOrEmpty(Username) && Password.Length > 0;
            if (!isValid)
            {
                ShowHelpButton();
                WindowHelp = "\n";
                if (string.IsNullOrEmpty(Username))
                    WindowHelp += "Username cannot be empty!\n";
                if (Password.Length == 0)
                    WindowHelp += "Password cannot be empty!\n";
            }
            else
                HideHelpButton();
            return isValid;
        }

        /// <summary>
        /// Validates the required information for recovering the password of an account
        /// </summary>
        /// <returns>True if required information is fine, False otherwise</returns>
        private bool ValidateRecoverPassword()
        {
            return !string.IsNullOrEmpty(Username);
        }

        /// <summary>
        /// Opens up the Register View
        /// </summary>
        private void RegisterUsername()
        {
            IsWindowVisible = false;
            viewFactory.CreateView<IRegisterView>().ShowDialog();
            IsWindowVisible = true;
        }

        /// <summary>
        /// Opens up the Recover Password view
        /// </summary>
        private void RecoverPassword()
        {
            IsWindowVisible = false;
            viewFactory.CreateView<IRecoverPasswordView>(Username).ShowDialog();
            IsWindowVisible = true;
        }

        /// <summary>
        /// Opens up the Change Password view
        /// </summary>
        private void ChangePassword()
        {
            IsWindowVisible = false;
            viewFactory.CreateView<IChangePasswordView>(Username).ShowDialog();
            IsWindowVisible = true;
        }
        #endregion

        #region ============================================================= EVENT HANDLERS ================================================================================
        /// <summary>
        /// Handles the ContentRendered event of the view
        /// </summary>
        private void ContentRendered()
        {
            if (!string.IsNullOrEmpty(config.Settings.Username))
                Username = Crypto.Decrypt(config.Settings.Username);
            if (config.Settings.RememberCredentials)
            {
                RememberCredentials = true;
                if (config.Settings.Autologin)
                    AutoLogin = true;
            }
            WindowTitle = "LEYA - Authentication";
        }
        #endregion
    }
}
