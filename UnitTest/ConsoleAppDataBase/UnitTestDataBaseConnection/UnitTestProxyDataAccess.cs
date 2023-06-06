/**************************************************************************
 *                                                                        *
 *  File:        UnitTestProxyDataAccess.cs                               *
 *  Copyright:   (c) 2023, Dumea Cristian                                 *
 *  E-mail:      cristian.dumea@student.tuiasi.ro                         *
 *  Description: Class that tests the methods that connect to the database*
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
using PersonalInformations;
using Movies;
using DataBaseConnection;
using System.Collections.Generic;


namespace UnitTestDataBaseConnection
{
    [TestClass]
    public class UnitTestProxyDataAccess
    {
        
        private DataAccess _dataAccess;
        private DataAccessSecurityProxy _dataAccessProxy;
        /// <summary>
        /// Crearea obiectelor
        /// </summary>
        public void Setup()
        {
            _dataAccess = new DataAccess();
            _dataAccessProxy = new DataAccessSecurityProxy(_dataAccess);
        }
        /// <summary>
        /// Metoda ce genereaza un sir de caractere
        /// </summary>
        /// <param name="length">Lungimea sirului de caractere</param>
        /// <returns>Sirul generat</returns>
        private string GenerateRandomString(int length)
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            char[] result = new char[length];
            for (int i = 0; i < length; i++)
            {
                result[i] = chars[random.Next(chars.Length)];
            }
            return new string(result);
        }

        /// <summary>
        /// Metoda de testare pentru inserarea unei noi persoane 
        /// </summary>
        [TestMethod]
        public void InsertUserTest()
        {
            Setup();
            string username = GenerateRandomString(8); 
            string password = "Random1_pasd11";
            string email = GenerateRandomString(8)+"@example.com"; 
            string firstName = "Random";
            string lastName = "Person";
            try
            {
                Person person = new Person(username, password, email, firstName, lastName);
                _dataAccessProxy.InsertUser(person);
                Person retrievedPerson = _dataAccessProxy.ReadPersonByUsername(username);
                Assert.IsNotNull(retrievedPerson);
                Assert.AreEqual(username, retrievedPerson.Username);
                Assert.AreEqual(email, retrievedPerson.Email);
                Assert.AreEqual(firstName, retrievedPerson.FirstName);
                Assert.AreEqual(lastName, retrievedPerson.LastName);
            }
            catch (InvalidFormatException ex)
            {
                Assert.Fail(ex.Message);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
            Assert.AreEqual(1, 1);
        }

        /// <summary>
        /// Metoda de testare pentru citirea datelor unei persoane
        /// </summary>
        [TestMethod]
        public void ReadPersonByUserNameTest()
        {
            Setup();

            Person person = _dataAccessProxy.ReadPersonByUsername("random_username_");
            Assert.AreEqual(person.Username, "random_username_");
        }

        /// <summary>
        /// Metoda de testare pentru inserarea unui nou film 
        /// </summary>
        [TestMethod]
        public void InsertMovieTest()
        {
            Setup();
            string title = "the God Father";
            int year = 1999;
            List<string> genres = new List<string> { "dsa" };
            string description = "the best movie";
            string imageurl = "hhhhh.com";
            string username = "adsdsa";
            double rating = 9.5;
            try
            {
                Movie movie = new Movie(title, year, genres, description, rating, imageurl);
                _dataAccessProxy.InsertMovie(movie, username);
                List<Movie> movies = _dataAccessProxy.viewMovies(username);
                Assert.IsNotNull(movies);
                foreach (Movie f in movies)
                {
                    Assert.AreEqual(title, f.Title);
                    Assert.AreEqual(year, f.Year);
                    CollectionAssert.AreEqual(genres, f.Genres);
                    Assert.AreEqual(imageurl, f.ImageUrl);
                    Assert.AreEqual(description, f.Description);
                    Assert.AreEqual(rating, f.Rating);
                }
            }
            catch (InvalidFormatException ex)
            {
                Assert.Fail(ex.Message);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        /// <summary>
        /// Testarea unui email gresit
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidFormatException))]
        public void InsertUserTestWrongEmail()
        {
            Setup();
            string username = "ransername1123_";
            string password = "random1_pasd11";
            string email = "random_emexample.com"; 
            string firstName = "Random";
            string lastName = "Person";
            Person person = new Person(username, password, email, firstName, lastName);
            _dataAccessProxy.InsertUser(person);
        }

        /// <summary>
        /// Testarea prenumelui gresit
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidFormatException))]
        public void InsertUserTestWrongLastName()
        {
            Setup();
            string username = "ransername1123_"; 
            string password = "random1_pasd11";
            string email = "random_emexample.com";
            string firstName = "Random";
            string lastName = "P";
            Person person = new Person(username, password, email, firstName, lastName);
            _dataAccessProxy.InsertUser(person);
        }

        /// <summary>
        /// testarea numelui gresit
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidFormatException))]
        public void InsertUserTestWrongFirstName()
        {
            Setup();
            string username = "ransername1123_";
            string password = "random1_pasd11";
            string email = "random_emexample.com"; 
            string firstName = "R";
            string lastName = "Paa";
            Person person = new Person(username, password, email, firstName, lastName);
            _dataAccessProxy.InsertUser(person);
        }

        /// <summary>
        /// Testarea parolei gresite
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidFormatException))]
        public void InsertUserTestWrongPassword()
        {
            Setup();
            string username = "ransername1123_"; 
            string password = "eas";
            string email = "random_emexample.com"; 
            string firstName = "Ra";
            string lastName = "Paa";
            Person person = new Person(username, password, email, firstName, lastName);
            _dataAccessProxy.InsertUser(person);
        }

        /// <summary>
        /// Testarea numelui de utilizator gresit
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidFormatException))]
        public void InsertUserTestWrongUsername()
        {
            Setup();
            string username = "asd"; 
            string password = "eass23AA";
            string email = "random_emexample.com";
            string firstName = "Ra";
            string lastName = "Paa";
            Person person = new Person(username, password, email, firstName, lastName);
            _dataAccessProxy.InsertUser(person);
        }

        /// <summary>
        /// Metoda de testare pentru citirea filmelor unei persoane
        /// </summary>
        [TestMethod]
        public void viewMovieTest()
        {
            Setup();

            string username = "cristi22";
            List<Movie> movies = new List<Movie>();
            try
            {
                movies = _dataAccessProxy.viewMovies(username);
                Assert.IsNotNull(movies);
            }
            catch (InvalidFormatException ex)
            {
                Assert.Fail(ex.Message);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }
    }
}
