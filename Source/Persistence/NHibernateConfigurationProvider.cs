using System;
using DDDIntro.Domain;
using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;

namespace Persistence
{
    public static class NHibernateConfigurationProvider
    {
        public static Configuration GetSqLiteInMemoryDatabaseConfiguration()
        {
            return Fluently.Configure()
                .Database(SQLiteConfiguration.Standard.InMemory)
                .Mappings(m => m.AutoMappings.Add(AutoMap.AssemblyOf<Supplier>))
                .ExposeConfiguration(BuildDatabase)
                .BuildConfiguration();
        }

        private static void BuildDatabase(Configuration configuration)
        {
            new SchemaExport(configuration).Create(script => System.Diagnostics.Debug.WriteLine(script), true);
        }
    }
}