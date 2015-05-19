using Grit.Utility.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Web;
using Yue.Users.Model;

namespace Yue.WebApi
{
    public interface IAuthenticator
    {
        ICookieTicketConfig CookieTicketConfig { get; }
        CookieHeaderValue GetCookieTicket(User user);
        bool ValidateCookieTicket(CookieState cookie, out CookieTicket ticket, out CookieHeaderValue renewCookie);
    }
}