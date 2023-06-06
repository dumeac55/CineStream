/**************************************************************************
 *                                                                        *
 *  File:        CommedyCommand.cs                                        *
 *  Copyright:   (c) 2023, Fodor Maria-Gabriela                           *
 *  E-mail:      maria-gabriela.fodor@student.tuiasi.ro                   *
 *  Description: Class with the function of movie scraping                *
 *  no particular gender being required                                   *
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
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Movies;

namespace WebCrawl
{
    /// <summary>
    /// Clasa care implementează interfața IWebScraper și are funcționalitatea de a selecta primele filme din top
    /// primind ca argument un număr de filme. Această clasă utilizează un obiect implementationWebScraper care separă
    /// abstractizarea de implementare, ilustrând design pattern-ul "Bridge" deoarece modalitatea de extragere
    /// a informațiilor poate fi modificată odată cu modificarea site-ului.
    /// </summary>
    public class AllWebScreaper : IWebScraper
    {   
        private IImplementationWebScraper _implementationWebScrapler = new ImplementationWebScraperAll();

        /// <summary>
        /// Metodă care returnează o listă cu obiecte de tip Movie folosind date extrase de pe
        /// top-ul principal IMDb.
        /// </summary>
        /// <param name="moviesNr">Numărul de filme de returnat</param>
        /// <returns>O listă de filme</returns>
        public MovieList GetMovies(int moviesNr)
        {
            var url = "https://www.imdb.com/chart/top";
            var web = new HtmlWeb();
            var doc = web.Load(url);

            return new MovieList(_implementationWebScrapler.GetMovies(doc, moviesNr));
        }
    }
}
