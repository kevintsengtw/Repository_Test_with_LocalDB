using System;

namespace NorthwindRepository.TestResources.TableSchemas
{
    /// <summary>
    /// Class TableCommands.
    /// </summary>
    public static class TableCommands
    {
        /// <summary>
        /// Drops the table.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <returns>System.String.</returns>
        /// <exception cref="ArgumentNullException">please input tableName.</exception>
        public static string DropTable(string tableName)
        {
            if (string.IsNullOrWhiteSpace(tableName))
            {
                throw new ArgumentNullException(nameof(tableName), "please input tableName.");
            }

            var sqlCommand = new System.Text.StringBuilder();
            sqlCommand.AppendLine($@"IF OBJECT_ID('dbo.{tableName}', 'U') IS NOT NULL");
            sqlCommand.AppendLine($@"  DROP TABLE dbo.{tableName}; ");
            return sqlCommand.ToString();
        }

        /// <summary>
        /// Truncates the table.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <returns>System.String.</returns>
        /// <exception cref="ArgumentNullException">please input tableName.</exception>
        public static string TruncateTable(string tableName)
        {
            if (string.IsNullOrWhiteSpace(tableName))
            {
                throw new ArgumentNullException(nameof(tableName), "please input tableName.");
            }

            var sqlCommand = new System.Text.StringBuilder();
            sqlCommand.AppendLine($@"IF OBJECT_ID('dbo.{tableName}', 'U') IS NOT NULL");
            sqlCommand.AppendLine($@"  TRUNCATE TABLE dbo.{tableName}; ");
            return sqlCommand.ToString();
        }
    }

}