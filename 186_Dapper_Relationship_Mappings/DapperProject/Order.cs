using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DapperProject
{
    public class Order
    {
        public long OrderId { get; init; }

        public List<LineItem> LineItems { get; init; } = new();
    }
}
