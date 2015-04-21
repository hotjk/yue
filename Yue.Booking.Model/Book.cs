using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yue.Booking.Model
{
    public class Book
    {
        public int BookId { get; set; }
        public int ResourceId { get; set; }
        public int ParticipantId { get; set; }
        public TimeSlot TimeSlot { get; set; }
    }
}
