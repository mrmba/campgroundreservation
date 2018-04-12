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
    public class EmployeeSqlDALtest
    {
        private TransactionScope tran;
        private string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=Project Organizer;Integrated Security = True";
        private string countEmp = @"select count(*) from employee;";
        //private string insertEmp = @"insert into employee"
        private int headCount = 0;
        private string firstN;
        private string lastN;

        private int projless = 0;

        [TestInitialize]
        public void Initialize()
        {
            tran = new TransactionScope();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd;
                connection.Open();
                cmd = new SqlCommand(countEmp, connection);
                headCount = (int)cmd.ExecuteScalar();         //  Counts existing employees

                cmd = new SqlCommand(@"select first_name from employee where employee_id = 1;", connection);
                firstN = (string)cmd.ExecuteScalar();
                cmd = new SqlCommand(@"select last_name from employee where employee_id = 1;", connection);
                lastN = (string)cmd.ExecuteScalar();

                cmd = new SqlCommand(@"select count(*) from employee where employee_id not in (select employee_id from project_employee);", connection);
                projless = (int)cmd.ExecuteScalar();
            }
        }

        [TestCleanup]
        public void Cleanup()
        {
            tran.Dispose();
        }

        [TestMethod]
        public void EmployeeTest()
        {
            EmployeeSqlDAL esdTest = new EmployeeSqlDAL(connectionString);
            List<Employee> happyEmployees = esdTest.GetAllEmployees();

            Assert.AreEqual(headCount, 12);

            Assert.AreEqual(1, esdTest.Search(firstN, lastN).Count);    //  EMployee ID = 1, only employee with that first and last anme

            Assert.AreEqual(projless, esdTest.GetEmployeesWithoutProjects().Count);
        }
    }
}
