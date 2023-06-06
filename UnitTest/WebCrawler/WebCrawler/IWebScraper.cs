/**************************************************************************
 *                                                                        *
 *  File:        CommedyCommand.cs                                        *
 *  Copyright:   (c) 2023, Fodor Maria-Gabriela                           *
 *  E-mail:      maria-gabriela.fodor@student.tuiasi.ro                   *
 *  Description: Interface with the function of movie scraping            *
 *                                                                        *
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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Movies;

namespace WebCrawl
{
    // <summary>
    /// Interfața pentru extragerea informațiilor despre un număr de filme.
    /// </summary>
    public interface IWebScraper
    {
        /// <summary>
        /// Metodă care returnează o listă cu obiecte de tip Movie folosind date extrase de pe
        /// una din paginile IMDb.
        /// </summary>
        /// <param name="nrMovies">Numărul de filme de returnat</param>
        /// <returns>O listă de filme</returns>
        MovieList GetMovies(int nrMovies);
    }
}
