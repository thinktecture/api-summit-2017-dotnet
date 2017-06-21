using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueuingMessages
{
    public class NewOrderMessage
    {
        public Order Order { get; set; }
        public string UserId { get; set; }
    }
}
