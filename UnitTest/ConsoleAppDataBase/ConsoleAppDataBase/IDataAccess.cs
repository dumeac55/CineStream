/**************************************************************************
 *                                                                        *
 *  File:        IDataAccess.cs                                           *
 *  Copyright:   (c) 2023, Dumea Cristian                                 *
 *  E-mail:      cristian.dumea@student.tuiasi.ro                         *
 *  Description: Interface with method for comunication with databases    *
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
using PersonalInformations;

namespace DataBaseConnection
{
    /// <summary>
    /// Interfață care gestionează interacțiunea cu baza de date
    /// </summary>
    public interface IDataAccess
    {
        /// <summary>
        /// Metoda pentru inserarea unui film in tabela MOVIES
        /// </summary>
        /// <param name="movie">Filmul ce trebuie inserat</param>
        /// <param name="username">Username-ul utilizatorului interesat de film</param>
        void InsertMovie(Movie movie, string username);

        /// <summary>
        /// Metoda pentru inserarea unei persoane în tabela PEOPLE în timpul înregistrării
        /// </summary>
        /// <param name="person">Persoana care urmează să fie inserată</param>
        void InsertUser(Person person);

        /// <summary>
        /// Metoda care returnează informațiile despre o persoană după numele de utilizator dat
        /// </summary>
        /// <param name="username">Numele de utilizator al persoanei despre care dorim să obținem informații</param>
        Person ReadPersonByUsername(string username);

        /// <summary>
        /// Metoda care returnează filmele salvate de utilizatorul cu numele de utilizator dat
        /// </summary>
        /// <param name="username">Username-ul persoanei ale cărei filme salvate le dorim</param>
        /// <returns>Lista de filme</returns>
        List<Movie> viewMovies(string username);
    }
}
