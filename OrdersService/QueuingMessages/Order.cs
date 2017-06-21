using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueuingMessages
{
    public class Order
    {
        public Guid Id { get; set; }
        public DateTime Created { get; set; }
        public List<OrderItem> Items { get; set; }
    }
}
