using FluentAssertions;
using JobAllocation.JobAllocationContext.Domain.Aggregates.Policies;
using JobAllocation.JobAllocationContext.Domain.Shared.ValueObject;
using JobAllocation.Tests.Unit.Builders;

namespace JobAllocation.Tests.Unit.Tests;

public class PolicyDegreeLevelTest
{
    private readonly PolicyDegreeLevel _policyDegreeLevel;

    public PolicyDegreeLevelTest()
    {
        _policyDegreeLevel = new PolicyDegreeLevel()
        {
            DegreeCode = 7
        };
    }

    [Fact]
    public void ApplyPolicy_WithIlliterateApplicant_ShouldReturnOnSiteAllocation()
    {
        // Arrange
        var command = CommandForTestBuilder.WithValidData(degreeCode: 7);

        // Act
        var result = _policyDegreeLevel.ApplyTo(command);

        // Assert
        result.Value.Should().Be(AllocationType.OnSite());
    }

    [Fact]
    public void ApplyPolicy_WithLiterateApplicant_ShouldNotReturnAllocation()
    {
        // Arrange
        var command = CommandForTestBuilder.WithValidData(degreeCode: 1);

        // Act
        var result = _policyDegreeLevel.ApplyTo(command);

        // Assert
        result.HasNoValue.Should().BeTrue();
    }
}
