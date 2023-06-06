/**************************************************************************
 *                                                                        *
 *  File:        UnitTestConnection.cs                                    *
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
using DataBaseConnection;

namespace UnitTestDataBaseConnection
{
    [TestClass]
    public class UnitTestConnection
    {
        /// <summary>
        /// Metoda ce testeaza conexiunea
        /// </summary>
        [TestMethod]
        public void GetsConnectionDone()
        {
            var connection1 = Connection.GetConnection();
            Assert.IsNotNull(connection1);
            connection1.Close();
        }

        /// <summary>
        /// Metoda ce verifica sablonul Singleton
        /// </summary>
        [TestMethod]
        public void ReturnsSingletonInstance()
        {
            var connection1 = Connection.GetConnection();
            var connection2 = Connection.GetConnection();
            Assert.IsNotNull(connection1);
            Assert.IsNotNull(connection2);
            Assert.AreSame(connection1, connection2);
        }
    }
}