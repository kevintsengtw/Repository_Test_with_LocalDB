using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NorthwindRepository.TestResources.DB;

namespace NorthwindRepositoryTests
{
    [TestClass]
    public class TestHook
    {
        [AssemblyInitialize]
        public static void AssemblyInitialize(TestContext context)
        {
            // Northwind
            var northwindDatabase = new TestDatabase(DatabaseName.Northwind);
            if (northwindDatabase.IsLocalDbExists())
            {
                northwindDatabase.DeleteLocalDb();
            }
            northwindDatabase.CreateDatabase();
        }

        [AssemblyCleanup]
        public static void AssemblyCleanup()
        {
            var defaultDatabase = new TestDatabase(DatabaseName.Default);
            defaultDatabase.DeleteLocalDb(TestDbConnection.LocalDb.Northwind);
        }
    }
}