using System;
using System.Security;
using System.Text;

namespace kokos.WPF.Security
{
    /// <summary>
    /// http://weblogs.asp.net/jgalloway/archive/2008/04/13/encrypting-passwords-in-a-net-app-config-file.aspx
    /// </summary>
    public static class PasswordExtensions
    {
        static readonly byte[] Entropy = Encoding.Unicode.GetBytes("Salt Is Not A Password");

        public static string Encrypt(this string password)
        {
            if (string.IsNullOrEmpty(password))
                return password;

            byte[] encryptedData = System.Security.Cryptography.ProtectedData.Protect(
                Encoding.Unicode.GetBytes(password), Entropy,
                System.Security.Cryptography.DataProtectionScope.CurrentUser);

            return Convert.ToBase64String(encryptedData);
        }

        public static string Decrypt(this string password)
        {
            if (string.IsNullOrEmpty(password))
                return password;

            byte[] decryptedData = System.Security.Cryptography.ProtectedData.Unprotect(
                Convert.FromBase64String(password),  Entropy,
                System.Security.Cryptography.DataProtectionScope.CurrentUser);

            return Encoding.Unicode.GetString(decryptedData);
        }

        public static SecureString ToSecureString(this string input)
        {
            var secure = new SecureString();
            foreach (char c in input)
            {
                secure.AppendChar(c);
            }
            secure.MakeReadOnly();
            return secure;
        }

        public static string ToInsecureString(this SecureString input)
        {
            string returnValue;
            IntPtr ptr = System.Runtime.InteropServices.Marshal.SecureStringToBSTR(input);
            try
            {
                returnValue = System.Runtime.InteropServices.Marshal.PtrToStringBSTR(ptr);
            }
            finally
            {
                System.Runtime.InteropServices.Marshal.ZeroFreeBSTR(ptr);
            }
            return returnValue;
        }
    }
}
