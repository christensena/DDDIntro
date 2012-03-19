﻿using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using DDDIntro.Domain;
using DDDIntro.Persistence.NHibernate.MappingConventions;
using DDDIntro.Persistence.NHibernate.MappingOverrides;
using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using Configuration = NHibernate.Cfg.Configuration;

namespace DDDIntro.Persistence.NHibernate
{
    public static class NHibernateConfigurationProvider
    {
        private static readonly string SqlConnectionString = ConfigurationManager.ConnectionStrings["Default"].ConnectionString;

        public static Configuration GetDatabaseConfiguration()
        {
            return GetDatabaseConfiguration(MsSqlConfiguration.MsSql2008.ConnectionString(SqlConnectionString), true);
        }

        public static Configuration GetDatabaseConfiguration(IPersistenceConfigurer databaseDriver, bool buildDatabase)
        {
            var fluentConfiguration =  
                Fluently.Configure()
                .Database(databaseDriver)
                .Mappings(m => m.AutoMappings.Add(
                    AutoMap.AssemblyOf<Player>(new DefaultMappingConfiguration())
                    .Conventions.AddFromAssemblyOf<IdGenerationConvention>()
                     .UseOverridesFromAssemblyOf<OverMappingOverride>()));

            if (buildDatabase)
            {
                fluentConfiguration.ExposeConfiguration(BuildDatabase);
            }

            return fluentConfiguration.BuildConfiguration();
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

                try
                {
                    // create the database
                    var masterConnectionString = Regex.Replace(SqlConnectionString, "(Database|Initial Catalog)=[^;]+", "Database=master", RegexOptions.IgnoreCase);
                    using (var connection = new SqlConnection(masterConnectionString))
                    {
                        connection.Open();

                        var command = connection.CreateCommand();
                        command.CommandText = "CREATE DATABASE "+GetDatabaseName();
                        command.ExecuteNonQuery();
                    }

                    // now we can try again to export it
                    ExportSchemaToDatabase(configuration);
                }
                catch (Exception)
                {
                    throw;
                }
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