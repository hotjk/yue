using Grit.Utility.Authentication;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using Yue.WebApi.Controllers;

namespace Yue.WebApi
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class ApiAuthorizeAttribute : System.Web.Http.AuthorizeAttribute
    {
        public const string RenewAuthorizationCookieToken = "RenewAuthorizationCookieToken";

        public override void OnAuthorization(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            bool authorized = false;
            var controller = actionContext.ControllerContext.Controller as ApiAuthorizeController;
            if (controller != null)
            {
                var cookie = actionContext.Request.Headers.GetCookies().Select(
                c => c[controller.Authenticator.CookieTicketConfig.CookieName]).FirstOrDefault();

                CookieHeaderValue cookieValue;
                CookieTicket ticket;
                if (controller.Authenticator.ValidateCookieTicket(cookie, out ticket, out cookieValue))
                {
                    authorized = true;
                    controller.UserId = int.Parse(ticket.Name);
                    if (cookieValue != null)
                    {
                        actionContext.Request.Properties.Add(RenewAuthorizationCookieToken, cookieValue);
                    }
                }
            }
            if (!authorized)
            {
                var message = new HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized);
                actionContext.Response = message;
            }
        }
    }
}