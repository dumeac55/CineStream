/**************************************************************************
 *                                                                        *
 *  File:        CommedyCommand.cs                                        *
 *  Copyright:   (c) 2023, Fodor Maria-Gabriela                           *
 *  E-mail:      maria-gabriela.fodor@student.tuiasi.ro                   *
 *  Description: Abstract Class containing the tests for the              *
 *  Implementations of Web Scraplers                                      *                       
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
    /// Clasa mostenita de clasele de test;
    /// 
    /// Testul este ales pe datele filmului clasat pe locul 1
    /// astfel incat atat testul de an cat si cel de gen vor 
    /// returna aceleasi date.
    /// 
    /// Testele vor fi picate doar daca:
    /// 1. filmul de pe locul 1 in top250 nu mai este "Închisoarea îngerilor"
    /// 2. structura html a paginilor imdb a fost modificata caz in care implementarile trebuie adaptate
    /// 3. a fost efectuata o modificare incorecta in cod
    /// 
    /// Pentru a verifica prima contitie acesati linkul:
    /// https://www.imdb.com/chart/top?ref_=tt_awd
    /// </summary>
    public abstract class UnitTestIImpWebScr
    {
        protected IImplementationWebScraper _implementationWebScrapler;

        protected HtmlWeb _web = new HtmlWeb();

        protected string _url;

        public abstract void Init();


        [TestMethod]
        public void ValidFirstMovie()
        {
            // Metoda pentru testarea extragerii primului film

            var doc = _web.Load(_url);

            Movie movie = _implementationWebScrapler.GetMovies(doc, 1)[0];

            Assert.AreEqual("Închisoarea îngerilor", movie.Title);
            Assert.AreEqual(1994, movie.Year);
            Assert.AreEqual(9.3, movie.Rating, 0.15);
            Assert.AreEqual("Drama", movie.Genres[0]);
        }

        [TestMethod]
        public void AcceptabelMoviesNumberReturned()
        {
            // Metoda pentru testarea gasirii unui numar apropiat de filme de cel dorit
            // la 50 de filme consideram ca 3 ar putea fi omise

            var doc = _web.Load(_url);

            List<Movie> movies = _implementationWebScrapler.GetMovies(doc, 50);

            Assert.AreEqual(movies.Count, 50, 3);
        }
    }
}
