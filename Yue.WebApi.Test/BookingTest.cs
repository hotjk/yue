using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestSharp;
using Newtonsoft.Json;
using Yue.Bookings.Model;
using System.Net;

namespace Yue.WebApi.Test
{
    [TestClass]
    public class BookingTest
    {
        private RestClient client;
        private BookingVM bookingVM;

        [TestInitialize]
        public void Initialize()
        {
            client = new RestClient("http://localhost:64777/");
            Booking();
        }

        private void Booking()
        {
            var request = new RestRequest("api/bookings");
            request.AddParameter("message", "Hello");
            request.AddParameter("from", DateTime.Now.ToString("o"));
            request.AddParameter("to", DateTime.Now.AddMinutes(30).ToString("o"));
            request.AddParameter("resource", 2);
            request.Method = Method.POST;
            IRestResponse response = client.Execute(request);
            Assert.IsTrue(response.StatusCode == HttpStatusCode.Created);
            var content = response.Content;
            Console.WriteLine(content);
            bookingVM = JsonConvert.DeserializeObject<BookingVM>(content);
        }

        [TestMethod]
        public void Get()
        {
            var request = new RestRequest("api/bookings/" + bookingVM.BookingId);
            request.AddParameter("activity", true);
            request.Method = Method.GET;
            IRestResponse response = client.Execute(request);
            Assert.IsTrue(response.StatusCode == HttpStatusCode.OK);
            var content = response.Content;
            Console.WriteLine(content);
        }

        [TestMethod]
        public void Confirm()
        {
            var request = new RestRequest("api/bookings/" + bookingVM.BookingId + "/actions/confirm");
            request.Method = Method.PATCH;
            IRestResponse response = client.Execute(request);
            Assert.IsTrue(response.StatusCode == HttpStatusCode.OK);
            var content = response.Content;
            Console.WriteLine(content);
        }

        [TestMethod]
        public void Message()
        {
            var request = new RestRequest("api/bookings/" + bookingVM.BookingId + "/actions/message");
            request.AddParameter("message", "Good Job!");
            request.Method = Method.PATCH;
            IRestResponse response = client.Execute(request);
            Assert.IsTrue(response.StatusCode == HttpStatusCode.OK);
            var content = response.Content;
            Console.WriteLine(content);
        }

        [TestMethod]
        public void ChangeTime()
        {
            var request = new RestRequest("api/bookings/" + bookingVM.BookingId + "/actions/time");
            request.AddParameter("message", "Change Time");
            request.AddParameter("from", DateTime.Now.ToString("o"));
            request.AddParameter("to", DateTime.Now.AddMinutes(30).ToString("o"));
            request.Method = Method.PATCH;
            IRestResponse response = client.Execute(request);
            Assert.IsTrue(response.StatusCode == HttpStatusCode.OK);
            var content = response.Content;
            Console.WriteLine(content);
        }

        [TestMethod]
        public void BookingsByResource()
        {
            var request = new RestRequest("api/bookings/" + bookingVM.BookingId);
            request.AddParameter("resource", 1);
            request.AddParameter("from", DateTime.Now.AddMonths(-1));
            request.AddParameter("to", DateTime.Now.AddMonths(1));
            request.Method = Method.GET;
            IRestResponse response = client.Execute(request);
            Assert.IsTrue(response.StatusCode == HttpStatusCode.OK);
            var content = response.Content;
            Console.WriteLine(content);
        }

        [TestMethod]
        public void BookingsByUser()
        {
            var request = new RestRequest("api/bookings/" + bookingVM.BookingId);
            request.AddParameter("user", 0);
            request.AddParameter("from", DateTime.Now.AddMonths(-1));
            request.AddParameter("to", DateTime.Now.AddMonths(1));
            request.Method = Method.GET;
            IRestResponse response = client.Execute(request);
            Assert.IsTrue(response.StatusCode == HttpStatusCode.OK);
            var content = response.Content;
            Console.WriteLine(content);
        }

        [TestCleanup]
        public void Delete()
        {
            var request = new RestRequest("api/bookings/" + bookingVM.BookingId);
            request.Method = Method.DELETE;
            IRestResponse response = client.Execute(request);
            var content = response.Content;
            Console.WriteLine(content);
        }
    }
}
