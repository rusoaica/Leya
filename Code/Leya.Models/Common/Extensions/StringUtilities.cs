/// Written by: Yulia Danilova
/// Creation Date: 25th of June, 2021
/// Purpose: String extension method to handle conversions between string and securestring
#region ========================================================================= USING =====================================================================================
using System.Security;
#endregion

namespace Leya.Models.Common.Extensions
{
    public static class StringUtilities
    {
        #region ================================================================= METHODS ===================================================================================
        /// <summary>
        /// Converts a <see cref="string"/> into a <see cref="SecureString"/> 
        /// </summary>
        /// <param name="data">The <see cref="string"/> to be converted</param>
        /// <returns>A <see cref="SecureString"/> containing the encrypted <see cref="string"/> data.</returns>
        public static SecureString ToSecureString(this string data)
        {
            SecureString secureString = new SecureString();
            char[] chars = data.ToCharArray();
            foreach (char c in chars)
                secureString.AppendChar(c);
            return secureString;
        }
        #endregion
    }
}
