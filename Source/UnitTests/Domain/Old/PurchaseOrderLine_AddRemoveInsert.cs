using System.Linq;
using DDDIntro.Domain;
using Machine.Specifications;

namespace DDDIntro.UnitTests.Domain
{
    [Subject(typeof(PurchaseOrder), "Adding order lines")]
    public class PurchaseOrderLine_AddOrderLine : PurchaseOrderLinesListSpecs
    {
        Because of = () => _order.AddOrderLine(_product1);

        It should_have_one_orderline = () => _order.OrderLines.Count().ShouldEqual(1);

        It should_have_default_quantity_of_one = () => _order.OrderLines.First().Quantity.ShouldEqual(1);
    }

    [Subject(typeof(PurchaseOrder), "Adding order line where same product already exists on order")]
    public class PurchaseOrderLine_AddOrderLineExistingProduct : PurchaseOrderLinesListSpecs
    {
        Establish context = () => _order.AddOrderLine(_product1);

        Because of = () => _order.AddOrderLine(_product1);

        It should_have_one_orderline = () => _order.OrderLines.Count().ShouldEqual(1);

        It should_have_incremented_quantity = () => _order.OrderLines.First().Quantity.ShouldEqual(2);
    }

    [Subject(typeof(PurchaseOrder), "Removing order lines")]
    public class PurchaseOrderLine_RemoveFirstOrderLine : PurchaseOrderLinesListSpecs
    {
        Establish context = () =>
                                        {
                                            _order.AddOrderLine(_product1);
                                            _order.AddOrderLine(_product2);
                                        };

        Because of = () => _order.RemoveOrderLine(_order.OrderLines.First());

        It should_have_one_orderline = () => _order.OrderLines.Count().ShouldEqual(1);
    }

    [Subject(typeof(PurchaseOrder), "Inserting order line")]
    public class PurchaseOrderLine_InsertOrderLine : PurchaseOrderLinesListSpecs
    {
        static PurchaseOrderLine _insertedOrderLine;

        Establish context = () =>
        {
            _order.AddOrderLine(_product1);
            _order.AddOrderLine(_product2);
        };

        Because of = () =>
                                 {
                                     _insertedOrderLine = _order.InsertOrderLineAfter(_order.OrderLines.First(),
                                                                                      new Product().WithId(678));
                                 };

        It should_have_three_orderlines = () => _order.OrderLines.Count().ShouldEqual(3);

        It should_have_new_orderline_after_first_line =
            () => _order.OrderLines.Skip(1).First().ShouldBeTheSameAs(_insertedOrderLine);
    }

    public abstract class PurchaseOrderLinesListSpecs
    {
        protected static Product _product1;
        protected static Product _product2;
        protected static PurchaseOrder _order;

        Establish context = () =>
        {
            _product1 = new Product().WithId(123);
            _product2 = new Product().WithId(456);
            _order = new PurchaseOrder("0001", new Supplier());
        };
    }

}