using DDDIntro.Domain;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;

namespace DDDIntro.Persistence.MappingOverrides
{
    public class PurchaseOrderLineMappingOverride : IAutoMappingOverride<PurchaseOrderLine>
    {
        public void Override(AutoMapping<PurchaseOrderLine> mapping)
        {
            mapping.Id().Access.BackingField();
            mapping.HasOne(x => x.Order);
        }
    }
}