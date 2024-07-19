using CSharpFunctionalExtensions;

namespace JobAllocation.JobAllocationContext.Domain.Aggregates.Policies.Application;

public readonly record struct DefineApplicantAllocationCommand
{
    public Maybe<int> DisabilityCode { get; }
    public int DegreeCode { get; }
    public DateTime Birthday { get; }
    public string CompanyCode { get; }
    public string State { get; }
    public decimal Salary { get; }

    private DefineApplicantAllocationCommand(Maybe<int> disabilityCode,
                                             int degreeCode,
                                             DateTime birthday,
                                             string companyCode,
                                             string state,
                                             decimal salary)
    {
        DisabilityCode = disabilityCode;
        DegreeCode = degreeCode;
        Birthday = birthday;
        CompanyCode = companyCode;
        Salary = salary;
        State = state;
    }

    public static Result<DefineApplicantAllocationCommand> Create(int? disabilityCode,
                                                                  int degreeCode,
                                                                  DateTime birthday,
                                                                  string companyCode,
                                                                  string state,
                                                                  decimal salary)
    {
        var result = Result.Combine(Result.FailureIf(degreeCode == default, "[DegreeCode] Degree must be informed"),
                                    Result.FailureIf(birthday == default, "[Birthday] Date must be valid"),
                                    Result.FailureIf(string.IsNullOrEmpty(companyCode), "[CompanyCode] Company must be informed"),
                                    Result.FailureIf(string.IsNullOrEmpty(state), "[State] State must be informed"),
                                    Result.FailureIf(salary <= 0, "[Salary] Salary must be informed and greater than zero"));

        if (result.IsFailure)
            return result.ConvertFailure<DefineApplicantAllocationCommand>();
        
        return new DefineApplicantAllocationCommand(disabilityCode ?? Maybe<int>.None,
                                                      degreeCode,
                                                      birthday,
                                                      companyCode,
                                                      state,
                                                      salary);
    }
}