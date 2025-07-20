using RedFox.Api.Config;
using RedFox.Api.Endpoints;
using RedFox.Api.Jobs;
using RedFox.Api.Middlewares;
using RedFox.Application;
using RedFox.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHostedService<DbInitWorker>();
builder.Services.AddCustomCors(builder.Environment);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCustomCors(app.Environment);
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.MapUserEndpoints();

await app.RunAsync();
