using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Order.API.Models;
using Order.API.Models.Entities;
using Order.API.Models.Enums;
using Order.API.Models.ViewModels;
using Shared.Events;

namespace Order.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        readonly OrderAPIDbContext _context;
        readonly IPublishEndpoint _publishEndpoint;

        public OrdersController(OrderAPIDbContext context, IPublishEndpoint publishEndpoint)
        {
            _context = context;
            _publishEndpoint = publishEndpoint;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder(CreateOrderVM createOrder)
        {
            Order.API.Models.Entities.Order order = new()
            {
                OrderID = Guid.NewGuid(),
                BuyerID = createOrder.BuyerID,
                CreatedDate = DateTime.Now,
                OrderStatus = OrderStatus.Suspend,
            };

            order.OrderItems = createOrder.OrderItems.Select(orderItem => new OrderItem
            {
                Count = orderItem.Count,
                ProductId = orderItem.ProductId,
                Price = orderItem.Price,
            }).ToList(); 
            
            order.TotalPrice = createOrder.OrderItems.Sum(orderItem => orderItem.Price * orderItem.Count);

            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();

            OrderCreatedEvent orderCreatedEvent = new()
            {
                BuyerId = order.BuyerID,
                OrderId = order.OrderID,
                OrderItems = order.OrderItems.Select(oi => new Shared.Messages.OrderItemMessage
                {
                    Count = oi.Count,
                    ProductId = oi.ProductId
                }).ToList(),
                TotalPrice = order.TotalPrice
            };

            await _publishEndpoint.Publish(orderCreatedEvent);
            return Ok();
        }
    }
}
