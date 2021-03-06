﻿using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yue.Bookings.Contract;
using Yue.Bookings.Contract.Actions;
using Yue.Bookings.Contract.Commands;
using Yue.Bookings.Model;

namespace Yue.Bookings.View.Model
{
    public class BookingVM
    {
        public int ResourceId { get; set; }
        public int BookingId { get; set; }
        public TimeSlot TimeSlot { get; set; }
        public BookingState State { get; set; }

        public int CreateBy { get; set; }
        public DateTime CreateAt { get; set; }
        public int UpdateBy { get; set; }
        public DateTime UpdateAt { get; set; }

        public IEnumerable<BookingActivityVM> Activities { get; set; }

        static BookingVM()
        {
            Mapper.CreateMap<Booking, BookingVM>();
            Mapper.CreateMap<BookingCommandBase, BookingActivityVM>();
        }

        public static BookingVM ToVM(Booking booking)
        {
            BookingVM vm = Mapper.Map<BookingVM>(booking);
            if (!vm.Activities.Any())
            {
                vm.Activities = null;
            }
            return vm;
        }

        public static IEnumerable<BookingVM> ToVM(IEnumerable<Booking> bookings)
        {
            IEnumerable<BookingVM> vms = Mapper.Map<IEnumerable<BookingVM>>(bookings);
            foreach(var vm in vms)
            {
                if (!vm.Activities.Any())
                {
                    vm.Activities = null;
                }
            }
            return vms;
        }
    }
}
