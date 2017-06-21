using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topshelf;

namespace OrdersService
{
    class Program
    {
        static void Main(string[] args)
        {
            HostFactory.Run(x =>
            {
                x.Service<OrdersServiceHost>(s =>
                {
                    s.ConstructUsing(name => new OrdersServiceHost());
                    s.WhenStarted(tc => tc.Start());
                    s.WhenStopped(tc => tc.Stop());
                });

                x.RunAsNetworkService();

                x.SetDescription("My super-duper Orders Service");
                x.SetServiceName("OrdersService");
                x.SetDisplayName("Orders Service");
            });
        }
    }
}
