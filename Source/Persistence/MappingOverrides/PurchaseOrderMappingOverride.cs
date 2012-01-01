using DDDIntro.Domain;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;

namespace DDDIntro.Persistence.MappingOverrides
{
    public class PurchaseOrderMappingOverride : IAutoMappingOverride<PurchaseOrder>
    {
        public void Override(AutoMapping<PurchaseOrder> mapping)
        {
            mapping.Map(x => x.OrderNumber).Unique();

            mapping.HasMany(x => x.OrderLines)
                .Access.CamelCaseField() // without this, the orderLines backing field is not proxy populated correctly by NH, producing errors when manipulating the list of a rehydrated order
                .AsList(x => x.Column("LineNumber")) // this means ordering is maintained but I don't need LineNumber cluttering+complicating my model
                .Cascade.AllDeleteOrphan(); // with Cascade.All I include saves which means I don't need to add orderlines to a NH session to get them saved. just save the order
        }
    }
}