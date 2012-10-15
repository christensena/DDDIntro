using System;
using System.Runtime.CompilerServices;
using DDDIntro.Domain;
using DDDIntro.Domain.Abstract;
using FluentNHibernate.Automapping;

namespace DDDIntro.Persistence.NHibernate.MappingConventions
{
    public class DefaultMappingConfiguration : DefaultAutomappingConfiguration
    {
        public override bool ShouldMap(Type type)
        {
            return type.Namespace == typeof(Player).Namespace
                && !type.IsDefined(typeof(CompilerGeneratedAttribute), false); // see http://stackoverflow.com/a/11447966/84590
        }

        public override bool IsComponent(Type type)
        {
            return typeof(ValueObject).IsAssignableFrom(type) || typeof(ValueObject<>).IsAssignableFrom(type);
        }
    }
}