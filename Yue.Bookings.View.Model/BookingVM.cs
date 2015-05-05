using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yue.Bookings.Contract;
using Yue.Bookings.Contract.Actions;

namespace Yue.Bookings.Model
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

        public IEnumerable<BookingActionVM> Actions { get; set; }

        static BookingVM()
        {
            Mapper.CreateMap<Booking, BookingVM>();
            Mapper.CreateMap<BookingActionBase, BookingActionVM>();
        }

        public static BookingVM ToVM(Booking booking)
        {
            BookingVM vm = Mapper.Map<BookingVM>(booking);
            if (!vm.Actions.Any())
            {
                vm.Actions = null;
            }
            return vm;
        }
    }
}
