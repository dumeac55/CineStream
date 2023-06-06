/**************************************************************************
 *                                                                        *
 *  File:        UnitTestCryptograph.cs                                   *
 *  Copyright:   (c) 2023, Dumea Cristian                                 *
 *  E-mail:      cristian.dumea@student.tuiasi.ro                         *
 *  Description: Class that tests the connection to the database          *
 *                                                                        *
 *  This code and information is provided "as is" without warranty of     *
 *  any kind, either expressed or implied, including but not limited      *
 *  to the implied warranties of merchantability or fitness for a         *
 *  particular purpose. You are free to use this source code in your      *
 *  applications as long as the original copyright notice is included.    *
 *                                                                        *
 **************************************************************************/
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Cryptography;

namespace UnitTestCryptography
{
    [TestClass]
    public class UnitTestCryptograph
    {
        [TestMethod]
        public void EncryptAndDecryptString_Success()
        {
            // test care verifica criptarea si decriptarea corecta

            // Arrange
            string key = "b14ca5898a4e4133bbce2ea2315a1916";
            string plainText = "Secret!";
            Cryptograph cryptograph = new Cryptograph();

            // Act
            string encryptedText = cryptograph.EncryptString(key, plainText);
            string decryptedText = cryptograph.DecryptString(key, encryptedText);

            // Assert
            Assert.AreEqual(plainText, decryptedText);
        }
    }
}
