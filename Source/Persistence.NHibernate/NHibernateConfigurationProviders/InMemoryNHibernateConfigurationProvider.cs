using System;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;

namespace DDDIntro.Persistence.NHibernate.NHibernateConfigurationProviders
{
    public class InMemoryNHibernateConfigurationProvider : NHibernateConfigurationProvider
    {
        public override Configuration GetDatabaseConfiguration()
        {
            var databaseDriver = SQLiteConfiguration.Standard.InMemory().ShowSql();
            return CreateCoreDatabaseConfiguration(databaseDriver);
        }

        public static void InitialiseDatabase(Configuration configuration, ISession session)
        {
            new SchemaExport(configuration).Execute(true, true, false, session.Connection, Console.Out);
        }
    }
}