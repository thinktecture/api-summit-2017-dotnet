using System;
using Nanophone.Core;

namespace OrdersService.Infrastructure
{
    public class CustomWebApiRegistryTenant : IRegistryTenant
    {
        public Uri Uri { get; }

        public CustomWebApiRegistryTenant(Uri uri)
        {
            Uri = uri;
        }
    }
}
