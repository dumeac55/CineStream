/**************************************************************************
 *                                                                        *
 *  File:        CommedyCommand.cs                                        *
 *  Copyright:   (c) 2023, Fodor Maria-Gabriela                           *
 *  E-mail:      maria-gabriela.fodor@student.tuiasi.ro                   *
 *  Description: Test Class for the MovieList class                       *
 *                                                                        *
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
using MovieHandler;

namespace UnitTestMovieHandler
{
    [TestClass]
    public class UnitTestMovieList
    {
        [TestMethod]
        public void TestMovieListGetter()
        {
            // test  pentru getter

            // Arrange
            List<Movie> movies = new List<Movie>
            {
                new Movie("Movie 1", 2021, new List<string> { "Action" }, "Description 1", 7.5, "https://example.com/image1.jpg"),
                new Movie("Movie 2", 2022, new List<string> { "Comedy" }, "Description 2", 8.0, "https://example.com/image2.jpg")
            };
            MovieList movieList = new MovieList(movies);

            // Act
            List<Movie> result = movieList.Movies;

            // Assert
            CollectionAssert.AreEqual(movies, result);
        }

        [TestMethod]
        public void TestMovieListSetter()
        {
            // test pentru setter

            // Arrange
            List<Movie> movies = new List<Movie>
            {
                new Movie("Movie 1", 2021, new List<string> { "Action" }, "Description 1", 7.5, "https://example.com/image1.jpg"),
                new Movie("Movie 2", 2022, new List<string> { "Comedy" }, "Description 2", 8.0, "https://example.com/image2.jpg")
            };
            MovieList movieList = new MovieList(new List<Movie>());

            // Act
            movieList.SetMovies(movies);

            // Assert
            CollectionAssert.AreEqual(movies, movieList.Movies);
        }

    }
}
