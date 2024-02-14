using NameCheap.Dns;
using NameCheap.Domain;
using Namecheap.WebApi;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddNamecheapClients();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
    {
        var forecast = Enumerable.Range(1, 5).Select(index =>
                new WeatherForecast
                (
                    DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    Random.Shared.Next(-20, 55),
                    summaries[Random.Shared.Next(summaries.Length)]
                ))
            .ToArray();
        return forecast;
    })
    .WithName("GetWeatherForecast")
    .WithOpenApi();

app.MapNamecheapApi();
app.Run();
record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

public class NamecheapDnsService(ILogger<NamecheapDnsService> logger, IServiceProvider serviceProvider) : BackgroundService
{
    private readonly string[] NAMESERVERS = { "ns1.digitalocean.com", "ns2.digitalocean.com", "ns3.digitalocean.com" };
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var domainsApi = serviceProvider.GetRequiredService<IDomainsApi>();
        var domains = await domainsApi.GetList(stoppingToken);
        
        foreach (var domain in domains.Domains)
        {
            try
            {
                logger.LogInformation("Found Domain {Host}", domain);
                await SetCustomDnsServers(domain, stoppingToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Unexpected exception with {Domain}", domain.Name);
            }

        }
    }

    private async Task SetCustomDnsServers(Domain domain, CancellationToken ctx)
    {
        try
        {
            var dnsClient = serviceProvider.GetRequiredService<IDnsClient>();
            var lds = domain.Name.Split('.');
            var sld = lds[0];
            var tld = lds[1];
            await dnsClient.SetCustom(sld, tld, NAMESERVERS, ctx);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unable to set custom dns for {Domain}", domain);
        }
    }
    private async Task<IEnumerable<string>> GetDnsServers(Domain domain, CancellationToken ctx)
    {
        try
        {
            var dnsClient = serviceProvider.GetRequiredService<IDnsClient>();
            var lds = domain.Name.Split('.');
            var sld = lds[0];
            var tld = lds[1];
            var domainInfo = await dnsClient.GetList(sld, tld, ctx);
            foreach (var dnsServer in domainInfo.NameServers)
                logger.LogInformation("{Domain} dns servers: {Server}", domain.Name, dnsServer);

            return domainInfo.NameServers;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unable to locate {Domain}", domain.Name);
            return Enumerable.Empty<string>();
        }
    }
}