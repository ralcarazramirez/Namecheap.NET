using System.Threading;
using System.Threading.Tasks;

namespace NameCheap.Domain;

public interface IDomainsApi
{
    /// <summary>
    ///  Checks the availability of domains.
    /// </summary>
    /// <param name="domains">Domains to check.</param>
    /// <param name="ctx">token to cancel in progress requests</param>
    /// <returns>List of results for each parameter. Order is not guaranteed to match the order of parameters.</returns>
    /// <exception cref="ApplicationException">
    /// Exception when the following problems are encountered:
    /// - 3031510	Error response from Enom when the error count != 0
    /// - 3011511	Unknown response from the provider
    /// - 2011169	Only 50 domains are allowed in a single check command
    /// </exception>
    Task<DomainCheckResult[]> AreAvailable(string[] domains, CancellationToken ctx = default);

    /// <summary>
    /// Registers a new domain.
    /// </summary>
    /// <param name="domain">Information about domain to register.</param>
    /// <param name="ctx">token to cancel in progress requests</param>
    /// <returns>Information about the created domain.</returns>
    /// <exception cref="ApplicationException">
    /// Exception when the following problems are encountered:
    /// - 2033409	Possibly a logical error at the authentication phase. The order chargeable for the Username is not found
    /// - 2033407, 2033270	Cannot enable Whoisguard when AddWhoisguard is set to NO
    /// - 2015182	Contact phone is invalid. The phone number format is +NNN.NNNNNNNNNN
    /// - 2015267	EUAgreeDelete option should not be set to NO
    /// - 2011170	Validation error from PromotionCode
    /// - 2011280	Validation error from TLD
    /// - 2015167	Validation error from Years
    /// - 2030280	TLD is not supported in API
    /// - 2011168	Nameservers are not valid
    /// - 2011322	Extended Attributes are not valid
    /// - 2010323	Check the required field for billing domain contacts
    /// - 2528166	Order creation failed
    /// - 3019166, 4019166	Domain not available
    /// - 3031166	Error while getting information from the provider
    /// - 3028166	Error from Enom ( Errcount <> 0 )
    /// - 3031900	Unknown response from the provider
    /// - 4023271	Error while adding a free PositiveSSL for the domain
    /// - 3031166	Error while getting a domain status from Enom
    /// - 4023166	Error while adding a domain
    /// - 5050900	Unknown error while adding a domain to your account
    /// - 4026312	Error in refunding funds
    /// - 5026900	Unknown exceptions error while refunding funds
    /// - 2515610	Prices do not match
    /// - 2515623	Domain is premium while considered regular or is regular while considered premium
    /// - 2005	Country name is not valid
    /// </exception>
    Task<DomainCreateResult> Create(DomainCreateRequest domain, CancellationToken ctx = default);

    /// <summary>
    /// Gets contact information for the requested domain.
    /// </summary>
    /// <param name="domain">Domain to get contacts.</param>
    /// <param name="ctx">token to cancel in progress requests</param>
    /// <returns>All the contacts, Admin, AuxBilling, Registrant, and Tech for the domain.</returns>
    /// <exception cref="ApplicationException">
    /// Exception when the following problems are encountered:
    /// - 2019166	Domain not found
    /// - 2016166	Domain is not associated with your account
    /// - 4019337	Unable to retrieve domain contacts
    /// - 3016166	Domain is not associated with Enom
    /// - 3019510	This domain has expired/ was transferred out/ is not associated with your account
    /// - 3050900	Unknown response from provider
    /// - 5050900	Unknown exceptions
    /// </exception>
    Task<DomainContactsResult> GetContacts(string domain, CancellationToken ctx = default);

    /// <summary>
    /// Returns information about the requested domain.
    /// </summary>
    /// <param name="domain">Domain name for which domain information needs to be requested.</param>
    /// <param name="ctx">token to cancel in progress requests</param>
    /// <exception cref="ApplicationException">
    /// Exception when the following problems are encountered:
    /// - 5019169	Unknown exceptions
    /// - 2030166	Domain is invalid
    /// - 4011103 - DomainName not Available; or UserName not Available; or Access denied
    /// </exception>
    Task<DomainInfoResult> GetInfo(string domain, CancellationToken ctx = default);

