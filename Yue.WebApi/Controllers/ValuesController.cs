using ACE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Yue.Bookings.Model;

namespace Yue.WebApi.Controllers
{
    public class ValuesController : ApiController
    {
        private IBookingService _bookingService;
        public ValuesController(
            IBookingService bookingService)
        {
            _bookingService = bookingService;
        }
        // GET api/values
        public IEnumerable<string> Get()
        {
            Booking booking = _bookingService.Get(100);
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
