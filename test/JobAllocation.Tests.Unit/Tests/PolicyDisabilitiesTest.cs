using FluentAssertions;
using JobAllocation.JobAllocationContext.Domain.Aggregates.Policies;
using JobAllocation.JobAllocationContext.Domain.Shared.ValueObject;
using JobAllocation.Tests.Unit.Builders;

namespace JobAllocation.Tests.Unit.Tests;

public class PolicyDisabilitiesTest
{
    private readonly PolicyDisabilities _policyDisabilities;

    public PolicyDisabilitiesTest()
    {
        _policyDisabilities = new PolicyDisabilities()
        {
            Disabilities = GetDisabilitiesList()
        };
    }

    private static IEnumerable<Disability> GetDisabilitiesList()
    {
        return new List<Disability>()
        {
            new Disability  {
                Code = 3,
                Name = "DeficienteAssistido"
            },
            new Disability  {
                Code = 41,
                Name = "VisualAmpliado"
            },
            new Disability {
                Code = 42,
                Name = "VisualBraille"
            },
            new Disability {
                Code = 63,
                Name = "Intelectual"
            },
            new Disability {
                Code = 81,
                Name = "Curatela"
            }
        };
    }

    [Theory]
    [InlineData(3)]
    [InlineData(41)]
    [InlineData(42)]
    [InlineData(63)]
    [InlineData(81)]
    public void ApplyPolicy_WithImpeditiveDisability_ShouldReturnOnSiteAllocation(int code)
    {
        //arrange
        var command = CommandForTestBuilder.WithValidData(disabilityCode: code);

        //act
        var result = _policyDisabilities.ApplyTo(command);

        //assert
        result.Value.Should().Be(AllocationType.OnSite());
    }

    [Fact]
    public void ApplyPolicy_WithNonImpeditiveDisability_ShouldNotReturnAllocation()
    {
        //arrange
        var command = CommandForTestBuilder.WithValidData(disabilityCode: 1);

        //act
        var result = _policyDisabilities.ApplyTo(command);

        //assert
        result.HasNoValue.Should().BeTrue();
    }
}
