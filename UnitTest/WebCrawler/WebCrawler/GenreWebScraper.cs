/**************************************************************************
 *                                                                        *
 *  File:        CommedyCommand.cs                                        *
 *  Copyright:   (c) 2023, Fodor Maria-Gabriela                           *
 *  E-mail:      maria-gabriela.fodor@student.tuiasi.ro                   *
 *  Description: Class with the function of movie scraping                *
 *  on particular genre of movie                                          *
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
    /// Clasa care implementează interfața IWebScraper și are funcționalitatea de a selecta primele filme din top-ul pe genuri
    /// primind ca argument un număr de filme. Această clasă utilizează un obiect implementationWebScraper care separă
    /// abstractizarea de implementare, ilustrând design pattern-ul "Bridge" deoarece modalitatea de extragere
    /// a informațiilor poate fi modificată odată cu modificarea site-ului.
    /// </summary>
    public class GenreWebScraper : IWebScraper
    {
        private string _genre;
        private IImplementationWebScraper _implementationWebScrapler = new ImplementationWebScraperGenres();

        /// <summary>
        /// Constructor cu un argument care setează genul filmelor căutate.
        /// </summary>
        /// <param name="genre">Genul filmelor căutate</param>
        public GenreWebScraper(string genre)
        {
            _genre = genre;
        }

        /// <summary>
        /// Metodă care returnează o listă cu obiecte de tip Movie folosind date extrase de pe
        /// top-ul principal IMDb.
        /// </summary>
        /// <param name="moviesNr">Numărul de filme de returnat</param>
        /// <returns>O listă de filme</returns>
        public MovieList GetMovies(int moviesNr)
        {
            var web = new HtmlWeb();
            var baseURL = "https://www.imdb.com/search/title/?genres=GENRE&sort=user_rating,desc&title_type=feature&num_votes=25000,&pf_rd_m=A2FGELUUNOQJNL&pf_rd_p=94365f40-17a1-4450-9ea8-01159990ef7f&pf_rd_r=MWWKWPHG5022Y3561KDP&pf_rd_s=right-6&pf_rd_t=15506&pf_rd_i=top&ref_=chttp_gnr_5";
            var url = baseURL.Replace("GENRE", _genre);

            var doc = web.Load(url);

            return new MovieList(_implementationWebScrapler.GetMovies(doc, moviesNr));
        }
    }
}
