using MassTransit;
using Microsoft.EntityFrameworkCore;
using Order.API.Models;
using Order.API.Models.Enums;
using Shared.Events;

namespace Order.API.Comsumers
{
    public class PaymentCompletedEventConsumer : IConsumer<PaymentCompletedEvent>
    {
        readonly OrderAPIDbContext orderAPIDbContext;

        public PaymentCompletedEventConsumer(OrderAPIDbContext orderAPIDbContext)
        {
            this.orderAPIDbContext = orderAPIDbContext;
        }

        public async Task Consume(ConsumeContext<PaymentCompletedEvent> context)
        {
            Order.API.Models.Entities.Order order = await orderAPIDbContext.Orders.FirstOrDefaultAsync(o => o.OrderID == context.Message.OrderId);
            order.OrderStatus = OrderStatus.Competed;
            await orderAPIDbContext.SaveChangesAsync();
        }
    }
}
