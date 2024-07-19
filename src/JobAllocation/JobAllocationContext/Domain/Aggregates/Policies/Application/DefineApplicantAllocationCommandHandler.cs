using CSharpFunctionalExtensions;
using JobAllocation.JobAllocationContext.Domain.Shared.ValueObject;
using JobAllocation.Shared;

namespace JobAllocation.JobAllocationContext.Domain.Aggregates.Policies.Application;

public sealed class DefineApplicantAllocationCommandHandler : IService<DefineApplicantAllocationCommandHandler>
{
    private readonly IPoliciesRepository _policiesRepository;

    public DefineApplicantAllocationCommandHandler(IPoliciesRepository policiesRepository)
    {
        _policiesRepository = policiesRepository;
    }

    public async Task<Result<Maybe<AllocationType>>> Handle(DefineApplicantAllocationCommand command, CancellationToken cancellationToken)
    {
        var policies = await _policiesRepository.FindAll(cancellationToken);
        if (!policies.Any())
            return Result.Failure<Maybe<AllocationType>>("No policies found!");

        foreach (var policy in policies)
        {
            var result = policy.ApplyTo(command);
            if (result.HasValue)
                return Result.Success(Maybe<AllocationType>.From(result.Value));
        }
        
        return Result.Success(Maybe<AllocationType>.None);
    }
}