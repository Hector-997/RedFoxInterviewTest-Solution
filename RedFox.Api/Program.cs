#region

using MediatR;
using RedFox.Api.Jobs;
using RedFox.Application;
using RedFox.Application.DTO;
using RedFox.Application.Features.Users.Create;
using RedFox.Application.Features.Users.GetAll;
using RedFox.Application.Features.Users.GetSingle;
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

app.MapGet("/users", 
        async (IMediator context) => await context.Send(new GetAllUserWithRelatedQuery()))
    .WithName("GetUsers")
    .WithOpenApi();

app.MapGet("/users/{id}", 
        async (int id, IMediator context) => await context.Send(new GetUserWithRelatedQuery(id)))
    .WithName("GetUser")
    .WithOpenApi();

app.MapPost("/users", async (UserCreationDto userDto, IMediator mediator) =>
    {
        var result = await mediator.Send(new AddUsersWithRelatedCommand([userDto]));

        if (result.Any())
        {
            return Results.Created($"/users/{result.First()}", result);
        }

        return Results.Problem("User was not created");
    })
    .WithName("CreateUser")
    .WithOpenApi();


await app.RunAsync();