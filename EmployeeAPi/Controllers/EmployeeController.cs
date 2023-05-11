using Microsoft.AspNetCore.Mvc;
using EmployeeAPi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Data.SqlClient;
namespace EmployeeAPi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : Controller
    {
        private readonly IWebHostEnvironment 
            _webHostEnvironment;
        private readonly IConfiguration _configuration;
        public EmployeeController(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            _configuration = configuration;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public JsonResult Get()
        {
            string query = @"
                              select EmployeeId, EmployeeName,Department,convert(varchar(10),DateOfJoining,120) as DateOfJoining,PhotoFileName from dbo.Employee
                            ";
            DataTable table = new DataTable();
            string dataSource = _configuration.GetConnectionString("EmployeeAppCon");
            SqlDataReader myReader;
            using (SqlConnection sqlConnection = new SqlConnection(dataSource))
            {
                sqlConnection.Open();
                using (SqlCommand command = new SqlCommand(query, sqlConnection))
                {
                    myReader = command.ExecuteReader();
                    table.Load(myReader);
                    sqlConnection.Close();
                }
            }
            return new JsonResult(table);
        }
        [HttpPost]
        public JsonResult Post(Employee employee)
        {
            string query = @"
                              insert into dbo.Employee(EmployeeName,Department,DateOfJoining,PhotoFileName)
                                values(@EmployeeName,@Department,@DateOfJoining,@PhotoFileName)
                            ";
            DataTable table = new DataTable();
            string dataSource = _configuration.GetConnectionString("EmployeeAppCon");
            SqlDataReader myReader;
            using (SqlConnection sqlConnection = new SqlConnection(dataSource))
            {
                sqlConnection.Open();
                using (SqlCommand command = new SqlCommand(query, sqlConnection))
                {
                    command.Parameters.AddWithValue("@EmployeeName", employee.EmployeeName);
                    command.Parameters.AddWithValue("@Department", employee.Department);
                    command.Parameters.AddWithValue("@DateOfJoining", employee.DateofJoining);
                    command.Parameters.AddWithValue("@PhotoFileName", employee.PhotoFileName);
                    myReader = command.ExecuteReader();
                    table.Load(myReader);
                    sqlConnection.Close();
                }
            }
            return new JsonResult("Employee Recorded Added Successfully");
        }

        [HttpPut]
        public JsonResult Put(Employee employee)
        {
            string query = @"
                              update dbo.Employee set EmployeeName = @EmployeeName,Department = @Department,DateOfJoining = @DateOfJoining,PhotoFileName = @PhotoFileName
                                where EmployeeId = @EmployeeId
                            ";
            DataTable table = new DataTable();
            string dataSource = _configuration.GetConnectionString("EmployeeAppCon");
            SqlDataReader myReader;
            using (SqlConnection sqlConnection = new SqlConnection(dataSource))
            {
                sqlConnection.Open();
                using (SqlCommand command = new SqlCommand(query, sqlConnection))
                {
                    command.Parameters.AddWithValue("@EmployeeName", employee.EmployeeName);
                    command.Parameters.AddWithValue("@Department", employee.Department);
                    command.Parameters.AddWithValue("@DateOfJoining", employee.DateofJoining);
                    command.Parameters.AddWithValue("@PhotoFileName", employee.PhotoFileName);
                    myReader = command.ExecuteReader();
                    table.Load(myReader);
                    sqlConnection.Close();
                }
            }
            return new JsonResult("Employee Recorded Updated Successfully");
        }
        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            string query = @" delete from dbo.Employee where EmployeeId = @EmployeeId";

            DataTable table = new DataTable();

            string dataSource = _configuration.GetConnectionString("EmployeeAppCon");
            SqlDataReader myReader;
            using (SqlConnection sqlConnection = new SqlConnection(dataSource))
            {
                sqlConnection.Open();
                using (SqlCommand command = new SqlCommand(query, sqlConnection))
                {
                    command.Parameters.AddWithValue("@EmployeeId", id);
                    myReader = command.ExecuteReader();
                    table.Load(myReader);
                    sqlConnection.Close();
                }
            }
            return new JsonResult("Record Successfully Deleted");
        }

        [Route("SaveFile")]
        [HttpPost]
        public JsonResult SaveFile()
        {
            try
            {
                var httpRequest = Request.Form;
                var postedFile = httpRequest.Files[0];
                string filename=postedFile.FileName;
                var physicalPath = _webHostEnvironment.ContentRootPath + "/Photos" + filename;
                using (var stream=new FileStream(physicalPath, FileMode.Create))
                {
                    postedFile.CopyTo(stream);
                }
                return new JsonResult(filename);
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }
        }
    }
}
