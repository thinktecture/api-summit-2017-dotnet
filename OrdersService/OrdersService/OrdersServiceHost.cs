using System;
using AutoMapper;
using EasyNetQ;
using EasyNetQ.Topology;
using Microsoft.Owin;
using Microsoft.Owin.Hosting;
using Nanophone.Core;
using Nanophone.RegistryHost.ConsulRegistry;
using OrdersService.Infrastructure;
using OrdersService.Properties;
using QueuingMessages;
using Order = OrdersService.DTOs.Order;
using OrderItem = OrdersService.DTOs.OrderItem;

namespace OrdersService
{
    public class OrdersServiceHost
    {
        private static IDisposable _server;
        private static ServiceRegistry _registry;
        private static RegistryInformation _registryInformation;
        private static IBus _bus;

        public void Start()
        {
            var hostBaseUrl = Settings.Default.SelfHostBaseUrl;
            var webApiBaseUrl = Settings.Default.WebApiBaseUrl;
            var healthUrl = Settings.Default.WebApiHealthUrl;

            InitializeMapper();
            SetupQueues();
            ListenOnQueues();

            StartWebApi(hostBaseUrl, webApiBaseUrl, healthUrl);
        }

        private void ListenOnQueues()
        {
            _bus = RabbitHutch.CreateBus(Settings.Default.RabbitMQConnectionString);

            _bus.Subscribe<ShippingCreatedMessage>("shipping", msg =>
            {
                Console.WriteLine("###Shipping created: " + msg.Created + " for " + msg.OrderId);
            });
        }

        private void SetupQueues()
        {
            using (var advancedBus = RabbitHutch.CreateBus(Settings.Default.RabbitMQConnectionString).Advanced)
            {
                var newOrderQueue = advancedBus.QueueDeclare("QueuingMessages.NewOrderMessage:QueuingMessages_shipping");
                var newOrderExchange = advancedBus.ExchangeDeclare("QueuingMessages.NewOrderMessage:QueuingMessages", ExchangeType.Topic);
                advancedBus.Bind(newOrderExchange, newOrderQueue, String.Empty);

                var shippingCreatedQueue = advancedBus.QueueDeclare("QueuingMessages.ShippingCreatedMessage:QueuingMessages_shipping");
                var shippingCreatedExchange = advancedBus.ExchangeDeclare("QueuingMessages.ShippingCreatedMessage:QueuingMessages", ExchangeType.Topic);
                advancedBus.Bind(shippingCreatedExchange, shippingCreatedQueue, String.Empty);
            }

        }

        private static void InitializeMapper()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<OrderItem, QueuingMessages.OrderItem>();
                cfg.CreateMap<Order, QueuingMessages.Order>();
                //.ForMember(d => d.Items, opt => opt.MapFrom(s => s.Items));
            });
            
            Mapper.AssertConfigurationIsValid();
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

            _bus?.Dispose();
            _server?.Dispose();
        }
    }
}