/**************************************************************************
 *                                                                        *
 *  File:        Person.cs                                                *
 *  Copyright:   (c) 2023, Dumea Cristian                                 *
 *  E-mail:      cristian.dumea@student.tuiasi.ro                         *
 *  Description: Class that contains the user's data                      *
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

namespace PersonalInformation
{
    /// <summary>
    /// Reprezintă o clasă care definește o persoană.
    /// </summary>
    public class Person
    {
        private string _username;
        private string _password;
        private string _email;
        private string _firstName;
        private string _lastName;

        /// <summary>
        /// Obține sau setează numele de utilizator al persoanei.
        /// </summary>
        public string Username
        {
            get { return _username; }
            set { _username = value; }
        }

        /// <summary>
        /// Obține sau setează parola persoanei.
        /// </summary>
        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }

        /// <summary>
        /// Obține sau setează adresa de email a persoanei.
        /// </summary>
        public string Email
        {
            get { return _email; }
            set { _email = value; }
        }

        /// <summary>
        /// Obține sau setează prenumele persoanei.
        /// </summary>
        public string FirstName
        {
            get { return _firstName; }
            set { _firstName = value; }
        }

        /// <summary>
        /// Obține sau setează numele de familie al persoanei.
        /// </summary>
        public string LastName
        {
            get { return _lastName; }
            set { _lastName = value; }
        }

        /// <summary>
        /// Inițializează o nouă instanță a clasei Person cu valorile primite ca parametri.
        /// </summary>
        /// <param name="username">Numele de utilizator al persoanei.</param>
        /// <param name="password">Parola persoanei.</param>
        /// <param name="email">Adresa de email a persoanei.</param>
        /// <param name="firstName">Prenumele persoanei.</param>
        /// <param name="lastName">Numele de familie al persoanei.</param>
        public Person(string username, string password, string email, string firstName, string lastName)
        {
            Username = username;
            Password = password;
            Email = email;
            FirstName = firstName;
            LastName = lastName;
        }
    }
}
