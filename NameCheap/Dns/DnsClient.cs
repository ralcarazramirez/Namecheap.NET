using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace NameCheap.Dns;

/// <summary>Set of functions used to manipulate DNS records: hosts, name servers, and even email forwarding.</summary>
public class DnsClient : IDnsClient
{
    private readonly XNamespace _ns = XNamespace.Get("http://api.namecheap.com/xml.response");
    private readonly NamecheapOptions _params;
    private readonly ILogger<DnsClient> _logger;
    private NamecheapClient _query;
    
    public DnsClient(IOptions<NamecheapOptions> globalParams, ILogger<DnsClient> logger, NamecheapClient query)
    {
        _params = globalParams.Value;
        _logger = logger;
        _query = query;
    }
    /// <summary>
    /// Gets email forwarding settings for the requested domain.
    /// </summary>
    /// <param name="domain">the domain for which to get forwarding settings.</param>
    /// <exception cref="ApplicationException">
    /// Exception when the following problems are encountered:
    /// - 2019166	Domain not found
    /// - 2030166	Edit permission for domain is not supported
    /// - 2030288	Cannot complete this command as this domain is not using proper DNS servers
    /// - 3031510	Error From Enom when Errorcount != 0
    /// - 3050900	Unknown error from Enom
    /// - 4022328	Unable to get EmailForwarding records from database
    /// - 3011288	Invalid nameserver
    /// - 5050900	Unhandled Exceptions
    /// </exception>
    public async Task<DnsEmailForwardingResult> GetEmailForwarding(string? domain)
    {
        _query.AddParameter("DomainName", domain);

        var doc = await _query.ExecuteAsync("namecheap.domains.dns.getEmailForwarding");

        var serializer = new XmlSerializer(typeof(DnsEmailForwardingResult), _ns.NamespaceName);

        using var reader = doc.Root.Element(_ns + "CommandResponse").Element(_ns + "DomainDNSGetEmailForwardingResult").CreateReader();
        return (DnsEmailForwardingResult)serializer.Deserialize(reader);
    }

    /// <summary>
    /// Sets email forwarding for a domain name.
    /// </summary>
    /// <param name="domain">The domain for which to set email forwarding.</param>
    /// <param name="request">The entire list of forwards to set up.</param>
    /// <exception cref="ApplicationException">
    /// Exception when the following problems are encountered:
    /// - 2019166	Domain not found 
    /// - 2016166 Domain is not associated with your account
    /// - 2030288 Cannot complete this command as this domain is not using proper DNS servers
    /// - 2030166 Edit Permission for domain is not supported
    /// - 3013288 Too many records
    /// - 3031510 Error From Enom when Errorcount != 0
    /// - 3050900 Unknown error from Enom
    /// - 4022228 Unable to get nameserver list
    /// </exception>
    public async Task SetEmailForwarding(string? domain, EmailForwarding[] request)
    {
        _query.AddParameter("DomainName", domain);
        
        for (var i = 0; i < request.Length; i++)
        {
            _query.AddParameter("MailBox" + (i + 1), request[i].MailBox);
            _query.AddParameter("ForwardTo" + (i + 1), request[i].ForwardTo);
        }

        await _query.ExecuteAsync("namecheap.domains.dns.setEmailForwarding");
    }

    /// <summary>
    /// Deletes all the email forwarding for a domain.
    /// </summary>
    /// <param name="domain">The domain for which to delete the forwards.</param>
    /// <exception cref="ApplicationException">
    /// Exception when the following problems are encountered:
    /// - 2019166	Domain not found 
    /// - 2016166 Domain is not associated with your account
    /// - 2030288 Cannot complete this command as this domain is not using proper DNS servers
    /// - 2030166 Edit Permission for domain is not supported
    /// - 3031510 Error From Enom when Errorcount != 0
    /// - 3050900 Unknown error from Enom
    /// - 4022228 Unable to get nameserver list
    /// </exception>
    public void DeleteAllEmailForwarding(string? domain)
    {
        SetEmailForwarding(domain, new EmailForwarding[0]);
    }

    /// <summary>
    /// Gets a list of DNS servers associated with the requested domain.
    /// </summary>
    /// <param name="sld">The second level domain, SLD, of the domain for which to get the list of name servers (the abc in abc.xyz).</param>
    /// <param name="tld">The top-level domain, TLD, of the domain for which to get the list of name servers (the xyz of abc.xyz).</param>
    /// <exception cref="ApplicationException">
    /// Exception when the following problems are encountered:
    /// - 2019166	Domain not found
    /// - 2016166	Domain is not associated with your account
    /// - 3031510	Error From Enom when Errorcount != 0
    /// - 3050900	Unknown error from Enom
    /// - 4022288	Unable to get nameserver list
    /// </exception>
    public async Task<DnsListResult> GetList(string? sld, string? tld)
    {
        var doc = await _query
            .AddParameter("SLD", sld)
            .AddParameter("TLD", tld)
            .ExecuteAsync("namecheap.domains.dns.getList");
        var serializer = new XmlSerializer(typeof(DnsListResult), _ns.NamespaceName);

        using var reader = doc.Root?.Element(_ns + "CommandResponse")?.Element(_ns + "DomainDNSGetListResult")?.CreateReader();
        if (reader is not null)
            return (DnsListResult)serializer.Deserialize(reader);
        throw new ApplicationException();
    }

