using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestSharp;
using Newtonsoft.Json;
using System.Net;
using System.Diagnostics;
using Yue.Bookings.View.Model;

namespace Yue.WebApi.Test
{
    [TestClass]
    public class BookingTest
    {
        private RestClient client;
        private BookingVM bookingVM;
        private SecurityTest securityTest;

        private const int TestResourceId = 2;
        private const int TestUserId = 0;

        [TestInitialize]
        public void Initialize()
        {
            securityTest = new SecurityTest();
            securityTest.Initialize();
            securityTest.RegisterLoginActivate();

            client = new RestClient("http://localhost:64777/");
            Booking();
        }

        private void Booking()
        {
            var request = new RestRequest("api/bookings");
            request.AddParameter("message", "Hello");
            request.AddParameter("from", DateTime.Now.ToString("o"));
            request.AddParameter("to", DateTime.Now.AddMinutes(30).ToString("o"));
            request.AddParameter("resource", TestResourceId);
            request.AddCookie(SecurityTest.authCookieName, securityTest.authCookieValue);
            request.Method = Method.POST;
            IRestResponse response = client.Execute(request);
            Assert.IsTrue(response.StatusCode == HttpStatusCode.Created);
            var content = response.Content;
            Trace.WriteLine(content);
            bookingVM = JsonConvert.DeserializeObject<BookingVM>(content);
        }

        [TestMethod]
        public void Get()
        {
            var request = new RestRequest("api/bookings/" + bookingVM.BookingId);
            request.AddParameter("activity", true);
            request.AddCookie(SecurityTest.authCookieName, securityTest.authCookieValue);
            request.Method = Method.GET;
            IRestResponse response = client.Execute(request);
            Assert.IsTrue(response.StatusCode == HttpStatusCode.OK);
            var content = response.Content;
            Trace.WriteLine(content);
        }

        [TestMethod]
        public void Confirm()
        {
            var request = new RestRequest("api/bookings/" + bookingVM.BookingId + "/actions/confirm");
            request.AddCookie(SecurityTest.authCookieName, securityTest.authCookieValue);
            request.Method = Method.PATCH;
            IRestResponse response = client.Execute(request);
            Assert.IsTrue(response.StatusCode == HttpStatusCode.OK);
            var content = response.Content;
            Trace.WriteLine(content);
        }

        [TestMethod]
        public void Message()
        {
            var request = new RestRequest("api/bookings/" + bookingVM.BookingId + "/actions/message");
            request.AddParameter("message", "Good Job!");
            request.AddCookie(SecurityTest.authCookieName, securityTest.authCookieValue);
            request.Method = Method.PATCH;
            IRestResponse response = client.Execute(request);
            Assert.IsTrue(response.StatusCode == HttpStatusCode.OK);
            var content = response.Content;
            Trace.WriteLine(content);
        }

        [TestMethod]
        public void ChangeTime()
        {
            var request = new RestRequest("api/bookings/" + bookingVM.BookingId + "/actions/time");
            request.AddParameter("message", "Change Time");
            request.AddParameter("from", DateTime.Now.ToString("o"));
            request.AddParameter("to", DateTime.Now.AddMinutes(30).ToString("o"));
            request.AddCookie(SecurityTest.authCookieName, securityTest.authCookieValue);
            request.Method = Method.PATCH;
            IRestResponse response = client.Execute(request);
            Assert.IsTrue(response.StatusCode == HttpStatusCode.OK);
            var content = response.Content;
            Trace.WriteLine(content);
        }

        [TestMethod]
        public void BookingsByResource()
        {
            var request = new RestRequest("api/bookings/" + bookingVM.BookingId);
            request.AddParameter("resource", TestResourceId);
            request.AddParameter("from", DateTime.Now.AddMonths(-1));
            request.AddParameter("to", DateTime.Now.AddMonths(1));
            request.AddCookie(SecurityTest.authCookieName, securityTest.authCookieValue);
            request.Method = Method.GET;
            IRestResponse response = client.Execute(request);
            Assert.IsTrue(response.StatusCode == HttpStatusCode.OK);
            var content = response.Content;
            Trace.WriteLine(content);
        }

        [TestMethod]
        public void BookingsByUser()
        {
            var request = new RestRequest("api/bookings/" + bookingVM.BookingId);
            request.AddParameter("user", TestUserId);
            request.AddParameter("from", DateTime.Now.AddMonths(-1));
            request.AddParameter("to", DateTime.Now.AddMonths(1));
            request.AddCookie(SecurityTest.authCookieName, securityTest.authCookieValue);
            request.Method = Method.GET;
            IRestResponse response = client.Execute(request);
            Assert.IsTrue(response.StatusCode == HttpStatusCode.OK);
            var content = response.Content;
            Trace.WriteLine(content);
        }

        [TestCleanup]
        public void Delete()
        {
            var request = new RestRequest("api/bookings/" + bookingVM.BookingId);
            request.AddCookie(SecurityTest.authCookieName, securityTest.authCookieValue);
            request.Method = Method.DELETE;
            IRestResponse response = client.Execute(request);
            var content = response.Content;
            Trace.WriteLine(content);
        }
    }
}
