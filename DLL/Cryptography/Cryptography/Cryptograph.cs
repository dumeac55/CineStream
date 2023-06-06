/**************************************************************************
 *                                                                        *
 *  File:        Conexion.cs                                              *
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
    public class Cryptograph
    {
        public string key
        {
            get; set;
        }
        /// <summary>
        /// Seteaza cheia unica
        /// </summary>
        public Cryptograph()
        {
            key = "b14ca5898a4e4133bbce2ea2315a1916";
        }
        /// <summary>
        /// Cripteaza textul in functie de key
        /// </summary>
        /// <param name="key">cheia unica</param>
        /// <param name="plainText">textul ce trebuie criptat</param>
        /// <returns>textul criptat</returns>
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
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream))
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
        /// Decripteaza textul in functie de key
        /// </summary>
        /// <param name="key">cheia unica</param>
        /// <param name="cipherText">textul ce trebuie decriptat</param>
        /// <returns>textul decriptat</returns>
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
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader streamReader = new StreamReader((Stream)cryptoStream))
                        {
                            return Convert.ToString(streamReader.ReadToEnd());
                        }
                    }
                }
            }
        }
    }
}
