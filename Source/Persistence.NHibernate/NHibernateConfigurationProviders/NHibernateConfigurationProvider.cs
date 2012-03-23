using System;
using DDDIntro.Domain;
using DDDIntro.Persistence.NHibernate.MappingConventions;
using DDDIntro.Persistence.NHibernate.MappingOverrides;
using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate.Cfg;

namespace DDDIntro.Persistence.NHibernate.NHibernateConfigurationProviders
{
    public abstract class NHibernateConfigurationProvider : INHibernateConfigurationProvider
    {
        public abstract Configuration GetDatabaseConfiguration();

        public Configuration CreateCoreDatabaseConfiguration(
            IPersistenceConfigurer databaseDriver, 
            Action<Configuration> databaseBuilder = null)
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