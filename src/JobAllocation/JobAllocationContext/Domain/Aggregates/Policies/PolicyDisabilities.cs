using CSharpFunctionalExtensions;
using JobAllocation.JobAllocationContext.Domain.Aggregates.Policies.Application;
using JobAllocation.JobAllocationContext.Domain.Shared.ValueObject;

namespace JobAllocation.JobAllocationContext.Domain.Aggregates.Policies;

public sealed class PolicyDisabilities : IPolicy
{
    public IEnumerable<Disability> Disabilities { get; init; }

    public Maybe<AllocationType> ApplyTo(DefineApplicantAllocationCommand defineApplicantAllocationCommand)
    {
        if (Disabilities.Any(disability => disability.Code == defineApplicantAllocationCommand.DisabilityCode))
            return AllocationType.OnSite();

        return Maybe<AllocationType>.None;
    }
}

public record Disability
{
    public int Code { get; init; }
    public string Name { get; init; }
}