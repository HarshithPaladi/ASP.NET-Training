using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace WebApplication1.Pages.Books
{
    public class CreateBookModel : PageModel
    {
        public void OnGet()
        {

        }
        Books book = new Books();
        public string errorMessage = "";
        public string successMessage = "";
        public void OnPost()
        {
            SqlConnection sqlConnection = new SqlConnection("Data Source = HARSHITHP-PC;Initial Catalog = LMS_DB; Integrated Security = True; Connect Timeout = 30; Encrypt = False; Trust Server Certificate = False; Application Intent = ReadWrite; Multi Subnet Failover = False");
            sqlConnection.Open();
            book.book_code = Request.Form["id"];
            book.book_title = Request.Form["title"];
            book.Author = Request.Form["author"];
            book.book_edition = Convert.ToInt32(Request.Form["edition"]);
            book.category = Request.Form["category"];
            book.Price = Convert.ToDouble(Request.Form["price"]);
            book.published_date = Convert.ToDateTime(Request.Form["date_published"]);
            book.Publication = Request.Form["publication"];
            if (book.Price> 1000)
            {
                errorMessage = "The price can't be more than 100";
                return;
            }

            book.date_arrival = Convert.ToDateTime("2023-04-11");
            book.rack_num = "a2";
            book.supplier_id = "s06";
            try
            {
                errorMessage = "";
                successMessage = "";
                string query = $"INSERT INTO LMS_BOOK_DETAILS values('{book.book_code}','{book.book_title}','{book.category}'," +
                                $"'{book.Author}','{book.Publication}','{book.published_date}'" +
                                $",'{book.book_edition}',{book.Price},'{book.rack_num}','{book.date_arrival}'" +
                                $",'{book.supplier_id}')";
                SqlCommand command = new SqlCommand(query, sqlConnection);
                Console.WriteLine(query);
                command.ExecuteNonQuery();
                successMessage = "Book added successfully";

            }
            catch(Exception ex)
            {
                Console.WriteLine("Error: "+ex.Message);
                errorMessage = ex.Message;
            }
            
        }
    }
    public class Books
    {
        public string book_code { get; set; }
        public string book_title { get; set; }
        public string category{ get; set; }

        public string Author { get; set; }
        public string Publication { get; set; }
        public DateTime published_date { get; set; }
        public int book_edition { get; set; }

        public double Price { get; set; }
        public string rack_num { get; set; }
        public DateTime date_arrival { get; set; }
        public string supplier_id { get; set; }
    }
}
