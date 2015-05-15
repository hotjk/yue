using ACE;
using ACE.Actions;
using AppHarbor.Web.Security;
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
curl "http://localhost:64777/api/users/17" -i --cookie ".auth=AHSZwuZ%252F0XthPX28wVZwW8eKDqRko8Cead4n7M2XecA%252BkQsWPBEEt8F40jgFr3CYimv0fsPeksBDcwI5p9gOG%252BO607VhXTtl0TNOnmd7xoxwdboeM6LB2r2uHm5%252FXHvC2Q%253D%253D"
curl --data "email=zhongwx@gmail.com&password=pwd" "http://localhost:64777/api/users/login"
curl -X PATCH --data "password=pwd&newPassword=pwd1" "http://localhost:64777/api/users/17/actions/change_password"
*/

namespace Yue.WebApi.Controllers
{
    [RoutePrefix("api/users")]
    public class UserController : ApiControllerBase
    {
        private IActionBus _actionBus;
        private IEventBus _eventBus;
        private ICookieAuthenticationConfiguration _cookieAuthenticationConfiguration;
        private ISequenceService _sequenceService;
        private IUserService _userService;
        private IUserSecurityService _userSecurityService;

        public UserController(IActionBus actionBus,
            IEventBus eventBus,
            ICookieAuthenticationConfiguration cookieAuthenticationConfiguration,
            ISequenceService sequenceService,
            IUserService userService,
            IUserSecurityService userSecurityService)
        {
            _actionBus = actionBus;
            _eventBus = eventBus;
            _cookieAuthenticationConfiguration = cookieAuthenticationConfiguration;
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

            //if (match)
            //{
            //    HttpResponseMessage responseMsg = Request.CreateResponse<bool>(HttpStatusCode.OK, match);
            //    var cookie = AuthenticationHelper.GetAuthCookie(user.UserId.ToString());
            //    responseMsg.Headers.AddCookies(new CookieHeaderValue[] { cookie });
            //    return ResponseMessage(responseMsg);
            //}

            if(match)
            {
                //_authenticator.SetCookie(user.UserId.ToString());
                var cookie = new AuthenticationTicket(0, Guid.NewGuid(), false, user.UserId.ToString());
                using (var protector = new CookieProtector(_cookieAuthenticationConfiguration))
                {
                    CookieHeaderValue cookieValue = new CookieHeaderValue(_cookieAuthenticationConfiguration.CookieName,
                        WebUtility.UrlEncode(protector.Protect(cookie.Serialize())));
                    cookieValue.HttpOnly = true;
                    cookieValue.Secure = _cookieAuthenticationConfiguration.RequireSSL;
                    cookieValue.Path = "/";
                    if (cookie.Persistent)
                    {
                        cookieValue.Expires = cookie.IssueDate + _cookieAuthenticationConfiguration.Timeout;
                    }
                    HttpResponseMessage responseMsg = Request.CreateResponse<bool>(HttpStatusCode.OK, match);
                    responseMsg.Headers.AddCookies(new CookieHeaderValue[] { cookieValue });
                    return ResponseMessage(responseMsg);
                }
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
