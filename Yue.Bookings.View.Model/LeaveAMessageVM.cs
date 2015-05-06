using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yue.Bookings.View.Model
{
    public class LeaveAMessageVM
    {
        [Required]
        [StringLength(200)]
        public string Message { get; set; }
    }
}
