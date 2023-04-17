using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using WebAPI.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IConfiguration configuration;
        public ProductController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        // GET: api/<ProductController>
        [HttpGet]
        public IActionResult GetProducts()
        {
            List<ProductModel> products = new List<ProductModel>();
            try
            {
                using (SqlConnection connection = new SqlConnection(configuration.GetConnectionString("DB")))
                {
                    SqlCommand command = new SqlCommand("select * from Products", connection);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        ProductModel model = new ProductModel();
                        model.product_code = reader.GetString(0);
                        model.product_name = reader.GetString(1);
                        model.price = reader.GetFloat(2);
                        model.quantity_remaining = reader.GetInt32(3);
                        model.quantity_sold = reader.GetInt32(4);
                        products.Add(model);
                    }
                    connection.Close();
                    return Ok(products);

                }
                
            }
            catch (SqlException ex) 
            {
                Console.WriteLine(ex.Message);
                return NotFound();

            }
            //return new string[] { "value1", "value2" };
        }

        // GET api/<ProductController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ProductController>
        [HttpPost]
        public IActionResult insertProduct(ProductModel product)
        {
            try
            {
                using(SqlConnection connection = new SqlConnection(configuration.GetConnectionString("DB")))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("insertProducts", connection);
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@id", product.product_code);
                    command.Parameters.AddWithValue("@name", product.product_name);
                    command.Parameters.AddWithValue("@sold", product.quantity_sold);
                    command.Parameters.AddWithValue("@avail", product.quantity_remaining);
                    command.Parameters.AddWithValue("@price", product.price);

                    command.ExecuteNonQuery();
                    return Ok(new
                    {
                        status = true,
                        message = "product created"
                    });
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
                return NoContent();
            }
        }

        // PUT api/<ProductController>/5
        [HttpPut("{id}")]
        public IActionResult Put(string id, ProductModel product)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(configuration.GetConnectionString("DB")))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("Edit_Prod", connection);
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@id", product.product_code);
                    command.Parameters.AddWithValue("@name", product.product_name);
                    command.Parameters.AddWithValue("@sold", product.quantity_sold);
                    command.Parameters.AddWithValue("@avail", product.quantity_remaining);
                    command.Parameters.AddWithValue("@price", product.price);

                    command.ExecuteNonQuery();
                    return Ok(new
                    {
                        status = true,
                        message = "product altered"
                    });
                }
            }
            catch (SqlException s)
            {
                Console.WriteLine(s.Message);
                return NoContent();
            }
        }

        // DELETE api/<ProductController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(configuration.GetConnectionString("DB")))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("delete_prod", connection);
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@code", id);
                    command.ExecuteNonQuery();
                    return Ok(new
                    {
                        status = true,
                        message = "Deleted succesfully"
                    });

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NoContent();
            }
        }
    }
}
