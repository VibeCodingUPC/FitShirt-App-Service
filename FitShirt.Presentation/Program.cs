using System.Reflection;
using FitShirt.Application;
using FitShirt.Infrastructure;
using FitShirt.Infrastructure.Shared.Contexts;
using FitShirt.Presentation.Middleware;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "FitShirt Api",
        Description = "FitShirt API working"
    });
    
    // using System.Reflection;
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

builder.Services.AddInfrastructureService(builder.Configuration);
builder.Services.AddApplicationServices();

var app = builder.Build();

app.UseMiddleware<ErrorHandlerMiddleware>();

using (var scope = app.Services.CreateScope())
await using (var context = scope.ServiceProvider.GetService<FitShirtDbContext>())
{
    context!.Database.EnsureCreated();
    await FitShirtDbContextSeed.LoadDataAsync(context);
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