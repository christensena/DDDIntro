using System;
using DDDIntro.Domain;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;

namespace Persistence.MappingOverrides
{
    public class PurchaseOrderMappingOverride : IAutoMappingOverride<PurchaseOrder>
    {
        public void Override(AutoMapping<PurchaseOrder> mapping)
        {
            mapping.HasMany(x => x.OrderLines)
                .AsList(x => x.Column("LineNumber")) // wonderful piece of magic! this means ordering is maintained but I don't need LineNumber cluttering my model
                .Cascade.AllDeleteOrphan(); // with Cascade.All I include saves which means I don't need to add orderlines to a NH session to get them saved. just save the order
        }
    }
}