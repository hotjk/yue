using ACE;
using Grit.Sequence;
using Grit.Utility.Authentication;
using Grit.Utility.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Yue.Bookings.View.Model;
using Yue.Common.Contract;
using Yue.Users.Contract.Actions;
using Yue.Users.Contract.Events;
using Yue.Users.Model;
using Yue.Users.View.Model;
using Yue.WebApi.Models;

/*
 * Get
 * curl "http://localhost:64777/api/test" -i
 * 
 */

namespace Yue.WebApi.Controllers
{
    [RoutePrefix("api/test")]
    public class TestController: ApiController
    {
        public TestController(ISequenceService authenticator) 
        {
        }

        [HttpGet]
        [Route("")]
        public IHttpActionResult Get([FromUri]TestModel test)
        {
            if (!ModelState.IsValid)
            {
                return Ok(ModelState);
            }
            return Ok();
        }
    }
}
