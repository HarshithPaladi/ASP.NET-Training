using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using MVC.Models;

namespace MVC.Controllers
{
    public class MovieController : Controller
    {
        IConfiguration _configuration;
        SqlConnection _GetSqlConnection;

        public MovieController(IConfiguration configuration)
        {
            _configuration = configuration;
            _GetSqlConnection = new SqlConnection(_configuration.GetConnectionString("Movies"));

        }
        public List<MovieModel> GetMoviesList()
        {
            List<MovieModel> movies = new();
            _GetSqlConnection.Open();
            SqlCommand command = new SqlCommand("fetch_movies",_GetSqlConnection);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                MovieModel movie = new();
                movie.id = (int)reader["id"];
                movie.name = (string)reader["name"];
                movie.director = (string)reader["director"];
                movie.release_year = (int)reader["release_year"];
                movie.genre = (string)reader["genre"];
                movie.rating = (double)reader["rating"];
                movies.Add(movie);
            }
            reader.Close();
            _GetSqlConnection.Close();

            return movies;
        }
        // GET: MovieController
        public ActionResult Index()
        {
            return View(GetMoviesList());
        }

        // GET: MovieController/Details/5
        public ActionResult Details(int id)
        {
            return View(GetMovie(id));
        }

        // GET: MovieController/Create
        public ActionResult Create()
        {
            return View();
        }
        void InsertMovie(MovieModel movie)
        {
            _GetSqlConnection.Open();
            SqlCommand cmd = new SqlCommand("addMovies", _GetSqlConnection);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@name", movie.name);
            cmd.Parameters.AddWithValue("@release_year", movie.release_year);
            cmd.Parameters.AddWithValue("@genre", movie.genre);
            cmd.Parameters.AddWithValue("@director", movie.director);
            cmd.Parameters.AddWithValue("@rating", movie.rating);


            cmd.ExecuteNonQuery();
            _GetSqlConnection.Close();
        }

        // POST: MovieController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(MovieModel movie)
        {
            try
            {
                InsertMovie(movie);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return View();
            }
        }
        MovieModel GetMovie(int id)
        {
            _GetSqlConnection.Open();
            SqlCommand cmd = new SqlCommand("fetch_movieDetails", _GetSqlConnection);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@id", id);

            SqlDataReader reader = cmd.ExecuteReader();

            MovieModel movie = new();

            while (reader.Read())
            {

                movie.id = (int)reader["id"];
                movie.name = (string)reader["name"];
                movie.release_year = (int)reader["release_year"];
                movie.genre = (string)reader["genre"];
                movie.director = (string)reader["director"];
                movie.rating = (double)reader["rating"];
            }
            return movie;
        }

        // GET: MovieController/Edit/5
        public ActionResult Edit(int id)
        {
            return View(GetMovie(id));
        }
        void UpdateMovies(MovieModel movie)
        {
            _GetSqlConnection.Open();
            SqlCommand cmd = new SqlCommand("editMovies", _GetSqlConnection);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@name", movie.name);
            cmd.Parameters.AddWithValue("@id", movie.id);
            cmd.Parameters.AddWithValue("@rating", movie.rating);
            cmd.Parameters.AddWithValue("@director", movie.director);
            cmd.Parameters.AddWithValue("@genre", movie.genre);
            cmd.Parameters.AddWithValue("@release_year", movie.release_year);
            //Console.WriteLine(cmd.CommandText);
            cmd.ExecuteNonQuery();
            _GetSqlConnection.Close();


        }
        // POST: MovieController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id,MovieModel movie)
        {
            try
            {
                Console.WriteLine($"ID - {id}");
                UpdateMovies(movie);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: MovieController/Delete/5
        public ActionResult Delete(int id)
        {
            return View(GetMovie(id));
        }
        void DeleteMovie(MovieModel movie)
        {
            _GetSqlConnection.Open();
            SqlCommand cmd = new SqlCommand("deleteMovies", _GetSqlConnection);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@id", movie.id);
            cmd.ExecuteNonQuery();
            _GetSqlConnection.Close();

        }
        // POST: MovieController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, MovieModel movie)
        {
            try
            {
                DeleteMovie(movie);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
