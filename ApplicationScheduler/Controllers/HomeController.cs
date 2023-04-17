using ApplicationScheduler.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Diagnostics;

namespace ApplicationScheduler.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        IConfiguration configuration;
        public SqlConnection connection;


        public HomeController(IConfiguration configuration)
        {
            this.configuration = configuration;
            this.connection = new SqlConnection(configuration.GetConnectionString("DevDB"));
        }

        public IActionResult Index()
        {
            return View();
        }
        public bool checkUser(int userId, string password)
        {
            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand("getUser", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@userId", userId);
                SqlDataReader reader = command.ExecuteReader();
                string getPassword = "";
                while (reader.Read())
                {
                    getPassword = (string)reader["userPassword"];
                }
                Console.WriteLine($"given password: {password}\t actual password: {getPassword}");
                if (password.Equals(getPassword.Trim()))
                {
                    Console.WriteLine("Correct Password");
                    return true;
                }
                reader.Close();
                connection.Close();
            }
            catch(SqlException  ex)
            {
                Console.WriteLine($"Error: {ex}");
            }
            Console.WriteLine("Passwords arent equal");
            return false;
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(int userId, string password, Users user)
        {
            try
            {
                Console.WriteLine("in post index: ");
                if (checkUser(userId, password))
                {
                    Console.WriteLine("inside check user");
                    return RedirectToAction("UserPage", "Users", new { userId = userId });
                }
                else
                {
                    Console.WriteLine("Password mismatch");
                    TempData["ErrorMessage"] = "Invalid userid or password";
                    return View();
                }

            }
            catch
            {
                return View();
            }
        }

        public ActionResult Register()
        {
            return View();
        }
        public void insertUser(Users user)
        {
            Console.WriteLine("inside insert user");
            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand("insertUser", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@userid", user.userId);
                command.Parameters.AddWithValue("@username", user.userName);
                command.Parameters.AddWithValue("@useremail", user.email);
                command.Parameters.AddWithValue("@userpassword", user.password);
                command.Parameters.AddWithValue("@userphonenumber", user.phoneNumber);

                command.ExecuteNonQuery();
                connection.Close();
            }
            catch (SqlException e)
            {
                Console.WriteLine("error: " + e.Message);
                throw;
            }
            Console.WriteLine(user.phoneNumber);
            Console.WriteLine("exiting insert user");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(Users user)
        {
            try
            {

                insertUser(user);
                return RedirectToAction("Index", "Home");
            }
            catch (SqlException e)
            {
                TempData["ErrorMessage"] = "userid exists";
                return View();
            }
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}