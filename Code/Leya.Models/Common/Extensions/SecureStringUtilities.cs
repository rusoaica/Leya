/// Written by: Yulia Danilova
/// Creation Date: 24th of October, 2019
/// Purpose: SecureString extension method to handle decrypting secure strings to unmanaged memory and zeroing the memory afterwards
/// Remarks: This is the closest you can get to UI security in WPF/MVVM....
#region ========================================================================= USING =====================================================================================
using System;
using System.Security;
using System.Runtime.InteropServices;
#endregion

namespace Leya.Models.Common.Extensions
{
    public static class SecureStringUtilities
    {
        #region ================================================================= METHODS ===================================================================================
        /// <summary>
        /// Converts a <see cref="SecureString"/> into a <see cref="string"/> using unmanaged memory
        /// </summary>
        /// <param name="data">The <see cref="SecureString"/> to be converted</param>
        /// <returns>A <see cref="string"/> containing the decrypted <see cref="SecureString"/> data.</returns>
        public static string ConvertSecureStringToString(this SecureString data)
        {
            // declare a pointer that will hold the address of the unmanaged memory where the SecureString password will be decrypted
            IntPtr pointer = IntPtr.Zero;
            try
            {
                // copy the contents of the managed SecureString password into unmanaged memory - THIS IS STILL A SECURITY VULNERABILITY! (thanks, Microsoft... -_-)
                pointer = Marshal.SecureStringToGlobalAllocUnicode(data);
                return Marshal.PtrToStringUni(pointer);
            }
            finally
            {
                // zero out and free the unmanaged string reference
                Marshal.ZeroFreeGlobalAllocUnicode(pointer);
            }
        }

        /// <summary>
        /// Checks if a <see cref="SecureString"/> is equal to another <see cref="SecureString"/>
        /// </summary>
        /// <param name="operandOne">The <see cref="SecureString"/> to check against</param>
        /// <param name="operandTwo">The <see cref="SecureString"/> to be checked</param>
        /// <returns>True if <paramref name="operandOne"/> and <paramref name="operandTwo"/> are equal.</returns>
        public static bool IsSecureStringEqual(this SecureString operandOne, SecureString operandTwo)
        {
            // declare a pointer that will hold the address of the unmanaged memory where the SecureString password will be decrypted
            IntPtr pointerOne = IntPtr.Zero;
            IntPtr pointerTwo = IntPtr.Zero;
            try
            {
                pointerOne = Marshal.SecureStringToGlobalAllocUnicode(operandOne);
                pointerTwo = Marshal.SecureStringToGlobalAllocUnicode(operandTwo);
                return Marshal.PtrToStringUni(pointerOne) == Marshal.PtrToStringUni(pointerTwo);
            }
            finally
            {
                // zero out and free the unmanaged string reference
                Marshal.ZeroFreeGlobalAllocUnicode(pointerOne);
                Marshal.ZeroFreeGlobalAllocUnicode(pointerTwo);
            }
        }
        #endregion
    }
}
