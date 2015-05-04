using ACE;
using ACE.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Yue.Bookings.Contract.Actions;
using Yue.Bookings.Model;
using Yue.Bookings.View.Model;

namespace Yue.WebApi.Controllers
{
    public class BookingController : ApiController
    {
        private IActionBus _actionBus;
        private IBookingService _bookingService;
        public BookingController(IActionBus actionBus,
            IBookingService bookingService)
        {
            _actionBus = actionBus;
            _bookingService = bookingService;
        }

        public IHttpActionResult Get(int id)
        {
            Booking booking = _bookingService.Get(id, true);
            return Ok(booking);
        }

        [HttpPost]
        public async Task<IHttpActionResult> Subscribe([FromBody]SubscribeResourceVM vm)
        {
            SubscribeResource action = new SubscribeResource(2, 2, 3, "hello", 4, DateTime.Now,
                new Bookings.Contract.TimeSlot(DateTime.Now, DateTime.Now.AddMonths(30)));
            ActionResponse actionResponse = await _actionBus.SendAsync<BookingActionBase, SubscribeResource>(action);
            return Ok(actionResponse);
        }
    }
}
