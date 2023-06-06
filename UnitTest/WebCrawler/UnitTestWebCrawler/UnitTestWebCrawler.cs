/**************************************************************************
 *                                                                        *
 *  File:        CommedyCommand.cs                                        *
 *  Copyright:   (c) 2023, Fodor Maria-Gabriela                           *
 *  E-mail:      maria-gabriela.fodor@student.tuiasi.ro                   *
 *  Description: Test Class for testing the WebCrawler class funtionality *                                                      
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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Movies;
using WebCrawl;

namespace UnitTestWebCrawler
{
    [TestClass]
    public class UnitTestWebCrawler
    {
        // Metoda de test pentru returnarea a 5 filme de genul Drama

        [TestMethod]
        public void ReturnsMoviesOfGivenGenre()
        {
            // Arrange
            string genre = "Drama";
            int nrMovies = 5;
            WebCrawler webCrawler = new WebCrawler();

            // Act
            MovieList movies = webCrawler.GetMovies(genre, nrMovies);

            // Assert
            Assert.AreEqual(nrMovies, movies.Movies.Count());
            foreach (Movie movie in movies.Movies)
            {
                CollectionAssert.Contains(movie.Genres, genre);
            }
        }


        // Metoda de test pentru returnarea a 5 filme din anul 2000

        [TestMethod]
        public void ReturnsMoviesReleasedInGivenYear()
        {
            // Arrange
            int year = 2000;
            int nrMovies = 5;
            WebCrawler webCrawler = new WebCrawler();

            // Act
            MovieList movies = webCrawler.GetMovies(year, nrMovies);

            // Assert
            Assert.AreEqual(nrMovies, movies.Movies.Count);
            foreach (Movie movie in movies.Movies)
            {
                Assert.AreEqual(year, movie.Year);
            }
        }


        // Metoda de test pentru returnarea a 5

        [TestMethod]
        public void ReturnsAllMovies()
        {
            // Arrange
            int nrMovies = 10;
            WebCrawler webCrawler = new WebCrawler();

            // Act
            MovieList movies = webCrawler.GetMovies(nrMovies);

            // Assert
            Assert.AreEqual(nrMovies, movies.Movies.Count);
        }
    }
}
