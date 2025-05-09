using Application.Handlers;
using Domain.Repositories;
using Domain.Services.OtpService;
using Domain.UseCases.CompleteUserRegistration;
using Domain.UseCases.RegisterUser;
using Domain.UseCases.ResetEmail;
using Domain.UseCases.RetrieveUser;
using Infrastructure.Contexts;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using Serilog;

Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateBootstrapLogger();
Log.Information("Starting scheduling-iam");

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("UserContext"); 

builder.Services.AddOpenApi();
builder.Services.AddSerilog((services, lc) => lc
    .ReadFrom.Services(services)
    .ReadFrom.Configuration(builder.Configuration)
);
builder.Services.AddProblemDetails();

// Add injected services
builder.Services.AddDbContext<UserContext>(options => options.UseNpgsql(connectionString));

builder.Services.AddTransient<IUserRepository, UserRepository>();

builder.Services.AddScoped<IOtpService, OtpService>();

builder.Services.AddScoped<IRegisterUserUseCase, RegisterUserUseCase>();
builder.Services.AddScoped<IRetrieveUserUseCase, RetrieveUserUseCase>();
builder.Services.AddScoped<ICompleteUserRegistrationUseCase, CompleteUserRegistrationUseCase>();
builder.Services.AddScoped<IResetEmailUseCase, ResetEmailUseCase>();


if (builder.Environment.IsDevelopment())
{
    builder.Logging.AddConsole();
    
    Console.WriteLine($"Connection string: {connectionString}");
}

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseSerilogRequestLogging();
app.UseHttpsRedirection();

app.MapUserHandlers();

if (app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<UserContext>();
        await context.Database.MigrateAsync();
    }

    app.MapOpenApi();
    app.MapScalarApiReference();
}

await app.RunAsync();

public static partial class Program {}