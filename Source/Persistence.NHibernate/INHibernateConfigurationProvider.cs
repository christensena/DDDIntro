using NHibernate.Cfg;

namespace DDDIntro.Persistence.NHibernate
{
    public interface INHibernateConfigurationProvider
    {
        Configuration GetDatabaseConfiguration();
    }
}