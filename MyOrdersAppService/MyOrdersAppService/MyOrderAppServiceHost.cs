using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin.Hosting;
using MyOrdersAppService.Properties;

namespace MyOrdersAppService
{
    class MyOrderAppServiceHost
    {
        private static IDisposable _server;

        public void Start()
        {
            _server = WebApp.Start<Startup>(Settings.Default.SelfHostBaseUrl);
        }

        public void Stop()
        {
            _server?.Dispose();
        }
    }
}
