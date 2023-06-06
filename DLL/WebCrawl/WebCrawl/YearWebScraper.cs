/**************************************************************************
 *                                                                        *
 *  File:        CommedyCommand.cs                                        *
 *  Copyright:   (c) 2023, Fodor Maria-Gabriela                           *
 *  E-mail:      maria-gabriela.fodor@student.tuiasi.ro                   *
 *  Description: Class with the function of movie scraping                *
 *  on particular launch year of movie                                    *
 *                                                                        *
 *  This code and information is provided "as is" without warranty of     *
 *  any kind, either expressed or implied, including but not limited      *
 *  to the implied warranties of merchantability or fitness for a         *
 *  particular purpose. You are free to use this source code in your      *
 *  applications as long as the original copyright notice is included.    *
 *                                                                        *
 **************************************************************************/
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using Movies;

namespace WebCrawl
{
    /// <summary>
    /// Clasa care implementează interfața IWebScraper și are funcționalitatea de a selecta primele filme în funcție de popularitate
    /// primind ca argument anul lansării. Această clasă utilizează un obiect implementationWebScraper care separă
    /// abstractizarea de implementare, ilustrând design pattern-ul "Bridge" deoarece modalitatea de extragere
    /// a informațiilor poate fi modificată odată cu modificarea site-ului.
    /// </summary>
    public class YearWebScraper : IWebScraper
    {
        private string _year;
        private IImplementationWebScraper _implementationWebScrapler = new ImplementationWebScraperYears();

        /// <summary>
        /// Constructor care primește ca argument anul
        /// din care se caută filmele.
        /// </summary>
        /// <param name="year">Anul de căutare a filmelor</param>
        public YearWebScraper(int year)
        {
            int currentYear = DateTime.Now.Year;

            if (year > currentYear)
            {
                throw new ArgumentException("Invalid year. Year cannot be greater than the current year.");
            }

            _year = year.ToString();
        }

        /// <summary>
        /// Metodă care returnează o listă cu obiecte de tip Movie folosind date extrase de pe
        /// top-ul pe genuri de pe IMDb.
        /// </summary>
        /// <param name="moviesNr">Numărul de filme de returnat</param>
        /// <returns>O listă de filme</returns>
        public MovieList GetMovies(int moviesNr)
        {
            var web = new HtmlWeb();
            var baseURL = "https://www.imdb.com/search/title/?title_type=feature&year=YEAR-01-01,YEAR-12-31";
            var url = baseURL.Replace("YEAR", _year);

            var doc = web.Load(url);

            return new MovieList(_implementationWebScrapler.GetMovies(doc,moviesNr));
        }
    }
}
