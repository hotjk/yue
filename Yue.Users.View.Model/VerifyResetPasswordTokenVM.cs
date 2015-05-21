using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yue.Users.View.Model
{
    public class VerifyResetPasswordTokenVM
    {
        public int User { get; set; }
        public string Token { get; set; }
    }
}
