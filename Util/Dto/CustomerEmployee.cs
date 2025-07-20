using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util.Models;

namespace Util.Dto
{
    public class CustomerEmployee
    {
        public Customer Customer { get; set; }

        public Employee Employee { get; set; }
    }
}
