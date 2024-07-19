using CSharpFunctionalExtensions;
using JobAllocation.JobAllocationContext.Domain.Aggregates.Policies.Application;
using JobAllocation.JobAllocationContext.Domain.Shared.ValueObject;

namespace JobAllocation.JobAllocationContext.Domain.Aggregates.Policies;

public sealed class PolicyCapSalary : IPolicy
{
    public decimal MaximumSalary { get; init; }

    public Maybe<AllocationType> ApplyTo(DefineApplicantAllocationCommand defineApplicantAllocationCommand)
    {
        if (defineApplicantAllocationCommand.Salary >= MaximumSalary)
            return AllocationType.OnSite();

        return Maybe<AllocationType>.None;
    }
}