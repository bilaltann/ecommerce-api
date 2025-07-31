using MassTransit;
using Microsoft.EntityFrameworkCore;
using OrderAPI.Models.Entities;
using Shared.Events;

namespace OrderAPI.Consumer
{
    public class PaymentCompletedEventConsumer(OrderAPIDbContext _orderAPIDbContext , IPublishEndpoint _publishEndpoint) : IConsumer<PaymentCompletedEvent>
    {
      
        public async Task Consume(ConsumeContext<PaymentCompletedEvent> context)
        {
            var order = await _orderAPIDbContext.Orders.FirstOrDefaultAsync(o => o.OrderId == context.Message.OrderId);

            if (order == null)
            {
                throw new NullReferenceException();
            }

            order.OrderStatus = Models.Enums.OrderStatus.Completed;

            await _orderAPIDbContext.SaveChangesAsync(); // önce kaydet

            var orderCompletedEvent = new OrderCompletedEvent
            {
                OrderId = context.Message.OrderId,
                BuyerId = order.BuyerId,
                BuyerEmail = order.BuyerEmail,
                Message = "Order has been completed successfully."
            };

            await _publishEndpoint.Publish(orderCompletedEvent); // sonra publish et
        }
    }
}
