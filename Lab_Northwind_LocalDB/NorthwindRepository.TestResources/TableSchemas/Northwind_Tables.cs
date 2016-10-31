// ReSharper disable InconsistentNaming

namespace NorthwindRepository.TestResources.TableSchemas
{
    public class Northwind_Tables
    {
        public static string Customers_Create()
        {
            var sqlCommand = new System.Text.StringBuilder();
            sqlCommand.AppendLine(@"IF OBJECT_ID('dbo.Customers', 'U') IS NOT NULL");
            sqlCommand.AppendLine(@"  DROP TABLE dbo.Customers; ");
            sqlCommand.AppendLine(@"");
            sqlCommand.AppendLine(@"CREATE TABLE [dbo].[Customers](");
            sqlCommand.AppendLine(@"	[CustomerID] [nchar](5) NOT NULL,");
            sqlCommand.AppendLine(@"	[CompanyName] [nvarchar](40) NOT NULL,");
            sqlCommand.AppendLine(@"	[ContactName] [nvarchar](30) NULL,");
            sqlCommand.AppendLine(@"	[ContactTitle] [nvarchar](30) NULL,");
            sqlCommand.AppendLine(@"	[Address] [nvarchar](60) NULL,");
            sqlCommand.AppendLine(@"	[City] [nvarchar](15) NULL,");
            sqlCommand.AppendLine(@"	[Region] [nvarchar](15) NULL,");
            sqlCommand.AppendLine(@"	[PostalCode] [nvarchar](10) NULL,");
            sqlCommand.AppendLine(@"	[Country] [nvarchar](15) NULL,");
            sqlCommand.AppendLine(@"	[Phone] [nvarchar](24) NULL,");
            sqlCommand.AppendLine(@"	[Fax] [nvarchar](24) NULL,");
            sqlCommand.AppendLine(@" CONSTRAINT [PK_Customers] PRIMARY KEY CLUSTERED ");
            sqlCommand.AppendLine(@"(");
            sqlCommand.AppendLine(@"	[CustomerID] ASC");
            sqlCommand.AppendLine(@"));");

            return sqlCommand.ToString();
        }

        public static string Customers_Insert()
        {
            var sqlCommand = new System.Text.StringBuilder(371);
            sqlCommand.AppendLine(@"INSERT INTO [dbo].[Customers]");
            sqlCommand.AppendLine(@"(");
            sqlCommand.AppendLine(@"  [CustomerID],");
            sqlCommand.AppendLine(@"  [CompanyName],");
            sqlCommand.AppendLine(@"  [ContactName],");
            sqlCommand.AppendLine(@"  [ContactTitle],");
            sqlCommand.AppendLine(@"  [Address],");
            sqlCommand.AppendLine(@"  [City],");
            sqlCommand.AppendLine(@"  [Region],");
            sqlCommand.AppendLine(@"  [PostalCode],");
            sqlCommand.AppendLine(@"  [Country],");
            sqlCommand.AppendLine(@"  [Phone],");
            sqlCommand.AppendLine(@"  [Fax]");
            sqlCommand.AppendLine(@")");
            sqlCommand.AppendLine(@"VALUES");
            sqlCommand.AppendLine(@"(");
            sqlCommand.AppendLine(@"  @CustomerID,");
            sqlCommand.AppendLine(@"  @CompanyName,");
            sqlCommand.AppendLine(@"  @ContactName,");
            sqlCommand.AppendLine(@"  @ContactTitle,");
            sqlCommand.AppendLine(@"  @Address,");
            sqlCommand.AppendLine(@"  @City,");
            sqlCommand.AppendLine(@"  @Region,");
            sqlCommand.AppendLine(@"  @PostalCode,");
            sqlCommand.AppendLine(@"  @Country,");
            sqlCommand.AppendLine(@"  @Phone,");
            sqlCommand.AppendLine(@"  @Fax");
            sqlCommand.AppendLine(@");");

            return sqlCommand.ToString();
        }
    }
}