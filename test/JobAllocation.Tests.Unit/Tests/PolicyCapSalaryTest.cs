using FluentAssertions;
using JobAllocation.JobAllocationContext.Domain.Aggregates.Policies;
using JobAllocation.JobAllocationContext.Domain.Shared.ValueObject;
using JobAllocation.Tests.Unit.Builders;

namespace JobAllocation.Tests.Unit.Tests;

public class PolicyCapSalaryTest
{
    private readonly PolicyCapSalary _policyCapSalary;

    public PolicyCapSalaryTest()
    {
        _policyCapSalary = new PolicyCapSalary()
        {
            MaximumSalary = 100
        };
    }

    [Theory]
    [InlineData(100)]
    [InlineData(200)]
    public void ApplyPolicy_WithOperationValueGreaterOrEqualThanMaximum_ShouldReturnOnSiteAllocation(decimal value)
    {
        // Arrange
        var command = CommandForTestBuilder.WithValidData(salary: value);

        // Act
        var result = _policyCapSalary.ApplyTo(command);

        // Assert
        result.Value.Should().Be(AllocationType.OnSite());
    }

    [Fact]
    public void ApplyPolicy_WithOperationValueLessThanMaximum_ShouldNotReturnAllocation()
    {
        // Arrange
        var command = CommandForTestBuilder.WithValidData(salary: 90);

        // Act
        var result = _policyCapSalary.ApplyTo(command);

        // Assert
        result.HasNoValue.Should().BeTrue();
    }
}