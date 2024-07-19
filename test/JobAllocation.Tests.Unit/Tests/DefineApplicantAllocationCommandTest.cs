using FluentAssertions;
using JobAllocation.JobAllocationContext.Domain.Aggregates.Policies;
using JobAllocation.JobAllocationContext.Domain.Aggregates.Policies.Application;

namespace JobAllocation.Tests.Unit.Tests;

public class DefineApplicantAllocationCommandTest
{
    [Fact]
    public void CreateRequest_WithValidData_ShouldCreateSuccessfully()
    {
        // Act
        var result = DefineApplicantAllocationCommand.Create(1, 1, DateTime.Now, "000020", "RS", 100);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeOfType<DefineApplicantAllocationCommand>();
    }

    [Fact]
    public void CreateRequest_WithInvalidBirthDate_ShouldFail()
    {
        // Act
        var result = DefineApplicantAllocationCommand.Create(1, 1, DateTime.MinValue, "000020", "RS", 100);

        // Assert
        result.IsFailure.Should().BeTrue();
    }

    [Fact]
    public void CreateRequest_WithInvalidCompany_ShouldFail()
    {
        // Act
        var result = DefineApplicantAllocationCommand.Create(1, 1, DateTime.Now, "", "RS", 100);

        // Assert
        result.IsFailure.Should().BeTrue();
    }

    [Fact]
    public void CreateRequest_WithInvalidResidentialState_ShouldFail()
    {
        // Act
        var result = DefineApplicantAllocationCommand.Create(1, 1, DateTime.Now, "000020", "", 100);

        // Assert
        result.IsFailure.Should().BeTrue();
    }

    [Fact]
    public void CreateRequest_WithInvalidFinancedValue_ShouldFail()
    {
        // Act
        var result = DefineApplicantAllocationCommand.Create(1, 1, DateTime.Now, "000020", "RS", 0);

        // Assert
        result.IsFailure.Should().BeTrue();
    }
}