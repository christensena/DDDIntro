using System.IO;
using DDDIntro.Persistence.NHibernate;
using FluentNHibernate.Cfg.Db;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;

namespace DDDIntro.IntegrationTests
{
    public static class TempDatabaseNHibernateConfigurationProvider
    {
        private const string SqlLiteDbFilename = "tmpdb.db";

        public static Configuration GetTempDatabaseConfiguration()
        {
            // TODO: problem is that the in-memory database is recreated each session
            // so just going to use a temporary file system one instead
            // see http://www.tigraine.at/2009/05/29/fluent-nhibernate-gotchas-when-testing-with-an-in-memory-database/
            if (File.Exists(SqlLiteDbFilename))
                File.Delete(SqlLiteDbFilename);

            //SQLiteConfiguration.Standard.InMemory()
            var databaseDriver = SQLiteConfiguration.Standard.UsingFile(SqlLiteDbFilename).ShowSql();
            return NHibernateConfigurationProvider.GetDatabaseConfiguration(databaseDriver, ExportSchemaToDatabase);
        }

        private static void ExportSchemaToDatabase(Configuration configuration)
        {
            new SchemaExport(configuration).Create(script => System.Diagnostics.Debug.WriteLine(script), true);
        }
    }
}