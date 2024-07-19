using JobAllocation.JobAllocationContext.Domain.Shared.ValueObject;

namespace JobAllocation.JobAllocationContext.Domain.Aggregates.Companies;

public sealed class Company // AggregateRoot
{
    public string Code { get; init; }

    public string Name { get; init; }

    public AllocationType AllocationTypePreference { get; init; }
}