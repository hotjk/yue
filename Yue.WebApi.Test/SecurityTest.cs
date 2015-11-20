using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestSharp;
using Newtonsoft.Json;
using System.Net;
using System.Diagnostics;
using Grit.Utility.Security;
using System.Linq;

namespace Yue.WebApi.Test
{
    [TestClass]
    public class SecurityTest
    {
        private RestClient client;
        private string email;
        private string password;
        public const string authCookieName = ".auth";
        public string authCookieValue { get; private set; }

        [TestInitialize]
        public void Initialize()
        {
            client = new RestClient("http://localhost/Yue.WebApi/");
            Register();
        }

        private void Register()
        {
            email = RandomText.Generate(10) + "@" + RandomText.Generate(6) + ".com";
            password = RandomText.Generate(10);
            var request = new RestRequest("api/security/actions/register");
            request.AddParameter("email", email);
            request.AddParameter("name", RandomText.Generate(10));
            request.AddParameter("password", password);
            request.Method = Method.POST;
            IRestResponse response = client.Execute(request);
            Assert.IsTrue(response.StatusCode == HttpStatusCode.OK);
            var content = response.Content;
            Trace.WriteLine(content);
        }

        private void Login()
        {
            var request = new RestRequest("api/security/actions/login");
            request.AddParameter("email", email);
            request.AddParameter("password", password);
            request.Method = Method.POST;
            IRestResponse response = client.Execute(request);
            Assert.IsTrue(response.StatusCode == HttpStatusCode.OK);
            authCookieValue = response.Cookies.FirstOrDefault(n => n.Name == authCookieName).Value;
            Assert.IsNotNull(authCookieValue);
            var content = response.Content;
            Trace.WriteLine(content);
        }

        private void Signout()
        {
            var request = new RestRequest("api/security/actions/signout");
            request.Method = Method.POST;
            IRestResponse response = client.Execute(request);
            Assert.IsTrue(response.StatusCode == HttpStatusCode.OK);
            var content = response.Content;
            Trace.WriteLine(content);
        }

        private void ActivateToken()
        {
            var request = new RestRequest("api/security/actions/activate_token");
            request.AddCookie(authCookieName, authCookieValue);
            request.Method = Method.POST;
            IRestResponse response = client.Execute(request);
            Assert.IsTrue(response.StatusCode == HttpStatusCode.OK);
            var content = response.Content;
            Trace.WriteLine(content);
        }

        [TestMethod]
        public void RegisterLoginSignout()
        {
            Register();
            Login();
            Signout();
        }

        [TestMethod]
        public void RegisterLoginActivate()
        {
            Register();
            Login();
            ActivateToken();
        }
    }
}
