using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using MinimalApi.Domain.UseCases;
using MinimalApi.Domain.UseCases.Commands;
using MinimalApi.Domain.UseCases.Queries;
using MinimalApi.Observability;
using MinimalApi.WebApi.Presenters;

namespace MinimalApi.WebApi.Router;

public static class CompanyRouter
{
    public static IEndpointRouteBuilder MapCompanyRoutes(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/v1/companies",
                async ([FromHeader(Name = "x-user-id")] Guid? authenticatedUserId, CreateCompanyInput input, HttpContext context, IUseCaseHandler<CreateCompanyCmd> handler) =>
                {
                    var mdc = context.GetMdc();
                    using var activity = AppObservability.ActivitySource.StartActivity(ActivityKind.Server, default, mdc.Attributes);
                    
                    var cmd = CreateCompanyPresenter.ToUseCaseInput(authenticatedUserId, input, mdc);
                    cmd = await handler.ExecuteAsync(cmd);
                    var result = CreateCompanyPresenter.ToOutput(cmd);
                    return Results.Created("/v1/companies/{id}", result);
                })
            // example of custom error handle
            .AddEndpointFilter(async (efiContext, next) =>
            {
                try
                {
                    return await next(efiContext);
                }
                catch (SqlException e)
                {
                    const int duplicateKey = 2601;
                    if (e.Number == duplicateKey)
                    {
                        return Results.Conflict(ErrorPresenter.ToBusinessError("A company with the same document already exists"));
                    }

                    throw;
                }
            })
            .WithName("CreateCompany")
            .Produces<CreateCompanyOutput>(StatusCodes.Status201Created)
            .WithOpenApi();

        endpoints.MapGet("/v1/companies/{id}", async (Guid id, HttpContext context, IUseCaseHandler<GetCompanyByIdQuery> handler) =>
            {
                var mdc = context.GetMdc();
                mdc.CompanyId = id;
                    
                using var activity = AppObservability.ActivitySource.StartActivity(ActivityKind.Server, default, mdc.Attributes)
                    .SetTag("ExampleText", "Hello, World!")
                    .SetTag("ExampleNumber", 1)
                    .SetTag("ExampleArray", new int[] { 1, 2, 3 });

                var query = GetCompanyByIdPresenter.ToUseCaseInput(id, mdc);
                query = await handler.ExecuteAsync(query);
                if (query.CompanyRetrieved == null)
                {
                    return Results.NotFound();
                }

                var result = GetCompanyByIdPresenter.ToOutput(query);
                return Results.Ok(result);
            })
            .WithName("GetCompanyById")
            .Produces<GetCompanyByIdOutput>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .WithOpenApi();

        endpoints.MapGet("/v1/companies/{id}/employees", async (Guid id, int pageSize, int? lastPageItem, HttpContext context, IUseCaseHandler<GetCompanyEmployeesQuery> handler) =>
            {
                var mdc = context.GetMdc();
                mdc.CompanyId = id;
                using var activity = AppObservability.ActivitySource.StartActivity(ActivityKind.Server, default, mdc.Attributes);
                
                var query = GetCompanyEmployeesPresenter.ToUseCaseInput(id, pageSize, lastPageItem);
                query = await handler.ExecuteAsync(query);
                var result = GetCompanyEmployeesPresenter.ToOutput(query);
                return Results.Ok(result);
            })
            .WithName("GetCompanyEmployees")
            .Produces<GetCompanyEmployeesOutput>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .WithOpenApi();

        return endpoints;
    }
}
