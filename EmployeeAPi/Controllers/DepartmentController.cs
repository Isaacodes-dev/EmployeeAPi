using EmployeeAPi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Data.SqlClient;

namespace EmployeeAPi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : Controller
    {
        private readonly IConfiguration _configuration;

        public DepartmentController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        //public string CheckId(int id)
        //{
        //    string query = @"
        //                    select DepartmentId, DepartmentName from dbo.Department
        //                    ";
        //    DataTable dataTable= new DataTable();
        //    string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");

        //}
        [HttpGet]
        public JsonResult Get()
        {
            string query = @"
                            select DepartmentId, DepartmentName from dbo.Department
                            ";
            DataTable table = new DataTable();

            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            SqlDataReader myReader;
            using(SqlConnection myConnection = new SqlConnection(sqlDataSource))
            {
                myConnection.Open();
                using(SqlCommand myCommand = new SqlCommand(query,myConnection)) 
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myConnection.Close();
                }
            }
            return new JsonResult(table);
        }
        [HttpPost]
        public JsonResult Post(Department department)
        {
            string query = @"
                            Insert into dbo.Department
                            values (@DepartmentName)
                            ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource)) 
            {
                myCon.Open();
                using (SqlCommand sqlCommand = new SqlCommand(query, myCon))
                {
                    sqlCommand.Parameters.AddWithValue("@DepartmentName", department.DepartmentName);
                    myReader = sqlCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                }
            }
            return new JsonResult("Added Sucessfully");
        }
        [HttpPut("{id}")]
        public JsonResult Update(Department department, int id)
        {
            string query = @"
                            Update dbo.Department
                            set DepartmentName = @DepartmentName
                            where DepartmentId = @DepartmentId
                            ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand sqlCommand = new SqlCommand(query, myCon))
                {
                    sqlCommand.Parameters.AddWithValue("@DepartmentId", id);
                    sqlCommand.Parameters.AddWithValue("@DepartmentName", department.DepartmentName);
                    myReader = sqlCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                }
            }
            return new JsonResult("Updated Sucessfully");
        }
        [HttpDelete("{id}")]
        public JsonResult Delete (int id)
        {
            string query = @"
                            delete from dbo.Department
                            where DepartmentId = @DepartmentId
                            ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand sqlCommand = new SqlCommand(query, myCon))
                {
                    sqlCommand.Parameters.AddWithValue("@DepartmentId", id);
                    myReader = sqlCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                }
            }
            return new JsonResult("Record Deleted Successfully");
        }
    }
}
