/**************************************************************************
 *                                                                        *
 *  File:        CommedyCommand.cs                                        *
 *  Copyright:   (c) 2023, Fodor Maria-Gabriela                           *
 *  E-mail:      maria-gabriela.fodor@student.tuiasi.ro                   *
 *  Description: Class for the Implementors of the Web Scrapler           *
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
using Movies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace WebCrawl
{
    /// <summary>
    /// Clasa care implementează interfața IImplementationWebScraper.
    /// Are funcționalitatea de a explora un document HTML și de a crea
    /// obiecte de tip Movie cu informațiile găsite.
    /// </summary>
    public class ImplementationWebScraperAll : IImplementationWebScraper
    {
        /// <summary>
        /// Metodă pentru returnarea unei liste de obiecte de tip film
        /// dintr-un document HTML.
        /// </summary>
        /// <param name="doc">Documentul HTML de explorat.</param>
        /// <param name="moviesNr">Numărul de filme de returnat.</param>
        /// <returns>O listă de obiecte de tip Movie.</returns>
        public List<Movie> GetMovies(HtmlDocument doc, int moviesNr)
        {
            List<Movie> movies = new List<Movie>();

            var movieNodes = doc.DocumentNode.SelectNodes("//tbody/tr[position()]")
                 .Take(moviesNr);

            Parallel.ForEach(movieNodes, (movieNode) =>
            {
                try
                {
                    var titleNode = movieNode.SelectSingleNode(".//td[@class='titleColumn']/a");
                    var title = WebUtility.HtmlDecode(titleNode.InnerText.Trim());

                    // deoarece genurile si descrierea nu apar pe pagina principala se va naviga pe o pagina auxiliara

                    var link = "https://www.imdb.com" + titleNode.GetAttributeValue("href", "");
                    var web = new HtmlWeb();
                    var movieDoc = web.Load(link);

                    var genreNodes = movieDoc.DocumentNode.SelectNodes("//div[@class='ipc-chip-list__scroller']//span[@class='ipc-chip__text']");
                    var genres = genreNodes?.Select(node => node.InnerText.Trim()).ToList();

                    var descriptionNode = movieDoc.DocumentNode.SelectSingleNode("//span[@data-testid='plot-xs_to_m']");
                    var description = descriptionNode.InnerText.Trim() ?? "";

                    var yearNode = movieNode.SelectSingleNode(".//td[@class='titleColumn']//span[@class='secondaryInfo']");
                    var year = WebUtility.HtmlDecode(yearNode?.InnerText.Trim(new char[] { '(', ')' })) ?? "";

                    var ratingNode = movieNode.SelectSingleNode(".//td[@class='ratingColumn imdbRating']/strong");
                    var ratingText = ratingNode?.InnerText.Trim().Replace(",", ".");
                    var rating = double.TryParse(ratingText, out double parsedRating) ? parsedRating : 0.0;

                    var imageUrlNode = movieNode.SelectSingleNode(".//td[@class='posterColumn']//img");
                    var imageUrl = imageUrlNode?.GetAttributeValue("src", "");
                    int lastIndex = imageUrl.LastIndexOf('@');

                    string highQualityImageUrl = imageUrl;
                    if (lastIndex >= 0)
                    {
                        highQualityImageUrl = imageUrl.Substring(0, lastIndex) + "@._V1_FMjpg_UY720_.jpg";
                    }


                    var movie = new Movie(title, Int32.Parse(year), genres, description, rating, highQualityImageUrl);
                    movies.Add(movie);
                }
                catch (Exception)
                {
                    // daca unul dintre filme are un format atipic vom sari peste el doar
                }
            });

            return movies;
        }


    }
}
