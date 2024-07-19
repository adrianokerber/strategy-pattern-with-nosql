using Autofac;
using JobAllocation.JobAllocationContext.Domain.Aggregates.Companies;
using JobAllocation.JobAllocationContext.Domain.Aggregates.Policies;
using JobAllocation.Shared;

namespace JobAllocation.HttpService.StartupInfra;

public class ApplicationModule : Autofac.Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder
            .RegisterAssemblyTypes(typeof(JobAllocation.Environment).Assembly)
            .AsClosedTypesOf(typeof(IService<>))
            .InstancePerLifetimeScope();

        builder.RegisterType<HttpContextAccessor>().As<IHttpContextAccessor>().InstancePerLifetimeScope();
        builder.RegisterType<PoliciesRepository>().As<IPoliciesRepository>().InstancePerLifetimeScope();
        builder.RegisterType<CompaniesRepository>().As<ICompaniesRepository>().InstancePerLifetimeScope();
    }
}