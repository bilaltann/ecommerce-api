using MassTransit;
using MassTransit.Transports;
using Microsoft.EntityFrameworkCore;
using OrderAPI.Models.Entities;
using Shared.Events;

namespace OrderAPI.Consumer
{
    public class PaymentFailedEventConsumer(OrderAPIDbContext _orderAPIDbContext , IPublishEndpoint _publishEndpoint) : IConsumer<PaymentFailedEvent>
    {
     
        public async Task Consume(ConsumeContext<PaymentFailedEvent> context)
        {
            OrderAPI.Models.Entities.Order order = await _orderAPIDbContext.Orders.FirstOrDefaultAsync(o => o.OrderId == context.Message.OrderId);
            if (order == null)
            {
                throw new NullReferenceException();
            }
                
            order.OrderStatus =Models.Enums.OrderStatus.Failed;
            await _orderAPIDbContext.SaveChangesAsync();
            OrderFailedEvent orderFailedEvent = new()
            {
                OrderId = context.Message.OrderId,
                BuyerId = order.BuyerId,
                BuyerEmail = order.BuyerEmail,

            };
            _publishEndpoint.Publish(orderFailedEvent);
        }
    }
}
