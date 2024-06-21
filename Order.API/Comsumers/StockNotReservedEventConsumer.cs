using MassTransit;
using Microsoft.EntityFrameworkCore;
using Order.API.Models;
using Shared.Events;

namespace Order.API.Comsumers
{
    public class StockNotReservedEventConsumer : IConsumer<StockNotReveivedEvent>
    {
        readonly OrderAPIDbContext orderAPIDbContext;

        public StockNotReservedEventConsumer(OrderAPIDbContext orderAPIDbContext)
        {
            this.orderAPIDbContext = orderAPIDbContext;
        }

        public async Task Consume(ConsumeContext<StockNotReveivedEvent> context)
        {
            Order.API.Models.Entities.Order order = await orderAPIDbContext.Orders.FirstOrDefaultAsync(o => o.OrderID == context.Message.OrderId);
            order.OrderStatus = Models.Enums.OrderStatus.Failed;
            await orderAPIDbContext.SaveChangesAsync();
        }
    }
}
