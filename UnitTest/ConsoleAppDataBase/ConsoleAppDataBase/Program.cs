using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Movies;
using PersonalInformations;
namespace DataBaseConnection
{
    class Program
    {
        static void Main(string[] args)
        {

            /*Person person = new Person("as123asasa32", "sas123as", "sa2sasa3sas2", "asasa", "ssasa");
            DataAccess a = new DataAccess();
            a.InsertUser(person);
            Movie movie = new Movie("dada",100,new List<string> { "dsa" },"dasdas",33.3,"222.com");
            a.InsertMovie(movie, person.Username);
            Person dsa2 = a.ReadPersonByUsername("as123asasa32");
            Console.WriteLine(dsa2.Username + dsa2.Password);
            List<Movie> movie2 = a.viewMovies("as123asasa32");
            foreach(Movie f in movie2)
            {
                Console.WriteLine(f.Title+f.ImageUrl);
            }
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();*/

            /*Person person = new Person("cristi2312", "sa@32F", "cristi2132@gmail.com", "asda", "sssa");*/
            DataAccess a = new DataAccess();
            DataAccessSecurityProxy b = new DataAccessSecurityProxy(a);
            Person d = b.ReadPersonByUsername("cristi2");
            Console.WriteLine(d.Email+d.LastName+d.Password);
            
        }
    }
}
