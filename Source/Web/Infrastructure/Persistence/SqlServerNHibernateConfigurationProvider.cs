using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using DDDIntro.Persistence.NHibernate;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using Configuration = NHibernate.Cfg.Configuration;

namespace DDDIntro.Web.Infrastructure.Persistence
{
    public class SqlServerNHibernateConfigurationProvider
    {
        private static readonly string SqlConnectionString = ConfigurationManager.ConnectionStrings["Default"].ConnectionString;

        public static Configuration GetDatabaseConfiguration()
        {
            return NHibernateConfigurationProvider.GetDatabaseConfiguration(MsSqlConfiguration.MsSql2008.ConnectionString(SqlConnectionString), BuildDatabase);
        }

        private static void BuildDatabase(Configuration configuration)
        {
            // run ddl scripts on the database to create our test schema
            try
            {
                ExportSchemaToDatabase(configuration);
            }
            catch (HibernateException exception)
            {
                // if we got an error, maybe we need to create the database
                // (brittle way of determining cause of error but enough for this demo)
                if (!exception.Message.Contains("Cannot open database"))
                    throw;

                // create the database
                var masterConnectionString = Regex.Replace(SqlConnectionString, "(Database|Initial Catalog)=[^;]+", "Database=master", RegexOptions.IgnoreCase);
                using (var connection = new SqlConnection(masterConnectionString))
                {
                    connection.Open();

                    var command = connection.CreateCommand();
                    command.CommandText = "CREATE DATABASE " + GetDatabaseName();
                    command.ExecuteNonQuery();
                }

                // now we can try again to export it
                ExportSchemaToDatabase(configuration);
            }

        }

        private static void ExportSchemaToDatabase(Configuration configuration)
        {
            new SchemaExport(configuration).Create(script => System.Diagnostics.Debug.WriteLine(script), true);
        }

        private static string GetDatabaseName()
        {
            using (var connection = new SqlConnection(SqlConnectionString))
            {
                return connection.Database;
            }
        }
    }
}