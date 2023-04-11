using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using static WebApplication1.Pages.Books.BookDBModel;

namespace WebApplication1.Pages.Books
{
    public class UpdateBookModel : PageModel
    {
            public Books book = new Books();
        public string errorMessage="";
        public string successMessage = "";


        public void OnGet()
        {
            try 
            {
                string BookCode = Request.Query["BookCode"];
                SqlConnection sqlConnection = new SqlConnection("Data Source = HARSHITHP-PC;Initial Catalog = LMS_DB; Integrated Security = True; Connect Timeout = 30; Encrypt = False; Trust Server Certificate = False; Application Intent = ReadWrite; Multi Subnet Failover = False");
                sqlConnection.Open();
                SqlCommand command = sqlConnection.CreateCommand(); 

                command.CommandText = $"SELECT * from LMS_BOOK_DETAILS WHERE book_code = '{BookCode}';";
                Console.WriteLine("Query is fetched");
                SqlDataReader reader = command.ExecuteReader();
                while(reader.Read())
                {
                    book.book_code = (string)reader["book_code"];
                    book.book_title = (string)reader["book_title"];
                    book.book_edition = (int)reader["book_edition"];
                    book.Author = (string)reader["author"];
                    book.Publication = (string)reader["publication"];
                    book.Price = (double)reader["price"];
                    book.category = (string)reader["category"];
                    book.published_date = (DateTime)reader["published_date"];

                }
            }
            catch (Exception ex) 
            { 
                Console.WriteLine(ex.Message);
            }
            
        }
        public void OnPost()
        {
            SqlConnection sqlConnection = new SqlConnection("Data Source = HARSHITHP-PC;Initial Catalog = LMS_DB; Integrated Security = True; Connect Timeout = 30; Encrypt = False; Trust Server Certificate = False; Application Intent = ReadWrite; Multi Subnet Failover = False");
            try
            {
                sqlConnection.Open();
                string BookCode = Request.Query["BookCode"];
                errorMessage = "";
                successMessage = "";
                book.book_code = Request.Form["id"];
                book.book_title = Request.Form["title"];
                book.Author = Request.Form["author"];
                book.book_edition = Convert.ToInt32(Request.Form["edition"]);
                book.category = Request.Form["category"];
                book.Price = Convert.ToDouble(Request.Form["price"]);
                book.published_date = Convert.ToDateTime(Request.Form["date_published"]);
                book.Publication = Request.Form["publication"];
                SqlCommand command = sqlConnection.CreateCommand();
                string query = $"Update LMS_BOOK_DETAILS set book_title='{book.book_title}',category='{book.category}'," +
                                $"author='{book.Author}',Publication='{book.Publication}',published_date='{book.published_date}'" +
                                $",book_edition={book.book_edition},price={book.Price} where book_code='{BookCode}'";
                command.CommandText = query;
             
                Console.WriteLine(query);
                command.ExecuteNonQuery();
                successMessage = "Book added successfully";

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                errorMessage = ex.Message;
            }
        }
    }
}
