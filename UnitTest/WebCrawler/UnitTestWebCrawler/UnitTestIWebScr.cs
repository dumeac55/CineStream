/**************************************************************************
 *                                                                        *
 *  File:        CommedyCommand.cs                                        *
 *  Copyright:   (c) 2023, Fodor Maria-Gabriela                           *
 *  E-mail:      maria-gabriela.fodor@student.tuiasi.ro                   *
 *  Description: Abstract Class containing the tests for the              *
 *  Web Scraplers                                                         *                       
 *                                                                        *
 *  This code and information is provided "as is" without warranty of     *
 *  any kind, either expressed or implied, including but not limited      *
 *  to the implied warranties of merchantability or fitness for a         *
 *  particular purpose. You are free to use this source code in your      *
 *  applications as long as the original copyright notice is included.    *
 *                                                                        *
 **************************************************************************/

using HtmlAgilityPack;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using WebCrawl;
using Movies;
using System.Collections.Generic;

namespace UnitTestWebCrawler
{
    /// <summary>
    /// Clasa moștenită de clasele de test.
    ///
    /// Testul este ales pe datele filmului clasat pe locul 1
    /// astfel încât atât testul de an, cât și cel de gen vor
    /// returna aceleași date.
    ///
    /// Testele vor fi picate doar dacă:
    /// 1. filmul de pe locul 1 în top250 nu mai este "Închisoarea îngerilor"
    /// 2. structura HTML a paginilor IMDb a fost modificată, caz în care implementările trebuie adaptate,
    /// dar nu trebuie modificate clasele ce moștenesc IWebScraper
    /// 3. a fost efectuată o modificare incorectă în cod
    ///
    /// Pentru a verifica prima condiție, accesați linkul:
    /// https://www.imdb.com/chart/top?ref_=tt_awd
    /// </summary>
    public abstract class UnitTestIWebScr
    {
        protected IWebScraper _webScraper;

        public abstract void Init();

        [TestMethod]
        public void ValidMovie()
        {
            // Metoda pentu testarea returnarii corecte a unui obiect de tip Movie

            Movie returnedMovie = _webScraper.GetMovies(1).Movies[0];

            Assert.AreEqual("Închisoarea îngerilor", returnedMovie.Title);
            Assert.AreEqual(1994, returnedMovie.Year);
            Assert.AreEqual(9.3, returnedMovie.Rating, 0.15);
            Assert.AreEqual("Drama", returnedMovie.Genres[0]);
            Assert.AreEqual("Over the course of several years, two convicts form a friendship," +
                " seeking consolation and, eventually, redemption through basic compassion.", 
                returnedMovie.Description);
        }

        [TestMethod]
        public void RelevantNumberOfMoviesReturned()
        {
            // Metoda care testteaza returnarea unui numar relevant de filme
            // acest lucru presupune maxim 3 filme omise datorită unui format atipic

            MovieList returnedMovie = _webScraper.GetMovies(50);

            Assert.AreEqual(returnedMovie.Movies.Count, 50, 3);
        }
    }
}
