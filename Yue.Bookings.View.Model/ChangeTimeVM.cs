using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yue.Bookings.View.Model
{
    public class ChangeTimeVM
    {
        [Required]
        [StringLength(200)]
        public string Message { get; set; }

        [Required]
        public DateTime From { get; set; }

        [Required]
        public DateTime To { get; set; }
    }
}
