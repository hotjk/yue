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
curl --data "email=zhongwx@gmail.com&name=weixiao&password=pwd" "http://localhost:64777/api/users/register" -i
curl --data "email=zhongwx@gmail.com&password=pwd" "http://localhost:64777/api/users/login" -i
curl "http://localhost:64777/api/users" -i --cookie ".auth=5e5HJJgON2LldfFURg5zUgW%252BlNFbfN2HX9IxwweaNV78b%252BBG4Lw3QogFtbQDlNAo1j1i67V1pTAXP%252Fmfl7M27g%253D%253D;"
curl -X POST "http://localhost:64777/api/users/signout" -i
curl -X PATCH --data "password=pwd&newPassword=pwd1" "http://localhost:64777/api/users/change_password" -i --cookie ".auth=5e5HJJgON2LldfFURg5zUgW%252BlNFbfN2HX9IxwweaNV78b%252BBG4Lw3QogFtbQDlNAo1j1i67V1pTAXP%252Fmfl7M27g%253D%253D;"
curl -X GET "http://localhost:64777/api/users/activate" -i --cookie ".auth=5e5HJJgON2LldfFURg5zUgW%252BlNFbfN2HX9IxwweaNV78b%252BBG4Lw3QogFtbQDlNAo1j1i67V1pTAXP%252Fmfl7M27g%253D%253D;"
*/

namespace Yue.WebApi.Controllers
{
    [RoutePrefix("api/users")]
    public class UserController : ApiAuthorizeController
    {
        private ISequenceService _sequenceService;
        private IUserService _userService;
        private IUserSecurityService _userSecurityService;

        public UserController(IAuthenticator authenticator, 
            IActionBus actionBus,
            ISequenceService sequenceService,
            IUserService userService,
            IUserSecurityService userSecurityService) 
            : base(authenticator, actionBus)
        {
            Authenticator = authenticator;
            ActionBus = actionBus;
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
        [Route("register")]
        public async Task<IHttpActionResult> Register([FromBody]RegisterVM vm)
        {
            User user = _userService.UserByEmail(vm.Email);
            if (user != null)
            {
                return Conflict();
            }

            int userId = _sequenceService.Next(Sequence.User);
            Register action = new Register(
                userId, DateTime.Now, userId, _userSecurityService.PasswordHash(vm.Password), vm.Email, vm.Name);
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
        [Route("login")]
        public async Task<IHttpActionResult> Login([FromBody]LoginVM vm)
        {
            User user = _userService.UserByEmail(vm.Email);
            if (user == null)
            {
                return NotFound();
            }

            VerifyPassword action = new VerifyPassword(
                UserId.Value,DateTime.Now, UserId.Value, _userSecurityService.PasswordHash(vm.Password));
            ActionResponse actionResponse = await ActionBus.SendAsync<UserActionBase, VerifyPassword>(action);

            if (actionResponse.Result != ActionResponse.ActionResponseResult.OK)
            {
                return Unauthorized(new AuthenticationHeaderValue("Basic"));
            }
            var cookie = Authenticator.GetCookieTicket(user);
            HttpResponseMessage responseMsg = Request.CreateResponse<User>(HttpStatusCode.OK, user);
            responseMsg.Headers.AddCookies(new CookieHeaderValue[] { cookie });
            return ResponseMessage(responseMsg);
        }

        [HttpPost]
        [Route("signout")]
        public IHttpActionResult Signout()
        {
            HttpResponseMessage msg = Request.CreateResponse(HttpStatusCode.OK);
            var cookieValue = new CookieHeaderValue(Authenticator.CookieTicketConfig.CookieName, "");
            cookieValue.Expires = DateTime.UtcNow.AddMonths(-100);
            msg.Headers.AddCookies(new CookieHeaderValue[] { cookieValue });
            return ResponseMessage(msg);
        }

        [HttpGet]
        [Route("activate")]
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

        [HttpPatch]
        [Route("activate")]
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
            Activate action = new Activate(UserId.Value, DateTime.Now, UserId.Value, vm.Token);
            ActionResponse actionResponse = await ActionBus.SendAsync<UserActionBase, Activate>(action);
            return Ok(ActionResponseVM.ToVM(actionResponse));
        }

        [HttpGet]
        [Route("reset_password")]
        public async Task<IHttpActionResult> RequestResetPassword(RequestResetPasswordVM vm)
        {
            User user = _userService.UserByEmail(vm.Email);
            if (!user.EnsoureState(Users.Contract.UserSecurityCommand.RequestResetPasswordToken))
            {
                return Conflict();
            }
            RequestResetPasswordToken action = new RequestResetPasswordToken(
                UserId.Value, DateTime.Now, UserId.Value, Guid.NewGuid().ToString());
            ActionResponse actionResponse = await ActionBus.SendAsync<UserActionBase, RequestResetPasswordToken>(action);
            return Ok(ActionResponseVM.ToVM(actionResponse));
        }

        [HttpPost]
        [Route("reset_password")]
        public async Task<IHttpActionResult> VerifyResetPasswordToken(VerifyResetPasswordTokenVM vm)
        {
            User user = _userService.Get(vm.User);
            if (!user.EnsoureState(Users.Contract.UserSecurityCommand.VerifyResetPasswordToken))
            {
                return Conflict();
            }
            VerifyResetPasswordToken action = new VerifyResetPasswordToken(
                UserId.Value, DateTime.Now, UserId.Value, vm.Token);
            ActionResponse actionResponse = await ActionBus.SendAsync<UserActionBase, VerifyResetPasswordToken>(action);
            return Ok(ActionResponseVM.ToVM(actionResponse));
        }

        [HttpPatch]
        [Route("reset_password")]
        public async Task<IHttpActionResult> ResetPassword(ResetPasswordVM vm)
        {
            User user = _userService.Get(vm.User);
            if (!user.EnsoureState(Users.Contract.UserSecurityCommand.ResetPassword))
            {
                return Conflict();
            }
            if(vm.Reset)
            {
                ResetPassword action = new ResetPassword(
                UserId.Value, DateTime.Now, UserId.Value, _userSecurityService.PasswordHash(vm.Password), vm.Token);
                ActionResponse actionResponse = await ActionBus.SendAsync<UserActionBase, ResetPassword>(action);
                return Ok(ActionResponseVM.ToVM(actionResponse));
            }
            else
            {
                CancelResetPasswordToken action = new CancelResetPasswordToken(
                UserId.Value, DateTime.Now, UserId.Value, vm.Token);
                ActionResponse actionResponse = await ActionBus.SendAsync<UserActionBase, CancelResetPasswordToken>(action);
                return Ok(ActionResponseVM.ToVM(actionResponse));
            }
        }

        [HttpPatch]
        [Route("change_password")]
        [ApiAuthorize]
        public async Task<IHttpActionResult> ChangePassword([FromBody]ChangePasswordVM vm)
        {
            bool match = _userSecurityService.VerifyPassword(UserId.Value, vm.Password);
            if (!match)
            {
                return BadRequest();
            }
            ChangePassword action = new ChangePassword(
                UserId.Value,DateTime.Now,UserId.Value, _userSecurityService.PasswordHash(vm.NewPassword));
            ActionResponse actionResponse = await ActionBus.SendAsync<UserActionBase, ChangePassword>(action);
            return Ok(ActionResponseVM.ToVM(actionResponse));
        }
    }
}
