﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yue.Bookings.Contract;

namespace Yue.Bookings.Model.Commands
{
    public class CancelSubscriotion : BookingCommandBase, ACE.ICommand
    {
        public CancelSubscriotion(int activityId, int resourceId, int bookingId, string message, int createBy, DateTime createAt)
            : base(activityId, resourceId, bookingId, BookingCommand.CancelSubscriotion, message, createBy, createAt) { }
    }
}