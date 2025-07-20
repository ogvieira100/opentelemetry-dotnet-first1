using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Util.Models
{
    public class Order
    {
        public string Description { get; set; }
        public long Id { get; set; }
        public long  CustomerId { get; set; }
        public virtual Customer Customer { get; set; }
        public long EmployeeId { get; set; }
        public virtual Employee Employee { get; set; }
    }
}
