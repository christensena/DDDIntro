using DDDIntro.Domain;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;

namespace DDDIntro.Persistence.NHibernate.MappingOverrides
{
    public class TeamMappingOverride : IAutoMappingOverride<Team>
    {
        public void Override(AutoMapping<Team> mapping)
        {
            mapping.HasManyToMany(x => x.Members)
                .Access.CamelCaseField() // without this, the 'members' backing field is not proxy populated correctly by NH, producing errors when manipulating the list of a rehydrated Team
                .AsList(x => x.Column("BattingSequence")) // this means ordering is maintained but I don't need BattingSequence cluttering+complicating my model
                .Cascade.SaveUpdate(); // with Cascade.SaveUpdate I include saves which means I don't need to add Players to a NH session to get them saved. just save the Team

            mapping.References(x => x.Match);
        }
    }
}