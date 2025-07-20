using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Util.Models
{
    public class Employee
    {
        public long Id { get; set; }
        public string CNPJ { get; set; }
        public string NomeFantasia { get; set; }
        public virtual IEnumerable<Order> Orders { get; set; }

        public Employee()
        {
                Orders = new List<Order>();     
        }
    }
}
