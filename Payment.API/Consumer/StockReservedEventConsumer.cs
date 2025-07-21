using MassTransit;
using Shared.Events;

namespace Payment.API.Consumer
{
    public class StockReservedEventConsumer(IPublishEndpoint _publishEndpoint) : IConsumer<StockReservedEvent>
    {
      

        public Task Consume(ConsumeContext<StockReservedEvent> context)
        {
            //ödeme işlemleri

            if (true)
            {
                PaymentCompletedEvent paymentCompletedEvent = new()
                {
                    OrderId = context.Message.OrderId
               
                };
                _publishEndpoint.Publish(paymentCompletedEvent);
                Console.WriteLine("ödeme başarılı");
            }

            else
            {
                //ödemede sıkıntı olduğunu 
                PaymentFailedEvent paymentFailedEvent = new()
                {
                    OrderId = context.Message.OrderId,
                    Message = "bakiye yetersiz",
                    OrderItems = context.Message.OrderItems.Select(oi =>
                    
                        new Shared.Messages.OrderItemMessage
                        {
                            ProductId = oi.ProductId,
                            Count = oi.Count
                        }).ToList()

                };
                _publishEndpoint.Publish(paymentFailedEvent);
                Console.WriteLine("ödeme başarısız");

            }
            return Task.CompletedTask;
        }
    }
}
