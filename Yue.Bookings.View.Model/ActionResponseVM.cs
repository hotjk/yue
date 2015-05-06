using ACE.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yue.Bookings.View.Model
{
    public class ActionResponseVM
    {
        public ActionResponse.ActionResponseResult Result { get; set; }
        public string Message { get; set; }

        static ActionResponseVM()
        {
            AutoMapper.Mapper.CreateMap<ActionResponse, ActionResponseVM>();
        }

        public static ActionResponseVM ToVM(ActionResponse m)
        {
            return AutoMapper.Mapper.Map<ActionResponseVM>(m);
        }
    }
}
