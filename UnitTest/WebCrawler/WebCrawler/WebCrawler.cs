/**************************************************************************
 *                                                                        *
 *  File:        CommedyCommand.cs                                        *
 *  Copyright:   (c) 2023, Fodor Maria-Gabriela                           *
 *  E-mail:      maria-gabriela.fodor@student.tuiasi.ro                   *
 *  Description: Class creating for creating a Web Scraper                *
 *  of a given type and unsing it for getting movie information           *
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
using Movies;

namespace WebCrawl
{
    /// <summary>
    /// Clasa care ține locul cretorului in Factory Pattern pentru a creea diverse tipuri de Web Scrapers.
    /// </summary>
    public class WebCrawler
    {
        /// <summary>
        /// Returneaza un scraper pentru toate filmele.
        /// </summary>
        /// <returns></returns>
        private IWebScraper CreateScraper()
        {
            return new AllWebScreaper();
        }

        /// <summary>
        /// Returneaza un scraper dupa gen.
        /// </summary>
        /// <param name="genre">Genul filmelor solicitate</param>
        /// <returns>WebScraper</returns>
        private  IWebScraper CreateScraper(string genre)
        {
            return new GenreWebScraper(genre);
        }

        /// <summary>
        /// Returneaza un scraper dupa an.
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        private IWebScraper CreateScraper(int year)
        {
            Console.WriteLine("year");
            return new YearWebScraper(year);
        }

        /// <summary>
        /// Inițializează și apoi utilizează un Web Scraper pentru a obține un număr de filme dintr-un anumit gen.
        /// </summary>
        /// <param name="genre">Genul filmelor</param>
        /// <param name="nrMovies">Numărul de filme de returnat</param>
        /// <returns>O listă de filme</returns>
        public MovieList GetMovies(string genre, int nrMovies)
        {
            IWebScraper scraper = CreateScraper(genre);
            MovieList movies = scraper.GetMovies(nrMovies);
            return movies;
        }

        /// <summary>
        /// Inițializează și apoi utilizează un Web Scraper pentru a obține un număr de filme lansate într-un an specificat.
        /// </summary>
        /// <param name="year">Anul de lansare al filmelor</param>
        /// <param name="nrMovies">Numărul de filme de returnat</param>
        /// <returns>O listă de filme</returns>
        public MovieList GetMovies(int year, int nrMovies)
        {
            IWebScraper scraper = CreateScraper(year);
            MovieList movies = scraper.GetMovies(nrMovies);
            return movies;
        }

        /// <summary>
        /// Inițializează și apoi utilizează un Web Scraper pentru a obține un număr de filme.
        /// </summary>
        /// <param name="nrMovies">Numărul de filme de returnat</param>
        /// <returns>O listă de filme</returns>
        public MovieList GetMovies(int nrMovies)
        {
            IWebScraper scraper = CreateScraper();
            MovieList movies = scraper.GetMovies(nrMovies);
            return movies;
        }
    }
}
