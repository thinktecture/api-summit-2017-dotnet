using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web.Http;
using AutoMapper;
using EasyNetQ;
using OrdersService.Properties;
using QueuingMessages;
using Order = OrdersService.DTOs.Order;

namespace OrdersService.Controllers
{
    [RoutePrefix("api/orders")]
    public class OrdersController : ApiController
    {
        private static readonly ConcurrentDictionary<Guid, Order> Datastore;

        static OrdersController()
        {
            Datastore = new ConcurrentDictionary<Guid, Order>();
        }

        [HttpGet]
        [Route]
        public List<Order> GetOrders()
        {
            // TODO: Exception handling
            return Datastore.Values.OrderByDescending(o => o.Created).ToList();
        }

        [HttpPost]
        [Route]
        public void AddNewOrder(Order newOrder)
        {
            var orderId = Guid.NewGuid();
            newOrder.Id = orderId;

            Datastore.TryAdd(orderId, newOrder);

            // TODO: Retry & exception handling
            using (var bus = RabbitHutch.CreateBus(Settings.Default.RabbitMQConnectionString))
            {
                var identity = User.Identity as ClaimsIdentity;
                var subjectId = identity?.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;

                var message = new NewOrderMessage
                {
                    UserId = subjectId,
                    Order = Mapper.Map<QueuingMessages.Order>(newOrder)
                };

                // TODO: Exception handling
                bus.Publish(message);
            }
        }
    }
}
