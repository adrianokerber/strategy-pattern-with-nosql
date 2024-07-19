using CSharpFunctionalExtensions;
using JobAllocation.JobAllocationContext.Domain.Aggregates.Policies.Application;
using JobAllocation.JobAllocationContext.Domain.Shared.ValueObject;

namespace JobAllocation.JobAllocationContext.Domain.Aggregates.Policies;

public sealed class PolicyMaxAgePerState : IPolicy
{
    public IEnumerable<State> States { get; init; }

    public Maybe<AllocationType> ApplyTo(DefineApplicantAllocationCommand defineApplicantAllocationCommand)
    {
        var age = CalculateAge(defineApplicantAllocationCommand.Birthday);

        if (States.Any(i => i.FederatedState == defineApplicantAllocationCommand.State && i.MaximumAge <= age))
            return AllocationType.OnSite();

        return Maybe<AllocationType>.None;
    }

    private int CalculateAge(DateTime birthday)
    {
        int years = new DateTime(DateTime.Now.Subtract(birthday.Date).Ticks).Year - 1;

        return years;
    }
}

public class State
{
    public string FederatedState { get; init; }

    public int MaximumAge { get; init; }
}