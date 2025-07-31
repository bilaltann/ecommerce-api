using Microsoft.EntityFrameworkCore;
using Product.API.Models.Entities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",policy =>
    {
        policy.WithOrigins("http://localhost:3000")    // <--- geliþtirirken tüm origin'lere izin ver
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// *** DÝKKAT: Buraya almalýsýn ***
builder.Services.AddDbContext<ProductAPIDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SQLServer"));
});

var app = builder.Build();

// HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowFrontend");

app.UseAuthorization();
app.MapControllers();
app.Run();
