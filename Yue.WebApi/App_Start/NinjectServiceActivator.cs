using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;

namespace Yue.WebApi
{
    public class NinjectServiceActivator : IHttpControllerActivator
    {
        private IKernel _container;
        public NinjectServiceActivator(HttpConfiguration configuration, IKernel container) 
        {
            _container = container;
        }

        public IHttpController Create(HttpRequestMessage request, HttpControllerDescriptor controllerDescriptor, Type controllerType)
        {
            var controller = _container.GetService(controllerType) as IHttpController;
            return controller;
        }
    }
}