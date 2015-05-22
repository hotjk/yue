using ACE;
using ACE.IActions;
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
 * Register
 * curl --data "email=zhongwx@gmail.com&name=weixiao&password=pwd" "http://localhost:64777/api/users/register" -i
 * 
 * Login
 * curl --data "email=zhongwx@gmail.com&password=pwd" "http://localhost:64777/api/users/actions/login" -i
 * 
 * Get
 * curl "http://localhost:64777/api/users" -i --cookie ".auth=VlKks%252FpLOPteEbgKotr72EiyKvgwi%252F3N1cEMAv9jZBIUjxLwgvohxvQP4IarZ5TU2Iwnh%252BdS2sgsVZpTr8eV3Q%253D%253D;"
 * 
 * Sign Out
 * curl -X POST --data "" "http://localhost:64777/api/users/signout" -i
 * 
 * Change Password
 * curl -X POST --data "password=pwd&newPassword=pwd1" "http://localhost:64777/api/users/change_password" -i --cookie ".auth=oebk2ctBVEwo4gC4AoYRArfBGR0nmN7PyB4KdVZLXKCRxsDKJCCaJ7nBi77IBHRWj7ycGvk6yCqbTRFlMbw36w%253D%253D;"
 * 
 * Request Activate Code
 * curl -X GET "http://localhost:64777/api/users/activate" -i --cookie ".auth=oebk2ctBVEwo4gC4AoYRArfBGR0nmN7PyB4KdVZLXKCRxsDKJCCaJ7nBi77IBHRWj7ycGvk6yCqbTRFlMbw36w%253D%253D;"
 * 
 * Activate
 * curl -X POST --data "user=33&token=5f162f8d-009e-4d3a-8f6c-21b99deb1550" "http://localhost:64777/api/users/activate" -i
 * 
 * Request Reset Password Token
 * curl -X POST --data "email=zhongwx@gmail.com" "http://localhost:64777/api/users/request_reset_password" -i 
 * 
 * Verify Reset Password Token
 * curl -X POST --data "user=33&token=ee90cff6-b35d-4b32-8346-8a22ce868eea" "http://localhost:64777/api/users/verify_reset_password" -i 
 * 
 * Cancel Reset Password
 * curl -X POST --data "user=33&token=ee90cff6-b35d-4b32-8346-8a22ce868eea" "http://localhost:64777/api/users/cancel_reset_password" -i 
 * 
 * Reset Password
 * curl -X POST --data "user=33&token=7e93e31f-ca8d-41b0-a3da-13bd2a4a9c0d&password=pwd" "http://localhost:64777/api/users/reset_password" -i 
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

        [HttpPost]
        [Route("")]
        public async Task<IHttpActionResult> Register(RegisterVM vm)
        {
            User user = _userService.UserByEmail(vm.Email);
            if (user != null)
            {
                return Conflict();
            }

            int userId = _sequenceService.Next(Sequence.User);
            Register action = new Register(
                userId, DateTime.Now, userId, _userSecurityService.PasswordHash(userId, vm.Password), vm.Email, vm.Name);
            ActionResponse actionResponse = await ActionBus.SendAsync<UserActionBase, Register>(action);

            if (actionResponse.Result == ActionResponse.ActionResponseResult.OK)
            {
                user = _userService.Get(action.UserId);
                return Created<User>(
                    Url.Link("", new { controller = "User", id = user.UserId }),
                    user);
            }
            return Ok(ActionResponseVM.ToVM(actionResponse));
        }

        [HttpPost]
        [Route("actions/login")]
        public IHttpActionResult Login(LoginVM vm)
        {
            User user = _userService.UserByEmail(vm.Email);
            if (user == null)
            {
                return NotFound();
            }

            if(!user.EnsoureState(Users.Contract.UserSecurityCommand.VerifyPassword))
            {
                return Conflict();
            }

            bool match = _userSecurityService.VerifyPassword(user.UserId, vm.Password);
            EventBus.FlushAnEvent(new UserPasswordVerified(user.UserId, DateTime.Now, user.UserId, match));

            if (!match)
            {
                return Unauthorized(new AuthenticationHeaderValue("Basic"));
            }

            var cookie = Authenticator.GetCookieTicket(user);
            HttpResponseMessage responseMsg = Request.CreateResponse<User>(HttpStatusCode.OK, user);
            responseMsg.Headers.AddCookies(new CookieHeaderValue[] { cookie });
            return ResponseMessage(responseMsg);
        }

        [HttpPost]
        [Route("actions/signout")]
        public IHttpActionResult Signout()
        {
            HttpResponseMessage msg = Request.CreateResponse(HttpStatusCode.OK);
            var cookieValue = new CookieHeaderValue(Authenticator.CookieTicketConfig.CookieName, "");
            cookieValue.Expires = DateTime.UtcNow.AddMonths(-100);
            msg.Headers.AddCookies(new CookieHeaderValue[] { cookieValue });
            return ResponseMessage(msg);
        }

        [HttpPost]
        [Route("actions/activate")]
        [ApiAuthorize]
        public async Task<IHttpActionResult> Activite()
        {
            User user = _userService.Get(UserId.Value);
            if (!user.EnsoureState(Users.Contract.UserSecurityCommand.RequestActivateToken))
            {
                return Conflict();
            }
            RequestActivateToken action = new RequestActivateToken(
                UserId.Value, DateTime.Now, UserId.Value, Guid.NewGuid().ToString());
            ActionResponse actionResponse = await ActionBus.SendAsync<UserActionBase, RequestActivateToken>(action);
            return Ok(ActionResponseVM.ToVM(actionResponse));
        }

        [HttpPost]
        [Route("actions/activate")]
        public async Task<IHttpActionResult> Activite(ActivateVM vm)
        {
            User user = _userService.Get(vm.User);
            if (user == null)
            {
                return NotFound();
            }
            if (!user.EnsoureState(Users.Contract.UserSecurityCommand.RequestActivateToken))
            {
                return Conflict();
            }
            Activate action = new Activate(user.UserId, DateTime.Now, user.UserId, vm.Token);
            ActionResponse actionResponse = await ActionBus.SendAsync<UserActionBase, Activate>(action);
            return Ok(ActionResponseVM.ToVM(actionResponse));
        }
    }
}
