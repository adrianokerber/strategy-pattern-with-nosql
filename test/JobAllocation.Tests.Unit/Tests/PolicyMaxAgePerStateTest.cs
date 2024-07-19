using FluentAssertions;
using JobAllocation.JobAllocationContext.Domain.Aggregates.Policies;
using JobAllocation.JobAllocationContext.Domain.Shared.ValueObject;
using JobAllocation.Tests.Unit.Builders;

namespace JobAllocation.Tests.Unit.Tests;

public class PolicyMaxAgePerStateTest
{
    private readonly PolicyMaxAgePerState _policyMaxAgePerState;

    public PolicyMaxAgePerStateTest()
    {
        _policyMaxAgePerState = new PolicyMaxAgePerState()
        {
            States = GetStatePoliciesList()
        };
    }

    private IEnumerable<State> GetStatePoliciesList()
    {
        return new List<State>()
        {
            new State
            {
                FederatedState = "PB",
                MaximumAge = 60
            },
            new State
            {
                FederatedState = "AP",
                MaximumAge = 60
            },
            new State
            {
                FederatedState = "RR",
                MaximumAge = 60
            },
            new State
            {
                FederatedState = "TO",
                MaximumAge = 60
            }
        };
    }

    [Theory]
    [InlineData("PB")]
    [InlineData("AP")]
    [InlineData("RR")]
    [InlineData("TO")]
    public void ApplyPolicy_WithAgeAboveStatePolicy_ShouldReturnOnSiteAllocation(string state)
    {
        //arrange
        var birthday = DateTime.Today.AddYears(-60);
        var command = CommandForTestBuilder.WithValidData(birthday: birthday, state: state);

        //act
        var result = _policyMaxAgePerState.ApplyTo(command);

        //assert
        result.Value.Should().Be(AllocationType.OnSite());
    }

    [Theory]
    [InlineData("PB")]
    [InlineData("AP")]
    [InlineData("RR")]
    [InlineData("TO")]
    public void ApplyPolicy_WithAgeBelowStatePolicy_ShouldNotReturnAllocation(string state)
    {
        //arrange
        var birthday = DateTime.Today.AddYears(-20);
        var command = CommandForTestBuilder.WithValidData(birthday: birthday, state: state);

        //act
        var result = _policyMaxAgePerState.ApplyTo(command);

        //assert
        result.HasNoValue.Should().BeTrue();
    }
}
