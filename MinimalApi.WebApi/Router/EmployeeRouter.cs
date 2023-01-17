using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MinimalApi.Domain.UseCases;
using MinimalApi.Domain.UseCases.Commands;
using MinimalApi.Domain.UseCases.Queries;
using MinimalApi.Observability;
using MinimalApi.WebApi.Presenters;

namespace MinimalApi.WebApi.Router;

public static class EmployeeRouter
{
    public static IEndpointRouteBuilder MapEmployeeRoutes(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/v1/employees", async ([FromHeader(Name = "x-user-id")] Guid? authenticatedUserId, CreateEmployeeInput input, HttpContext context, IUseCaseHandler<CreateEmployeeCmd> handler) =>
            {
                var mdc = context.GetMdc();
                using var activity = AppObservability.ActivitySource.StartActivity(ActivityKind.Server, default, mdc.Attributes);
                
                var cmd = CreateEmployeePresenter.ToUseCaseInput(authenticatedUserId, input, mdc);
                cmd = await handler.ExecuteAsync(cmd);
                var result = CreateEmployeePresenter.ToOutput(cmd);
                return Results.Created("/v1/employees/{id}", result);
            })
            .WithName("CreateEmployee")
            .Produces<CreateEmployeeOutput>(StatusCodes.Status201Created)
            .WithOpenApi();
        
        endpoints.MapGet("/v1/employees/{id}", async (Guid id, HttpContext context, IUseCaseHandler<GetEmployeeByIdQuery> handler) =>
            {
                var mdc = context.GetMdc();
                mdc.EmployeeId = id;
                using var activity = AppObservability.ActivitySource.StartActivity(ActivityKind.Server, default, mdc.Attributes);
                
                var query = GetEmployeeByIdPresenter.ToUseCaseInput(id, mdc);
                query = await handler.ExecuteAsync(query);
                if (query.EmployeeRetrieved == null)
                {
                    return Results.NotFound();
                }

                var result = GetEmployeeByIdPresenter.ToOutput(query);
                return Results.Ok(result);
            })
            .WithName("GetEmployeeById")
            .Produces<GetEmployeeByIdOutput>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .WithOpenApi();
            
        return endpoints;
    }
}
