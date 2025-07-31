using MassTransit;
using Microsoft.EntityFrameworkCore;
using OrderAPI.Consumer;
using OrderAPI.Models.Entities;
using Shared;

var builder = WebApplication.CreateBuilder(args);

// 1. CORS Politikasýný tanýmlýyoruz.
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins(
                          "http://localhost:3000"
)
.AllowAnyHeader()
.AllowAnyMethod();

                      });
});

// Diðer servisleri ekliyoruz.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// MassTransit ve DbContext yapýlandýrmalarý
builder.Services.AddMassTransit(configurator =>
{
    configurator.AddConsumer<PaymentCompletedEventConsumer>();
    configurator.AddConsumer<StockNotReservedEventConsumer>();
    configurator.AddConsumer<PaymentFailedEventConsumer>();
    configurator.UsingRabbitMq((context, _configurator) =>
    {
        _configurator.Host(builder.Configuration["RabbitMQ"]);
        _configurator.ReceiveEndpoint(RabbitMQSettings.Order_PaymentCompletedEventQueue, e => e.ConfigureConsumer<PaymentCompletedEventConsumer>(context));
        _configurator.ReceiveEndpoint(RabbitMQSettings.Order_StockNotEventQueue, e => e.ConfigureConsumer<StockNotReservedEventConsumer>(context));
        _configurator.ReceiveEndpoint(RabbitMQSettings.Order_PaymentFailedEventQueue, e => e.ConfigureConsumer<PaymentFailedEventConsumer>(context));
    });
});
builder.Services.AddDbContext<OrderAPIDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SQLServer"));
});


var app = builder.Build();


// Geliþtirme ortamý için Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();

// UseRouting'den SONRA, UseAuthorization'dan ÖNCE gelmelidir.
// Bu, yönlendirme belli olduktan sonra, yetkilendirmeden önce CORS kurallarýnýn uygulanmasýný saðlar.
app.UseCors(MyAllowSpecificOrigins);

app.UseAuthorization();

app.MapControllers();



app.Run();