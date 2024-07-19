using CSharpFunctionalExtensions;

namespace JobAllocation.JobAllocationContext.Domain.Aggregates.Companies;

public interface ICompaniesRepository
{
    Task<Maybe<Company>> FindById(string id, CancellationToken cancellationToken);
}
