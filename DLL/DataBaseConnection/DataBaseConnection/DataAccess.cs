/**************************************************************************
 *                                                                        *
 *  File:        DataAcess.cs                                             *
 *  Copyright:   (c) 2023, Dumea Cristian                                 *
 *  E-mail:      cristian.dumea@student.tuiasi.ro                         *
 *  Description: Class that implements the DataAccess interface           *
 *               communicating with the database                          *
 *                                                                        *
 *  This code and information is provided "as is" without warranty of     *
 *  any kind, either expressed or implied, including but not limited      *
 *  to the implied warranties of merchantability or fitness for a         *
 *  particular purpose. You are free to use this source code in your      *
 *  applications as long as the original copyright notice is included.    *
 *                                                                        *
 **************************************************************************/

using Movies;
using PersonalInformations;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cryptography;
using System.Globalization;

namespace DataBaseConnection
{
    /// <summary>
    /// Clasa care implementeaza interfata IDataAccess cu funtia de a insera inregistrari in tabelele bazei de date
    /// </summary>
    public class DataAccess : IDataAccess
    {
        private SqlConnection _connection;
        private Cryptograph _cripto= new Cryptograph();
        /// <summary>
        /// Metoda pentru inserarea unei noi inregistrari in tablela USERS
        /// </summary>
        /// <param name="person"></param>
        public void InsertUser(Person person)
        {
            try
            {
                _connection = Connection.GetConnection();
                _connection.Open();
                string checkExistingUserQuery = "SELECT * FROM users WHERE username = @username OR email = @email";
                SqlCommand checkExistingUserCmd = new SqlCommand(checkExistingUserQuery, _connection);
                checkExistingUserCmd.Parameters.AddWithValue("@username", person.Username);
                checkExistingUserCmd.Parameters.AddWithValue("@email", person.Email);

                SqlDataReader reader = checkExistingUserCmd.ExecuteReader();
                if (reader.Read())
                {
                    reader.Close();
                    _connection.Close();
                    throw new Exception("Nume de utilizator sau email folosit!");
                }
                else
                {
                    reader.Close();
                    string insertUserQuery = "INSERT INTO users (username, password, email, lastname, firstname) " +
                                             "VALUES (@username, @password, @email, @lastname, @firstname)";
                    SqlCommand insertUserCmd = new SqlCommand(insertUserQuery, _connection);
                    insertUserCmd.Parameters.AddWithValue("@username", person.Username);
                    insertUserCmd.Parameters.AddWithValue("@password", _cripto.EncryptString(_cripto.key, person.Password));
                    insertUserCmd.Parameters.AddWithValue("@email", person.Email);
                    insertUserCmd.Parameters.AddWithValue("@lastname", person.LastName);
                    insertUserCmd.Parameters.AddWithValue("@firstname", person.FirstName);
                    insertUserCmd.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                _connection.Close();
            }
        }

        /// <summary>
        /// Metoda de inserare a unei noi inregistrari in tabela MOVIE
        /// Este folosita atunci cand utilizatarul da un swipe right
        /// </summary>
        /// <param name="movie"> Datele filmului</param>
        public void InsertMovie(Movie movie, string username)
        {
            try
            {
                _connection = Connection.GetConnection();
                _connection.Open();
                string genres = string.Join(",", movie.Genres);
                string result = "insert into movies(title,year,genre,description,rating,imageurl,username)" +
                    "values(@title,@year,@genre,@description,@rating,@imageurl,@username)";
                SqlCommand cmd = new SqlCommand(result, _connection);
                cmd.Parameters.AddWithValue("@title", movie.Title);
                cmd.Parameters.AddWithValue("@year", movie.Year);
                cmd.Parameters.AddWithValue("@genre", genres);
                cmd.Parameters.AddWithValue("@description", movie.Description);
                cmd.Parameters.AddWithValue("@rating", movie.Rating);
                cmd.Parameters.AddWithValue("@imageurl", movie.ImageUrl);
                cmd.Parameters.AddWithValue("@username", username);
                cmd.ExecuteNonQuery();
                
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                _connection.Close();
            }

        }

        /// <summary>
        /// Setarea datelor persoanei
        /// </summary>
        /// <param name="username"></param>
        /// <returns>Returneaza noul utilizator</returns>
        public Person ReadPersonByUsername(string username)
        {
            string retrievedUsername="";
            string retrievedPassword="";
            string retrievedEmail="";
            string retrievedFirstName="";
            string retrievedLastName="";

            try
            {
                _connection = Connection.GetConnection();
                _connection.Open();
                string query = "SELECT * FROM users WHERE username = @username";
                using (SqlCommand command = new SqlCommand(query, _connection))
                {
                    command.Parameters.AddWithValue("@username", username);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            retrievedUsername = reader.GetString(reader.GetOrdinal("username")).Trim();
                            retrievedPassword = reader.GetString(reader.GetOrdinal("password")).Trim();
                            retrievedEmail = reader.GetString(reader.GetOrdinal("email")).Trim();
                            retrievedFirstName = reader.GetString(reader.GetOrdinal("firstname")).Trim();
                            retrievedLastName = reader.GetString(reader.GetOrdinal("lastname")).Trim();
                            
                        }
                    }
                }
            }
            catch (Exception)
            {
                _connection.Close();
                return null;
            }
            finally
            {
                _connection.Close();
            }
           
            return new Person(retrievedUsername, _cripto.DecryptString(_cripto.key, retrievedPassword), retrievedEmail, retrievedFirstName, retrievedLastName);

        }

        /// <summary>
        /// Functie ce returneza filmele pe care le-a ales utilizator sa le vizioneze
        /// </summary>
        /// <param name="username">utilizatorul</param>
        /// <returns>lista de filme</returns>
        public List<Movie> viewMovies(string username)
        {
            List<Movie> movies = new List<Movie>();
            try
            {
                _connection = Connection.GetConnection();
                _connection.Open();

                string result = "SELECT * FROM movies WHERE username = @username";
                SqlCommand cmd = new SqlCommand(result, _connection);
                cmd.Parameters.AddWithValue("@username", username);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    string title = reader.GetString(1);
                    int year = Convert.ToInt32(reader.GetString(2));
                    string genre = reader.GetString(3);
                    string description = reader.GetString(4);
                    double rating = Convert.ToDouble(reader.GetValue(5));
                    string imageUrl = reader.GetString(6);
                    List<string> genres = genre.Split(',').ToList();
                    movies.Add(new Movie(title, year, genres, description, rating, imageUrl));
                }
                _connection.Close();
                return movies;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}

