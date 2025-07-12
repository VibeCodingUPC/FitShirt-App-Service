using System.Reflection;
using FitShirt.Application;
using FitShirt.Application.Messaging.Features.CommandServices;
using FitShirt.Domain.Messaging.Repositories;
using FitShirt.Domain.Messaging.Services;
using FitShirt.Domain.Shared.Models.ImageCloudinary;
using FitShirt.Domain.Shared.Services.ImageCloudinary;
using FitShirt.Infrastructure;
using FitShirt.Infrastructure.Messaging.Persistence;
using FitShirt.Infrastructure.Shared.Contexts;
using FitShirt.Infrastructure.Shared.ImageCloudinary.Services;
using FitShirt.Presentation.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
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
    
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme
                }
            },
            Array.Empty<string>()
        }
    });
    
    // using System.Reflection;
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

builder.Services.AddInfrastructureService(builder.Configuration);
builder.Services.AddApplicationServices();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer();

builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("CloudinarySettings"));
builder.Services.AddScoped<IManageImageService, ManageImageService>();
builder.Services.AddScoped<IMessageCommandService, MessageCommandService>();
builder.Services.AddScoped<IMessageRepository, MessageRepository>();

var app = builder.Build();

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

app.UseCors(builder =>
{
    builder
        .WithOrigins("http://localhost:5173", "https://fitshirtupc.web.app")
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials();
});

//app.UseAuthentication();

//app.UseAuthorization();

app.UseMiddleware<ErrorHandlerMiddleware>();
app.UseMiddleware<AuthenticationMiddleware>();

app.MapControllers();

app.Run();
