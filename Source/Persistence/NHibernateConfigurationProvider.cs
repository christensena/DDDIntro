using System.IO;
using DDDIntro.Domain;
using DDDIntro.Persistence.MappingConventions;
using DDDIntro.Persistence.MappingOverrides;
using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;

namespace DDDIntro.Persistence
{
    public static class NHibernateConfigurationProvider
    {
        private const string SqlLiteDbFilename = "tmpdb.db";

        public static Configuration GetTempDatabaseConfiguration()
        {
            return Fluently.Configure()
                .Database(SQLiteConfiguration.Standard.UsingFile(SqlLiteDbFilename).ShowSql())
                //.Database(SQLiteConfiguration.Standard.InMemory())
                .Mappings(m => m.AutoMappings.Add(
                    AutoMap.AssemblyOf<Player>(new DefaultMappingConfiguration())
                    .Conventions.AddFromAssemblyOf<IdGenerationConvention>()
                     .UseOverridesFromAssemblyOf<OverMappingOverride>()))
                .ExposeConfiguration(BuildDatabase)
                .BuildConfiguration();
        }

        private static void BuildDatabase(Configuration configuration)
        {
            // TODO: problem is that the in-memory database is recreated each session
            // so just going to use a temporary file system one instead
            // see http://www.tigraine.at/2009/05/29/fluent-nhibernate-gotchas-when-testing-with-an-in-memory-database/
            if (File.Exists(SqlLiteDbFilename))
                File.Delete(SqlLiteDbFilename);
            new SchemaExport(configuration).Create(script => System.Diagnostics.Debug.WriteLine(script), true);
        }
    }
}