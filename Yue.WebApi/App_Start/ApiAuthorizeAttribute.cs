using AppHarbor.Web.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http.Controllers;

namespace Yue.WebApi
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class ApiAuthorizeAttribute : System.Web.Http.AuthorizeAttribute
    {
        public const string RenewAuthorizationCookieKey = "RenewAuthorizationCookie";
        private readonly ICookieAuthenticationConfiguration _configuration;

		public ApiAuthorizeAttribute()
			: this(new ConfigFileAuthenticationConfiguration())
		{
		}

        public ApiAuthorizeAttribute(ICookieAuthenticationConfiguration configuration)
		{
			_configuration = configuration;
		}

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            var cookie = actionContext.Request.Headers.GetCookies().Select(c => c[_configuration.CookieName]).FirstOrDefault();

            if (cookie != null)
            {
                var protector = new CookieProtector(_configuration);
                try
                {
                    byte[] data;
                    var cookieData = protector.Validate(System.Net.WebUtility.UrlDecode(cookie.Value), out data);
                    var authenticationCookie = AuthenticationTicket.Deserialize(data);
                    if (!authenticationCookie.IsExpired(_configuration.Timeout))
                    {
                        actionContext.RequestContext.Principal = authenticationCookie.GetPrincipal();
                        RenewCookieIfExpiring(actionContext, protector, authenticationCookie);
                        return;
                    }
                }
                catch
                {
                }
                finally
                {
                    if (protector != null)
                    {
                        protector.Dispose();
                    }
                }
            }

            var message = new HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized);
            actionContext.Response = message;
        }

        private void RenewCookieIfExpiring(HttpActionContext context, CookieProtector protector, AuthenticationTicket authenticationCookie)
        {
            if (!_configuration.SlidingExpiration || !authenticationCookie.IsExpired(TimeSpan.FromTicks(_configuration.Timeout.Ticks / 2)))
            {
                return;
            }
            authenticationCookie.Renew();

            CookieHeaderValue cookie = new CookieHeaderValue(_configuration.CookieName,
                        WebUtility.UrlEncode(protector.Protect(authenticationCookie.Serialize())));
            cookie.HttpOnly = true;
            cookie.Secure = _configuration.RequireSSL;
            cookie.Path = "/";
            if (authenticationCookie.Persistent)
            {
                cookie.Expires = authenticationCookie.IssueDate + _configuration.Timeout;
            }
            context.Request.Properties.Add(RenewAuthorizationCookieKey, cookie);
        }
    }
}