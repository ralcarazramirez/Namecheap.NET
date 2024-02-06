using NameCheap;
using NameCheap.Dns;
using NameCheap.Domain;

namespace Namecheap.WebApi;

public static class NamecheapExtensions
{
    public static IServiceCollection AddNamecheapClients(this IServiceCollection services)
    {
        services.AddHttpClient<NamecheapClient>();
//builder.Services.AddHostedService<NamecheapDnsService>();
        services.AddTransient<IDnsClient, DnsClient>();
        services.AddTransient<IDomainsApi, DomainsApi>();
        services.AddOptions<NamecheapOptions>()
            .BindConfiguration("Namecheap");
        return services;
    }

    public static WebApplication MapNamecheapApi(this WebApplication webApplication)
    {
        var namecheapGroup = webApplication.MapGroup("/namecheap");

        namecheapGroup.MapGet("/domains", async (IDomainsApi domainsApi) =>
            {
                var domains = await domainsApi.GetList();
                return domains.Domains;
            }).WithName("Get Domains")
            .WithOpenApi();
        
        return webApplication;
    }
}