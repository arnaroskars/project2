using Microsoft.EntityFrameworkCore;
using MerchantService.OrderDb;
using MerchantService.Services.Interfaces;
using MerchantService.Services.Implementations;
using MerchantService.Data.Implementations;
using MerchantService.Data.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure DbContext with PostgreSQL
builder.Services.AddDbContext<MerchantDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("MerchantDatabase")));

// Register your service layer for DI
builder.Services.AddScoped<IMerchantService, MerchantServiceClass>();
builder.Services.AddScoped<IMerchantRepository, MerchantRepository>();

var app = builder.Build();

// Ensure the database is created and apply any migrations
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<MerchantDbContext>();
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
