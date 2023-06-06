/**************************************************************************
 *                                                                        *
 *  File:        DataAcessSecurityProxy.cs                                *
 *  Copyright:   (c) 2023, Dumea Cristian                                 *
 *  E-mail:      cristian.dumea@student.tuiasi.ro                         *
 *  Description: Class that tests the data format of movies and people    *
 *                                                                        *
 *  This code and information is provided "as is" without warranty of     *
 *  any kind, either expressed or implied, including but not limited      *
 *  to the implied warranties of merchantability or fitness for a         *
 *  particular purpose. You are free to use this source code in your      *
 *  applications as long as the original copyright notice is included.    *
 *                                                                        *
 *************************************************************************/

using Movies;
using PersonalInformations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DataBaseConnection
{
    /// <summary>
    /// Clasa de exceptie noua care are un atribut public
    /// ce specifica exact care fromate nu au fost respectate
    /// </summary>
    public class InvalidFormatException : Exception
    {
        public List<string> InvalidFormats { get; }

        public InvalidFormatException(string message, List<string> invalidFormats) : base(message)
        {
            InvalidFormats = invalidFormats;
        }
    }

    /// <summary>
    /// Clasa folosita ca security proxy pentru a restrictiona accesul
    /// la baza de date in funtie de corectitudinea datelor introduse
    /// </summary>
    public class DataAccessSecurityProxy : IDataAccess
    {
        private DataAccess _dataAccess;

        /// <summary>
        /// Constructor care primeste ca argument un obiect de tip  DataAccess
        /// </summary>
        /// <param name="dataAccess"></param>
        public DataAccessSecurityProxy(DataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }

        #region METODE_PRIVATE_DE_CHECK_PERSOANA

        /// <summary>
        /// Verifica formatul de username: 3-20 caractere, alfanumerice sau cu underscore
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        private bool CheckUsernameFormat(string username)
        {
            string pattern = @"^\w{3,20}$";
            return Regex.IsMatch(username, pattern);
        }

        /// <summary>
        /// Verifica formatul de parola: minim 8 caractere, cel putin o litera mica, o litera mare si o cifra
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        private bool CheckPasswordFormat(string password)
        {
            string pattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$";
            return Regex.IsMatch(password, pattern);
        }

        /// <summary>
        /// Verifica formatul de email: pattern pentru o adresa de email valida
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        private bool CheckEmailFormat(string email)
        {
            string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, pattern);
        }

        /// <summary>
        /// Verifica formatul de nume: doar litere, minim 2 caractere
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private bool CheckNameFormat(string name)
        {
            string pattern = @"^[A-Za-z]{2,}$";
            return Regex.IsMatch(name, pattern);
        }

        /// <summary>
        /// Verifica formatul de prenume: doar litere, minim 2 caractere
        /// </summary>
        /// <param name="surname"></param>
        /// <returns></returns>
        private bool CheckSurnameFormat(string surname)
        {
            string pattern = @"^[A-Za-z]{2,}$";
            return Regex.IsMatch(surname, pattern);
        }
        #endregion

        #region METODE_DE_CHECK_FILM

        /// <summary>
        /// Verifică formatul titlului filmului: șir nevid
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        private bool CheckTitleFormat(string title)
        {
            return !string.IsNullOrEmpty(title);
        }

        /// <summary>
        /// Verifică formatul anului de apariție al filmului: întreg pozitiv
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        private bool CheckYearFormat(int year)
        {
            return year > 0;
        }

        /// <summary>
        /// Verifică formatul genurilor filmului: listă ne-nulă și nevidă
        /// </summary>
        /// <param name="genres"></param>
        /// <returns></returns>
        private bool CheckGenresFormat(List<string> genres)
        {
            return genres != null && genres.Count > 0;
        }

        /// <summary>
        /// Verifică formatul descrierii filmului: șir nevid
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        private bool CheckDescriptionFormat(string description)
        {
            return !string.IsNullOrEmpty(description);
        }

        /// <summary>
        /// Verifică formatul ratingului filmului: între 0 și 10
        /// </summary>
        /// <param name="rating"></param>
        /// <returns></returns>
        private bool CheckRatingFormat(double rating)
        {
            return rating >= 0 && rating <= 10;
        }

        /// <summary>
        /// Verifică formatul URL-ului imaginii filmului: șir nevid
        /// </summary>
        /// <param name="imageUrl"></param>
        /// <returns></returns>
        private bool CheckImageUrlFormat(string imageUrl)
        {
            return !string.IsNullOrEmpty(imageUrl);
        }
        #endregion

        #region METODE_DE_INSERT

        /// <summary>
        /// Metoda de inserare daca Movie 
        /// </summary>
        /// <param name="movie">Filmul de inserat</param>
        /// <param name="usename">Utilizatorul care a ales filmul</param>
        public void InsertMovie(Movie movie, string usename)
        {
            if (CheckTitleFormat(movie.Title) && CheckDescriptionFormat(movie.Description) && CheckGenresFormat(movie.Genres) && CheckYearFormat(movie.Year) && CheckRatingFormat(movie.Rating) && CheckImageUrlFormat(movie.ImageUrl))
            {
                try
                {
                    _dataAccess.InsertMovie(movie, usename);
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
            else
            {
                throw new InvalidFormatException("Invalid movie format", new List<string>());
            }
        }

        /// <summary>
        /// Metoda pentru inserarea unei noi Persoane
        /// </summary>
        /// <param name="person">Persoana de inserat</param>
        public void InsertUser(Person person)
        {
            List<string> invalidFormats = new List<string>();

            if (!CheckUsernameFormat(person.Username))
            {
                invalidFormats.Add("Invalid username format");
            }

            if (!CheckNameFormat(person.LastName))
            {
                invalidFormats.Add("Invalid name format");
            }

            if (!CheckPasswordFormat(person.Password))
            {
                invalidFormats.Add("Invalid password format");
            }

            if (!CheckEmailFormat(person.Email))
            {
                invalidFormats.Add("Invalid email format");
            }

            if (!CheckSurnameFormat(person.FirstName))
            {
                invalidFormats.Add("Invalid surname format");
            }

            if (invalidFormats.Count > 0)
            {
                string errorMessage = "Invalid format(s) detected: " + string.Join(", ", invalidFormats);
                throw new InvalidFormatException(errorMessage, invalidFormats);
            }
            try
            {
                _dataAccess.InsertUser(person);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Metoda de citire a datelor unei Persoane
        /// </summary>
        /// <param name="username">Recunoasterea persoanei dupa username</param>
        /// <returns>datele persoanei</returns>
        public Person ReadPersonByUsername(string username)
        {
            if (CheckUsernameFormat(username))
            {
                return _dataAccess.ReadPersonByUsername(username);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Metoda de citire a tuturor filmelor unui username
        /// </summary>
        /// <param name="username"></param>
        /// <returns>lista de filme</returns>
        public List<Movie> viewMovies(string username)
        {
            if (CheckUsernameFormat(username))
            {
                return _dataAccess.viewMovies(username);
            }
            else
            {
                return null;
            }
        }
        #endregion
    }
}
