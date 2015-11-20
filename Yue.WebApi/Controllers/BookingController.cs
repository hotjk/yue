using ACE;
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
/*
curl --data "message=hello&from=2015-01-01T01:01:01&to=2015-01-01T02:01:01&resource=1" "http://localhost:64777/api/bookings"
curl "http://localhost:64777/api/bookings/43?activity=true"
curl -X PATCH --data "message=hello" "http://localhost:64777/api/bookings/43/actions/confirm"
curl -X PATCH --data "message=hello" "http://localhost:64777/api/bookings/43/actions/message"
curl -X PATCH --data "message=hello&from=2015-01-01T01:01:01&to=2015-01-01T02:01:01" "http://localhost:64777/api/bookings/43/actions/time"
curl -X DELETE --data "message=hello" "http://localhost:64777/api/bookings/43"
curl "http://localhost:64777/api/bookings?resource=1&from=2015-01-01T01:01:01&to=2015-01-01T02:01:01"
curl "http://localhost:64777/api/bookings?user=0&from=2015-01-01T01:01:01&to=2015-01-01T02:01:01"
*/
namespace Yue.WebApi.Controllers
{
    [RoutePrefix("api/bookings")]
    public class BookingController : ApiAuthorizeController
    {
        private IActionBus _actionBus;
        private ISequenceService _sequenceService;
        private IBookingService _bookingService;

        public BookingController(
            IAuthenticator authenticator,
            IActionBus actionBus,
            IEventBus eventBus,
            IBookingService bookingService,
            ISequenceService sequenceService) 
            : base(authenticator, actionBus)
        {
            _actionBus = actionBus;
            _sequenceService = sequenceService;
            _bookingService = bookingService;
        }

        [HttpGet]
        [Route("{id}")]
        [ApiAuthorize]
        public IHttpActionResult Get(int id, bool activity=false)
        {
            Booking booking = _bookingService.Get(id, activity);
            return Ok(BookingVM.ToVM(booking));
        }

        [HttpGet]
        [Route("")]
        [ApiAuthorize]
        public IHttpActionResult BookingsByResource(DateTime from, DateTime to, int resource)
        {
            var bookings = _bookingService.BookingsByResource(new TimeSlot(from, to), resource);
            return Ok(BookingVM.ToVM(bookings));
        }

        [HttpGet]
        [Route("")]
        [ApiAuthorize]
        public IHttpActionResult BookingsByUser(DateTime from, DateTime to, int user)
        {
            var bookings = _bookingService.BookingsByUser(new TimeSlot(from, to), user);
            return Ok(BookingVM.ToVM(bookings));
        }

        [HttpPost]
        [Route("")]
        [ApiAuthorize]
        public async Task<IHttpActionResult> SubscribeResource([FromBody]SubscribeResourceVM vm)
        {
            SubscribeResource action = new SubscribeResource(
                _sequenceService.Next(Sequence.BookingAction),
                vm.Resource,
                _sequenceService.Next(Sequence.Booking),
                vm.Message,
                 new TimeSlot(vm.From, vm.To),
                DateTime.Now,
                UserId.Value);
            ActionResponse actionResponse = await _actionBus.SendAsync<BookingActionBase, SubscribeResource>(action);

            var booking = _bookingService.Get(action.BookingId);
            return Created<BookingVM>(
                Url.Link("", new { controller = "Booking", id = booking.BookingId }), 
                BookingVM.ToVM(booking));
        }

        [HttpDelete]
        [Route("{id}")]
        [ApiAuthorize]
        public async Task<IHttpActionResult> CancelSubscriotion(int id, [FromBody]LeaveAMessageVM vm)
        {
            Booking booking = _bookingService.Get(id);
            if (booking == null)
            {
                return NotFound();
            }

            if (!booking.EnsoureState(BookingCommand.CancelSubscriotion))
            {
                return BadRequest("Invalid booking state.");
            }

            CancelSubscriotion cs = new CancelSubscriotion(
                    _sequenceService.Next(Sequence.BookingAction),
                        booking.ResourceId,
                        booking.BookingId,
                        vm == null ? null : vm.Message,
                        DateTime.Now,
                        UserId.Value);
            ActionResponse actionResponse = await _actionBus.SendAsync<BookingActionBase, CancelSubscriotion>(cs);
            return Ok(ActionResponseVM.ToVM(actionResponse));
        }

        [HttpPatch]
        [Route("{id}/actions/confirm")]
        [ApiAuthorize]
        public async Task<IHttpActionResult> ConfirmSubscription(int id, [FromBody]LeaveAMessageVM vm)
        {
            Booking booking = _bookingService.Get(id);
            if (booking == null)
            {
                return NotFound();
            }

            if (!booking.EnsoureState(BookingCommand.ConfirmSubscription))
            {
                return BadRequest("Invalid booking state.");
            }

            ConfirmSubscription cs = new ConfirmSubscription(
                    _sequenceService.Next(Sequence.BookingAction),
                    booking.ResourceId,
                    booking.BookingId,
                    vm == null ? null : vm.Message,
                    DateTime.Now,
                    UserId.Value);
            ActionResponse actionResponse = await _actionBus.SendAsync<BookingActionBase, ConfirmSubscription>(cs);
            return Ok(ActionResponseVM.ToVM(actionResponse));
        }

        [HttpPatch]
        [Route("{id}/actions/message")]
        [ApiAuthorize]
        public async Task<IHttpActionResult> LeaveAMessage(int id, [FromBody]LeaveAMessageVM vm)
        {
            Booking booking = _bookingService.Get(id);
            if (booking == null)
            {
                return NotFound();
            }

            if (!booking.EnsoureState(BookingCommand.LeaveAMessage))
            {
                return BadRequest("Invalid booking state.");
            }
            LeaveAMessage cs = new LeaveAMessage(
                _sequenceService.Next(Sequence.BookingAction),
                    booking.ResourceId,
                    booking.BookingId,
                    vm.Message,
                    DateTime.Now,
                    UserId.Value);
            ActionResponse actionResponse = await _actionBus.SendAsync<BookingActionBase, LeaveAMessage>(cs);
            return Ok(ActionResponseVM.ToVM(actionResponse));
        }

        [HttpPatch]
        [Route("{id}/actions/time")]
        [ApiAuthorize]
        public async Task<IHttpActionResult> ChangeTime(int id, [FromBody]ChangeTimeVM vm)
        {
            Booking booking = _bookingService.Get(id);
            if (booking == null)
            {
                return NotFound();
            }

            if (!booking.EnsoureState(BookingCommand.ChangeTime))
            {
                return BadRequest("Invalid booking state.");
            }

            ChangeTime cs = new ChangeTime(
                _sequenceService.Next(Sequence.BookingAction),
                    booking.ResourceId,
                    booking.BookingId,
                    vm == null ? null : vm.Message,
                    new TimeSlot(vm.From, vm.To),
                    DateTime.Now,
                    UserId.Value);
            ActionResponse actionResponse = await _actionBus.SendAsync<BookingActionBase, ChangeTime>(cs);
            return Ok(ActionResponseVM.ToVM(actionResponse));
        }
    }
}
