using ProjectDB.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectDB.DAL
{
    public class EmployeeSqlDAL
    {
        private const string SQL_GetAllEmployees = "select employee_id, department_id, first_name, last_name, job_title, birth_date, gender, hire_date from employee;";
        private const string SQL_Search = "select employee_id, department_id, first_name, last_name, job_title, birth_date, gender, hire_date from employee where (first_name=@firstname) and (last_name=@lastname);";
        private const string SQL_GetEmployeesWithoutProjects = "select employee_id, department_id, first_name, last_name, job_title, birth_date, gender, hire_date from employee where employee_id not in(select employee_id from project_employee);";
        private string connectionString;  

        // Single Parameter Constructor
        public EmployeeSqlDAL(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        public List<Employee> GetAllEmployees()
        {
            List<Employee> output = new List<Employee>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(SQL_GetAllEmployees, conn);

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Employee emp = GetEmployeeFromReader(reader);
                        output.Add(emp);
                    }
                }
            }
            catch (SqlException)
            {
                throw;
            }
            return output;
        }

        public List<Employee> Search(string firstname, string lastname)
        {
            List<Employee> output = new List<Employee>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(SQL_Search, conn);
                    cmd.Parameters.AddWithValue("@firstname", firstname);
                    cmd.Parameters.AddWithValue("@lastname", lastname);

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Employee emp = GetEmployeeFromReader(reader);
                        output.Add(emp);
                    }

                }
            }
            catch (SqlException)
            {
                throw;
            }
            return output;
        }

        public List<Employee> GetEmployeesWithoutProjects()
        {
            List<Employee> output = new List<Employee>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(SQL_GetEmployeesWithoutProjects, conn);
  
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Employee emp = GetEmployeeFromReader(reader);
                        output.Add(emp);
                    }

                }
            }
            catch (SqlException)
            {
                throw;
            }
            return output;
        }

        private Employee GetEmployeeFromReader(SqlDataReader reader)
        {
            Employee employee = new Employee();
            employee.EmployeeId = Convert.ToInt32(reader["employee_id"]);
            employee.DepartmentId = Convert.ToInt32(reader["department_id"]);
            employee.JobTitle = Convert.ToString(reader["job_title"]);
            employee.FirstName = Convert.ToString(reader["first_name"]);
            employee.LastName = Convert.ToString(reader["last_name"]);
            employee.BirthDate = Convert.ToDateTime(reader["birth_date"]);
            employee.Gender = Convert.ToString(reader["gender"]);
            employee.HireDate = Convert.ToDateTime(reader["hire_date"]);

            return employee;
        }
    }
}
