using FitShirt.Application;
using FitShirt.Infrastructure;
using FitShirt.Infrastructure.Shared.Contexts;
using FitShirt.Presentation.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddInfrastructureService(builder.Configuration);
builder.Services.AddApplicationServices();

var app = builder.Build();

app.UseMiddleware<ErrorHandlerMiddleware>();

using (var scope = app.Services.CreateScope())
using (var context = scope.ServiceProvider.GetService<FitShirtDbContext>())
{
    context!.Database.EnsureCreated();
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