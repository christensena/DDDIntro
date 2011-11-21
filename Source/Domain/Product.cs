using System.Collections.Generic;
using System.Linq;

namespace DDDIntro.Domain
{
    public class Product
    {
        private ISet<Supplier> suppliers;

        public virtual int Id { get; private set; }

        public virtual string Title { get; set; }

        public virtual IEnumerable<Supplier> GetSuppliers()
        {   
            return suppliers.ToArray();
        }
    }
}