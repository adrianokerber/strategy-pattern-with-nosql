namespace JobAllocation.JobAllocationContext.Domain.Shared.ValueObject;

public readonly record struct AllocationType(int Code, string Description)
{
    public static AllocationType OnSite()
        => new AllocationType(0, "REMOTE");

    public static AllocationType Remote()
        => new AllocationType(1, "ON_SITE");

    public static AllocationType Hybrid()
        => new AllocationType(2, "HYBRID");
}