using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using NorthwindRepository.Database;
using NorthwindRepository.Interface;
using NorthwindRepository.Models;

namespace NorthwindRepository.Implements
{
    /// <summary>
    /// CustomerRepository
    /// </summary>
    public class CustomerRepository : ICustomerRepository
    {
        private IDatabaseConnectionFactory DatabaseConnectionFactory { get; }

        public CustomerRepository(IDatabaseConnectionFactory factory)
        {
            this.DatabaseConnectionFactory = factory;
        }

        /// <summary>
        /// 取得全部資料
        /// </summary>
        /// <returns></returns>
        public List<CustomerModel> GetAll()
        {
            var dbConnection = this.DatabaseConnectionFactory.Create();
            using (var conn = dbConnection)
            {
                var sqlCommand = "select * from dbo.Customers";
                var result = conn.Query<CustomerModel>(sqlCommand);
                return result.ToList();
            }
        }

        /// <summary>
        /// 以 CustomerID 取得指定資料
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public CustomerModel Get(string customerId)
        {
            if (string.IsNullOrWhiteSpace(customerId))
            {
                throw new ArgumentNullException(nameof(customerId));
            }

            var dbConnection = this.DatabaseConnectionFactory.Create();

            using (var conn = dbConnection)
            {
                var sqlCommand = "select * from dbo.Customers where CustomerID = @CustomerID";

                var result = conn.QueryFirstOrDefault<CustomerModel>(
                    sqlCommand,
                    new
                    {
                        CustomerID = customerId
                    });

                return result;
            }
        }
    }
}