using System;
using DDDIntro.Domain;
using DDDIntro.Persistence.NHibernate.MappingConventions;
using DDDIntro.Persistence.NHibernate.MappingOverrides;
using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using Configuration = NHibernate.Cfg.Configuration;

namespace DDDIntro.Persistence.NHibernate
{
    public static class NHibernateConfigurationProvider
    {
        public static Configuration GetDatabaseConfiguration(IPersistenceConfigurer databaseDriver, Action<Configuration> databaseBuilder = null)
        {
            var fluentConfiguration =  
                Fluently.Configure()
                .Database(databaseDriver)
                .Mappings(m => m.AutoMappings.Add(
                    AutoMap.AssemblyOf<Player>(new DefaultMappingConfiguration())
                    .Conventions.AddFromAssemblyOf<IdGenerationConvention>()
                     .UseOverridesFromAssemblyOf<OverMappingOverride>()));

            if (databaseBuilder != null)
            {
                fluentConfiguration.ExposeConfiguration(databaseBuilder);
            }

            return fluentConfiguration.BuildConfiguration();
        }
    }


}