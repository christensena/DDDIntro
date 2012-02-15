using System.Configuration;
using System.IO;
using DDDIntro.Domain;
using DDDIntro.Persistence.NHibernate.MappingConventions;
using DDDIntro.Persistence.NHibernate.MappingOverrides;
using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate.Tool.hbm2ddl;
using Configuration = NHibernate.Cfg.Configuration;

namespace DDDIntro.Persistence.NHibernate
{
    public static class NHibernateConfigurationProvider
    {
        private const string SqlLiteDbFilename = "tmpdb.db";

        public static Configuration GetDatabaseConfiguration()
        {
            return GetDatabaseConfiguration(MsSqlConfiguration.MsSql2008.ConnectionString(
                ConfigurationManager.ConnectionStrings["Default"].ConnectionString), true);
        }

        public static Configuration GetTempDatabaseConfiguration()
        {
            // TODO: problem is that the in-memory database is recreated each session
            // so just going to use a temporary file system one instead
            // see http://www.tigraine.at/2009/05/29/fluent-nhibernate-gotchas-when-testing-with-an-in-memory-database/
            if (File.Exists(SqlLiteDbFilename))
                File.Delete(SqlLiteDbFilename);

            var databaseDriver = SQLiteConfiguration.Standard.UsingFile(SqlLiteDbFilename).ShowSql();
            return GetDatabaseConfiguration(databaseDriver, true);
        }

        public static Configuration GetDatabaseConfiguration(IPersistenceConfigurer databaseDriver, bool buildDatabase)
        {
            var fluentConfiguration =  
                Fluently.Configure()
                .Database(databaseDriver)
                //.Database(SQLiteConfiguration.Standard.InMemory())
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
            new SchemaExport(configuration).Create(script => System.Diagnostics.Debug.WriteLine(script), true);
        }
    }
}