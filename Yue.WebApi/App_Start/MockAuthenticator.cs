using Grit.Utility.Authentication;
using Grit.Utility.Security;
using System;
using System.Net;
using System.Net.Http.Headers;
using Yue.Users.Model;

namespace Yue.WebApi
{
    public class MockAuthenticator : IAuthenticator
    {
        public ICookieTicketConfig CookieTicketConfig { get; private set; }
        private int _userId;
        public MockAuthenticator(ICookieTicketConfig cookieTicketConfig, int userId)
        {
            CookieTicketConfig = cookieTicketConfig;
            _userId = userId;
        }

        public CookieHeaderValue GetCookieTicket(User user)
        {
            var cookie = new CookieTicket(user.UserId.ToString());
            CookieHeaderValue cookieValue = new CookieHeaderValue(CookieTicketConfig.CookieName, "Just Mock");
            cookieValue.HttpOnly = true;
            cookieValue.Secure = CookieTicketConfig.RequireSSL;
            cookieValue.Path = "/";
            if (cookie.Persistent)
            {
                cookieValue.Expires = cookie.IssueDate + CookieTicketConfig.Timeout;
            }
            return cookieValue;
        }

        public bool ValidateCookieTicket(CookieState cookie, out CookieTicket ticket, out CookieHeaderValue renewCookie)
        {
            ticket = new CookieTicket(_userId.ToString());
            renewCookie = null;
            return true;
        }
    }
}