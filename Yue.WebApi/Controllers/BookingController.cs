using ACE;
using ACE.Actions;
using Grit.Sequence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Yue.Bookings.Contract;
using Yue.Bookings.Contract.Actions;
using Yue.Bookings.Model;
using Yue.Bookings.View.Model;
using Yue.Common.Contract;

namespace Yue.WebApi.Controllers
{
    [RoutePrefix("api/booking")]
    public class BookingController : ApiController
    {
        private IActionBus _actionBus;
        private ISequenceService _sequenceService;
        private IBookingService _bookingService;

        public BookingController(IActionBus actionBus,
            ISequenceService sequenceService,
            IBookingService bookingService)
        {
            _actionBus = actionBus;
            _sequenceService = sequenceService;
            _bookingService = bookingService;
        }

        private const int userId = 0;

        [HttpGet]
        [Route("{id}")]
        public IHttpActionResult Get(int id, bool action=false)
        {
            Booking booking = _bookingService.Get(id, action);
            return Ok(BookingVM.ToVM(booking));
        }

        [HttpPost]
        [Route("")]
        public async Task<IHttpActionResult> SubscribeResource([FromBody]SubscribeResourceVM vm)
        {
            SubscribeResource action = new SubscribeResource(
                _sequenceService.Next(Sequence.BookingAction),
                vm.Resource,
                _sequenceService.Next(Sequence.Booking),
                vm.Message,
                userId,
                DateTime.Now,
                new TimeSlot(vm.From, vm.To));
            ActionResponse actionResponse = await _actionBus.SendAsync<BookingActionBase, SubscribeResource>(action);

            var booking = _bookingService.Get(action.BookingId);
            return Created<BookingVM>("", BookingVM.ToVM(booking));
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IHttpActionResult> CancelSubscriotion(int id, [FromBody]LeaveAMessageVM vm)
        {
            Booking booking = _bookingService.Get(id);
            if (booking == null)
            {
                return NotFound();
            }

            if (!booking.EnsoureState(BookingAction.CancelSubscriotion))
            {
                return BadRequest("Invalid booking state.");
            }

            CancelSubscriotion cs = new CancelSubscriotion(
                    _sequenceService.Next(Sequence.BookingAction),
                        booking.ResourceId,
                        booking.BookingId,
                        vm.Message,
                        userId,
                        DateTime.Now);
            ActionResponse actionResponse = await _actionBus.SendAsync<BookingActionBase, CancelSubscriotion>(cs);
            return Ok(actionResponse);
        }

        [HttpPut]
        [Route("{id}/confirm")]
        public async Task<IHttpActionResult> ConfirmSubscription(int id, [FromBody]LeaveAMessageVM vm)
        {
            Booking booking = _bookingService.Get(id);
            if (booking == null)
            {
                return NotFound();
            }

            if (!booking.EnsoureState(BookingAction.ConfirmSubscription))
            {
                return BadRequest("Invalid booking state.");
            }

            ConfirmSubscription cs = new ConfirmSubscription(
                    _sequenceService.Next(Sequence.BookingAction),
                    booking.ResourceId,
                    booking.BookingId,
                    vm.Message,
                    userId,
                    DateTime.Now);
            ActionResponse actionResponse = await _actionBus.SendAsync<BookingActionBase, ConfirmSubscription>(cs);
            return Ok(actionResponse);
        }

        [HttpPut]
        [Route("{id}/message")]
        public async Task<IHttpActionResult> LeaveAMessage(int id, [FromBody]LeaveAMessageVM vm)
        {
            Booking booking = _bookingService.Get(id);
            if (booking == null)
            {
                return NotFound();
            }

            if (!booking.EnsoureState(BookingAction.LeaveAMessage))
            {
                return BadRequest("Invalid booking state.");
            }
            LeaveAMessage cs = new LeaveAMessage(
                _sequenceService.Next(Sequence.BookingAction),
                    booking.ResourceId,
                    booking.BookingId,
                    vm.Message,
                    userId,
                    DateTime.Now);
            ActionResponse actionResponse = await _actionBus.SendAsync<BookingActionBase, LeaveAMessage>(cs);
            return Ok(actionResponse);
        }

        [HttpPut]
        [Route("{id}/time")]
        public async Task<IHttpActionResult> ChangeTime(int id, [FromBody]ChangeTimeVM vm)
        {
            Booking booking = _bookingService.Get(id);
            if (booking == null)
            {
                return NotFound();
            }

            if (!booking.EnsoureState(BookingAction.ChangeTime))
            {
                return BadRequest("Invalid booking state.");
            }

            ChangeTime cs = new ChangeTime(
                _sequenceService.Next(Sequence.BookingAction),
                    booking.ResourceId,
                    booking.BookingId,
                    vm.Message,
                    userId,
                    DateTime.Now,
                    new TimeSlot(vm.From, vm.To));
            ActionResponse actionResponse = await _actionBus.SendAsync<BookingActionBase, ChangeTime>(cs);
            return Ok(actionResponse);
        }
    }
}
