using System;
using System.Configuration;
using System.Data.Entity;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using System.Text;

namespace NorthwindRepository.TestResources.DB
{
    public class TestDatabase
    {
        private readonly string _testConnectionString = 
            string.Concat(
                TestDbConnection.LocalDb.LocalDbConnectionString,
                ";AttachDBFilename={0}.mdf");

        private string DatabaseName { get; set; }

        private string ConnectionString { get; set; }

        public TestDatabase(string databaseName)
        {
            this.DatabaseName = databaseName;
            this.ConnectionString = string.Format(_testConnectionString, databaseName);
        }

        //-----------------------------------------------------------------------------------------

        /// <summary>
        /// 使用 EntityFramework 的 Database 類別 Exists 方法，確認 LocalDB 是否存在.
        /// </summary>
        /// <returns><c>true</c> if [is local database exists] [the specified connection string]; otherwise, <c>false</c>.</returns>
        public bool IsLocalDbExists()
        {
            using (var connection = new SqlConnection(this.ConnectionString))
            {
                return Database.Exists(connection);
            }
        }

        /// <summary>
        /// 使用 EntityFramework 的 Database 類別 Delete 方法，確認 LocalDB 存在後再移除.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public void DeleteLocalDb(string connectionString = "")
        {
            string currentConnectionString = this.ConnectionString;

            if (!string.IsNullOrWhiteSpace(connectionString))
            {
                currentConnectionString = connectionString;
            }

            if (!currentConnectionString.ToLower().Contains("localdb"))
            {
                return;
            }

            using (var connection = new SqlConnection(currentConnectionString))
            {
                if (Database.Exists(connection))
                {
                    Database.Delete(connection);
                }
            }
        }

        //-----------------------------------------------------------------------------------------

        /// <summary>
        /// Creates the database.
        /// </summary>
        public void CreateDatabase()
        {
            this.DetachDatabase();

            var fileName = this.CleanupDatabase();

            using (var connection = new SqlConnection(TestDbConnection.LocalDb.Default))
            {
                var commandText = new StringBuilder();
                commandText.AppendFormat(
                    "CREATE DATABASE {0} ON (NAME = N'{0}', FILENAME = '{1}.mdf');",
                    this.DatabaseName,
                    fileName);

                connection.Open();
                var cmd = connection.CreateCommand();
                cmd.CommandText = commandText.ToString();
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Initializes the connection string.
        /// </summary>
        /// <param name="connectionStringName">Name of the connection string.</param>
        public void InitConnectionString(string connectionStringName)
        {
            var connectionString = string.Format(
                _testConnectionString,
                this.DatabaseName,
                this.DatabaseFilePath());

            var config = ConfigurationManager.OpenExeConfiguration(
                Assembly.GetCallingAssembly().Location);

            var settings = config.ConnectionStrings.ConnectionStrings[connectionStringName];
            if (settings == null)
            {
                settings = new ConnectionStringSettings(
                    connectionStringName,
                    connectionString,
                    "System.Data.SqlClient");

                config.ConnectionStrings.ConnectionStrings.Add(settings);
            }
            settings.ConnectionString = connectionString;
            config.Save();
            ConfigurationManager.RefreshSection("connectionStrings");
        }

        /// <summary>
        /// Cleanups the database.
        /// </summary>
        /// <returns>System.String.</returns>
        private string CleanupDatabase()
        {
            var fileName = this.DatabaseFilePath();
            try
            {
                var mdfPath = string.Concat(fileName, ".mdf");
                var ldfPath = string.Concat(fileName, "_log.ldf");

                var mdfExists = File.Exists(mdfPath);
                var ldfExists = File.Exists(ldfPath);

                if (mdfExists) File.Delete(mdfPath);
                if (ldfExists) File.Delete(ldfPath);
            }
            catch
            {
                Console.WriteLine("Could not delete the files (open in Visual Studio?)");
            }
            return fileName;
        }

        /// <summary>
        /// Detaches the database.
        /// </summary>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private void DetachDatabase()
        {
            using (var connection = new SqlConnection(TestDbConnection.LocalDb.Default))
            {
                connection.Open();
                var cmd = connection.CreateCommand();
                cmd.CommandText = $"exec sp_detach_db '{this.DatabaseName}'";
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch
                {
                    Console.WriteLine("Could not detach");
                }
            }
        }

        /// <summary>
        /// Databases the file path.
        /// </summary>
        /// <returns>System.String.</returns>
        private string DatabaseFilePath()
        {
            return Path.Combine(
                Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                this.DatabaseName);
        }

    }
}