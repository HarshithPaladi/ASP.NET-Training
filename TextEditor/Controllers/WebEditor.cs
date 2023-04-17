﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using TextEditor.Models;

namespace TextEditor.Controllers
{
    public class WebEditor : Controller
    {
        private readonly ILogger<HomeController> _logger;
        IConfiguration configuration;
        public SqlConnection connection;
        public WebEditor(IConfiguration configuration)
        {
            this.configuration = configuration;
            this.connection = new SqlConnection(configuration.GetConnectionString("DB"));
        }
        // GET: Editor
        public ActionResult Index()
        {
            return View(GetDocs());
        }
        public List<WebEditorModel> GetDocs()
        {
            List<WebEditorModel> docs = new List<WebEditorModel>();
            try
            {
                using (SqlConnection connection = new SqlConnection(configuration.GetConnectionString("DB")))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand("select * from webEditor", connection);
                    SqlDataReader reader = cmd.ExecuteReader();

                    WebEditorModel editor = new();

                    while (reader.Read())
                    {

                        editor.doc_id = (int)reader["doc_id"];
                        editor.data = (string)reader["doc_data"];
                        editor.date_created = (DateTime)reader["date_created"];
                        editor.date_updated = (DateTime)reader["date_updated"];
                        editor.author = (string)reader["author"];
                        Console.WriteLine($"doc id - {editor.doc_id}");
                        docs.Add(editor);
                    }
                    return docs;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return docs;
            }
        }
        // GET: Editor/Details/5
        public ActionResult Details(int id)
        {

            return View(GetDocs());
        }
        public ActionResult List(WebEditorModel editor)
        {
            return View(nameof(List));
        }

        // GET: Editor/Create
        public ActionResult Create()
        {
            return View(nameof(Create));
        }
        public void insertData(WebEditorModel editor)
        {
            Console.WriteLine("Inside insert data");
            try
            {
                using (SqlConnection connection = new SqlConnection(configuration.GetConnectionString("DB")))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("insertData", connection);
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@doc_id", editor.doc_id);
                    command.Parameters.AddWithValue("@doc_data", editor.data);
                    command.Parameters.AddWithValue("@date_created", DateTime.Now);
                    command.Parameters.AddWithValue("@date_updated", DateTime.Now);
                    command.Parameters.AddWithValue("@author", editor.author);
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        // POST: Editor/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(WebEditorModel editor)
        {
            try
            {
                insertData(editor);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return View();
            }
        }
        WebEditorModel GetDocsObj(int id)
        {
            using (SqlConnection connection = new SqlConnection(configuration.GetConnectionString("DB")))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("fetch_docDetails", connection);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@doc_id", id);

                SqlDataReader reader = cmd.ExecuteReader();

                WebEditorModel doc= new();

                while (reader.Read())
                {

                    doc.doc_id = (int)reader["doc_id"];
                    doc.data = (string)reader["doc_data"];
                    doc.date_created = (DateTime)reader["date_created"];
                    doc.date_updated = (DateTime)reader["date_updated"];
                    doc.author = (string)reader["author"];
                    Console.WriteLine($"doc id - {doc.doc_id}");
                }
                return doc;
            }
        }

        // GET: Editor/Edit/5
        public ActionResult Edit(int id)
        {
            return View(GetDocsObj(id));
        }
        // POST: Editor/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, WebEditorModel docs)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(configuration.GetConnectionString("DB")))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand("update_doc", connection);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@doc_id", docs.doc_id);
                    cmd.Parameters.AddWithValue("@date_created", docs.date_created);
                    cmd.Parameters.AddWithValue("@date_updated", DateTime.Now);
                    cmd.ExecuteNonQuery();

                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
                return RedirectToAction("Index","WebEditor");
        }

        // GET: Editor/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Editor/Delete/5
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
