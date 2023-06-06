/**************************************************************************
 *                                                                        *
 *  File:        CommedyCommand.cs                                        *
 *  Copyright:   (c) 2023, Fodor Maria-Gabriela                           *
 *  E-mail:      maria-gabriela.fodor@student.tuiasi.ro                   *
 *  Description: Class for the Implementors of the Web Scrapler           *
 *  a particular gender being required                                    *
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
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using WebCrawl;

/// <summary>
/// Clasa care implementează interfața IImplementationWebScraper.
/// Are funcționalitatea de a explora un document HTML ce conține topul unor filme 
/// de un anumit gen și de a crea obiecte de tip Movie cu informațiile găsite.
/// </summary>
public class ImplementationWebScraperGenres : IImplementationWebScraper
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
        var movies = new List<Movie>();
        var movieNodes = doc.DocumentNode.SelectNodes("//div[@class='lister-item mode-advanced']")
             .Take(moviesNr);

        Parallel.ForEach(movieNodes, (movieNode) =>
        {
            try
            {
                var titleNode = movieNode.SelectSingleNode(".//h3[@class='lister-item-header']//a");
                var title = WebUtility.HtmlDecode(titleNode.InnerText.Trim());

                // deoarece imaginile nu pot fi extrase de pe pagina principala fiind de tip loadlate 
                // se va naviga pe un link auxiliar

                var link = "https://www.imdb.com" + titleNode.GetAttributeValue("href", "");
                var web = new HtmlWeb();
                var movieDoc = web.Load(link);

                var imageUrlNode = movieDoc.DocumentNode.SelectSingleNode("//img[@class='ipc-image']");
                var imageUrl = imageUrlNode?.GetAttributeValue("src", "");
                int lastIndex = imageUrl.LastIndexOf('@');

                string highQualityImageUrl = imageUrl;
                if (lastIndex >= 0)
                {
                    highQualityImageUrl = imageUrl.Substring(0, lastIndex) + "@._V1_FMjpg_UY720_.jpg";
                }

                var yearNode = movieNode.SelectSingleNode(".//span[@class='lister-item-year text-muted unbold']");
                var year = yearNode?.InnerText.Trim(new char[] { '(', ')' });

                var genreNode = movieNode.SelectSingleNode(".//span[@class='genre']");
                var genreText = genreNode?.InnerText.Trim();
                var genres = genreText?.Split(',').Select(genre => genre.Trim()).ToList();

                var descriptionNode = movieNode.SelectSingleNode(".//p[@class='text-muted']");
                var description = descriptionNode?.InnerText.Trim();

                var ratingNode = movieNode.SelectSingleNode(".//div[@class='inline-block ratings-imdb-rating']//strong");
                var ratingText = ratingNode?.InnerText.Trim();
                var rating = double.TryParse(ratingText, out double parsedRating) ? parsedRating : 0.0;

                var movie = new Movie(title, Int32.Parse(year), genres, description, rating, highQualityImageUrl);
                lock (movies)
                {
                    movies.Add(movie);
                }
            }
            catch (Exception)
            {
                // Skip the movie if it has an atypical format
            }
        });

        return movies;
    }

}



