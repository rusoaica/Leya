/// Written by: Yulia Danilova
/// Creation Date: 18th of November, 2020
/// Purpose: Business model for authentication
#region ========================================================================= USING =====================================================================================
using System;
using Leya.DataAccess;
using System.Threading.Tasks;
using Leya.Infrastructure.Security;
using Leya.Models.Common.Models.Users;
using Leya.Infrastructure.Notification;
using Leya.Models.Common.Infrastructure;
using Leya.Infrastructure.Configuration;
using Leya.DataAccess.Repositories.Users;
#endregion

namespace Leya.Models.Core.Security
{
    public class Authentication : IAuthentication
    {
        #region ============================================================== FIELD MEMBERS ================================================================================
        private readonly IAppConfig config;
        private readonly IUserRepository userRepository;
        private readonly INotificationService notificationService;
        #endregion

        #region ================================================================ PROPERTIES =================================================================================
        public bool AutoLogin { get; set; }
        public bool RememberCredentials { get; set; }
        public UserEntity User { get; set; } = new UserEntity();
        #endregion

        #region ================================================================== CTOR =====================================================================================
        /// <summary>
        /// Overload C-tor
        /// <param name="unitOfWork">Injected unit of work for interacting with the data access layer repositories</param>
        /// <param name="config">Injected configuration for itneracting with the application's configuration</param>
        /// <param name="notificationService">The injected service used for displaying notifications</param>
        /// </summary>
        public Authentication(IUnitOfWork unitOfWork, IAppConfig config, INotificationService notificationService)
        {
            this.config = config;
            this.notificationService = notificationService;
            userRepository = unitOfWork.GetRepository<IUserRepository>();
        }
        #endregion

        #region ================================================================= METHODS ===================================================================================
        /// <summary>
        /// Updates the password of the current account
        /// </summary>
        public async Task ChangePasswordAsync()
        {
            // make sure the username for whom the password is changed exists in the storage medium
            var result = await userRepository.GetByUsernameAsync(User.Username);
            if (string.IsNullOrEmpty(result.Error))
            {
                // there should be exactly one user with the specified username
                if (result.Count == 1)
                {
                    // update the password of the user
                    result.Data[0].Password = User.Password;
                    // update the user in the storage medium
                    var update = await userRepository.ChangePasswordAsync(User.ToStorageEntity());
                    if (!string.IsNullOrEmpty(update.Error))
                        throw new InvalidOperationException("Error updating the user in the repository: " + result.Error);
                }
                else
                    throw new InvalidOperationException("Cannot update a password of an inexistent account!");
            }
            else
                throw new InvalidOperationException("Error getting the user from the repository: " + result.Error);
        }

        /// <summary>
        /// Verifies the provided credentials against the credentials stored in the storage medium
        /// </summary>
        public async Task LoginAsync()
        {
            // check required members
            if (!string.IsNullOrEmpty(User.Username))
            {
                if (User.Password != null && User.Password.Length > 0)
                {
                    // get the details of the user from the storage medium
                    var result = await userRepository.GetByUsernameAsync(User.Username);
                    if (string.IsNullOrEmpty(result.Error) && result.Data != null)
                    {
                        // check if the provided password coincides with the one from the storage medium
                        if (!PasswordHash.CheckStringAgainstHash(Crypto.Encrypt(User.Password), Uri.UnescapeDataString(result.Data[0].Password)))
                            throw new InvalidOperationException("Invalid username or password!");
                    }
                    else
                        throw new InvalidOperationException("Invalid username or password! " + (result?.Error ?? string.Empty));
                }
                else
                    throw new ArgumentException("The password cannot be empty!", "Passowrd");
            }
            else
                throw new ArgumentException("The username cannot be empty!", "Username");
        }