    /// <summary>
    /// Returns a list of domains for the particular user.
    /// </summary>
    /// <param name="ctx">token to cancel in progress requests</param>
    /// <exception cref="ApplicationException">
    /// Exception when the following problems are encountered:
    /// 5050169	Unknown exceptions
    /// </exception>
    Task<DomainListResult> GetList(CancellationToken ctx = default);

    /// <summary>
    /// Gets the Registrar Lock status for the requested domain.
    /// </summary>
    /// <param name="domain">Domain name to get status for.</param>
    /// <param name="ctx">token to cancel in progress requests</param>
    /// <returns>true if the domain is locked for registrar transfer, false if unlocked.</returns>
    /// <exception cref="ApplicationException">
    /// Exception when the following problems are encountered:
    /// - 2019166	Domain not found
    /// - 2016166	Domain is not associated with your account
    /// - 3031510	Error response from provider when errorcount !=0
    /// - 3050900	Unknown error response from Enom
    /// - 5050900	Unknown exceptions
    /// </exception>
    Task<bool> GetRegistrarLock(string domain, CancellationToken ctx = default);

    /// <summary>
    /// Locks the domain for registrar transfer.
    /// </summary>
    /// <param name="domain">Domain name to lock.</param>
    /// <param name="ctx">token to cancel in progress requests</param>
    /// <exception cref="ApplicationException">
    /// Exception when the following problems are encountered:
    /// - 2015278	Invalid data specified for LockAction
    /// - 2019166	Domain not found
    /// - 2016166	Domain is not associated with your account
    /// - 3031510	Error from Enom when Errorcount != 0
    /// - 2030166	Edit permission for domain is not supported
    /// - 3050900	Unknown error response from Enom
    /// - 5050900	Unknown exceptions
    /// </exception>
    Task SetRegistrarLock(string domain, CancellationToken ctx = default);

    /// <summary>
    /// Unlocks (opens) the domain for registrar transfer.
    /// </summary>
    /// <param name="domain">Domain name to unlock.</param>
    /// <param name="ctx">token to cancel in progress requests</param>
    /// <exception cref="ApplicationException">
    /// Exception when the following problems are encountered:
    /// - 2015278	Invalid data specified for LockAction
    /// - 2019166	Domain not found
    /// - 2016166	Domain is not associated with your account
    /// - 3031510	Error from Enom when Errorcount != 0
    /// - 2030166	Edit permission for domain is not supported
    /// - 3050900	Unknown error response from Enom
    /// - 5050900	Unknown exceptions
    /// </exception>
    Task SetRegistrarUnlock(string domain, CancellationToken ctx = default);

    /// <summary>
    /// Returns a list of TLD - top level domains.
    /// </summary>
    /// <exception cref="ApplicationException">
    /// Exception when the following problems are encountered:
    /// - 2011166	UserName is invalid
    /// - 3050900	Unknown response from provider
    /// </exception>
    Task<TldListResult> GetTldList(CancellationToken ctx = default);

