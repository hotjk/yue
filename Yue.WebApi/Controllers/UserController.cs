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

/*
 * Get
 * curl "http://localhost:64777/api/users" -i --cookie ".auth=VlKks%252FpLOPteEbgKotr72EiyKvgwi%252F3N1cEMAv9jZBIUjxLwgvohxvQP4IarZ5TU2Iwnh%252BdS2sgsVZpTr8eV3Q%253D%253D;"
 * 
 */

namespace Yue.WebApi.Controllers
{
    [RoutePrefix("api/users")]
    public class UserController : ApiAuthorizeController
    {
        private IEventBus EventBus;
        private ISequenceService _sequenceService;
        private IUserService _userService;
        private IUserSecurityService _userSecurityService;

        public UserController(IAuthenticator authenticator, 
            IActionBus actionBus,
            IEventBus eventBus,
            ISequenceService sequenceService,
            IUserService userService,
            IUserSecurityService userSecurityService) 
            : base(authenticator, actionBus)
        {
            EventBus = eventBus;
            _sequenceService = sequenceService;
            _userService = userService;
            _userSecurityService = userSecurityService;
        }

        [HttpGet]
        [Route("")]
        [ApiAuthorize]
        public IHttpActionResult Get()
        {
            User user = _userService.Get(UserId.Value);
            return Ok(user);
        }
    }
}