        /// <summary>
        /// Registers a new username in the storage medium
        /// </summary>
        public async Task RegisterUsernameAsync()
        {
            // check required members
            if (!string.IsNullOrEmpty(User.Username))
            {
                if (User.Password != null && User.Password.Length > 0)
                {
                    if (!string.IsNullOrEmpty(User.SecurityQuestion))
                    {
                        if (User.SecurityAnswer != null && User.SecurityAnswer.Length > 0)
                        {
                            // get the details of the user from the storage medium
                            // make sure the username about to be registered does not already exists in the storage medium
                            var result = await userRepository.GetByUsernameAsync(User.Username);
                            if (string.IsNullOrEmpty(result.Error))
                            {
                                // there should be no other user with the same username
                                if (result.Count == 0)
                                {
                                    // insert the user in the storage medium
                                    var insert = await userRepository.InsertAsync(User.ToStorageEntity());
                                    if (!string.IsNullOrEmpty(insert.Error))
                                        throw new InvalidOperationException("Error inserting the user in the storage medium!");
                                }
                                else
                                    throw new InvalidOperationException("Specified username is already taken!");
                            }
                            else
                                throw new InvalidOperationException("Error connecting to the storage medium! " + (result.Error ?? string.Empty));
                        }
                        else
                            throw new ArgumentException("The security answer cannot be empty!", "SecurityAnswer");
                    }
                    else
                        throw new ArgumentException("The security question cannot be empty!", "SecurityQuestion");
                }
                else
                    throw new ArgumentException("The password cannot be empty!", "Passowrd");
            }
            else
                throw new ArgumentException("The username cannot be empty!", "Username");
        }

        /// <summary>
        /// Recovers an account by checking the security answer against the one stored in the storage medium
        /// </summary>
        public async Task RecoverAccountAsync()
        {
            // check required members
            if (!string.IsNullOrEmpty(User.Username))
            {
                if (User.SecurityAnswerConfirm != null && User.SecurityAnswerConfirm.Length > 0)
                {
                    // get the details of the user from the storage medium
                    var result = await userRepository.GetByUsernameAsync(User.Username);
                    if (string.IsNullOrEmpty(result.Error))
                    {
                        // check if the provided security answer coincides with the one from the storage medium
                        if (!PasswordHash.CheckStringAgainstHash(Crypto.Encrypt(User.SecurityAnswerConfirm), Uri.UnescapeDataString(result.Data[0].SecurityAnswer)))
                            throw new InvalidOperationException("Invalid security answer!");
                        else
                        {
                            User.Password = User.Username;
                            await userRepository.ChangePasswordAsync(User.ToStorageEntity());
                            if (string.IsNullOrEmpty(result.Error))
                                await notificationService.ShowAsync("Your password has been changed to " + User.Username + "!\nChange it to a secure password as soon as you log in!", "LEYA - Success");
                            else
                                throw new InvalidOperationException("Error updating the password of the account! " + (result.Error ?? string.Empty));
                        }
                    }
                    else
                        throw new InvalidOperationException("Error getting the user from the repository! " + (result.Error ?? string.Empty));
                }
                else
                    throw new ArgumentException("The security answer cannot be empty!", "SecurityAnswer");
            }
            else
                throw new ArgumentException("The username cannot be empty!", "Username");
        }

        /// <summary>
        /// Updates the security answer of the current account
        /// </summary>
        public async Task ChangeSecurityAnswerAsync()
        {
            // make sure the username for whom the password is changed exists in the storage medium
            var result = await userRepository.GetByUsernameAsync(User.Username);
            if (string.IsNullOrEmpty(result.Error))
            {
                // there should be exactly one user with the specified username
                if (result.Count == 1)
                {
                    // update the password of the user
                    result.Data[0].SecurityAnswer = User.SecurityAnswer;
                    // update the user in the storage medium
                    var update = await userRepository.UpdateAsync(result.Data[0]);
                    if (!string.IsNullOrEmpty(update.Error))
                        throw new InvalidOperationException("Error updating the user in the repository: " + result.Error);
                }
                else
                    throw new InvalidOperationException("Cannot update the security answer of an inexistent user!");
            }
            else
                throw new InvalidOperationException("Error getting the user from the repository: " + result.Error);
        }

