﻿using ACE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace Yue.WebApi.Controllers
{
    public class ApiAuthorizeController : ApiController
    {
        public IAuthenticator Authenticator { get; protected set; }
        public IActionBus ActionBus { get; protected set; }
        public int? UserId { get; set; }

        public ApiAuthorizeController(IAuthenticator authenticator,
             IActionBus actionBus)
        {
            this.Authenticator = authenticator;
            this.ActionBus = actionBus;
        }

        public override async Task<HttpResponseMessage> ExecuteAsync(HttpControllerContext controllerContext, CancellationToken cancellationToken)
        {
            var httpResponseMessage = await base.ExecuteAsync(controllerContext, cancellationToken);
            object renewAuthorizationCookie;
            if (controllerContext.Request.Properties.TryGetValue(ApiAuthorizeAttribute.RenewAuthorizationCookieToken, out renewAuthorizationCookie))
            {
                httpResponseMessage.Headers.AddCookies(new CookieHeaderValue[] { (CookieHeaderValue)renewAuthorizationCookie });
            }
            return httpResponseMessage;
        }
    }
}