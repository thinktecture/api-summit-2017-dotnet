using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using OrdersService.DTOs;

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
        }
    }
}
