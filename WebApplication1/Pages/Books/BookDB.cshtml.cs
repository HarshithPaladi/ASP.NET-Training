using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using static System.Net.Mime.MediaTypeNames;
using System;

namespace WebApplication1.Pages.Books
{
    public class BookDBModel : PageModel
    {
        public List<Books> bookList = new List<Books>();
        public static string str = "THIS IS STATIC PUBLIC";
        public void OnGet()
        {
            try
            {
                //SqlConnection sqlConnection = new SqlConnection("Data Source=HARSHITHP-PC;Initial Catalog=LMS_DB;Integrated Security=True;Encrypt=False;");
                SqlConnection sqlConnection = new SqlConnection("Data Source = HARSHITHP-PC;Initial Catalog = LMS_DB; Integrated Security = True; Connect Timeout = 30; Encrypt = False; Trust Server Certificate = False; Application Intent = ReadWrite; Multi Subnet Failover = False");
                sqlConnection.Open();
                string query = "SELECT book_code,book_title,author,publication,price from LMS_BOOK_DETAILS";
                SqlCommand command = new SqlCommand(query, sqlConnection);
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Books book = new Books();
                    book.Id = (string)reader["book_code"];
                    book.BookName = (string)reader["book_title"];
                    book.Author = (string)reader["publication"];
                    book.Publication = (string)reader["publication"];
                    book.Price = (double)reader["price"];

                    bookList.Add(book);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public class Books
        {
            public string Id { get; set; }
            public string BookName { get; set; }
            public string Author { get; set; }
            public string Publication { get; set; }
            public double Price { get; set; }
        }
    }
}
