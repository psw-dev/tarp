using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using PSW.RabbitMq;
using PSW.RabbitMq.ServiceCommand;

namespace PSW.ITMS.Common
{
    public class Utility
    {
        private const string AesIV256 = @"!QAZ2WSX#EDC4RFV";
        private const string AesKey256 = @"5TGB&YHN7UJM(IK<5TGB&YHN7UJM(IK<";

        /// <summary>
        /// Gets SHA256 Hash of incoming string
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static String GetSHA256(string value)
        {
            StringBuilder Sb = new StringBuilder();

            using (var hash = SHA256.Create())
            {
                Encoding enc = Encoding.UTF8;
                Byte[] result = hash.ComputeHash(enc.GetBytes(value));

                foreach (Byte b in result)
                    Sb.Append(b.ToString("x2"));
            }

            return Sb.ToString();
        }

        /// <summary>
        /// Checks if incoming type inherits from Generic Type 
        /// </summary>
        /// <param name="givenType"></param>
        /// <param name="genericType"></param>
        /// <returns></returns>
        public static bool IsAssignableToGenericType(Type givenType, Type genericType)
        {
            var interfaceTypes = givenType.GetInterfaces();

            foreach (var it in interfaceTypes)
            {
                if (it.IsGenericType && it.GetGenericTypeDefinition() == genericType)
                    return true;
            }

            if (givenType.IsGenericType && givenType.GetGenericTypeDefinition() == genericType)
                return true;

            Type baseType = givenType.BaseType;
            if (baseType == null) return false;

            return IsAssignableToGenericType(baseType, genericType);
        }


        /// <summary>
        /// Masks incoming CNICs
        /// </summary>
        /// <param name="CNICs"></param>
        /// <returns>List<string> list of modified CNICs (masked)</returns>
        public static List<string> maskCNIC(List<string> CNICs)
        {
            List<string> maskedCNIC = new List<string>();

            foreach (string cnic in CNICs)
            {
                string sub1 = cnic.Substring(0, 5);
                string sub2 = cnic.Substring(9, 4);
                string masked = sub1 + "****" + sub2;
                maskedCNIC.Add(masked);
            }
            return maskedCNIC;
        }

        public static List<string> maskCNIC(List<string> CNICs, bool removeDash)
        {

            List<string> maskedCNIC = new List<string>();

            // If Present Remove Dashs
            if (removeDash)
                CNICs.ForEach(x => x.Replace("-", ""));

            foreach (string cnic in CNICs)
            {
                string sub1 = cnic.Substring(0, 5);
                string sub2 = cnic.Substring(12, 1);
                string masked = sub1 + "XXXXXXX" + sub2;
                maskedCNIC.Add(masked);
            }

            return maskedCNIC;
        }

        /// <summary>
        /// Encrypts the string using AES256
        /// </summary>
        /// <param name="input"></param>
        /// <returns>AES256 encrypted data</returns>
        public static string AESEncrypt256(string input)
        {
            byte[] encrypted;
            byte[] IV;
            byte[] Salt = GetSalt();
            byte[] Key = CreateKey(AesKey256, Salt);

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.Padding = PaddingMode.PKCS7;
                aesAlg.Mode = CipherMode.CBC;

                aesAlg.GenerateIV();
                IV = aesAlg.IV;

                var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (var msEncrypt = new MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (var swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(input);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }
            byte[] combinedIvSaltCt = new byte[Salt.Length + IV.Length + encrypted.Length];
            Array.Copy(Salt, 0, combinedIvSaltCt, 0, Salt.Length);
            Array.Copy(IV, 0, combinedIvSaltCt, Salt.Length, IV.Length);
            Array.Copy(encrypted, 0, combinedIvSaltCt, Salt.Length + IV.Length, encrypted.Length);

            return Convert.ToBase64String(combinedIvSaltCt.ToArray());
        }
        /// <summary>
        /// Decrypts data using AES256
        /// </summary>
        /// <param name="input"></param>
        /// <returns>AES256 decrypted data</returns>
        public static string AESDecrypt256(string input)
        {
            byte[] inputAsByteArray;
            string plaintext = null;
            try
            {
                inputAsByteArray = Convert.FromBase64String(input);

                byte[] Salt = new byte[32];
                byte[] IV = new byte[16];
                byte[] Encoded = new byte[inputAsByteArray.Length - Salt.Length - IV.Length];

                Array.Copy(inputAsByteArray, 0, Salt, 0, Salt.Length);
                Array.Copy(inputAsByteArray, Salt.Length, IV, 0, IV.Length);
                Array.Copy(inputAsByteArray, Salt.Length + IV.Length, Encoded, 0, Encoded.Length);

                byte[] Key = CreateKey(AesKey256, Salt);

                using (Aes aesAlg = Aes.Create())
                {
                    aesAlg.Key = Key;
                    aesAlg.IV = IV;
                    aesAlg.Mode = CipherMode.CBC;
                    aesAlg.Padding = PaddingMode.PKCS7;

                    ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                    using (var msDecrypt = new MemoryStream(Encoded))
                    {
                        using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {
                            using (var srDecrypt = new StreamReader(csDecrypt))
                            {
                                plaintext = srDecrypt.ReadToEnd();
                            }
                        }
                    }
                }
                return plaintext;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Create key
        /// </summary>
        /// <param name="password"></param>
        /// <param name="salt"></param>
        /// <returns>Key</returns>
        public static byte[] CreateKey(string password, byte[] salt)
        {
            using (var rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, salt))
                return rfc2898DeriveBytes.GetBytes(32);
        }

        /// <summary>
        /// Return random salt
        /// </summary>
        /// <returns>salt</returns>
        private static byte[] GetSalt()
        {
            var salt = new byte[32];
            using (var random = new RNGCryptoServiceProvider())
            {
                random.GetNonZeroBytes(salt);
            }
            return salt;
        }

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        } 
    }



}
