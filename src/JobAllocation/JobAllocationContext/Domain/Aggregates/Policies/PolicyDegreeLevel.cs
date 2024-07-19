using CSharpFunctionalExtensions;
using JobAllocation.JobAllocationContext.Domain.Aggregates.Policies.Application;
using JobAllocation.JobAllocationContext.Domain.Shared.ValueObject;

namespace JobAllocation.JobAllocationContext.Domain.Aggregates.Policies;

public sealed class PolicyDegreeLevel : IPolicy
{
    public int DegreeCode { get; init; }

    public Maybe<AllocationType> ApplyTo(DefineApplicantAllocationCommand defineApplicantAllocationCommand)
    {
        if (defineApplicantAllocationCommand.DegreeCode == DegreeCode)
            return AllocationType.OnSite();

        return Maybe<AllocationType>.None;
    }
}