        /// <summary>
        /// Gets the details of an user identified by <see cref="User.Id"/>
        /// </summary>
        public async Task GetUserAsync()
        {
            // the id should always be a positive, non-zero value
            if (User.Id > 0)
            {
                // get the details of the user from the storage medium
                var result = await userRepository.GetByIdAsync(User.Id);
                if (string.IsNullOrEmpty(result.Error))
                    User = Services.AutoMapper.Map<UserEntity>(result.Data[0]);
                else
                    throw new InvalidOperationException("Error getting the user from the repository: " + result.Error);
            }
            else
                throw new InvalidOperationException("Cannot get a user without an ID specified!");
        }

        /// <summary>
        /// Gets the details of an user identified by <paramref name="username"/>
        /// </summary>
        public async Task GetUserAsync(string username)
        {
            // the id should always be a positive, non-zero value
            if (!string.IsNullOrEmpty(username))
            {
                // get the details of the user from the storage medium
                var result = await userRepository.GetByUsernameAsync(username);
                if (string.IsNullOrEmpty(result.Error))
                {
                    if (result.Count > 0)
                        User = Services.AutoMapper.Map<UserEntity>(result.Data[0]);
                    else
                        throw new InvalidOperationException("Specified username does not exist!");
                }
                else
                    throw new InvalidOperationException("Error getting the user from the repository: " + result.Error);
            }
            else
                throw new InvalidOperationException("Cannot get a user without a username specified!");
        }

        /// <summary>
        /// Stores the credentials in the application's configuration file, for later retrieval
        /// </summary>
        public async Task RememberLoginCredentialsAsync()
        {
            if (RememberCredentials)
            {
                // make sure both username and password are set, before accepting "remember credentials" options to be enabled
                if (string.IsNullOrEmpty(User.Username))
                {
                    RememberCredentials = false;
                    AutoLogin = false;
                    throw new InvalidOperationException("For automatic login, enter the username!");
                }
                else if (User.Password.Length == 0)
                {
                    RememberCredentials = false;
                    AutoLogin = false;
                    throw new InvalidOperationException("For automatic login, enter the password!");
                }
                else
                {
                    config.Settings.RememberCredentials = true;
                    config.Settings.Username = Crypto.Encrypt(User.Username);
                    config.Settings.Password = Crypto.Encrypt(User.Password);
                    await config.UpdateConfigurationAsync();
                }
            }
            else
            {
                // if "Remember Credentials" is not enabled, automatic login is not permited
                AutoLogin = false;
                config.Settings.Autologin = false;
                config.Settings.RememberCredentials = false;
                config.Settings.Username = string.Empty;
                config.Settings.Password = string.Empty;
                await config.UpdateConfigurationAsync();
            }
        }

        /// <summary>
        /// Logs in a user automatically, if <see cref="RememberCredentials"/> is True
        /// </summary>
        public async Task AutoLoginAsync()
        {
            if (AutoLogin)
            {
                // we need both a username and a password for automatic login
                if (string.IsNullOrEmpty(User.Username) || User.Password.Length == 0 || !RememberCredentials)
                {
                    RememberCredentials = false;
                    AutoLogin = false;
                    throw new InvalidOperationException("Remember Credentials must be enabled!");
                }
                // save the automatic login state in the application's configuration
                config.Settings.Autologin = true;
                await config.UpdateConfigurationAsync();
                // credentials remembering must be enabled for automatic login to work
                RememberCredentials = true;
                // wait 3 seconds for the user to be able to disable autologin, if so desired
                await Task.Delay(3000);
                await LoginAsync();
            }
            else
            {
                config.Settings.Autologin = false;
                await config.UpdateConfigurationAsync();
            }
        }
        #endregion
    }
}