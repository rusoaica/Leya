/// Written by: Yulia Danilova
/// Creation Date: 16th of June, 2020
/// Purpose: User data transfer object
#region ========================================================================= USING =====================================================================================
using System;
using System.Security;
using Leya.Models.Common.Infrastructure;
#endregion

namespace Leya.Models.Common.Models.Users
{
    public class UserEntity
    {
        #region ================================================================ PROPERTIES =================================================================================
        public int Id { get; set; }
        public string Username { get; set; }
        public string SecurityQuestion { get; set; }
        public string Password { get; set; }
        public string SecurityAnswer { get; set; }
        public string PasswordConfirm { get; set; }
        public string SecurityAnswerConfirm { get; set; }
        public DateTime Created { get; set; }
        #endregion

        #region ================================================================= METHODS ===================================================================================
        /// <summary>
        /// Customized ToString() method
        /// </summary>
        /// <returns>Custom string value showing relevant data for current class</returns>
        public override string ToString()
        {
            return Id + " :: " + Username;
        }

        /// <summary>
        /// Maps between this entity and the coresponding persistance entity
        /// </summary>
        /// <returns>A data storage entity representation of this entity</returns>
        public DataAccess.Common.Models.Users.UserEntity ToStorageEntity()
        {
            return Services.AutoMapper.Map<DataAccess.Common.Models.Users.UserEntity>(this);
        }
        #endregion
    }
}
