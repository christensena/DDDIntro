using System;
using DDDIntro.Domain;
using FluentNHibernate.Automapping;

namespace Persistence.MappingConventions
{
    public class DefaultMappingConfiguration : DefaultAutomappingConfiguration
    {
        public override bool ShouldMap(Type type)
        {
            return type.Namespace == typeof(Supplier).Namespace;
        }

        public override bool IsComponent(Type type)
        {
            return type == typeof (Address);
        }
    }
}