    /// <summary>
    /// Sets domain to use custom DNS servers.
    /// </summary>
    /// <param name="sld">The second level domain, SLD, of the domain for which to set the list of name servers (the abc in abc.xyz).</param>
    /// <param name="tld">The top-level domain, TLD, of the domain for which to set the list of name servers (the xyz of abc.xyz).</param>
    /// <param name="nameservers">IP address of the custom domain name servers.</param>
    /// <exception cref="ApplicationException">
    /// Exception when the following problems are encountered:
    /// - 2019166	Domain not found
    /// - 2016166	Domain is not associated with your account
    /// - 2030166	Edit permission for domain is not supported
    /// - 3031510	Error From Enom when Errorcount != 0
    /// - 3050900	Unknown error from Enom
    /// - 4022288	Unable to get nameserver list
    /// </exception>
    public async Task SetCustom(string? sld, string? tld, params string[] nameservers)
    {
        await _query
            .AddParameter("SLD", sld)
            .AddParameter("TLD", tld)
            .AddParameter("Nameservers", string.Join(",", nameservers))
            .ExecuteAsync("namecheap.domains.dns.setCustom");
    }

    /// <summary>
    /// Sets domain to use NameCheap's default DNS servers.
    /// </summary>
    /// <param name="sld">The second level domain, SLD, of the domain for which to use the default name servers (the abc in abc.xyz).</param>
    /// <param name="tld">The top-level domain, TLD, of the domain for which to use the default name servers (the xyz of abc.xyz).</param>
    /// <remarks>
    /// Required for free services like Host record management, URL forwarding, email forwarding, dynamic dns and other value added services.
    /// </remarks>
    /// <exception cref="ApplicationException">
    /// Exception when the following problems are encountered:
    /// - 2019166	Domain not found
    /// - 2016166	Domain is not associated with your account
    /// - 2030166	Edit permission for domain is not supported
    /// - 3013288	Too many records
    /// - 3031510	Error From Enom when Errorcount != 0
    /// - 3050900	Unknown error from Enom
    /// - 4022288	Unable to get nameserver list
    /// </exception>
    public async Task SetDefault(string? sld, string? tld)
    {
        await _query
            .AddParameter("SLD", sld)
            .AddParameter("TLD", tld)
            .ExecuteAsync("namecheap.domains.dns.setDefault");
    }

    public async Task SetHosts(string? secondLevelDomain, string? topLevelDomain, HostEntry[] hostEntries, CancellationToken ctx = default)
    {
        _query.AddParameter("SLD", secondLevelDomain);
        _query.AddParameter("TLD", topLevelDomain);

        for (var i = 0; i < hostEntries.Length; i++)
        {
            _query.AddParameter("HostName" + (i + 1), hostEntries[i].HostName);
            _query.AddParameter("Address" + (i + 1), hostEntries[i].Address);
            _query.AddParameter("MxPref" + (i + 1), hostEntries[i].MxPref);
            _query.AddParameter("RecordType" + (i + 1), Enum.GetName(typeof(RecordType), hostEntries[i].RecordType));

            if (!string.IsNullOrEmpty(hostEntries[i].Ttl))
                _query.AddParameter("TTL" + (i + 1), hostEntries[i].Ttl);
        }

        var doc = await _query.ExecuteAsync("namecheap.domains.dns.setHosts", ctx);
    }

    public async Task<DnsHostResult> GetHosts(string sld, string tld, CancellationToken ctx = default)
    {
        var doc = await _query
            .AddParameter("SLD", sld)
            .AddParameter("TLD", tld)
            .ExecuteAsync("namecheap.domains.dns.getHosts", ctx);

        var serializer = new XmlSerializer(typeof(DnsHostResult), _ns.NamespaceName);

        using var reader = doc.Root.Element(_ns + "CommandResponse").Element(_ns + "DomainDNSGetHostsResult").CreateReader();
        return (DnsHostResult)serializer.Deserialize(reader);
    }

    public async Task<DnsEmailForwardingResult> GetEmailForwarding(string domain, CancellationToken ctx = default)
        => throw new NotImplementedException();

    public async Task SetEmailForwarding(string domain, EmailForwarding[] request, CancellationToken ctx = default)
        => throw new NotImplementedException();

    public void DeleteAllEmailForwarding(string domain, CancellationToken ctx = default)
    {
        throw new NotImplementedException();
    }

    public async Task<DnsListResult> GetList(string sld, string tld, CancellationToken ctx = default)
    {
        var doc = await _query
            .AddParameter("SLD", sld)
            .AddParameter("TLD", tld)
            .ExecuteAsync("namecheap.domains.dns.getList", ctx);
        var serializer = new XmlSerializer(typeof(DnsListResult), _ns.NamespaceName);

        using var reader = doc.Root?.Element(_ns + "CommandResponse")?.Element(_ns + "DomainDNSGetListResult")?.CreateReader();
        if (reader is not null)
            return (DnsListResult)serializer.Deserialize(reader);
        throw new ApplicationException();
    }

    public async Task SetCustom(string sld, string tld, string[] nameservers, CancellationToken ctx = default)
        => await _query
            .AddParameter("SLD", sld)
            .AddParameter("TLD", tld)
            .AddParameter("Nameservers", string.Join(",", nameservers))
            .ExecuteAsync("namecheap.domains.dns.setCustom", ctx);

    public async Task SetDefault(string sld, string tld, CancellationToken ctx = default)
        => await _query
            .AddParameter("SLD", sld)
            .AddParameter("TLD", tld)
            .ExecuteAsync("namecheap.domains.dns.setDefault", ctx);
}

