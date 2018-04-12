using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Transactions;
using ProjectDB.DAL;
using ProjectDB.Models;

namespace ProjectDBTests
{
    [TestClass]
    public class ProjectSqlDALtest
    {
        private TransactionScope tran;
        private string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=Project Organizer;Integrated Security = True";
        private int projectCount = 0;
        private string countProjects = @"select count(*) from project;";

        [TestInitialize]
        public void Initialize()
        {
            tran = new TransactionScope();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd;
                connection.Open();

                cmd = new SqlCommand(countProjects, connection);
                projectCount = (int)cmd.ExecuteScalar();

            }
        }

        [TestCleanup]
        public void Cleanup()
        {
            tran.Dispose();
        }

        [TestMethod]
        public void ProjectsTest()
        {
            Project thisProj = new Project();

            thisProj.Name = "TESTprojTEST";
            thisProj.StartDate = DateTime.Now;
            thisProj.EndDate = DateTime.Now;

            ProjectSqlDAL psdTest = new ProjectSqlDAL(connectionString);
            Assert.AreEqual(true, psdTest.CreateProject(thisProj));
            Assert.AreEqual(projectCount + 1, psdTest.GetAllProjects().Count);

            Assert.AreEqual(true, psdTest.AssignEmployeeToProject(projectCount, 1));
            Assert.AreEqual(true, psdTest.RemoveEmployeeFromProject(projectCount, 1));
            Assert.AreNotEqual(true, psdTest.RemoveEmployeeFromProject(projectCount + 1, 1));

        }

    }
}
