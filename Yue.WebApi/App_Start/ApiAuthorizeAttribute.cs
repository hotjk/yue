using Grit.Utility.Authentication;
using Grit.Utility.Security;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.Http.Controllers;

namespace Yue.WebApi
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class ApiAuthorizeAttribute : System.Web.Http.AuthorizeAttribute
    {
        public const string RenewAuthorizationCookieToken = "RenewAuthorizationCookieToken";

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            var cookie = actionContext.Request.Headers.GetCookies().Select(
                c => c[BootStrapper.Authenticator.CookieTicketConfig.CookieName]).FirstOrDefault();

            CookieHeaderValue cookieValue;
            if(!BootStrapper.Authenticator.ValidateCookieTicket(cookie, out cookieValue))
            {
                var message = new HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized);
                actionContext.Response = message;
                return;
            }
            if (cookieValue != null)
            {
                actionContext.Request.Properties.Add(RenewAuthorizationCookieToken, cookieValue);
            }
        }
    }
}