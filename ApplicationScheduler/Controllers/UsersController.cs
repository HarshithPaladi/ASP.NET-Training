using ApplicationScheduler.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace ApplicationScheduler.Controllers
{
    public class UsersController : Controller
    {
        IConfiguration configuration;
        public SqlConnection connection;


        public UsersController(IConfiguration configuration)
        {
            this.configuration = configuration;
            this.connection = new SqlConnection(configuration.GetConnectionString("DevDB"));
        }
        public Users getUser(int userId)
        {
            Console.WriteLine("entered getUser method");
            Users user = new Users();
            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand("getUser", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@userid", userId);
                SqlDataReader reader = command.ExecuteReader();
                Console.WriteLine("reader excecuted");
                while (reader.Read())
                {
                    user.password = (string)reader["userPassword"];
                    user.phoneNumber = (int)reader["phnumber"];
                    user.userId = (int)reader["userId"];
                    user.email = (string)reader["emailId"];
                    user.userName = (string)reader["userName"];
                }
                reader.Close();
                connection.Close();

            }
            catch (SqlException ex)
            {
                Console.WriteLine("error: " + ex.Message);
            }
            return user;
        }
        public void insertAppointments(Appointment appointment)
        {
            try
            {
                Console.WriteLine("in inesrt appointments");
                connection.Open();
                SqlCommand command = new SqlCommand("addAppointment", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@userid", appointment.Id);
                command.Parameters.AddWithValue("@starttime", appointment.startTime);
                command.Parameters.AddWithValue("@endtime", appointment.endTime);
                command.Parameters.AddWithValue("@createdtime", appointment.CreatedDate);
                command.Parameters.AddWithValue("@updatedtime", DateTime.Now);
                command.Parameters.AddWithValue("@desc", appointment.Description);
                command.Parameters.AddWithValue("@status", appointment.Status);
                command.Parameters.AddWithValue("@place", appointment.place);
                command.Parameters.AddWithValue("@meet_person", appointment.meeting_person);
                command.Parameters.AddWithValue("@purpose", appointment.purpose==null?null:appointment.purpose);
                command.Parameters.AddWithValue("@reference", appointment.reference);
                
                Console.WriteLine("completed parameters");
                command.ExecuteNonQuery();
                connection.Close();
                Console.WriteLine("executed the query");
                Console.WriteLine("exit insert method");
            }
            catch(SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public List<Appointment> getdateAppointments(int userid)
        {
            // All appointments of that user
            Console.WriteLine("entered get date appointmensts method");
            List<Appointment> appointmentsList = new List<Appointment>();
            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand("getdateAppointments", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@userid", userid);
                command.Parameters.AddWithValue("@date", DateTime.Now);
                SqlDataReader reader = command.ExecuteReader();
                Console.WriteLine("reader excecuted");
                while (reader.Read())
                {
                    Appointment a = new Appointment();

                    a.Id = (int)reader["Id"];
                    a.Name = (string)reader["title"];
                    a.Description = (string)reader["description"];
                    a.CreatedDate = (DateTime)reader["Date_of_appointment"];
                    a.startTime = (DateTime)reader["startTime"];
                    a.endTime = (DateTime)reader["EndTime"];
                    //a.userId = (string)reader["userId"];

                    appointmentsList.Add(a);

                }
                reader.Close();
                connection.Close();

            }
            catch (SqlException ex)
            {
                Console.WriteLine("error: " + ex.Message);
            }

            return appointmentsList;
        }
        public ActionResult UserPage(int userId)
        {
            Console.WriteLine("user id in user page method: " + userId);
            Users currentUser = getUser(userId);
            ViewBag.appointmentList = getdateAppointments(userId);
            Console.WriteLine("user Name: " + currentUser.userName);

            return View(currentUser);
        }
        // GET: ApplicationScheduler
        public ActionResult Index()
        {
            return View();
        }

        // GET: ApplicationScheduler/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ApplicationScheduler/Create
        public ActionResult Create(int userId)
        {
            Console.WriteLine("in create method id: " + userId);
            ViewBag.userId = userId; 
            return View();
        }

        // POST: ApplicationScheduler/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Users user, Appointment appointment)
        {
            try
            {
                insertAppointments(appointment);
                Console.WriteLine("using user obj in create post method: user id: " + user.userId);

                return RedirectToAction("UserPage", "User", user);
            }
            catch
            {
                return View();
            }
        }

        public ActionResult List(string userId)
        {
            if (userId == null)
            {
                userId = (string)Request.Query["userId"];
            }
            try
            {
                Console.WriteLine("in list method userid after query" + userId);
                ViewBag.userId = userId;
                return View(GetAppointments(userId));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return View();
            }
        }
        // GET: ApplicationScheduler/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ApplicationScheduler/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ApplicationScheduler/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ApplicationScheduler/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
