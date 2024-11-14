using Microsoft.EntityFrameworkCore;
using BuyerService.OrderDb;
using BuyerService.Services.Interfaces;
using BuyerService.Services.Implementations;
using BuyerService.Data.Implementations;
using BuyerService.Data.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure DbContext with PostgreSQL
builder.Services.AddDbContext<BuyerDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("BuyerDatabase")));

// Register service and repository interfaces with implementations
builder.Services.AddScoped<IBuyerService, BuyerServiceClass>();
builder.Services.AddScoped<IBuyerRepository, BuyerRepository>();

var app = builder.Build();

// Ensure the database is created and apply any migrations
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<BuyerDbContext>();
    dbContext.Database.Migrate();
}

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
