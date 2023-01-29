using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Serilog.Events;
using Serilog.Exceptions;
using Serilog.Formatting.Compact;
using Travel.Application;
using Travel.Data;
using Travel.Shared;
using Travel.WebApi.Filters;

var name = Assembly.GetExecutingAssembly().GetName();
string connectionString = "host=localhost;database=TravelLOGS;username=jmramirez;password=devapplication;";
string tableName = "logs";

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .Enrich.FromLogContext()
    .Enrich.WithExceptionDetails()
    .Enrich.WithMachineName()
    .Enrich.WithProperty("Assembly", $"{name.Name}")
    .Enrich.WithProperty("Assembly", $"{name.Version}")
    .WriteTo.PostgreSQL(
        connectionString,
        tableName,
        restrictedToMinimumLevel: LogEventLevel.Information)
    .WriteTo.File(
        new CompactJsonFormatter(),
        Environment.CurrentDirectory + @"/Logs/log.json",
        rollingInterval: RollingInterval.Day,
        restrictedToMinimumLevel: LogEventLevel.Information)
    .WriteTo.Console()
    .CreateLogger();

try
{
    Log.Information("Starting host");
    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog();

// Add services to the container.
    builder.Services.AddApplication();
    builder.Services.AddInfrastructureData();
    builder.Services.AddInfrastructureShared(builder.Configuration);
    builder.Services.AddHttpContextAccessor();

    builder.Services.AddControllers();
    builder.Services.AddControllersWithViews(options => 
        options.Filters.Add(new ApiExceptionFilter()));

    builder.Services.Configure<ApiBehaviorOptions>(options =>
        options.SuppressModelStateInvalidFilter = true);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    var app = builder.Build();

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
    
}
catch (Exception e)
{
    Log.Fatal(e, "Host terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
