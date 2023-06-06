/**************************************************************************
 *                                                                        *
 *  File:        UnitTestPerson.cs                                        *
 *  Copyright:   (c) 2023, Dumea Cristian                                 *
 *  E-mail:      cristian.dumea@student.tuiasi.ro                         *
 *  Description: Class that tests if a person type object is created      *
 *                                                                        *
 *  This code and information is provided "as is" without warranty of     *
 *  any kind, either expressed or implied, including but not limited      *
 *  to the implied warranties of merchantability or fitness for a         *
 *  particular purpose. You are free to use this source code in your      *
 *  applications as long as the original copyright notice is included.    *
 *                                                                        *
 **************************************************************************/
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PersonalInformation;

namespace UnitTestPerson
{
    [TestClass]
    public class UnitTestPerson
    {
        [TestMethod]
        public void TestPersonCreation()
        {
            // test pentru functionarea corecta a constructorului 

            // Arrange
            string username = "maria-gabriela";
            string password = "password";
            string email = "maria.gabriela@example.com";
            string firstName = "Fodor";
            string lastName = "Maria";

            // Act
            Person person = new Person(username, password, email, firstName, lastName);

            // Assert
            Assert.AreEqual(username, person.Username);
            Assert.AreEqual(password, person.Password);
            Assert.AreEqual(email, person.Email);
            Assert.AreEqual(firstName, person.FirstName);
            Assert.AreEqual(lastName, person.LastName);
        }
    }
}
