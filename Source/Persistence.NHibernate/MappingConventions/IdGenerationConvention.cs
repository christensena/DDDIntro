using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;

namespace DDDIntro.Persistence.NHibernate.MappingConventions
{
    public class IdGenerationConvention : IIdConvention
    {
        public void Apply(IIdentityInstance instance)
        {
            // using HiLo allows NHibernate to provide ID's for objects without
            // persisting them first. see http://stackoverflow.com/questions/282099/whats-the-hi-lo-algorithm
            instance.GeneratedBy.HiLo("HiLo", "NextHighValue", "1000");
        }
    }
}