using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueuingMessages
{
    public class ShippingCreatedMessage
    {
        public Guid Id { get; set; }
        public DateTime Created { get; set; }
        public Guid OrderId { get; set; }
        public string UserId { get; set; }
    }
}
