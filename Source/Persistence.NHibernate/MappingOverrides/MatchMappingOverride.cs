using DDDIntro.Domain;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;

namespace DDDIntro.Persistence.NHibernate.MappingOverrides
{
    public class MatchMappingOverride : IAutoMappingOverride<Match>
    {
        public void Override(AutoMapping<Match> mapping)
        {
            mapping.HasMany(x => x.Innings)
                .Access.CamelCaseField() // without this, the 'members' backing field is not proxy populated correctly by NH, producing errors when manipulating the list of a rehydrated Team
                .AsList(x => x.Column("Number")) // this means ordering is maintained but I may not want Number cluttering+complicating my model
                .Cascade.AllDeleteOrphan(); // with Cascade.AllDeleteOrphan I include saves which means I don't need to add Overs to a NH session to get them saved. just save the Match
            // unlike for Team->Player, an Innings makes sense only in the context of a Match otherwise it doesn't exist
            // so I have AllDeleteOrphan to delete all innings if a match is deleted
        }
    }
}