    /// <summary>
    /// Renews an expiring domain.
    /// </summary>
    /// <param name="domain">Domain name to renew.</param>
    /// <param name="years">Number of years to renew.</param>
    /// <param name="ctx">token to cancel in progress requests</param>
    /// <returns>information about the renewal, such as the charged amount, or the order Id.</returns>
    /// <exception cref="ApplicationException">
    /// Exception when the following problems are encountered:
    /// - 2033409	Possibly a logical error at the authentication phase. The order chargeable for the Username is not found.
    /// - 2011170	Validation error from PromotionCode
    /// - 2011280	TLD is invalid
    /// - 2528166	Order creation failed
    /// - 2020166	Domain has expired. Please reactivate your domain.
    /// - 3028166	Failed to renew, error from Enom
    /// - 3031510	Error from Enom ( Errcount != 0 )
    /// - 3050900	Unknown error from Enom
    /// - 2016166	Domain is not associated with your account
    /// - 4024167	Failed to update years for your domain
    /// - 4023166	Error occurred during the domain renewal
    /// - 4022337	Error in refunding funds
    /// - 2015170	Promotion code is not allowed for premium domains
    /// - 2015167	Premium domain can be renewed for 1 year only
    /// - 2015610	Premium prices cannot be zero for premium domains
    /// - 2515623	You are trying to renew a premium domain. Premium price should be added to request to renew the premium domain.
    /// - 2511623	Domain name is not premium
    /// - 2515610	Premium price is incorrect. It should be (premium renewal price value).
    /// </exception>
    Task<DomainRenewResult> Renew(string domain, int years, CancellationToken ctx = default);

    /// <summary>
    /// Reactivates an expired domain.
    /// </summary>
    /// <param name="domain">Domain to reactivate.</param>
    /// <param name="ctx">token to cancel in progress requests</param>
    /// <returns>information about the renewal, such as the charged amount, or the order Id.</returns>
    /// <exception cref="ApplicationException">
    /// Exception when the following problems are encountered:
    /// - 2033409	Possibly a logical error at the authentication phase. The order chargeable for the Username is not found.
    /// - 2019166	Domain not found
    /// - 2030166	Edit permission for the domain is not supported
    /// - 2011170	Promotion code is invalid
    /// - 2011280	TLD is invalid
    /// - 2528166	Order creation failed
    /// - 3024510	Error response from Enom while updating the domain
    /// - 3050511	Unknown error response from Enom
    /// - 2020166	Domain does not meet the expiration date for reactivation
    /// - 2016166	Domain is not associated with your account
    /// - 5050900	Unhandled exceptions
    /// - 4024166	Failed to update the domain in your account
    /// - 2015170	Promotion code is not allowed for premium domains
    /// - 2015167	Premium domain can be reactivated for 1 year only
    /// - 2015610	Premium prices cannot be zero for premium domains
    /// - 2515623	You are trying to reactivate a premium domain. Premium price should be added to the request to reactivate the premium domain.
    /// - 2511623	Domain name is not premium
    /// - 2515610	Premium price is incorrect. It should be (premium renewal price value).
    /// </exception>
    Task<DomainReactivateResult> Reactivate(string domain, CancellationToken ctx = default);

    /// <summary>
    /// Sets contact information for the requested domain.
    /// </summary>
    /// <param name="contacts">
    /// The contact information to be set.
    /// All 4 parameters, Registrant, Tech, Admin, and Aux Billig
    /// need to be present. The required fields for each address
    /// are: FirstName, LastName, Address1, StateProvince,
    /// PostalCode, Country, Phone, and EmailAddress.</param>
    /// <param name="ctx">token to cancel in progress requests</param>
    /// <exception cref="ApplicationException">
    /// Exception when the following problems are encountered:
    /// - 2019166	Domain not found
    /// - 2030166	Edit permission for domain is not supported
    /// - 2010324	Registrant contacts such as firstname, lastname etc. are missing
    /// - 2010325	Tech contacts such as firstname, lastname etc. are missing
    /// - 2010326	Admin contacts such as firstname, lastname etc. are missing
    /// - 2015182	The contact phone is invalid. The phone number format is +NNN.NNNNNNNNNN
    /// - 2010327	AuxBilling contacts such as firstname, lastname etc. are missing
    /// - 2016166	Domain is not associated with your account
    /// - 2011280	Cannot see the contact information for your TLD
    /// - 4022323	Error retrieving domain Contacts
    /// - 2011323	Error retrieving domain Contacts from Enom (invalid errors)
    /// - 3031510	Error from Enom when error count != 0
    /// - 3050900	Unknown error from Enom
    /// </exception>
    void SetContacts(DomainContactsRequest contacts, CancellationToken ctx);
}