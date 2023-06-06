/**************************************************************************
 *                                                                        *
 *  File:        CommedyCommand.cs                                        *
 *  Copyright:   (c) 2023, Fodor Maria-Gabriela                           *
 *  E-mail:      maria-gabriela.fodor@student.tuiasi.ro                   *
 *  Description: Class used to store information about selected movies    *
 *                                                                        *
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

namespace Movies
{
    /// <summary>
    /// Clasa care gestioneaza o lista de filme de un anumit tip
    /// </summary>
    public class MovieList
    {
        private List<Movie> _movieList = new List<Movie>();

        /// <summary>
        /// Constructor ce primeste o referinta la lista de filme
        /// </summary>
        /// <param name="list"></param>
        public MovieList(List<Movie> list)
        {
            _movieList = list;
        }

        /// <summary>
        /// Getter pentru lista de filme
        /// </summary>
        /// <returns></returns>
        public List<Movie> Movies
        {
            get
            {
                return _movieList;
            }
        }

        /// <summary>
        /// Setter pentru lista de filme
        /// </summary>
        /// <param name="movieList"></param>
        public void SetMovies(List<Movie> movieList)
        {
            _movieList = movieList;
        }
    }
}
