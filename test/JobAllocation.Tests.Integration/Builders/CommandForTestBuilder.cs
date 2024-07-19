using JobAllocation.JobAllocationContext.Domain.Aggregates.Policies;
using JobAllocation.JobAllocationContext.Domain.Aggregates.Policies.Application;

namespace JobAllocation.Tests.Integration.Builders;

public static class CommandForTestBuilder
{
    public static DefineApplicantAllocationCommand WithValidData(
        int disabilityCode = 1,
        int degreeCode = 1,
        DateTime? birthday = null,
        string companyCode = "000020",
        string state = "RS",
        decimal salary = 1)
    {
        birthday ??= DateTime.Today;

        var command = DefineApplicantAllocationCommand.Create(disabilityCode, degreeCode, birthday.Value, companyCode, state, salary);

        return command.Value;
    }
}