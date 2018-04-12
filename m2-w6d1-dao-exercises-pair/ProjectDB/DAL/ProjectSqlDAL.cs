using ProjectDB.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectDB.DAL
{
    public class ProjectSqlDAL
    {
        private string SQL_GetAllProjects = "select * from project;";
        private string SQL_CreateProject = "insert into project (name, from_date, to_date) values (@projectname, @startdate, @enddate);";

        private string SQL_AssignEmployeeToProject = "insert into project_employee (project_id, employee_id) values (@projectid, @employeeid)";
        private string SQL_RemoveEmployeeFromProject = "delete from project_employee where (project_id = @projectid) and (employee_id = @employeeid);";

        private string connectionString;

        // Single Parameter Constructor
        public ProjectSqlDAL(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        public List<Project> GetAllProjects()
        {
            List<Project> output = new List<Project>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(SQL_GetAllProjects, conn);

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Project proj = GetProjectFromReader(reader);
                        output.Add(proj);
                    }
                }
            }
            catch (SqlException)
            {
                throw;
            }
            return output;
        }

        public bool AssignEmployeeToProject(int projectId, int employeeId)
        {
            bool success = false;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(SQL_AssignEmployeeToProject, conn);
                    cmd.Parameters.AddWithValue("@projectid", projectId);
                    cmd.Parameters.AddWithValue("@employeeid", employeeId);

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

        public bool RemoveEmployeeFromProject(int projectId, int employeeId)
        {
            bool success = false;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(SQL_RemoveEmployeeFromProject, conn);
                    cmd.Parameters.AddWithValue("@projectid", projectId);
                    cmd.Parameters.AddWithValue("@employeeid", employeeId);

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

        public bool CreateProject(Project newProject)
        {
            bool success = false;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(SQL_CreateProject, conn);
                    cmd.Parameters.AddWithValue("@projectname", newProject.Name);
                    cmd.Parameters.AddWithValue("@startdate", newProject.StartDate);
                    cmd.Parameters.AddWithValue("@enddate", newProject.EndDate);

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

        private Project GetProjectFromReader(SqlDataReader reader)
        {
            Project proj = new Project();
            proj.ProjectId = Convert.ToInt32(reader["project_id"]);
            proj.Name = Convert.ToString(reader["name"]);
            proj.StartDate = Convert.ToDateTime(reader["from_date"]);
            proj.EndDate = Convert.ToDateTime(reader["to_date"]);

            return proj;
        }
    }
}
