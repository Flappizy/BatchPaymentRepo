using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class PaymentDTO
    {
        public Guid SourceAccount { get; set; }
        public Guid DestinationAccount { get; set; }
        public decimal Amount { get; set; }
    }
}
