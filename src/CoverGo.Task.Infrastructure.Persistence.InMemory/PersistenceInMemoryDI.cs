using CoverGo.Task.Application;
using CoverGo.Task.Infrastructure.Persistence.InMemory;

namespace Microsoft.Extensions.DependencyInjection;

public static class PersistenceInMemoryDI
{
    public static IServiceCollection AddInMemoryPersistence(this IServiceCollection services)
    {
        services.AddScoped<ICompaniesQuery, InMemoryCompaniesRepository>();
        services.AddScoped<IPlansQuery, InMemoryPlansRepository>();
        services.AddScoped<ICompaniesWriteRepository, InMemoryCompaniesRepository>();
        services.AddScoped<IPlansWriteRepository, InMemoryPlansRepository>();
        services.AddScoped<IProposalWriteRepository, InMemoryProposalRespository>();
        services.AddScoped<IInsuredWriteRepository, InMemoryInsuredGroupRespository>();
        services.AddScoped<IProposalQuery, InMemoryProposalRespository>();
        services.AddScoped<IInsuredGroupQuery, InMemoryInsuredGroupRespository>();


        return services;
    }
}
