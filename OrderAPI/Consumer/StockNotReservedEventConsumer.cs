using MassTransit;
using Microsoft.EntityFrameworkCore;
using OrderAPI.Models.Entities;
using Shared.Events;

namespace OrderAPI.Consumer
{
    public class StockNotReservedEventConsumer(OrderAPIDbContext _orderAPIDbContext , IPublishEndpoint _publishEndpoint) : IConsumer<StockNotReservedEvent>
    {
   public async Task Consume(ConsumeContext<StockNotReservedEvent> context)
        {
            OrderAPI.Models.Entities.Order order = await _orderAPIDbContext.Orders.FirstOrDefaultAsync(o => o.OrderId == context.Message.OrderId);
            if (order == null)
            {
                throw new NullReferenceException();
            }
            order.OrderStatus = Models.Enums.OrderStatus.Failed;
            await _orderAPIDbContext.SaveChangesAsync();
            
            OrderFailedEvent orderFailedEvent = new()
            {
                OrderId = context.Message.OrderId,
                BuyerId = context.Message.BuyerId,
                BuyerEmail = order.BuyerEmail,
                //Message = context.Message.Message
                            
            };
            _publishEndpoint.Publish(orderFailedEvent);


        }
    }


}
