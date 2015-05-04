using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yue.Bookings.View.Model
{
    public class SubscribeResourceVM
    {
        public int ResourceId { get; set; }
        public string Message { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
    }
}
