using MassTransit;
using Microsoft.EntityFrameworkCore;
using Order.API.Models;
using Shared.Events;

namespace Order.API.Comsumers
{
    public class PaymentFailedEventConsumer : IConsumer<PaymentFailedEvent>
    {
        readonly OrderAPIDbContext orderAPIDbContext;

        public PaymentFailedEventConsumer(OrderAPIDbContext orderAPIDbContext)
        {
            this.orderAPIDbContext = orderAPIDbContext;
        }

        public async Task Consume(ConsumeContext<PaymentFailedEvent> context)
        {
            Order.API.Models.Entities.Order order = await orderAPIDbContext.Orders.FirstOrDefaultAsync(o => o.OrderID == context.Message.OrderId);
            order.OrderStatus = Models.Enums.OrderStatus.Failed;
            await orderAPIDbContext.SaveChangesAsync();
        }
    }
}
