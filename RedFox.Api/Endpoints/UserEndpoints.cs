#region usings
using MediatR;
using RedFox.Application.DTO;
using RedFox.Application.Features.Users.Create;
using RedFox.Application.Features.Users.Delete;
using RedFox.Application.Features.Users.GetAll;
using RedFox.Application.Features.Users.GetSingle;
using RedFox.Application.Features.Users.Update;
#endregion

namespace RedFox.Api.Endpoints;

public static class UserEndpoints
{
    public static IEndpointRouteBuilder MapUserEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/users", async (IMediator context) =>
        await context.Send(new GetAllUserWithRelatedQuery()))
        .WithName("GetUsers")
        .WithOpenApi();


        app.MapGet("/users/{id}", async (int id, IMediator context) =>
            await context.Send(new GetUserWithRelatedQuery(id)))
            .WithName("GetUser")
            .WithOpenApi();


        app.MapPost("/users", async (AddUserWithRelatedRequest request, IMediator mediator) =>
        {
            var result = await mediator.Send(new AddUserWithRelatedCommand(request));
            return Results.Created($"/users/{result}", result);
        })
        .WithName("CreateUser")
        .WithOpenApi();


        app.MapPut("/users/{id}", async (int id, UpdateUserWithRelatedRequest request, IMediator mediator) =>
        {
            var result = await mediator.Send(new UpdateUserWithRelatedCommand(id, request));
            return result is not null
                ? Results.Ok(result)
                : Results.NotFound($"User with ID {id} was not found.");
        });


        app.MapDelete("/users/{id}", async (int id, IMediator mediator) =>
        {
            var result = await mediator.Send(new DeleteUserCommand(id));
            return result
                ? Results.NoContent()
                : Results.NotFound($"User with ID {id} was not found.");
        })
        .WithName("DeleteUser")
        .WithOpenApi();


        return app;
    }
}
