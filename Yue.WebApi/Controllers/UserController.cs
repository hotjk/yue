using ACE;
using ACE.Actions;
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
curl --data "email=zhongwx@gmail.com&name=weixiao&password=pwd" "http://localhost:64777/api/users" -i
curl "http://localhost:64777/api/users/17" -i --cookie ".auth=g%252FXQOxPnqqS5c%252F%252B7AK2lnB5c0eat7btMdOxxQnvu1eHvcrEVMjgAP4NuEB0JA087gdkSv0eNCgtkfoTbgBX%252BEQ%253D%253D"
curl --data "email=zhongwx@gmail.com&password=pwd" "http://localhost:64777/api/users/login" -i
curl -X PATCH --data "password=pwd&newPassword=pwd1" "http://localhost:64777/api/users/17/actions/change_password" -i
*/

namespace Yue.WebApi.Controllers
{
    [RoutePrefix("api/users")]
    public class UserController : ApiControllerBase
    {
        private ISequenceService _sequenceService;
        private IUserService _userService;
        private IUserSecurityService _userSecurityService;

        public UserController(IAuthenticator authenticator, 
            IActionBus actionBus,
            IEventBus eventBus,
            ISequenceService sequenceService,
            IUserService userService,
            IUserSecurityService userSecurityService) 
            : base(authenticator, actionBus, eventBus)
        {
            _authenticator = authenticator;
            _actionBus = actionBus;
            _eventBus = eventBus;
            _sequenceService = sequenceService;
            _userService = userService;
            _userSecurityService = userSecurityService;
        }

        [HttpGet]
        [Route("{id}")]
        [ApiAuthorize]
        public IHttpActionResult Get(int id)
        {
            if (UserId != id)
            {
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.Forbidden));
            }
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

            if(!match)
            {
                return Unauthorized(new AuthenticationHeaderValue("Basic"));
            }
            var cookie = _authenticator.GetCookieTicket(user);
            HttpResponseMessage responseMsg = Request.CreateResponse<User>(HttpStatusCode.OK, user);
            responseMsg.Headers.AddCookies(new CookieHeaderValue[] { cookie });
            return ResponseMessage(responseMsg);
        }

        //[HttpPatch]
        //[Route("{id}/actions/singout")]
        //[ApiAuthorize]
        //public IHttpActionResult Signout()
        //{

        //}

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
