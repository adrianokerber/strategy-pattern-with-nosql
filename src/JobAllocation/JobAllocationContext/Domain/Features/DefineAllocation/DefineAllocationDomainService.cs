using CSharpFunctionalExtensions;
using JobAllocation.JobAllocationContext.Domain.Aggregates.Companies;
using JobAllocation.JobAllocationContext.Domain.Aggregates.Policies;
using JobAllocation.JobAllocationContext.Domain.Aggregates.Policies.Application;
using JobAllocation.JobAllocationContext.Domain.Shared.ValueObject;
using JobAllocation.Shared;

namespace JobAllocation.JobAllocationContext.Domain.Features.DefineAllocation;

public class DefineAllocationDomainService : IService<DefineAllocationDomainService>
{
    private readonly DefineApplicantAllocationCommandHandler _commandHandler;
    private readonly ICompaniesRepository _companiesRepository;

    public DefineAllocationDomainService(DefineApplicantAllocationCommandHandler commandHandler, ICompaniesRepository companiesRepository)
    {
        _commandHandler = commandHandler;
        _companiesRepository = companiesRepository;
    }

    public async Task<Result<AllocationType>> Execute(DefineApplicantAllocationCommand defineApplicantAllocationCommand, CancellationToken cancellationToken)
    {
        var handledCommand = await _commandHandler.Handle(defineApplicantAllocationCommand, cancellationToken);
        if (handledCommand.IsFailure)
            return handledCommand.ConvertFailure<AllocationType>();
        if (handledCommand.Value is { HasValue: true } allocationType)
            return Result.Success(allocationType.Value);

        var company = await _companiesRepository.FindById(defineApplicantAllocationCommand.CompanyCode, cancellationToken);
        if (company.HasValue)
            return Result.Success(company.Value.AllocationTypePreference);
        
        return Result.Failure<AllocationType>("Unable to define allocation type!");
    }
}