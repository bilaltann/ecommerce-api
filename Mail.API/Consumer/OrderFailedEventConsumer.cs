using Mail.API.Services;
using MassTransit;
using Shared.Events;

namespace Mail.API.Consumer
{
    public class OrderFailedEventConsumer(IMailService _mailService) : IConsumer<OrderFailedEvent>
    {
     
        public async Task Consume(ConsumeContext<OrderFailedEvent> context)
        {
            try
            {
                var orderEvent = context.Message;

                // E-posta konusunu belirleyelim
                var subject = "Sipariş işlemi başarısız ! ";
                var body = $"Sayın {orderEvent.BuyerId},\r\n\r\nVermiş olduğunuz sipariş ne yazık ki tamamlanamadı. " +
                    $"Yaşanan aksaklık nedeniyle üzgünüz.\r\n\r\nEn kısa sürede sizinle iletişime geçerek çözüm sunacağız. " +
                    $"Sorularınız için bize her zaman ulaşabilirsiniz.\r\n\r\nİyi günler dileriz.;";

                // Alıcıya e-posta gönder
                await _mailService.SendEmailAsync(orderEvent.BuyerEmail, subject, body);
            }
            catch (Exception ex)
            {
                // Hata işleme mekanizmalarını burada kullanabilirsiniz
                // Örneğin, hata mesajını loglayabilir veya bildirim gönderebilirsiniz
                throw new Exception("E-posta gönderme sırasında hata oluştu", ex);
            }
        }
    }
}
