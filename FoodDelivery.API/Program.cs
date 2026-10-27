using Confluent.Kafka;
using FoodDelivery.API.GrpcServices;
using FoodDelivery.BLL.Interfaces;
using FoodDelivery.BLL.Services;
using FoodDelivery.DAL.Interfaces;
using FoodDelivery.DAL.Repositories;
using FoodDelivery.Models.DTOs.Responses;
using Hangfire.DAL.Data;
using Hangfire.DAL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);
var dbConnectionString = builder.Configuration.GetConnectionString("FoodDeliveryDatabase");
var kafkaConnectionString = builder.Configuration.GetConnectionString("KafkaServer");

var producerConfig = new ProducerConfig
{
    BootstrapServers = kafkaConnectionString,
    AllowAutoCreateTopics = true,
    Acks = Acks.All
};

builder.Services.AddSingleton<IProducer<string, string>>(
    _ => new ProducerBuilder<string, string>(producerConfig).Build());

builder.Services.AddSingleton<IProducerService, ProducerService>();

// Add services to the container.

builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        options.InvalidModelStateResponseFactory = context =>
        {
            var errors = context.ModelState
                .Where(x => x.Value?.Errors.Count > 0)
                .Select(x => new
                {
                    field = x.Key,
                    messages = x.Value!.Errors.Select(e => e.ErrorMessage)
                });

            return new BadRequestObjectResult(new ApiResDto<object>
            {
                Success = false,
                ValidationErrors = errors
            });
        };
    });

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddSingleton<IDbConnectionFactory, DbConnectionFactory>();

builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IAnalyticsService, AnalyticsService>();

builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IMenuItemRepository, MenuItemRepository>();
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
builder.Services.AddScoped<IRiderRepository, RiderRepository>();
builder.Services.AddScoped<IAnalyticsRepository, AnalyticsRepository>();

builder.Services.AddGrpc();

var app = builder.Build();

app.MapGrpcService<OrderGrpcService>();
app.MapGrpcService<RiderGrpcService>();
app.MapGrpcService<AnalyticsGrpcService>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
