using MassTransit;
using Shared.Events;

namespace Payment.API.Consumers
{
    public class StockReservedEventConsumer : IConsumer<StockReservedEvent>
    {
        readonly IPublishEndpoint _publishEndpoint;

        public StockReservedEventConsumer(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        public Task Consume(ConsumeContext<StockReservedEvent> context)
        {
            //payment process
            PaymentCompletedEvent pcEvent = new()
            {
                OrderId = context.Message.OrderId
            };
            int randomNum = TakeRandomNum();
            if (randomNum <= 7)
            {
                PaymentCompletedEvent paymentCompletedEvent = new()
                {
                    OrderId = context.Message.OrderId
                };
                _publishEndpoint.Publish(paymentCompletedEvent);

                Console.WriteLine("Payment successfuly completed.");
            }
            else
            {
                PaymentFailedEvent paymentFailedEvent = new()
                {
                    OrderId = context.Message.OrderId,
                    Message = "Insufficient balance!!!"
                };
                _publishEndpoint.Publish(paymentFailedEvent);

                Console.WriteLine("Payment failed!!!");
            }


            return Task.CompletedTask;
        }

        private int TakeRandomNum() 
        {
            Random rnd = new Random();
            return rnd.Next(1, 10);
        }

    }
}
