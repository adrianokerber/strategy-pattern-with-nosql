namespace JobAllocation.JobAllocationContext.Domain.Aggregates.Policies;

public interface IPoliciesRepository
{
    Task<IEnumerable<IPolicy>> FindAll(CancellationToken cancellationToken);
}
