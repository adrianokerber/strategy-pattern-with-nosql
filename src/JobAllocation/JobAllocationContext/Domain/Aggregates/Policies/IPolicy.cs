using CSharpFunctionalExtensions;
using JobAllocation.JobAllocationContext.Domain.Aggregates.Policies.Application;
using JobAllocation.JobAllocationContext.Domain.Shared.ValueObject;

namespace JobAllocation.JobAllocationContext.Domain.Aggregates.Policies;

public interface IPolicy
{
    public Maybe<AllocationType> ApplyTo(DefineApplicantAllocationCommand defineApplicantAllocationCommand);
}