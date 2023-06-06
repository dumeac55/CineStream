/**************************************************************************
 *                                                                        *
 *  File:        Form1.cs                                                 *
 *  Copyright:   (c)2023, Tarlev Mateo                                    *
 *  E-mail:      mateo.tarlev@student.tuiasi.ro                           *
 *  Description: CineStream Interface                                     *
 *                                                                        *
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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PersonalInformations;
using Movies;
using DataBaseConnection;
using System.Data.SqlClient;
using WebCrawl;
using System.Threading;

namespace CineStream
{
    #region ENUM PENTRU CATEGORIILE DE FILME
    public enum MovieCategory
    {
        Horror,
        Action,
        Comedy,
        Biography,
        All,
        New
    }
    #endregion

    /// <summary>
    /// Clasa Form a aplicatiei
    /// </summary>
    public partial class Form1 : Form
    {
        #region DATELE PERSOANEI LOGATE

        private string _username = "";
        private List<Movie> _savedMovies = new List<Movie>();

        #endregion

        #region ELEMENTE DE GESTIONARE A EXTRAGERII SI PERSISTENTEI DATELOR

        private WebCrawler _webCrawler = new WebCrawler();
        private DataAccessSecurityProxy _dataAccessProxy;
        #endregion



        #region LISTELE DE FILME PE CATEGORII

        private MovieList _top50Horror;
        private MovieList _top50Action;
        private MovieList _top50Comedy;
        private MovieList _top50Biography;
        private MovieList _top50All;
        private MovieList _top50New;
        #endregion



        #region ATRIBUTE DE MEMORARE A STARII DE SWAPPING

        private List<Movie> _currentList; // lista curenta de afisat
        private MovieCategory _currentCategory;

        private bool _isFirstLog = true;
        private int _currentHorror = 0;
        private int _currentAction = 0;
        private int _currentComedy = 0;
        private int _currentBiography = 0;
        private int _currentAll = 0;
        private int _currentNew = 0;
        #endregion

        #region THREADURI DEFINITE PENTRU INCARCAREA FIECAEI CATEGORII DE FILME

        private Thread _threadHorror;
        private Thread _threadAction;
        private Thread _threadComedy;
        private Thread _threadBiography;
        private Thread _thread50All;
        private Thread _thread50New;
        #endregion



        #region ATRIBUTE CE RETIN STAREA DIN FLOW-UL APLICATIEI

        private Dictionary<string, TabPage> _stateDictionary;
        private TabPage _currentState;
        #endregion



        #region ELEMENTE PENTRU ANIMATIA DE LOGIN

        private PictureBox pictureBox; // PictureBox în care se încarcă imaginile
        private int currentIndex;
        private string filePath = "curiozitati.txt"; // Numele fișierului text cu curiozitati
        #endregion



        #region METODE RESPONSABILE DE MANIPULAREA THREAD-URILOR PENTRU EXTRAGERE INFORMATIILOR DESPRE FILME

        /// <summary>
        /// Metoda care porneste threadurile de extragere a datelor web
        /// </summary>
        private void LoadMovies()
        {
            _threadHorror = new Thread(() => _top50Horror = _webCrawler.GetMovies("Horror", 10));
            _threadHorror.Start();

            _threadAction = new Thread(() => _top50Action = _webCrawler.GetMovies("Action", 10));
            _threadAction.Start();

            _threadComedy = new Thread(() => _top50Comedy = _webCrawler.GetMovies("Comedy", 10));
            _threadComedy.Start();

            _threadBiography = new Thread(() => _top50Biography = _webCrawler.GetMovies("Biography", 10));
            _threadBiography.Start();

            _thread50All = new Thread(() => _top50All = _webCrawler.GetMovies(10));
            _thread50All.Start();

            _thread50New = new Thread(() => _top50New = _webCrawler.GetMovies(2023, 10));
            _thread50New.Start();
        }

        /// <summary>
        /// Metoda pentru a asteapta incarcarea tuturor filmelor
        /// </summary>
        private void WaitMovieLoading()
        {
            if ((_threadHorror != null && _threadHorror.IsAlive) ||
                (_threadAction != null && _threadAction.IsAlive) ||
                (_threadComedy != null && _threadComedy.IsAlive) ||
                (_threadBiography != null && _threadBiography.IsAlive) ||
                (_thread50All != null && _thread50All.IsAlive) ||
                (_thread50New != null && _thread50New.IsAlive))
            {
                // intram in modul de loading
                ChangeState("Loading");

                Thread waitThread = new Thread(() =>
                {
                    if (_threadHorror != null && _threadHorror.IsAlive)
                        _threadHorror.Join();
                    if (_threadAction != null && _threadAction.IsAlive)
                        _threadAction.Join();
                    if (_threadComedy != null && _threadComedy.IsAlive)
                        _threadComedy.Join();
                    if (_threadBiography != null && _threadBiography.IsAlive)
                        _threadBiography.Join();
                    if (_thread50All != null && _thread50All.IsAlive)
                        _thread50All.Join();
                    if (_thread50New != null && _thread50New.IsAlive)
                        _thread50New.Join();

                    // schimbarea se face pe thread-ul principal al interfetei 
                    this.Invoke((MethodInvoker)delegate {
                        ChangeState("Menu");
                        button_top50_Click(null, EventArgs.Empty);
                    });
                });

                // pornim thread-ul de asteptare
                waitThread.Start();
            }
        }
        #endregion



        #region METODE DE MANIPULARE A LISTELOR DE FILME
        /// <summary>
        /// Metoda pentru obtinerea filmului curent
        /// </summary>
        /// <returns></returns>
        private Movie GetCurrentMovie()
        {
            switch (_currentCategory)
            {
                case MovieCategory.Horror:
                    return _top50Horror.Movies[_currentHorror];
                case MovieCategory.Action:
                    return _top50Action.Movies[_currentAction];
                case MovieCategory.Comedy:
                    return _top50Comedy.Movies[_currentComedy];
                case MovieCategory.Biography:
                    return _top50Biography.Movies[_currentBiography];
                case MovieCategory.All:
                    return _top50All.Movies[_currentAll];
                case MovieCategory.New:
                    return _top50New.Movies[_currentNew];
                default:
                    return null;
            }
        }

        /// <summary>
        /// Metoda pentru afisarea urmatorului film 
        /// </summary>
        private void NextMovie()
        { 
            switch (_currentCategory)
            {
                case MovieCategory.Horror:
                    _currentHorror++;
                    if (_currentHorror < _top50Horror.Movies.Count)
                    {
                        while(isSaved(_top50Horror.Movies[_currentHorror]))
                        {
                            _currentHorror++;
                        }
                        PresentMovie(_top50Horror.Movies[_currentHorror]);
                    }
                    else
                    {
                        MessageBox.Show("No more horror movies to display.");
                        throw new Exception("No more movies to show.");
                    }
                    break;
                case MovieCategory.Action:
                    _currentAction++;
                    if (_currentAction < _top50Action.Movies.Count)
                    {
                        while (isSaved(_top50Action.Movies[_currentAction]))
                        {
                            _currentAction++;
                        }
                        PresentMovie(_top50Action.Movies[_currentAction]);
                    }
                    else
                    {
                        MessageBox.Show("No more action movies to display.");
                        throw new Exception("No more movies to show.");
                    }
                    break;
                case MovieCategory.Comedy:
                    _currentComedy++;
                    if (_currentComedy < _top50Comedy.Movies.Count)
                    {
                        while(isSaved(_top50Comedy.Movies[_currentComedy]))
                        {
                            _currentComedy++;
                        }
                        PresentMovie(_top50Comedy.Movies[_currentComedy]);
                    }
                    else
                    {
                        MessageBox.Show("No more comedy movies to display.");
                        throw new Exception("No more movies to show.");
                    }
                    break;
                case MovieCategory.Biography:
                    _currentBiography++;
                    if (_currentBiography < _top50Biography.Movies.Count)
                    {
                        while (isSaved(_top50Biography.Movies[_currentBiography]))
                        {
                            _currentBiography++;
                        }
                        PresentMovie(_top50Biography.Movies[_currentBiography]);
                    }
                    else
                    {
                        MessageBox.Show("No more biography movies to display.");
                        throw new Exception("No more movies to show.");
                    }
                    break;
                case MovieCategory.All:
                    _currentAll++;
                    if (_currentAll < _top50All.Movies.Count)
                    {
                        while (isSaved(_top50All.Movies[_currentAll]))
                        {
                            _currentAll++;
                        }
                        PresentMovie(_top50All.Movies[_currentAll]);
                    }
                    else
                    {
                        MessageBox.Show("No more movies from all categories to display.");
                        throw new Exception("No more movies to show.");
                    }
                    break;
                case MovieCategory.New:
                    _currentNew++;
                    if (_currentNew < _top50New.Movies.Count)
                    {
                        while(isSaved(_top50New.Movies[_currentNew]))
                        {
                            _currentNew++;
                        }
                        PresentMovie(_top50New.Movies[_currentNew]);
                    }
                    else
                    {
                        MessageBox.Show("No more new movies to display.");
                        throw new Exception("No more movies to show.");
                    }
                    break;
                default:
                    break;
            }
        }



        /// <summary>
        /// Metoda pentru schimbarea categoriei de filme
        /// </summary>
        /// <param name="category"></param>
        private void ChangeCategory(MovieCategory category)
        {
            // Change the category and update the current list of movies
            _currentCategory = category;
            try
            {
                switch (category)
                {
                    case MovieCategory.Horror:
                        while (isSaved(_top50Horror.Movies[_currentHorror]))
                        {
                            _currentHorror++;
                        }
                        _currentList = _top50Horror.Movies;
                        PresentMovie(_currentList[_currentHorror]);
                        break;
                    case MovieCategory.Action:
                        while (isSaved(_top50Action.Movies[_currentAction]))
                        {
                            _currentAction++;
                        }
                        _currentList = _top50Action.Movies;
                        PresentMovie(_currentList[_currentAction]);
                        break;
                    case MovieCategory.Comedy:
                        while (isSaved(_top50Comedy.Movies[_currentComedy]))
                        {
                            _currentComedy++;
                        }
                        _currentList = _top50Comedy.Movies;
                        PresentMovie(_currentList[_currentComedy]);
                        break;
                    case MovieCategory.Biography:
                        while (isSaved(_top50Biography.Movies[_currentBiography]))
                        {
                            _currentBiography++;
                        }
                        _currentList = _top50Biography.Movies;
                        PresentMovie(_currentList[_currentBiography]);
                        break;
                    case MovieCategory.All:
                        while (isSaved(_top50All.Movies[_currentAll]))
                        {
                            _currentAll++;
                        }
                        _currentList = _top50All.Movies;
                        PresentMovie(_currentList[_currentAll]);
                        break;
                    case MovieCategory.New:
                        while (isSaved(_top50New.Movies[_currentNew]))
                        {
                            _currentNew++;
                        }
                        _currentList = _top50New.Movies;
                        PresentMovie(_currentList[_currentNew]);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("No more movies to display in this cathegory.");
            }
        }

        /// <summary>
        /// Metoda pentru a incarca un film pe form
        /// </summary>
        private void PresentMovie(Movie movie)
        {
            textBoxTitlu.Text = movie.Title;
            textBoxGen.Text = string.Join(", ", movie.Genres);
            textBoxRating.Text = (movie.Rating != 0) ? movie.Rating.ToString() : "no rate available yet";
            textBoxAn.Text = movie.Year.ToString();
            textBoxDescriere.Text = movie.Description.ToString();

            // incarcarea imaginii
            LoadImageFromUrl(movie.ImageUrl);
        }

        #endregion

        /// <summary>
        /// Constructor apelat la rularea aplicatiei
        /// </summary>
        public Form1()
        {
            InitializeComponent();
            _dataAccessProxy = new DataAccessSecurityProxy(new DataAccess());
            //**pentru animatie Loading
            pictureBox = pictureBoxLoading;
            currentIndex = 0;
            

            // dictionar de stari ale meniului
            _stateDictionary = new Dictionary<string, TabPage>
            {
                { "Start", tabStart },
                { "Login", tabLogin},
                { "Menu", tabMenu },
                { "Inregistrare", tabInregistrare},
                { "MyProfile", tabMyProfile },
                {"Loading", tabPageLoading }
            };

            

            RotunjesteColturileButonului(buttonToMainMenu, 12);
            RotunjesteColturileButonului(buttonToMyProfile, 12);
            RotunjesteColturileButonului(buttonTop50, 14);
            RotunjesteColturileButonului(buttonAction, 14);
            RotunjesteColturileButonului(buttonBiography, 14);
            RotunjesteColturileButonului(buttonCurentYear, 14);
            RotunjesteColturileButonului(buttonHorror, 14);
            RotunjesteColturileButonului(buttonComedy, 14);
            RotunjesteColturileButonului(buttonLoginToMenu, 14);
            RotunjesteColturileButonului(buttonStartCreateAcount, 14);
            RotunjesteColturileButonului(buttonStartLogIn, 14);
            RotunjesteColturileButonului(buttonLogOut, 14);
            _currentState = tabStart; // Set the initial state
            ChangeState("Start");
            
            // pornesc thread-urile de incarcare a filmelor
            LoadMovies();

            pictureBoxImageMovie.SizeMode = PictureBoxSizeMode.Zoom;
            //ajustez din start imaginea pentru butonul de LogOut
            buttonLogOut.BackgroundImageLayout = ImageLayout.Zoom;
            buttonLogOut.FlatStyle = FlatStyle.Flat;
            buttonLogOut.FlatAppearance.BorderSize = 0;
            //incarcam un film cu datele de start
            /* textBoxDescriere.Text = "Don Vito Corleone, head of a mafia family, decides to hand over his empire to his youngest son Michael. However, his decision unintentionally puts the lives of his loved ones in grave danger.";
             pictureBoxImageMovie.ImageLocation = "https://m.media-amazon.com/images/M/MV5BM2MyNjYxNmUtYTAwNi00MTYxLWJmNWYtYzZlODY3ZTk3OTFlXkEyXkFqcGdeQXVyNzkwMjQ5NzM@._V1_FMjpg_UY1982_.jpg";
             pictureBoxImageMovie.SizeMode = PictureBoxSizeMode.Zoom;
             textBoxTitlu.Text = "The Godfather";
             textBoxRating.Text = "9.2";
             textBoxAn.Text = "1972";
             textBoxGen.Text = "Crime, Drama";*/
      

        }

        #region METODE ASINCRONE PENTRU ANIMATIA DE LOGIN ?
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Form1_LoadAsync(object sender, EventArgs e)
        {
            Task loadingTask = AnimateLoading();
            Task curiozitatiTask = LoadCuriozitatiAsync();

            await Task.WhenAll(loadingTask, curiozitatiTask);
        }

        /// <summary>
        /// 
        /// LoadCuriozitatiAsync() - va incarca curiozitati pe pagina de Loading, atat timp cat 
        ///                          vom extrage cu web crawler-ul informatiile de pe imdb, se vor incarca cate 1 rand
        ///                          la fiecare 5 secunde
        ///                          , iar cand ajunge in capat, se reia de la prima curiozitate afisarea
        /// 
        /// </summary>
        /// <returns></returns>
        private async Task LoadCuriozitatiAsync()
        {
            string[] lines = new string[0];
            int currentIndex = 0;

            while (true)
            {
                if (File.Exists(filePath))
                {
                    using (StreamReader reader = new StreamReader(filePath))
                    {
                        string content = await reader.ReadToEndAsync();
                        lines = content.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                    }
                }
                else
                {
                    lines = new string[0];
                }

                if (lines.Length > 0)
                {
                    string curiozitatiText = string.Empty;

                    for (int i = 0; i < 1; i++)
                    {
                        curiozitatiText += lines[currentIndex] + Environment.NewLine;
                        currentIndex++;

                        if (currentIndex >= lines.Length)
                        {
                            currentIndex = 0; // Se reia de la început
                        }
                    }

                    textBoxCuriozitati.Text = curiozitatiText.Trim();
                }
                else
                {
                    textBoxCuriozitati.Text = string.Empty;
                }

                await Task.Delay(5000); // Așteaptă 5 secunde
            }
        }


        /// <summary>
        /// 
        /// AnimateLoading() - va incarca cele 4 imagini de loading pentru rotita de loadin
        ///                     la fiecare secunda
        /// 
        /// </summary>
        /// <returns></returns>
        private async Task AnimateLoading()
        {
            while (true)
            {
                pictureBox.SizeMode = PictureBoxSizeMode.Zoom;

                // Construiți calea absolută către fișierul imaginii
                string imagePath = System.IO.Path.Combine(Application.StartupPath, $"load{currentIndex + 1}.png");

                // Încărcați imaginea curentă
                pictureBox.Image = Image.FromFile(imagePath);


                // Așteptați 1 secundă
                await Task.Delay(1000);

                // Ascundeți imaginea curentă
                pictureBox.Image = null;

                // Incrementați indexul pentru a trece la următoarea imagine
                currentIndex = (currentIndex + 1) % 4; // 4 reprezintă numărul total de imagini
            }
        }
        #endregion



        /// <summary>
        /// 
        /// RotunjesteColturileButonului- functie ce va rotunji colturile butonului dupa o anumite raza
        /// specificata.
        /// 
        /// </summary>
        /// <returns></returns>
        private void RotunjesteColturileButonului(Button button, int razaColturi)
        {
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.FlatAppearance.MouseDownBackColor = button.BackColor;
            button.FlatAppearance.MouseOverBackColor = button.BackColor;

            button.Paint += (sender, e) =>
            {
                GraphicsPath path = new GraphicsPath();
                path.AddArc(0, 0, razaColturi, razaColturi, 180, 90); // Colțul stânga-sus
                path.AddArc(button.Width - razaColturi, 0, razaColturi, razaColturi, 270, 90); // Colțul dreapta-sus
                path.AddArc(button.Width - razaColturi, button.Height - razaColturi, razaColturi, razaColturi, 0, 90); // Colțul dreapta-jos
                path.AddArc(0, button.Height - razaColturi, razaColturi, razaColturi, 90, 90); // Colțul stânga-jos
                path.CloseAllFigures();

                button.Region = new Region(path);
            };
        }

        /// <summary>
        /// Metoda de incarcare a unei imagini pe un control primind ca parametru un url
        /// </summary>
        /// <param name="imageUrl">URL-ul pentru descarcarea imaginii</param>
        private void LoadImageFromUrl(string imageUrl)
        {
            try
            {
                using (WebClient webClient = new WebClient())
                {
                    // Descărcăm imaginea de la URL
                    byte[] imageData = webClient.DownloadData(imageUrl);

                    // Transformăm datele imaginei în obiectul Image
                    using (var stream = new System.IO.MemoryStream(imageData))
                    {
                        Image image = Image.FromStream(stream);

                        // Setăm imaginea în PictureBox
                        pictureBoxImageMovie.Image = image;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Eroare la încărcarea imaginii: " + ex.Message);
            }
        }


        /// <summary>
        /// 
        /// SetButtonAppearance- functie ce va modifica culoarea de back a butonului si
        /// fontSize-ul, pentru a da o aparență mai plăcută la vedere.
        /// 
        /// </summary>
        /// <returns></returns>
        private void SetButtonAppearance(Button button, Color backColor, int fontSize, bool isBold)
        {
            button.BackColor = backColor;
            button.Font = new Font(button.Font.FontFamily, fontSize, isBold ? FontStyle.Bold : FontStyle.Regular);
        }


        /// <summary>
        /// 
        /// ChangeState- functie ce va modifica starea  in _currentState(tip TabPage) 
        /// din dictionarul de stari _stateDictionary, astfel vom putea comuta din o pagina in alta.
        /// 
        /// </summary>
        /// <returns></returns>
        public void ChangeState(string state)
        {
            if (_stateDictionary.ContainsKey(state))
            {

                _currentState = _stateDictionary[state]; // Update the current state
                tabControlAppFlow.Enabled = true;
                tabControlAppFlow.SelectedTab = _currentState;
            }
            else
            {
                throw new ArgumentException("Invalid state.");
            }
        }



        /// <summary>
        /// Funtie de callback pentru schimbarea tipului de filme incarcate pentru swapping
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_top50_Click(object sender, EventArgs e)
        {
            RotunjesteColturileButonului(buttonTop50, 14);

            SetButtonAppearance(buttonTop50, SystemColors.ButtonHighlight, 12, true);
            SetButtonAppearance(buttonAction, Color.Silver, 10, true);
            SetButtonAppearance(buttonBiography, Color.Silver, 10, true);
            SetButtonAppearance(buttonCurentYear, Color.Silver, 10, true);
            SetButtonAppearance(buttonHorror, Color.Silver, 10, true);
            SetButtonAppearance(buttonComedy, Color.Silver, 10, true);

            ChangeCategory(MovieCategory.All);
        }

        /// <summary>
        /// Funtie de callback pentru schimbarea tipului de filme incarcate pentru swapping
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonCurentYear_Click(object sender, EventArgs e)
        {
            RotunjesteColturileButonului(buttonCurentYear, 14);

            SetButtonAppearance(buttonCurentYear, SystemColors.ButtonHighlight, 12, true);
            SetButtonAppearance(buttonAction, Color.Silver, 10, true);
            SetButtonAppearance(buttonBiography, Color.Silver, 10, true);
            SetButtonAppearance(buttonTop50, Color.Silver, 10, true);
            SetButtonAppearance(buttonHorror, Color.Silver, 10, true);
            SetButtonAppearance(buttonComedy, Color.Silver, 10, true);

            ChangeCategory(MovieCategory.New);
        }

        /// <summary>
        /// Funtie de callback pentru schimbarea tipului de filme incarcate pentru swapping
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonAction_Click(object sender, EventArgs e)
        {
            RotunjesteColturileButonului(buttonAction, 14);

            SetButtonAppearance(buttonAction, SystemColors.ButtonHighlight, 12, true);
            SetButtonAppearance(buttonCurentYear, Color.Silver, 10, true);
            SetButtonAppearance(buttonBiography, Color.Silver, 10, true);
            SetButtonAppearance(buttonTop50, Color.Silver, 10, true);
            SetButtonAppearance(buttonHorror, Color.Silver, 10, true);
            SetButtonAppearance(buttonComedy, Color.Silver, 10, true);

            ChangeCategory(MovieCategory.Action);
        }

        /// <summary>
        /// Funtie de callback pentru schimbarea tipului de filme incarcate pentru swapping
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonBiography_Click(object sender, EventArgs e)
        {
            RotunjesteColturileButonului(buttonBiography, 14);

            SetButtonAppearance(buttonBiography, SystemColors.ButtonHighlight, 12, true);
            SetButtonAppearance(buttonAction, Color.Silver, 10, true);
            SetButtonAppearance(buttonCurentYear, Color.Silver, 10, true);
            SetButtonAppearance(buttonTop50, Color.Silver, 10, true);
            SetButtonAppearance(buttonHorror, Color.Silver, 10, true);
            SetButtonAppearance(buttonComedy, Color.Silver, 10, true);

            ChangeCategory(MovieCategory.Biography);
        }

        /// <summary>
        /// Funtie de callback pentru schimbarea tipului de filme incarcate pentru swapping
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonComedy_Click(object sender, EventArgs e)
        {
            RotunjesteColturileButonului(buttonComedy, 14);

            SetButtonAppearance(buttonComedy, SystemColors.ButtonHighlight, 12, true);
            SetButtonAppearance(buttonAction, Color.Silver, 10, true);
            SetButtonAppearance(buttonBiography, Color.Silver, 10, true);
            SetButtonAppearance(buttonTop50, Color.Silver, 10, true);
            SetButtonAppearance(buttonHorror, Color.Silver, 10, true);
            SetButtonAppearance(buttonCurentYear, Color.Silver, 10, true);

            ChangeCategory(MovieCategory.Comedy);
        }

        /// <summary>
        /// Funtie de callback pentru schimbarea tipului de filme incarcate pentru swapping
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonHorror_Click(object sender, EventArgs e)
        {

            RotunjesteColturileButonului(buttonHorror, 14);

            SetButtonAppearance(buttonHorror, SystemColors.ButtonHighlight, 12, true);
            SetButtonAppearance(buttonAction, Color.Silver, 10, true);
            SetButtonAppearance(buttonBiography, Color.Silver, 10, true);
            SetButtonAppearance(buttonTop50, Color.Silver, 10, true);
            SetButtonAppearance(buttonCurentYear, Color.Silver, 10, true);
            SetButtonAppearance(buttonComedy, Color.Silver, 10, true);
            ChangeCategory(MovieCategory.Horror);
           
        }

        /// <summary>
        /// Funtie de callback pentru trecerea la tab-ul de sign in
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void linkLabel_createAcount_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ChangeState("Inregistrare");
        }

        /// <summary>
        /// Functie de call-back pentru o incercare de login;
        /// Se face o interogare la baza de data prin intermediul _dataAccessSecurityProxy
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonLoginToMenu_Click(object sender, EventArgs e)
        {
            Person account = _dataAccessProxy.ReadPersonByUsername(textBoxLogInUsername.Text.Trim());

            if (account != null)
            {
                textBoxNumPrenMyProfile.Text = account.FirstName.ToString() + " " + account.LastName.ToString();
                if (account.Password.Equals(textBoxLogInPassword.Text.Trim()))
                {
                    // setam lista de filme si username-ul pentru persoana logata
                    _username = textBoxLogInUsername.Text.Trim();
                    _savedMovies = _dataAccessProxy.viewMovies(_username);

                    // astepam executia webCrawler-ului doar la prima logare
                    if (_isFirstLog)
                    {
                        WaitMovieLoading();
                        _isFirstLog = false;
                    }
                    else
                    {
                        ChangeState("Menu");
                        button_top50_Click(null, EventArgs.Empty);
                        _currentAction = 0;
                        _currentAll = 0;
                        _currentBiography = 0;
                        _currentComedy = 0;
                        _currentHorror = 0;
                        _currentNew = 0;
                    }
      
                }
                else 
                {
                    MessageBox.Show("Parolă greșită!", "Avertisment", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                textBoxLogInUsername.Text = "";
                textBoxLogInPassword.Text = "";
                textBoxRegEmail.Text = "";
                textBoxRegNume.Text = "";
                textBoxRegPassword.Text = "";
                textBoxRegPrenume.Text = "";
                textBoxRegUsername.Text = "";
                MessageBox.Show("Username greșit!", "Avertisment", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    
    /// <summary>
    ///  Funtie de callback pentru trecerea la tab-ul de login
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void buttonLogin_Click(object sender, EventArgs e)
        {
            ChangeState("Login");
        }

        private void buttonCreateAc_Click(object sender, EventArgs e)
        {
            ChangeState("Inregistrare");
        }

        /// <summary>
        /// Functie de callback care inserca inserarea unui nou cont in baza de date
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonInregistrare_Click(object sender, EventArgs e)
        {
            var password = textBoxRegPassword.Text;
            var username = textBoxRegUsername.Text;
            var nume = textBoxRegNume.Text;
            var prenume = textBoxRegPrenume.Text;
            var email = textBoxRegEmail.Text;

            Person newPerson = new Person(username, password, email, prenume, nume);
            try
            {
                _dataAccessProxy.InsertUser(new Person(username, password, email, prenume, nume));
                textBoxLogInUsername.Text = "";
                textBoxLogInPassword.Text = "";
                ChangeState("Login");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to sign in. " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 
        /// checkBox_showPasw1_CheckedChanged && checkBox_showpasw2_CheckedChanged:
        /// funcții ce vor modifica la bifarea ceckBoxului
        /// caracterele ascunse cu "*", cu caracterele introduse de utilizator.
        /// 
        /// </summary>
        /// <returns></returns>
        /// 
        private void checkBox_showPasw1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxShowPaswAtReg.Checked == true)
            {
                textBoxRegPassword.UseSystemPasswordChar = true;
            }
            else
            {
                textBoxRegPassword.UseSystemPasswordChar = false;
            }
        }

        private void checkBox_showpasw2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxShowPaswLogIn.Checked == true)
            {
                textBoxLogInPassword.UseSystemPasswordChar = true;
            }
            else
            {
                textBoxLogInPassword.UseSystemPasswordChar = false;
            }
        }

        /// <summary>
        /// Functie de callback care face trecerea la afisarea filmelor salvate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonToMyProfile_Click(object sender, EventArgs e)
        {
            
            RotunjesteColturileButonului(buttonToMyProfile, 12);
            ChangeState("MyProfile");


            // incarcarea filmelor in control
            dataGridViewMovies.Rows.Clear();
            _savedMovies = _dataAccessProxy.viewMovies(_username);
            foreach (Movie movie in _savedMovies)
            {
                dataGridViewMovies.Rows.Add(movie.Title, movie.Year, string.Join(", ", movie.Genres),  movie.Rating);
            }
        }

        private void buttonToMainMenu_Click(object sender, EventArgs e)
        {
            RotunjesteColturileButonului(buttonToMainMenu, 12);
            ChangeState("Menu");
             
        }

        private void sageataDreapta_Click(object sender, EventArgs e)
        {
            Person account = _dataAccessProxy.ReadPersonByUsername(textBoxLogInUsername.Text.Trim());
            try
            {
                if (!isSaved(GetCurrentMovie()))
                {
                    // salvare in baza de date
                    _dataAccessProxy.InsertMovie(GetCurrentMovie(), account.Username);
                    NextMovie();
                }
                else
                {
                    //schimbare film afisat
                    NextMovie();
                }
            }
            catch (Exception ex)
            {
                throw ex;
                // inseamna ca nu mai sunt filme de afisat
            }
        }

        private void sageataStanga_Click(object sender, EventArgs e)
        {
            try
            {
                NextMovie();
            }
            catch(Exception ex)
            {

            }
        }

        /// <summary>
        /// Metoda pentru a verifica daca un film a fost salvat
        /// </summary>
        /// <param name="movie"></param>
        /// <returns></returns>
        private bool isSaved(Movie movie)
        {
            List<Movie> savedMovies = _dataAccessProxy.viewMovies(_username);
            return savedMovies.Any(m => m.Title.Equals(movie.Title));
        }
        
        private void buttonLogOut_Click(object sender, EventArgs e)
        {
            //Login
            textBoxLogInUsername.Text = "";
            textBoxLogInPassword.Text = "";
            //Inregistrarea
            textBoxLogInUsername.Text = "";
            textBoxLogInPassword.Text = "";
            textBoxRegEmail.Text = "";
            textBoxRegNume.Text = "";
            textBoxRegPassword.Text = "";
            textBoxRegPrenume.Text = "";
            textBoxRegUsername.Text = "";
            ChangeState("Login");
        }
        private void button1_Click_1(object sender, EventArgs e)
        {
            Help.ShowHelp(this, "Help.chm");
        }
    }
}
