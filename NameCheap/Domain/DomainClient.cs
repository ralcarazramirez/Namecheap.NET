using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace NameCheap.Domain;

public class DomainClient : IDomainsApi
{
    private readonly ILogger<DomainClient> _logger;


    public DomainClient(ILogger<DomainClient> logger, HttpClient client)
    {
        _logger = logger;
    }

    public async Task SendCommand()
    {
        
    }
    
    public async Task<DomainCheckResult[]> AreAvailable(string[] domains, CancellationToken ctx = default)
    {
        
        throw new NotImplementedException();
    }

    public async Task<DomainCreateResult> Create(DomainCreateRequest domain, CancellationToken ctx = default)
        => throw new NotImplementedException();

    public async Task<DomainContactsResult> GetContacts(string domain, CancellationToken ctx = default)
        => throw new NotImplementedException();

    public async Task<DomainInfoResult> GetInfo(string domain, CancellationToken ctx = default)
        => throw new NotImplementedException();

    public async Task<DomainListResult> GetList(CancellationToken ctx = default)
        => throw new NotImplementedException();

    public async Task<bool> GetRegistrarLock(string domain, CancellationToken ctx = default)
        => throw new NotImplementedException();

    public async Task SetRegistrarLock(string domain, CancellationToken ctx = default)
        => throw new NotImplementedException();

    public async Task SetRegistrarUnlock(string domain, CancellationToken ctx = default)
        => throw new NotImplementedException();

    public async Task<TldListResult> GetTldList(CancellationToken ctx = default)
        => throw new NotImplementedException();

    public async Task<DomainRenewResult> Renew(string domain, int years, CancellationToken ctx = default)
        => throw new NotImplementedException();

    public async Task<DomainReactivateResult> Reactivate(string domain, CancellationToken ctx = default)
        => throw new NotImplementedException();

    public void SetContacts(DomainContactsRequest contacts, CancellationToken ctx)
    {
        throw new NotImplementedException();
    }
}

public class NamecheapDelegator : DelegatingHandler
{
    private readonly ILogger<NamecheapDelegator> _logger;
    private readonly IOptions<NamecheapOptions> _options;

    public NamecheapDelegator(ILogger<NamecheapDelegator> logger, IOptionsSnapshot<NamecheapOptions> options)
    {
        _logger = logger;
        _options = options;
    }
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var options = _options.Value;
        request.RequestUri = options.Url;
        
        var response = await base.SendAsync(request, cancellationToken);

        return response;
    }
}