using Asp.Versioning;
using FluentValidation;
using FluentValidation.AspNetCore;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.AspNetCore.HttpLogging;
using Serilog;
using System.Reflection;
using Users.Api.Constants;
using Users.Api.Contracts;
using Users.Api.Helpers;
using Users.Api.Middlewares;
using Users.Application;
using Users.Application.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog(logger: new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger());

builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly(), ServiceLifetime.Singleton);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory,
        $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"));
    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory,
        $"{typeof(IContractsMarker).Assembly.GetName().Name}.xml"));
});
builder.Services.AddApiVersioning(option =>
{
    option.DefaultApiVersion = new ApiVersion(1.0);
    option.AssumeDefaultVersionWhenUnspecified = true;
    option.ReportApiVersions = true;
    option.ApiVersionReader = new MediaTypeApiVersionReader("api-version");
});
builder.Services.AddHttpLogging(x =>
{
    x.LoggingFields = HttpLoggingFields.Request | HttpLoggingFields.Response;
    x.RequestBodyLogLimit = 4096;
    x.ResponseBodyLogLimit = 4096;
});
builder.Services.AddTransient<ExceptionHandlingMiddleware>();

builder.Services.AddApplication(users: FileHelper.GetListFromJsonFile<User>(builder.Configuration[Configurations.Resources.Users]!));

builder.Services.AddFluentValidationRulesToSwagger();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpLogging();

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.MapControllers();

app.Run();
