/**************************************************************************
 *                                                                        *
 *  File:        cryptograph.cs                                           *
 *  WebSite:      https://www.c-sharpcorner.com/article/encryption-and-de *
                  cryption-using-a-symmetric-key-in-c-sharp/              *
                  Adaptation by Dumea Cristian                            *
 *  Description: Class that performs password encryption/decryption       *
 *                                                                        *
 *  This code and information is provided "as is" without warranty of     *
 *  any kind, either expressed or implied, including but not limited      *
 *  to the implied warranties of merchantability or fitness for a         *
 *  particular purpose. You are free to use this source code in your      *
 *  applications as long as the original copyright notice is included.    *
 *                                                                        *
 **************************************************************************/
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Cryptography
{
    /// <summary>
    /// Clasa responsabilă de criptarea și decriptarea șirurilor de caractere utilizând algoritmul AES.
    /// </summary>
    public class Cryptograph
    {
        public string key
        {
            get; set;
        }

        public Cryptograph()
        {
            // Cheia de criptare implicită
            key = "b14ca5898a4e4133bbce2ea2315a1916";
        }

        /// <summary>
        /// Metoda pentru criptarea unui șir de caractere folosind cheia specificată.
        /// </summary>
        /// <param name="key">Cheia de criptare</param>
        /// <param name="plainText">Șirul de caractere de criptat</param>
        /// <returns>Șirul de caractere criptat</returns>
        public string EncryptString(string key, string plainText)
        {
            byte[] iv = new byte[16];
            byte[] array;

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iv;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter(cryptoStream))
                        {
                            streamWriter.Write(plainText);
                        }

                        array = memoryStream.ToArray();
                    }
                }
            }

            return Convert.ToBase64String(array);
        }

        /// <summary>
        /// Metoda pentru decriptarea unui șir de caractere criptat folosind cheia specificată.
        /// </summary>
        /// <param name="key">Cheia de criptare</param>
        /// <param name="cipherText">Șirul de caractere criptat</param>
        /// <returns>Șirul de caractere decriptat</returns>
        public string DecryptString(string key, string cipherText)
        {
            byte[] iv = new byte[16];
            byte[] buffer = Convert.FromBase64String(cipherText);

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iv;
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream(buffer))
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader streamReader = new StreamReader(cryptoStream))
                        {
                            return Convert.ToString(streamReader.ReadToEnd());
                        }
                    }
                }
            }
        }
    }
}
