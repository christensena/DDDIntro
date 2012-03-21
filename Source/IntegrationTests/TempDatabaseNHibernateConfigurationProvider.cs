using System;
using DDDIntro.Persistence.NHibernate;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;

namespace DDDIntro.IntegrationTests
{
    public static class TempDatabaseNHibernateConfigurationProvider
    {
        public static Configuration GetTempDatabaseConfiguration()
        {
            var databaseDriver = SQLiteConfiguration.Standard.InMemory().ShowSql();
            return NHibernateConfigurationProvider.GetDatabaseConfiguration(databaseDriver);
        }

        public static void InitialiseDatabase(Configuration configuration, ISession session)
        {
            new SchemaExport(configuration).Execute(true, true, false, session.Connection, Console.Out);
        }
    }
}