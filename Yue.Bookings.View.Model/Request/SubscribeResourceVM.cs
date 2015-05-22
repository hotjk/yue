using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yue.Bookings.Contract;

namespace Yue.Bookings.View.Model
{
    public class SubscribeResourceVM
    {
        public int Resource { get; set; }

        [Required]
        [StringLength(200)]
        public string Message { get; set; }

        [Required]
        public DateTime From { get; set; }

        [Required]
        public DateTime To { get; set; }
    }
}
