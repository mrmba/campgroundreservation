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
    public class DepartmentSqlDALtest
    {

        private TransactionScope tran;
        private string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=Project Organizer;Integrated Security = True";
        private string allDepartments = @"SELECT COUNT(*) FROM department";
        private string testDepartments = @"insert into department (name) values ('TESTaa'), ('TESTab'), ('TESTac');";
        private int departmentCount = 0;

        [TestInitialize]
        public void Initialize()
        {
            tran = new TransactionScope();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd;
                connection.Open();

                cmd = new SqlCommand(allDepartments, connection);
                departmentCount = (int)cmd.ExecuteScalar();         //  Counts existing departments

                cmd = new SqlCommand(testDepartments, connection);  //  Adds 3 test departments
                cmd.ExecuteNonQuery();
            }
        }

        [TestCleanup]
        public void Cleanup()
        {
            tran.Dispose();
        }

        [TestMethod]
        public void DepartmentTest()
        {
            DepartmentSqlDAL deptTest01 = new DepartmentSqlDAL(connectionString);
            List<Department> happyList = deptTest01.GetDepartments();

            Department goodDept02 = new Department();
            goodDept02.Id = 1;
            goodDept02.Name = "yeah";
            deptTest01.UpdateDepartment(goodDept02);

            Assert.AreEqual(departmentCount + 3, happyList.Count);
            Assert.AreEqual("yeah", deptTest01.GetDepartments(1).Name);
        }

        [TestMethod]
        [ExpectedException(typeof(SqlException))]
        public void DepartmentFailTest()
        {
            DepartmentSqlDAL deptTest02 = new DepartmentSqlDAL(connectionString);

            Department badDept02 = new Department();
            badDept02.Name = "12345678912345678912345678912345678912345";

            try
            {
                deptTest02.CreateDepartment(badDept02);
            }
            catch (SqlException)
            {
                throw;
            }

        }

    }
}
