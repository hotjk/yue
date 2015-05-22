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
 * curl --data "email=zhongwx@gmail.com&password=pwd" "http://localhost:64777/api/users/login" -i
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
    [RoutePrefix("api/password")]
    public class PasswordController : ApiAuthorizeController
    {
        private IEventBus EventBus;
        private ISequenceService _sequenceService;
        private IUserService _userService;
        private IUserSecurityService _userSecurityService;

        public PasswordController(IAuthenticator authenticator, 
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

        [HttpPost]
        [Route("actions/token")]
        public async Task<IHttpActionResult> RequestResetPasswordToken(RequestResetPasswordVM vm)
        {
            User user = _userService.UserByEmail(vm.Email);
            if (!user.EnsoureState(Users.Contract.UserSecurityCommand.RequestResetPasswordToken))
            {
                return Conflict();
            }
            RequestResetPasswordToken action = new RequestResetPasswordToken(
                user.UserId, DateTime.Now, user.UserId, Guid.NewGuid().ToString());
            ActionResponse actionResponse = await ActionBus.SendAsync<UserActionBase, RequestResetPasswordToken>(action);
            return Ok(ActionResponseVM.ToVM(actionResponse));
        }

        [HttpPost]
        [Route("actions/verify")]
        public async Task<IHttpActionResult> VerifyResetPasswordToken(VerifyResetPasswordTokenVM vm)
        {
            User user = _userService.Get(vm.User);
            if (!user.EnsoureState(Users.Contract.UserSecurityCommand.VerifyResetPasswordToken))
            {
                return Conflict();
            }
            VerifyResetPasswordToken action = new VerifyResetPasswordToken(
                user.UserId, DateTime.Now, user.UserId, vm.Token);
            ActionResponse actionResponse = await ActionBus.SendAsync<UserActionBase, VerifyResetPasswordToken>(action);
            return Ok(ActionResponseVM.ToVM(actionResponse));
        }

        [HttpPost]
        [Route("actions/cancel")]
        public async Task<IHttpActionResult> CancelResetPasswordToken(VerifyResetPasswordTokenVM vm)
        {
            User user = _userService.Get(vm.User);
            if (!user.EnsoureState(Users.Contract.UserSecurityCommand.CancelResetPasswordToken))
            {
                return Conflict();
            }
            CancelResetPasswordToken action = new CancelResetPasswordToken(
                user.UserId, DateTime.Now, user.UserId, vm.Token);
            ActionResponse actionResponse = await ActionBus.SendAsync<UserActionBase, CancelResetPasswordToken>(action);
            return Ok(ActionResponseVM.ToVM(actionResponse));
        }

        [HttpPost]
        [Route("actions/reset")]
        public async Task<IHttpActionResult> ResetPassword(ResetPasswordVM vm)
        {
            User user = _userService.Get(vm.User);
            if (!user.EnsoureState(Users.Contract.UserSecurityCommand.ResetPassword))
            {
                return Conflict();
            }
            ResetPassword action = new ResetPassword(
            user.UserId, DateTime.Now, user.UserId, _userSecurityService.PasswordHash(user.UserId, vm.Password), vm.Token);
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
