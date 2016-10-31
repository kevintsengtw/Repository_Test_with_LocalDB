using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using CsvHelper;
using Dapper;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NorthwindRepository.Database;
using NorthwindRepository.Implements;
using NorthwindRepository.Models;
using NorthwindRepository.TestResources.DB;
using NorthwindRepository.TestResources.SourceClass;
using NorthwindRepository.TestResources.TableSchemas;
using NSubstitute;
using ExpectedObjects;

namespace NorthwindRepositoryTests.Implements
{
    [TestClass()]
    [DeploymentItem(@"SourceData\Customer_Data.csv")]
    public class CustomerRepositoryTests
    {
        private IDatabaseConnectionFactory DatabaseConnectionFactory { get; set; }

        [TestInitialize]
        public void TestInitialize()
        {
            this.DatabaseConnectionFactory = Substitute.For<IDatabaseConnectionFactory>();

            this.DatabaseConnectionFactory.Create()
                .Returns(new SqlConnection(TestDbConnection.LocalDb.Northwind));
        }

        private CustomerRepository GetSystemUnderTest()
        {
            var sut = new CustomerRepository(this.DatabaseConnectionFactory);
            return sut;
        }

        #region -- Prepare to Test --

        public TestContext TestContext { get; set; }

        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            CreateTable();
            PrepareData();
        }

        private static void CreateTable()
        {
            using (var conn = new SqlConnection(TestDbConnection.LocalDb.Northwind))
            {
                conn.Open();
                string sqlCommand = Northwind_Tables.Customers_Create();
                conn.Execute(sqlCommand);
            }
        }

        private static void PrepareData()
        {
            List<Customer_Data> sourceData = new List<Customer_Data>();
            using (var sr = new StreamReader(@"Customer_Data.csv"))
            {
                using (var reader = new CsvReader(sr))
                {
                    var records = reader.GetRecords<Customer_Data>();
                    sourceData.AddRange(records);
                }
            }

            using (var conn = new SqlConnection(TestDbConnection.LocalDb.Northwind))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    var sqlCommand = Northwind_Tables.Customers_Insert();
                    conn.Execute(sqlCommand, sourceData, transaction: trans);
                    trans.Commit();
                }
            }
        }

        [ClassCleanup()]
        public static void TestClassCleanup()
        {
            using (var conn = new SqlConnection(TestDbConnection.LocalDb.Northwind))
            {
                conn.Open();
                string sqlCommand = TableCommands.DropTable(TableNames.Northwind.Customers);
                conn.Execute(sqlCommand);
            }
        }

        #endregion

        //-----------------------------------------------------------------------------------------

        [TestMethod()]
        public void GetAll_取得全部的Customer資料()
        {
            // arrange
            var sut = this.GetSystemUnderTest();

            // act
            var actual = sut.GetAll();

            // assert
            actual.Should().NotBeNull();
            actual.Count.Should().Be(91);
        }

        [TestMethod]
        public void Get_CustomerId輸入ALFKI_應回傳符合的資料()
        {
            // arrange
            var customerId = "ALFKI";

            CustomerModel expected = new CustomerModel
            {
                CustomerID = "ALFKI",
                CompanyName = "Alfreds Futterkiste",
                ContactName = "Maria Anders",
                ContactTitle = "Sales Representative",
                Address = "Obere Str. 57",
                City = "Berlin",
                Region = "",
                PostalCode = "12209",
                Country = "Germany",
                Phone = "030-0074321",
                Fax = "030-0076545"
            };

            var sut = this.GetSystemUnderTest();

            // assert
            var actual = sut.Get(customerId);

            // act
            actual.Should().NotBeNull();
            expected.ToExpectedObject().ShouldEqual(actual);
        }
    }
}