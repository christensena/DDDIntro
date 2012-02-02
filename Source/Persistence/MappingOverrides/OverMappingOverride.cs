using System;
using DDDIntro.Domain;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;
using FluentNHibernate.Mapping;

namespace DDDIntro.Persistence.MappingOverrides
{
    public class OverMappingOverride : IAutoMappingOverride<Over>
    {
        public void Override(AutoMapping<Over> mapping)
        {
            mapping.HasMany(x => x.Balls)
                .Access.CamelCaseField()
                .AsList(x => x.Column("Number"))
                .Component(BallComponentMapping.Map)
                .Cascade.AllDeleteOrphan();
        }
    }

    public sealed class BallComponentMapping
    {
        public static void Map(CompositeElementPart<Ball> part)
        {
            part.References(x => x.Batter);
            part.References(x => x.Bowler);
            part.Map(x => x.RunsScored);
        }
    }
}