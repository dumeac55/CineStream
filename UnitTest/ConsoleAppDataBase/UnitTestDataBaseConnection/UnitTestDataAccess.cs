/**************************************************************************
 *                                                                        *
 *  File:        UnitTestDataAccess.cs                                    *
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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PersonalInformations;
using Movies;
using DataBaseConnection;
using System.Globalization;

namespace UnitTestDataBaseConnection
{
    [TestClass]
    public class UnitTestDataAccess
    {
        private DataAccess _dataAccess = new DataAccess();

        /// <summary>
        /// Metoda de generare a unui sir de caractere
        /// </summary>
        /// <param name="length">lungimea sirului</param>
        /// <returns>sirul generat</returns>
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
        /// Metoda de testare pentru inserarea unei Persoane
        /// </summary>
        [TestMethod]
        public void InsertUserTest()
        {
            string username = GenerateRandomString(8);
            string password = "random1_password11";
            string email = GenerateRandomString(8)+"@example.com"; 
            string firstName = "Random";
            string lastName = "Person";
            try
            {
                Person person = new Person(username, password, email, firstName, lastName);
                _dataAccess.InsertUser(person);
                Person retrievedPerson = _dataAccess.ReadPersonByUsername(username);
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
        }

        /// <summary>
        /// Metoda de testare pentru citirea datelor unei persoane
        /// </summary>
        [TestMethod]
        public void ReadByUsernameTest()
        {
            Person person = _dataAccess.ReadPersonByUsername("random_username_");
            Assert.AreEqual(person.Username, "random_username_");
        }

        /// <summary>
        /// Metoda de testare pentru inserarea unui film
        /// </summary>
        [TestMethod]
        public void InsertMovieTest()
        {
            string title = "the God Father";
            int year = 1999;
            List<string> genres=new List<string>{"dsa"};
            string description = "the best movie";
            string imageurl = "hhhhh.com";
            string username = "adsdsa";
            double rating = 9.5;
            try
            {
                Movie movie = new Movie(title, year, genres, description, rating, imageurl);
                _dataAccess.InsertMovie(movie, username);
                List<Movie> movies = _dataAccess.viewMovies(username);
                Assert.AreNotEqual(0,movies.Count());
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
        /// Metoda de testare pentru citirea filmelor unei persoane
        /// </summary>
        [TestMethod]
        public void viewMoviesTest()
        {
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
                _dataAccess.InsertMovie(movie, username);
                List<Movie> movies = _dataAccess.viewMovies(username);
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
