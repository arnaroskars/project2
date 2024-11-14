using OrderService.Data.Interfaces;
using OrderService.Data.Implementation;
using OrderService.Services.Interfaces;
using OrderService.Services.Implementations;
using OrderService.OrderDb;
using Microsoft.EntityFrameworkCore;
using OrderService.Services.HttpClients.Interfaces;
using OrderService.Events;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient<IInventoryServiceClient, InventoryServiceClient>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["InventoryService:BaseUrl"]!);
});

builder.Services.AddHttpClient<IMerchantServiceClient, MerchantServiceClient>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["MerchantService:BaseUrl"]!);
});

builder.Services.AddHttpClient<IBuyerServiceClient, BuyerServiceClient>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["BuyerService:BaseUrl"]!);
});

// Register DbContext with the PostgreSQL connection string
builder.Services.AddDbContext<OrderDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("OrderDatabase")));

// Register repository and service interfaces with implementations
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderService, OrderServiceClass>();
builder.Services.AddSingleton<IEventPublisher, EventPublisher>();


builder.Services.AddControllers();

// Swagger/OpenAPI configuration
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<OrderDbContext>(); // Replace with OrderDbContext or InventoryDbContext
    dbContext.Database.Migrate();
}
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
