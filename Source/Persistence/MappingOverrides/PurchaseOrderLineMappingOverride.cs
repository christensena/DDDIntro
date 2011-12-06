using DDDIntro.Domain;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;

namespace Persistence.MappingOverrides
{
    public class PurchaseOrderLineMappingOverride : IAutoMappingOverride<PurchaseOrderLine>
    {
        public void Override(AutoMapping<PurchaseOrderLine> mapping)
        {
            mapping.HasOne(x => x.Order);
        }
    }
}