/// Written by: Yulia Danilova
/// Creation Date: 12th of November, 2020
/// Purpose: Deserialization model for the Users storage container
#region ========================================================================= USING =====================================================================================
using System;
using Leya.DataAccess.StorageAccess;
using Leya.DataAccess.Common.Attributes;
#endregion

namespace Leya.DataAccess.Common.Models.Users
{
    public sealed class UserEntity : IStorageEntity
    {
        #region ================================================================ PROPERTIES =================================================================================
        [IgnoreOnInsert]
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string SecurityAnswer { get; set; }
        public string SecurityQuestion { get; set; }
        [IgnoreOnInsert]
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
        #endregion
    }
}
