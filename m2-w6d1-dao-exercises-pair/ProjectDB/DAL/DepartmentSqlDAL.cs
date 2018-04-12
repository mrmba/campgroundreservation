using ProjectDB.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectDB.DAL
{
    public class DepartmentSqlDAL
    {
        private string SQL_GetDepartments = "select * from department;";
        private string SQL_GetDepartmentsOL = "select * from department where department_id = @index;";
        private string SQL_CreateDepartment = "Insert Into department (name) values (@departmentname);";
        private string SQL_UpdateDepartment = "update department set name = @departmentname where department_id = @departmentid;";
        private string connectionString;

        // Single Parameter Constructor
        public DepartmentSqlDAL(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        public List<Department> GetDepartments()
        {
            List<Department> output = new List<Department>();

            try
            {
                using(SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(SQL_GetDepartments, conn);

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Department dept = GetDepartmentFromReader(reader);
                        output.Add(dept);
                    }
                }
            }
            catch(SqlException)
            {
                throw;
            }

            return output;
        }       // All departments

        public Department GetDepartments(int index)
        {
            Department output = new Department();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(SQL_GetDepartments, conn);
                    cmd.Parameters.AddWithValue("@index", index);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Department dept = GetDepartmentFromReader(reader);
                        output = dept;
                    }
                }
            }
            catch (SqlException)
            {
                throw;
            }

            return output;
        }   //  Department at specified index

        public bool CreateDepartment(Department newDepartment)
        {
            bool success = false;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(SQL_CreateDepartment, conn);
                    cmd.Parameters.AddWithValue("@departmentname", newDepartment.Name);

                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.RecordsAffected > 0)
                    {
                        success = true;
                    }
                }
            }
            catch (SqlException)
            {
                throw;
            }
            return success;
        }

        public bool UpdateDepartment(Department updatedDepartment)
        {
            bool success = false;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(SQL_UpdateDepartment, conn);
                    cmd.Parameters.AddWithValue("@departmentname", updatedDepartment.Name);
                    cmd.Parameters.AddWithValue("@departmentid", updatedDepartment.Id);

                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.RecordsAffected > 0)
                    {
                        success = true;
                    }
                }
            }
            catch(SqlException)
            {
                throw;
            }
            return success;
        }

        private Department GetDepartmentFromReader(SqlDataReader reader)
        {
            Department department = new Department();
            department.Id = Convert.ToInt32(reader["department_id"]);
            department.Name = Convert.ToString(reader["name"]);
            
            return department;
        }
    }
}
