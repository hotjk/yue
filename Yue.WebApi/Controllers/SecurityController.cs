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
using Yue.Users.Contract;
using Yue.Users.Contract.Actions;
using Yue.Users.Contract.Events;
using Yue.Users.Model;
using Yue.Users.View.Model;

/*
 * Register
 * curl --data "email=zhongwx@gmail.com&name=weixiao&password=pwd" "http://localhost:64777/api/security/actions/register" -i
 * 
 * Login
 * curl --data "email=zhongwx@gmail.com&password=pwd" "http://localhost:64777/api/users/actions/login" -i
 * 
 * Sign Out
 * curl -X POST --data "" "http://localhost:64777/api/users/signout" -i
 * 
 * Request Activate Code
 * curl -X POST "http://localhost:64777/api/users/actions/activate" -i --cookie ".auth=dAN8etO20uU7DSCsLRStv1oSPMV0pt1xxhJhceYPTmvGVUhKs5ovqTAlgwz8g3FfDGyvGlqz7CuYV%252BE7bLQlog%253D%253D;"
 * 
 * Activate
 * curl -X POST --data "user=33&token=5f162f8d-009e-4d3a-8f6c-21b99deb1550" "http://localhost:64777/api/users/activate" -i
 * 
 * Change Password
 * curl -X POST --data "password=pwd&newPassword=pwd1" "http://localhost:64777/api/users/change_password" -i --cookie ".auth=oebk2ctBVEwo4gC4AoYRArfBGR0nmN7PyB4KdVZLXKCRxsDKJCCaJ7nBi77IBHRWj7ycGvk6yCqbTRFlMbw36w%253D%253D;"
 * 
 * Request Reset Password Token
 * curl -X POST --data "email=zhongwx@gmail.com" "http://localhost:64777/api/password/actions/token" -i 
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
    [RoutePrefix("api/security")]
    public class SecurityController : ApiAuthorizeController
    {
        private IEventBus EventBus;
        private ISequenceService _sequenceService;
        private IUserSecurityService _userSecurityService;

        public SecurityController(IAuthenticator authenticator, 
            IActionBus actionBus,
            IEventBus eventBus,
            ISequenceService sequenceService,
            IUserSecurityService userSecurityService) 
            : base(authenticator, actionBus)
        {
            EventBus = eventBus;
            _sequenceService = sequenceService;
            _userSecurityService = userSecurityService;
        }

        [HttpPost]
        [Route("actions/register")]
        public async Task<IHttpActionResult> Register(RegisterVM vm)
        {
            var userSecruity = _userSecurityService.UserSecurityByEmail(vm.Email);
            if (userSecruity != null)
            {
                return Conflict();
            }

            int userId = _sequenceService.Next(Sequence.User);
            Register action = new Register(
                userId, DateTime.Now, userId, _userSecurityService.PasswordHash(userId, vm.Password), vm.Email, vm.Name);
            ActionResponse actionResponse = await ActionBus.SendAsync<UserActionBase, Register>(action);
            return Ok(ActionResponseVM.ToVM(actionResponse));
        }

        [HttpPost]
        [Route("actions/login")]
        public IHttpActionResult Login(LoginVM vm)
        {
            var userSecruity = _userSecurityService.UserSecurityByEmail(vm.Email);
            if (userSecruity == null)
            {
                return NotFound();
            }
            if (!userSecruity.EnsoureState(Users.Contract.UserSecurityCommand.VerifyPassword))
            {
                return Conflict();
            }

            bool match = _userSecurityService.VerifyPassword(userSecruity.UserId, vm.Password);
            EventBus.FlushAnEvent(new UserPasswordVerified(userSecruity.UserId, DateTime.Now, userSecruity.UserId, match));

            if (!match)
            {
                return Unauthorized(new AuthenticationHeaderValue("Basic"));
            }

            var cookie = Authenticator.GetCookieTicket(userSecruity.UserId.ToString());
            HttpResponseMessage msg = Request.CreateResponse(HttpStatusCode.OK);
            msg.Headers.AddCookies(new CookieHeaderValue[] { cookie });
            return ResponseMessage(msg);
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
            var userSecruity = _userSecurityService.Get(UserId.Value);
            if (!userSecruity.EnsoureState(Users.Contract.UserSecurityCommand.RequestActivateToken))
            {
                return Conflict();
            }
            RequestActivateToken action = new RequestActivateToken(
                userSecruity.UserId, DateTime.Now, userSecruity.UserId, Guid.NewGuid().ToString());
            ActionResponse actionResponse = await ActionBus.SendAsync<UserActionBase, RequestActivateToken>(action);
            return Ok(ActionResponseVM.ToVM(actionResponse));
        }

        [HttpPost]
        [Route("actions/activate")]
        public async Task<IHttpActionResult> Activite(ActivateVM vm)
        {
            var userSecruity = _userSecurityService.Get(UserId.Value);
            if (!userSecruity.EnsoureState(Users.Contract.UserSecurityCommand.RequestActivateToken))
            {
                return Conflict();
            }
            if (!userSecruity.EnsoureState(Users.Contract.UserSecurityCommand.RequestActivateToken))
            {
                return Conflict();
            }
            Activate action = new Activate(userSecruity.UserId, DateTime.Now, userSecruity.UserId, vm.Token);
            ActionResponse actionResponse = await ActionBus.SendAsync<UserActionBase, Activate>(action);
            return Ok(ActionResponseVM.ToVM(actionResponse));
        }

        [HttpPost]
        [Route("actions/token")]
        public async Task<IHttpActionResult> RequestResetPasswordToken(RequestResetPasswordVM vm)
        {
            var userSecurity = _userSecurityService.UserSecurityByEmail(vm.Email);
            if (!userSecurity.EnsoureState(UserSecurityCommand.RequestResetPasswordToken))
            {
                return Conflict();
            }
            RequestResetPasswordToken action = new RequestResetPasswordToken(
                userSecurity.UserId, DateTime.Now, userSecurity.UserId, Guid.NewGuid().ToString());
            ActionResponse actionResponse = await ActionBus.SendAsync<UserActionBase, RequestResetPasswordToken>(action);
            return Ok(ActionResponseVM.ToVM(actionResponse));
        }

        [HttpPost]
        [Route("actions/verify")]
        public async Task<IHttpActionResult> VerifyResetPasswordToken(VerifyResetPasswordTokenVM vm)
        {
            var userSecurity = _userSecurityService.Get(vm.User);
            if (!userSecurity.EnsoureState(UserSecurityCommand.VerifyResetPasswordToken))
            {
                return Conflict();
            }
            VerifyResetPasswordToken action = new VerifyResetPasswordToken(
                userSecurity.UserId, DateTime.Now, userSecurity.UserId, vm.Token);
            ActionResponse actionResponse = await ActionBus.SendAsync<UserActionBase, VerifyResetPasswordToken>(action);
            return Ok(ActionResponseVM.ToVM(actionResponse));
        }

        [HttpPost]
        [Route("actions/cancel")]
        public async Task<IHttpActionResult> CancelResetPasswordToken(VerifyResetPasswordTokenVM vm)
        {
            var userSecurity = _userSecurityService.Get(vm.User);
            if (!userSecurity.EnsoureState(UserSecurityCommand.CancelResetPasswordToken))
            {
                return Conflict();
            }
            CancelResetPasswordToken action = new CancelResetPasswordToken(
                userSecurity.UserId, DateTime.Now, userSecurity.UserId, vm.Token);
            ActionResponse actionResponse = await ActionBus.SendAsync<UserActionBase, CancelResetPasswordToken>(action);
            return Ok(ActionResponseVM.ToVM(actionResponse));
        }

        [HttpPost]
        [Route("actions/reset")]
        public async Task<IHttpActionResult> ResetPassword(ResetPasswordVM vm)
        {
            var userSecurity = _userSecurityService.Get(vm.User);
            if (!userSecurity.EnsoureState(UserSecurityCommand.ResetPassword))
            {
                return Conflict();
            }

            ResetPassword action = new ResetPassword(
            userSecurity.UserId, DateTime.Now, userSecurity.UserId, _userSecurityService.PasswordHash(userSecurity.UserId, vm.Password), vm.Token);
            ActionResponse actionResponse = await ActionBus.SendAsync<UserActionBase, ResetPassword>(action);
            return Ok(ActionResponseVM.ToVM(actionResponse));
        }

        [HttpPost]
        [Route("actions/change")]
        [ApiAuthorize]
        public async Task<IHttpActionResult> ChangePassword(ChangePasswordVM vm)
        {
            bool match = _userSecurityService.VerifyPassword(UserId.Value, vm.Password);
            if (!match)
            {
                return BadRequest();
            }
            ChangePassword action = new ChangePassword(
                UserId.Value,DateTime.Now,UserId.Value, _userSecurityService.PasswordHash(UserId.Value, vm.NewPassword));
            ActionResponse actionResponse = await ActionBus.SendAsync<UserActionBase, ChangePassword>(action);
            return Ok(ActionResponseVM.ToVM(actionResponse));
        }
    }
}
