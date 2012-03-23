using System.Configuration;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using FluentNHibernate.Cfg.Db;
using NHibernate.Tool.hbm2ddl;
using Configuration = NHibernate.Cfg.Configuration;

namespace DDDIntro.Persistence.NHibernate.NHibernateConfigurationProviders
{
    public class SqlServerNHibernateConfigurationProvider : NHibernateConfigurationProvider
    {
        private static readonly string SqlConnectionString = ConfigurationManager.ConnectionStrings["Default"].ConnectionString;

        public override Configuration GetDatabaseConfiguration()
        {
            return CreateCoreDatabaseConfiguration(
                MsSqlConfiguration.MsSql2008.ConnectionString(SqlConnectionString), 
                BuildDatabase);
        }

        private static void BuildDatabase(Configuration configuration)
        {
            using (var connection = new SqlConnection(SqlConnectionString))
            {
                try
                {
                    connection.Open();
                }
                catch (SqlException)
                {
                    CreateDatabase();
                    ExportSchemaToDatabase(configuration);
                }
            }

        }

        private static void CreateDatabase()
        {
            var masterConnectionString = Regex.Replace(SqlConnectionString, "(Database|Initial Catalog)=[^;]+", "Database=master", RegexOptions.IgnoreCase);
            using (var connection = new SqlConnection(masterConnectionString))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = "CREATE DATABASE " + GetDatabaseName();
                command.ExecuteNonQuery();
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