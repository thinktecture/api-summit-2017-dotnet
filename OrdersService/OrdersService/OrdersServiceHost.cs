using System;
using Microsoft.Owin;
using Microsoft.Owin.Hosting;
using Nanophone.Core;
using Nanophone.RegistryHost.ConsulRegistry;
using OrdersService.Infrastructure;
using OrdersService.Properties;

namespace OrdersService
{
    public class OrdersServiceHost
    {
        private static IDisposable _server;
        private static ServiceRegistry _registry;
        private static RegistryInformation _registryInformation;

        public void Start()
        {
            var hostBaseUrl = Settings.Default.SelfHostBaseUrl;
            var webApiBaseUrl = Settings.Default.WebApiBaseUrl;
            var healthUrl = Settings.Default.WebApiHealthUrl;

            StartWebApi(hostBaseUrl, webApiBaseUrl, healthUrl);
        }

        private static async void StartWebApi(string hostBaseUrl, string webApiBaseUrl, string healthUrl)
        {
            var registryHost = new ConsulRegistryHost();
            _registry = new ServiceRegistry(registryHost);

            _registryInformation = await _registry.AddTenantAsync(
                new CustomWebApiRegistryTenant(new Uri(webApiBaseUrl)),"orders", "0.0.1", new Uri(healthUrl));

            _server = WebApp.Start<Startup>(hostBaseUrl);
        }

        public async void Stop()
        {
            await _registry.DeregisterServiceAsync(_registryInformation.Id);
            _server?.Dispose();
        }
    }
}