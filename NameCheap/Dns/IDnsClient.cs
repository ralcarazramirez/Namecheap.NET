using System.Threading;
using System.Threading.Tasks;

namespace NameCheap.Dns;

public interface IDnsClient
{
    /// <summary>
    /// Sets DNS host records settings for the requested domain.
    /// </summary>
    /// <param name="secondLevelDomain">The second level domain, SLD, of the domain for which to set the hosts (the abc in abc.xyz).</param>
    /// <param name="topLevelDomain">The top-level domain, TLD, of the domain for which to set the hosts (the xyz of abc.xyz).</param>
    /// <param name="hostEntries">The list of hosts entries to set.
    /// These need to obey their respective DNS record type rules"
    /// correct IP address for A-records,
    /// domain (not IP) for CNAME, etc</param>
    /// <exception cref="ApplicationException">
    /// Exception when the following problems are encountered:
    /// - 2019166	Domain not found
    /// - 2016166	Domain is not associated with your account
    /// - 2030166	Edit permission for domain is not supported
    /// - 3013288, 4013288	Too many records
    /// - 3031510	Error From Enom when Errorcount != 0
    /// - 3050900	Unknown error from Enom
    /// - 4022288	Unable to get nameserver list
    /// </exception>
    Task SetHosts(string? secondLevelDomain, string? topLevelDomain, HostEntry[] hostEntries, CancellationToken ctx = default);

    /// <summary>
    /// Retrieves DNS host record settings for the requested domain..
    /// </summary>
    /// <param name="sld">The second level domain, SLD, of the domain for which to get the hosts (the abc in abc.xyz).</param>
    /// <param name="tld">The top-level domain, TLD, of the domain for which to get the hosts (the xyz of abc.xyz).</param>
    /// <exception cref="ApplicationException">
    /// Exception when the following problems are encountered:
    /// - 2019166	Domain not found
    /// - 2030166	Edit permission for domain is not supported
    /// - 2030288	Cannot complete this command as this domain is not using proper DNS servers
    /// - 4023330	Unable to get DNS hosts from list
    /// - 3031510	Error From Enom when Errorcount != 0
    /// - 3050900	Unknown error from Enom
    /// - 3011288	Invalid name server specified
    /// - 5050900	Unhandled Exceptions
    /// </exception>
    Task<DnsHostResult> GetHosts(string sld, string tld, CancellationToken ctx = default);

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
    Task<DnsEmailForwardingResult> GetEmailForwarding(string domain, CancellationToken ctx = default);

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
    Task SetEmailForwarding(string domain, EmailForwarding[] request, CancellationToken ctx = default);

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
    void DeleteAllEmailForwarding(string domain, CancellationToken ctx = default);

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
    Task<DnsListResult> GetList(string sld, string tld, CancellationToken ctx = default);

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
    Task SetCustom(string sld, string tld, string[] nameservers, CancellationToken ctx = default);

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
    Task SetDefault(string sld, string tld, CancellationToken ctx = default);
}
