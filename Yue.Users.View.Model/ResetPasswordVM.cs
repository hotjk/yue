using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yue.Users.View.Model
{
    public class ResetPasswordVM
    {
        public int User { get; set; }
        public string Token { get; set; }
        public bool Reset { get; set; }
        public string Password { get; set; }
    }
}
