using System;
using DDDIntro.Domain;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;

namespace DDDIntro.Persistence.MappingOverrides
{
    public class TeamInningsMappingOverride : IAutoMappingOverride<TeamInnings>
    {
        public void Override(AutoMapping<TeamInnings> mapping)
        {
            mapping.HasMany(x => x.Overs)
                .Access.CamelCaseField() // without this, the 'overs' backing field is not proxy populated correctly by NH, producing errors when manipulating the list of a rehydrated TeamInnings
                .AsList(x => x.Column("Number")) // this means ordering is maintained but I may not want Number cluttering+complicating my model
                .Cascade.AllDeleteOrphan(); // with Cascade.AllDeleteOrphan I include saves which means I don't need to add Overs to a NH session to get them saved. just save the Match

            mapping.HasMany(x => x.BatterInnings)
                .Access.CamelCaseField()
                .AsList(x => x.Column("BattingOrder"))
                .Cascade.AllDeleteOrphan();
        }
    }
}