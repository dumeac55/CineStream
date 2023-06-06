/**************************************************************************
 *                                                                        *
 *  File:        Conection.cs                                             *
 *  Copyright:   (c) 2023, Dumea Cristian                                 *
 *  E-mail:      cristian.dumea@student.tuiasi.ro                         *
 *  Description: Class that establishes the connection with the database  *
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
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace DataBaseConnection
{
    /// <summary>
    /// Clasa care gestioneaza instantierea unei conexiuni SQL la baza de date
    /// </summary>
    public class Connection
    {
        private static SqlConnection _connection;
        private static string _connectionString = $@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename={GetDatabaseFilePath()};Integrated Security=True;Connect Timeout=30";

        /// <summary>
        /// Constructorul privat
        /// </summary>
        private Connection() { }

        /// <summary>
        /// Metoda privata care obtine calea relativa la baza de date
        /// </summary>
        /// <returns>Calea catre baza de date</returns>
        private static string GetDatabaseFilePath()
        {
            string relativePath = @"..\..\Database.mdf";
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string absolutePath = Path.GetFullPath(Path.Combine(baseDirectory, relativePath));
            return absolutePath;
        }

        /// <summary>
        /// Logica sablonului Singleton
        /// </summary>
        /// <returns>Conexiunea</returns>
        public static SqlConnection GetConnection()
        {
            if (_connection == null)
            {
                try
                {
                    _connection = new SqlConnection(_connectionString);
                }
                catch (Exception e)
                {
                    throw new Exception(e + "Conexiune la baza de date esuata!");
                }
            }
            return _connection;
        }
    }
}
