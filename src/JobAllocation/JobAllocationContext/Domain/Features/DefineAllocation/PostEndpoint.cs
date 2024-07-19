using FastEndpoints;
using JobAllocation.JobAllocationContext.Domain.Aggregates.Policies.Application;
using JobAllocation.Shared;
using Microsoft.AspNetCore.Http;

namespace JobAllocation.JobAllocationContext.Domain.Features.DefineAllocation;

public class PostEndpoint : Endpoint<Request, IResult>
{
    private readonly DefineAllocationDomainService _defineAllocationDomainService;
    private readonly HttpResponseFactory _httpResponseFactory;

    public PostEndpoint(DefineAllocationDomainService defineAllocationDomainService, HttpResponseFactory httpResponseFactory)
    {
        _defineAllocationDomainService = defineAllocationDomainService;
        _httpResponseFactory = httpResponseFactory;
    }

    public override void Configure()
    {
        Post("/define-allocation");
        AllowAnonymous();
    }

    public override async Task HandleAsync(Request req, CancellationToken cancellationToken)
    {
        var requisition = DefineApplicantAllocationCommand.Create(req.DisabilityCode, req.DegreeCode, req.Birthday, req.CompanyCode, req.State, req.Salary);
        if (requisition.IsFailure)
        {
            await SendResultAsync(_httpResponseFactory.CreateError400("Invalid payload", requisition.Error));
            return;
        }

        var result = await _defineAllocationDomainService.Execute(requisition.Value, cancellationToken);
        if (result.IsFailure)
        {
            await SendResultAsync(_httpResponseFactory.CreateError400("Incapable of determine formalization method", result.Error));
            return;
        }
        
        await SendResultAsync(_httpResponseFactory.CreateSuccess200(result.Value));
    }
}

public record Request
{
    public int DisabilityCode { get; init; }

    public int DegreeCode { get; init; }

    public DateTime Birthday { get; init; }

    public string CompanyCode { get; init; }

    public string State { get; init; }

    public decimal Salary { get; init; }
}