using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Movies;

namespace WebCrawl
{
    class Program
    {
        static void Main(string[] args)
        {
            WebCrawler webCrawler = new WebCrawler();
            MovieList movieList = webCrawler.GetMovies(50);
            Console.WriteLine(movieList.Movies[0].Title);
     
            Console.WriteLine(movieList.Movies[0].Rating);
            Console.WriteLine(movieList.Movies.Count);
            foreach (var genre in movieList.Movies[0].Genres)
            {
                Console.WriteLine(genre);
            }
            Console.WriteLine(movieList.Movies[0].Year);
            Console.WriteLine(movieList.Movies[0].ImageUrl);
            Console.WriteLine(movieList.Movies[0].Description);
        }
    }
}
