using CSharpFunctionalExtensions;
using FluentAssertions;
using Moq;
using JobAllocation.JobAllocationContext.Domain.Aggregates.Companies;
using JobAllocation.JobAllocationContext.Domain.Aggregates.Policies;
using JobAllocation.JobAllocationContext.Domain.Aggregates.Policies.Application;
using JobAllocation.JobAllocationContext.Domain.Features.DefineAllocation;
using JobAllocation.JobAllocationContext.Domain.Shared.ValueObject;
using JobAllocation.Tests.Integration.Builders;

namespace JobAllocation.Tests.Integration.Tests;

public class DefineAllocationDomainServiceTest
{
    private readonly DefineAllocationDomainService _defineAllocationDomainService;
    private readonly Mock<IPoliciesRepository> _policiesRepository;
    private readonly Mock<IPolicy> _policyMock;
    private readonly Mock<ICompaniesRepository> _companiesRepository;
    private readonly DefineApplicantAllocationCommandHandler _commandHandler;

    public DefineAllocationDomainServiceTest()
    {
        _policyMock = new Mock<IPolicy>();
        _policyMock.Setup(x => x.ApplyTo(It.IsAny<DefineApplicantAllocationCommand>())).Returns(Maybe<AllocationType>.None);

        _policiesRepository = new Mock<IPoliciesRepository>();
        _policiesRepository.Setup(x => x.FindAll(It.IsAny<CancellationToken>()))
         .ReturnsAsync(new List<IPolicy>()
         {
                _policyMock.Object
         });

        _companiesRepository = new Mock<ICompaniesRepository>();
        _companiesRepository.Setup(x => x.FindById(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Company()
            {
                Code = "000020",
                Name = "INSS",
                AllocationTypePreference = AllocationType.Remote()
            });
        
        _commandHandler = new DefineApplicantAllocationCommandHandler(_policiesRepository.Object);
        _defineAllocationDomainService = new DefineAllocationDomainService(_commandHandler, _companiesRepository.Object);
    }

    [Fact]
    public async void DetermineAllocation_WithPolicyBeingApplied_ShouldReturnPolicyAllocation()
    {
        //arrange
        CancellationToken cancellationToken = new CancellationToken();
        var request = CommandForTestBuilder.WithValidData(1, 1, DateTime.Now, "000020", "RS", 100);

        _policyMock.Setup(x => x.ApplyTo(It.IsAny<DefineApplicantAllocationCommand>())).Returns(AllocationType.OnSite());

        //act
        var result = await _defineAllocationDomainService.Execute(request, cancellationToken);

        //assert
        _policiesRepository.Verify(policyRepository => policyRepository.FindAll(It.IsAny<CancellationToken>()), Times.Once);
        _companiesRepository.Verify(companyRepository =>
            companyRepository.FindById(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);

        result.Value.Should().Be(AllocationType.OnSite());
    }

    [Fact]
    public async void DetermineAllocation_WithPolicyNotBeingApplied_ShouldReturnCompanyPreferredAllocation()
    {
        //arrange
        CancellationToken cancellationToken = new CancellationToken();
        var request = CommandForTestBuilder.WithValidData(1, 1, DateTime.Now, "000020", "RS", 1);

        //act
        var result = await _defineAllocationDomainService.Execute(request, cancellationToken);

        //assert
        _policiesRepository.Verify(policyRepository => policyRepository.FindAll(It.IsAny<CancellationToken>()), Times.Once);
        _companiesRepository.Verify(companyRepository =>
            companyRepository.FindById(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);

        result.Value.Should().Be(AllocationType.Remote());
    }

    [Fact]
    public async void DetermineAllocation_WithNoPolicyReturned_ShouldReturnFailure()
    {
        //arrange
        CancellationToken cancellationToken = new CancellationToken();
        _policiesRepository.Setup(x => x.FindAll(It.IsAny<CancellationToken>())).ReturnsAsync(new List<IPolicy>());

        //act
        var result = await _defineAllocationDomainService.Execute(It.IsAny<DefineApplicantAllocationCommand>(), cancellationToken);

        //assert
        _policiesRepository.Verify(policyRepository => policyRepository.FindAll(It.IsAny<CancellationToken>()), Times.Once);
        _companiesRepository.Verify(companyRepository =>
            companyRepository.FindById(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);

        result.IsFailure.Should().BeTrue();
    }

    [Fact]
    public async void DetermineAllocation_WithNoCompanyReturned_ShouldReturnFailure()
    {
        //arrange
        _companiesRepository.Setup(x => x.FindById(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Maybe<Company>.None);

        CancellationToken cancellationToken = new CancellationToken();
        var request = CommandForTestBuilder.WithValidData(1, 1, DateTime.Now, "000020", "RS", 1);

        //act
        var result = await _defineAllocationDomainService.Execute(request, cancellationToken);

        //assert
        _policiesRepository.Verify(policyRepository => policyRepository.FindAll(It.IsAny<CancellationToken>()), Times.Once);
        _companiesRepository.Verify(companyRepository =>
            companyRepository.FindById(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);

        result.IsFailure.Should().BeTrue();
    }
}