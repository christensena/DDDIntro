using System.Configuration;
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
        public static Configuration GetDatabaseConfiguration()
        {
            return GetDatabaseConfiguration(MsSqlConfiguration.MsSql2008.ConnectionString(
                ConfigurationManager.ConnectionStrings["Default"].ConnectionString), true);
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
            new SchemaExport(configuration).Create(script => System.Diagnostics.Debug.WriteLine(script), true);
        }
    }
}