/**************************************************************************
 *                                                                        *
 *  File:        CommedyCommand.cs                                        *
 *  Copyright:   (c) 2023, Fodor Maria-Gabriela                           *
 *  E-mail:      maria-gabriela.fodor@student.tuiasi.ro                   *
 *  Description: Class including informations about a movie               *
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
    /// clasa ce modeleaza informatiile vizate despre un film
    /// </summary>
    public class Movie
    {
        private string _title;
        private int _year;
        private List<string> _genres;
        private string _description;
        private double _rating;
        private string _imageUrl;

        public string Title { get => _title; set => _title = value; }
        public int Year { get => _year; set => _year = value; }
        public List<string> Genres { get => _genres; set => _genres = value; }
        public string Description { get => _description; set => _description = value; }
        public double Rating { get => _rating; set => _rating = value; }
        public string ImageUrl { get => _imageUrl; set => _imageUrl = value; }

        /// <summary>
        /// Constructor ce seteaza membrii unei clase
        /// </summary>
        /// <param name="title">Titlul filmului</param>
        /// <param name="year">Anul aparitiei filmului</param>
        /// <param name="genres">Genurile filmului</param>
        /// <param name="description">Descrierea filmului</param>
        /// <param name="rating">Ratingul filmului</param>
        public Movie(string title, int year, List<string> genres, string description, double rating, string imageUrl)
        {
            _title = title;
            _year = year;
            _genres = genres;
            _description = description;
            _rating = rating;
            _imageUrl = imageUrl;
        }
    }
}
