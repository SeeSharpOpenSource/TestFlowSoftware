using System.Security.Cryptography;
using System.Text;
using TestStation.Authentication.Data;

namespace TestStation.Authentication
{
    internal static class EncryptionHandler
    {
        internal static string Encrypt(AuthenticationSession session, string password)
        {
            char[] hashValueMapping = new char[]
            {'0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F'};
            byte[] hashValues;
            using (SHA256 sha256 = SHA256.Create())
            {
                string featureStr = $"{session.UserGroup}_#_{session.UserName}_#_{session.CreationTime}_#_{password}";
                hashValues = sha256.ComputeHash(Encoding.UTF8.GetBytes(featureStr));
            }
            StringBuilder hashStr = new StringBuilder(256);
            foreach (byte hashValue in hashValues)
            {
                hashStr.Append(hashValueMapping[(hashValue >> 4) & 0x0f]).Append(hashValueMapping[hashValue & 0x0f]);
            }
            return hashStr.ToString();
        }

        internal static string Encrypt(UserInfo userInfo, string password)
        {
            char[] hashValueMapping = new char[]
            {'0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F'};
            byte[] hashValues;
            using (SHA256 sha256 = SHA256.Create())
            {
                string featureStr = $"{userInfo.UserGroup}_#_{userInfo.Name}_#_{userInfo.CreationTime}_#_{password}";
                hashValues = sha256.ComputeHash(Encoding.UTF8.GetBytes(featureStr));
            }
            StringBuilder hashStr = new StringBuilder(256);
            foreach (byte hashValue in hashValues)
            {
                hashStr.Append(hashValueMapping[(hashValue >> 4) & 0x0f]).Append(hashValueMapping[hashValue & 0x0f]);
            }
            return hashStr.ToString();
        }
    }
}