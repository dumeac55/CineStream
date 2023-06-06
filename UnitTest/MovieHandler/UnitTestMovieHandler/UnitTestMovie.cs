/**************************************************************************
 *                                                                        *
 *  File:        CommedyCommand.cs                                        *
 *  Copyright:   (c) 2023, Fodor Maria-Gabriela                           *
 *  E-mail:      maria-gabriela.fodor@student.tuiasi.ro                   *
 *  Description: Test Class for the Movie class                           *
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
using MovieHandler;
using System.Collections.Generic;

namespace UnitTestMovieHandler
{
    [TestClass]
    public class UnitTestMovie
    {
        [TestMethod]
        public void TestMovieConstructor()
        {
            // testeaza constructorul clasei

            // Arrange
            string title = "Închisoarea îngerilor";
            int year = 1994;
            List<string> genres = new List<string> { "Drama", "Crime" };
            string description = "Two imprisoned men bond over several years, finding solace and eventual redemption through acts of common decency.";
            double rating = 9.3;
            string imageUrl = "https://example.com/image.jpg";

            // Act
            Movie movie = new Movie(title, year, genres, description, rating, imageUrl);

            // Assert
            Assert.AreEqual(title, movie.Title);
            Assert.AreEqual(year, movie.Year);
            CollectionAssert.AreEqual(genres, movie.Genres);
            Assert.AreEqual(description, movie.Description);
            Assert.AreEqual(rating, movie.Rating);
            Assert.AreEqual(imageUrl, movie.ImageUrl);
        }

        [TestMethod]
        public void TestMovieRatingRange()
        {
            //testeaza daca rating-ul este o valoare valida 

            // Arrange
            string title = "The Shawshank Redemption";
            int year = 1994;
            List<string> genres = new List<string> { "Drama", "Crime" };
            string description = "Two imprisoned men bond over several years, finding solace and eventual redemption through acts of common decency.";
            double rating = 9.3;
            string imageUrl = "https://example.com/image.jpg";
            Movie movie = new Movie(title, year, genres, description, rating, imageUrl);

            // Assert
            Assert.IsTrue(movie.Rating >= 0 && movie.Rating <= 10);
        }
    }
}
