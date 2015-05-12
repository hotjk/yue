using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Security;

namespace Yue.WebApi
{
    public static class AuthenticationHelper
    {
        public static CookieHeaderValue GetAuthCookie(string name)
        {
            var cookie = FormsAuthentication.GetAuthCookie(name, false);
            var ticket = FormsAuthentication.Decrypt(cookie.Value);

            var newTicket = new FormsAuthenticationTicket(ticket.Version, ticket.Name, ticket.IssueDate, ticket.Expiration,
                ticket.IsPersistent, name, ticket.CookiePath);
            var encTicket = FormsAuthentication.Encrypt(newTicket);

            CookieHeaderValue cookieHeaderValue = new CookieHeaderValue(cookie.Name, encTicket);
            cookieHeaderValue.Domain = cookie.Domain;
            cookieHeaderValue.Expires = DateTimeOffset.Now.AddDays(1);
            cookieHeaderValue.Path = cookie.Path;
            return cookieHeaderValue;
        }

        public static int? UserId()
        {
            if (HttpContext.Current.Request.IsAuthenticated)
            {
                FormsIdentity formIdentity = HttpContext.Current.User.Identity as FormsIdentity;
                FormsAuthenticationTicket ticket = formIdentity.Ticket as FormsAuthenticationTicket;
                string userData = ticket.UserData;
                return int.Parse(userData);
            }
            return null;
        }
    }
}