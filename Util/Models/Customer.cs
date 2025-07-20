using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Util.Models
{
    public class Customer
    {
        public long Id { get; set; }
        public string Nome { get; set; }
        public string CPF { get; set; }
        public virtual IEnumerable<Order> Orders { get; set; }
        public Customer()
        {
            Orders = new List<Order>();
        }

    }
}
