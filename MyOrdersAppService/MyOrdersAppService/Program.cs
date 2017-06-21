using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topshelf;

namespace MyOrdersAppService
{
    class Program
    {
        static void Main(string[] args)
        {
            HostFactory.Run(x =>
            {
                x.Service<MyOrderAppServiceHost>(s =>
                {
                    s.ConstructUsing(name => new MyOrderAppServiceHost());
                    s.WhenStarted(tc => tc.Start());
                    s.WhenStopped(tc => tc.Stop());
                });
                x.RunAsNetworkService();

                x.SetDescription("MyOrders App Service");
                x.SetDisplayName("MyOrders App Service");
                x.SetServiceName("MyOrdersAppService");
            });

            Console.ReadLine();
        }
    }


}
