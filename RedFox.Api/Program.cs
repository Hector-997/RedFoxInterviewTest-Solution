#region

using MediatR;
using RedFox.Api.Jobs;
using RedFox.Application;
using RedFox.Application.Features.Query;
using RedFox.Infrastructure;

#endregion

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHostedService<DbInitWorker>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/users", async (IMediator context) => await context.Send(new GetAllUserWithRelatedQuery()))
    .WithName("GetUsers")
    .WithOpenApi();


await app.RunAsync();