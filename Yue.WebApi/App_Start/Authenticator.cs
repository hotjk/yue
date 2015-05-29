using Grit.Utility.Authentication;
using Grit.Utility.Security;
using System;
using System.Net;
using System.Net.Http.Headers;
using Yue.Users.Model;

namespace Yue.WebApi
{
    public class Authenticator : IAuthenticator
    {
        public ICookieTicketConfig CookieTicketConfig { get; private set; }
        public Authenticator(ICookieTicketConfig cookieTicketConfig)
        {
            CookieTicketConfig = cookieTicketConfig;
        }

        public CookieHeaderValue GetCookieTicket(string content)
        {
            var cookie = new CookieTicket(content);
            using (var protector = new EncryptSignManager(
                CookieTicketConfig.EncryptionKey,
                CookieTicketConfig.EncryptionIV,
                CookieTicketConfig.ValidationKey))
            {
                var encrypted = Convert.ToBase64String(protector.EncryptThenSign(cookie.Serialize()));
                CookieHeaderValue cookieValue = new CookieHeaderValue(CookieTicketConfig.CookieName,
                    WebUtility.UrlEncode(encrypted));
                cookieValue.HttpOnly = true;
                cookieValue.Secure = CookieTicketConfig.RequireSSL;
                cookieValue.Path = "/";
                if (cookie.Persistent)
                {
                    cookieValue.Expires = cookie.IssueDate + CookieTicketConfig.Timeout;
                }
                return cookieValue;
            }
        }

        public bool ValidateCookieTicket(CookieState cookie, out CookieTicket ticket, out CookieHeaderValue renewCookie)
        {
            if (cookie != null)
            {
                var protector = new EncryptSignManager(CookieTicketConfig.EncryptionKey, CookieTicketConfig.EncryptionIV, CookieTicketConfig.ValidationKey);
                try
                {
                    byte[] data;
                    var cookieData = protector.ValidateThenDecrypt(Convert.FromBase64String(System.Net.WebUtility.UrlDecode(cookie.Value)), out data);
                    ticket = CookieTicket.Deserialize(data);
                    if (!ticket.IsExpired(CookieTicketConfig.Timeout))
                    {
                        if (!CookieTicketConfig.SlidingExpiration || !ticket.IsExpired(TimeSpan.FromTicks(CookieTicketConfig.Timeout.Ticks / 2)))
                        {
                            renewCookie = null;
                            return true;
                        }
                        ticket.Renew();

                        var encrypted = Convert.ToBase64String(protector.EncryptThenSign(ticket.Serialize()));
                        CookieHeaderValue cookieValue = new CookieHeaderValue(CookieTicketConfig.CookieName,
                                    WebUtility.UrlEncode(encrypted));

                        cookieValue.HttpOnly = true;
                        cookieValue.Secure = CookieTicketConfig.RequireSSL;
                        cookieValue.Path = CookieTicketConfig.CookiePath;
                        if (ticket.Persistent)
                        {
                            cookieValue.Expires = ticket.IssueDate + CookieTicketConfig.Timeout;
                        }

                        renewCookie = cookieValue;
                        return true;
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

            renewCookie = null;
            ticket = null;
            return false;
        }
    }
}