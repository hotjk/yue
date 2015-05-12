using ACE;
using ACE.Actions;
using Grit.Sequence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;
using Yue.Bookings.View.Model;
using Yue.Common.Contract;
using Yue.Users.Contract.Actions;
using Yue.Users.Contract.Events;
using Yue.Users.Model;
using Yue.Users.View.Model;

/*
curl --data "email=zhongwx@gmail.com&name=weixiao&password=pwd" "http://localhost:64777/api/users"
curl "http://localhost:64777/api/users/17" -i --cookie ".auth=A109B8C21B2201483739B2D01AC23121F3EAF8B18E1412E3531F0ACEA5F650DD6FF6F14A008D256F4125AE6C914C60A627E69368DD56E7983374114E7F096739CC22A4F64AE9E207FF082EEF0FA6568799543E61F02C09D1FD5B6030F0D5CA42"
curl --data "email=zhongwx@gmail.com&password=pwd" "http://localhost:64777/api/users/login"
curl -X PATCH --data "password=pwd&newPassword=pwd1" "http://localhost:64777/api/users/17/actions/change_password"
*/

namespace Yue.WebApi.Controllers
{
    [RoutePrefix("api/users")]
    public class UserController : ApiController
    {
        private IActionBus _actionBus;
        private IEventBus _eventBus;
        private ISequenceService _sequenceService;
        private IUserService _userService;
        private IUserSecurityService _userSecurityService;

        public UserController(IActionBus actionBus,
            IEventBus eventBus,
            ISequenceService sequenceService,
            IUserService userService,
            IUserSecurityService userSecurityService)
        {
            _actionBus = actionBus;
            _eventBus = eventBus;
            _sequenceService = sequenceService;
            _userService = userService;
            _userSecurityService = userSecurityService;
        }

        private const int userId = 0;

        [HttpGet]
        [Route("{id}")]
        [ApiAuthorize]
        public IHttpActionResult Get(int id)
        {
            User user = _userService.Get(id);
            return Ok(user);
        }

        [HttpPost]
        [Route("")]
        public async Task<IHttpActionResult> Register([FromBody]RegisterVM vm)
        {
            User user = _userService.UserByEmail(vm.Email);
            if (user != null)
            {
                return Conflict();
            }

            Register action = new Register(
                _sequenceService.Next(Sequence.User),
                vm.Email,
                vm.Name,
                _userSecurityService.PasswordHash(vm.Password),
                DateTime.Now);
            ActionResponse actionResponse = await _actionBus.SendAsync<UserActionBase, Register>(action);

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
        [Route("login")]
        public IHttpActionResult Login([FromBody]LoginVM vm)
        {
            User user = _userService.UserByEmail(vm.Email);
            if (user == null)
            {
                return NotFound();
            }

            bool match = _userSecurityService.VerifyPassword(user.UserId, vm.Password);
            _eventBus.Publish(new UserPasswordVerified(user.UserId, match, DateTime.Now, user.UserId).ToExternalQueue());

            if (match)
            {
                HttpResponseMessage responseMsg = Request.CreateResponse<bool>(HttpStatusCode.OK, match);
                var cookie = AuthenticationHelper.GetAuthCookie(user.UserId.ToString());
                responseMsg.Headers.AddCookies(new CookieHeaderValue[] { cookie });
                return ResponseMessage(responseMsg);
            }
            
            return Ok(match);
        }

        [HttpPatch]
        [Route("{id}/actions/change_password")]
        [ApiAuthorize]
        public async Task<IHttpActionResult> ChangePassword(int id, [FromBody]ChangePasswordVM vm)
        {
            bool match = _userSecurityService.VerifyPassword(id, vm.Password);
            if (!match)
            {
                return BadRequest();
            }
            ChangePassword action = new ChangePassword(
                id,
                _userSecurityService.PasswordHash(vm.Password),
                DateTime.Now,
                id);
            ActionResponse actionResponse = await _actionBus.SendAsync<UserActionBase, ChangePassword>(action);
            return Ok(ActionResponseVM.ToVM(actionResponse));
        }
    }
}
